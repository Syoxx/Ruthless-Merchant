// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "AFS/Foliage Shader Touchbending v4" {
	Properties {
		[LM_Albedo] [LM_Transparency] _MainTex ("Base (RGB)", 2D) = "white" {}
		[LM_TransparencyCutOff] _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
		[LM_Glossiness] _BumpTransSpecMap ("Normal (GA) Trans(R) Smoothness(B)", 2D) = "bump" {}
		[LM_Specular] _SpecularReflectivity("Specular Reflectivity", Color) = (0.2,0.2,0.2)
		_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1)
		_TranslucencyViewDependency ("Translucency View Dependency", Range(0,1)) = 0.5
		_LeafTurbulence ("Leaf Turbulence", Range(0,1)) = 0.5
		[KeywordEnum(Vertex Colors, Vertex Colors And UV4)] _BendingControls ("Bending Parameters", Float) = 0 // 0 = vertex colors, 1 = uv4
		_TouchBendingPosition ("TouchBendingPosition", Vector) = (0,0,0,0)
		_TouchBendingForce ("TouchBendingForce", Vector) = (0,0,0,0)
	}

	SubShader {
		Tags {
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="AFSFoliageTouchBending"
			"AfsMode"="Foliage"
		}
		LOD 200
		
		CGPROGRAM
		
		// Use our own early alpha testing: so no alphatest:_Cutoff
		#pragma surface surf AFSSpecular vertex:AfsFoligeBendingGSFull addshadow fullforwardshadows exclude_path:prepass exclude_path:deferred
		#pragma target 3.0
		#include "Includes/AfsPBSLighting.cginc"
		#pragma shader_feature _AFS_DEFERRED

		// Next to the additional properties this is all it needs to make the shader supporting touch bending... make sure you define it before all includes
		#define AFS_TOUCHBENDING

		// Vertex Functions
		#include "TerrainEngine.cginc"
		#include "Includes/AfsFoliageBending.cginc"

		#ifdef _AFS_DEFERRED
			#undef UNITY_APPLY_FOG
			#define UNITY_APPLY_FOG(coord,col) /**/
		#endif
		
		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _Cutoff;

		#if !defined(UNITY_PASS_SHADOWCASTER)
			sampler2D _BumpTransSpecMap;
			sampler2D _BumpMap;
			fixed3 _SpecularReflectivity;
			// Global vars
			float _AfsRainamount;
			float2 _AfsSpecFade;
		#endif

		struct Input {
			float4 myuv_MainTex;		// here we need float4
			// Unity 5.2 does not like this?!
			//#if !defined(UNITY_PASS_SHADOWCASTER)
				fixed4 color : COLOR;	// color.a = AO
				float3 worldNormal;
				INTERNAL_DATA
			//#endif
		};

		void AfsFoligeBendingGSFull (inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.myuv_MainTex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			float4 bendingCoords;
			bendingCoords.rg = v.color.rg;

		//	Legacy Bending:	Primary and secondary bending stored in vertex color blue
		//	New Bending:	Primary and secondary bending stored in uv4
		//	x = primary bending = blue
		//	y = secondary = alpha
			bendingCoords.ba = (_BendingControls == 0) ? v.color.bb : v.texcoord3.xy;
		//	Add variation only if the shader uses UV4
			float variation = (_BendingControls == 0) ? 1.0 : v.color.b * 2;
		//	
			v.vertex = AfsAnimateVertex (v.vertex, v.normal, bendingCoords, variation);
			
		//	also animate the normal
			v.normal = lerp( v.normal , mul(_RotMatrix, float4(v.normal, 0)).xyz, bendingCoords.b * 10 * (1 + bendingCoords.r ) );
			
			v.normal = normalize(v.normal);
			v.tangent.xyz = normalize(v.tangent.xyz);
			
			#if !defined(UNITY_PASS_SHADOWCASTER)	
			//	Store Fade for specular highlights
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.myuv_MainTex.w = saturate( ( _AfsSpecFade.x - distance(_WorldSpaceCameraPos, worldPos)) / _AfsSpecFade.y);
			#endif	
		}

		void surf (Input IN, inout SurfaceOutputAFSSpecular o) {
			half4 c = tex2D (_MainTex, IN.myuv_MainTex.xy);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			// Do early alpha test
			clip(o.Alpha - _Cutoff);

			#if !defined(UNITY_PASS_SHADOWCASTER)
				fixed4 trngls = tex2D(_BumpTransSpecMap, IN.myuv_MainTex.xy);
				o.Smoothness = trngls.b;
				o.Specular = _SpecularReflectivity;
				o.Normal = UnpackNormalDXT5nm(trngls);
				o.Translucency = trngls.r;
				o.Occlusion = IN.color.a;
				//	Rain
				if (_AfsRainamount > 0.0f) {
					//	Calc WorldNormal
					float3 worldNormal = WorldNormalVector (IN, o.Normal);
				 	float Rainamount = saturate(_AfsRainamount * worldNormal.y);
					float porosity = saturate( ((1-o.Smoothness) - 0.5) / 0.4 );
					// Calc diffuse factor
					float factor = lerp(1, 0.2, porosity);
					// Water influence on material BRDF
					o.Albedo *= lerp(1.0, factor, Rainamount); // Attenuate diffuse
					o.Smoothness = lerp(o.Smoothness, 1.0, Rainamount);
					// Lerp specular Color towards IOR of Water
					o.Specular = lerp(o.Specular, 0.02, Rainamount);
				}
				o.Smoothness *= IN.myuv_MainTex.w; // specfade
			#endif
		}
		ENDCG
	}
}
