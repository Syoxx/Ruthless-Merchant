// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "AFS/Foliage Shader Deferred SingleSided v4" {
	Properties {
		[LM_Albedo] [LM_Transparency] _MainTex ("Base (RGB)", 2D) = "white" {}
		[LM_TransparencyCutOff] _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
		[LM_Glossiness] _BumpTransSpecMap ("Normal (GA) Trans(R) Smoothness(B)", 2D) = "bump" {}
		[LM_Specular] _SpecularReflectivity("Specular Reflectivity", Color) = (0.2,0.2,0.2)
		_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1)
		_TranslucencyViewDependency ("Translucency View Dependency", Range(0,1)) = 0.5
		_LeafTurbulence ("Leaf Turbulence", Range(0,1)) = 0.5
		[KeywordEnum(Vertex Colors, Vertex Colors And UV4)] _BendingControls ("Bending Parameters", Float) = 0 // 0 = vertex colors, 1 = uv4
	}
	SubShader {
		Tags {
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="AFSFoliageBendingSingleSided"
			"AfsMode"="Foliage"
		}
		LOD 200
		Cull Off
		
		CGPROGRAM
		#pragma surface surf StandardSpecular vertex:AfsFoligeBendingGSFull addshadow fullforwardshadows
		#pragma target 3.0

		// Vertex Functions
		#include "TerrainEngine.cginc"
		#include "Includes/AfsFoliageBending.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _BumpTransSpecMap;
		sampler2D _BumpMap;
		fixed3 _SpecularReflectivity;
		fixed _Cutoff;

		fixed4 _TranslucencyColor;
		fixed _TranslucencyViewDependency;

		// Global vars
		float _AfsRainamount;
		float2 _AfsSpecFade;

		float4 _AfsDirectSunDir;
		fixed3 _AfsDirectSunCol;

		struct Input {
			float4 myuv_MainTex;	// here we need float4
			fixed4 color : COLOR;	// color.a = AO
			float3 worldNormal;
			float4 translucency;
			INTERNAL_DATA
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
			v.vertex = AfsAnimateVertex (v.vertex, v.normal, bendingCoords, variation);

			#if !defined(UNITY_PASS_SHADOWCASTER)
			//	Store Fade for specular highlights
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.myuv_MainTex.w = saturate( ( _AfsSpecFade.x - distance(_WorldSpaceCameraPos, worldPos)) / _AfsSpecFade.y);
				v.normal = normalize(v.normal);
				v.tangent.xyz = normalize(v.tangent.xyz);
			//	Calculate per vertex translucency according to _AfsDirectSunDir
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				float3 worldNormal = UnityObjectToWorldNormal( v.normal );
				float dotNV = dot(worldNormal, worldViewDir);
				// Adds too many artifacts...
				// worldNormal  = dotNV < 0.0 ? -worldNormal : worldNormal;
				float MinusDotNL = saturate(dot(worldNormal, _AfsDirectSunDir.xyz ));
				fixed backContrib = saturate(dot(worldViewDir, _AfsDirectSunDir.xyz));
				// normally translucency is more like -dotNL, but looks better when it's view dependent
				backContrib = lerp(MinusDotNL, backContrib, _TranslucencyViewDependency);
				o.translucency.rgb = backContrib * v.color.a * _AfsDirectSunDir.w * _TranslucencyColor;
				// Mask Smoothness on back faces
				//o.translucency.a = dotNV > 0.0 ? 1.0 : 0.0;
				o.translucency.a = dotNV > 0.0 ? 1.0 : saturate(1.0+dotNV*2);
			#endif
		}

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
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

			//	The well known occlusion problem... so we multiply it later on top of albedo, huuuh
			//	#if defined (UNITY_PASS_FORWARDBASE) || defined (UNITY_PASS_FORWARDADD) || defined (UNITY_PASS_DEFERRED)
			//		o.Occlusion = IN.color.a;
			//	#else
			//		o.Occlusion = 1;
			//	#endif
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
			//	Add approximated Translucency / and AO - unfortunately
				o.Albedo = o.Albedo * IN.color.a + trngls.r * IN.translucency.rgb;
				o.Smoothness *= IN.myuv_MainTex.w; // specfade
				o.Smoothness *= IN.translucency.a;	// fade out smoothness on back faces
			#endif
		}
		ENDCG
	} 
}
