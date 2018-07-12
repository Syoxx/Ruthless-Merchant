// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Advanced Foliage shader for the terrain engine replacing the built in vertex lit shader

Shader "Hidden/TerrainEngine/Details/Vertexlit" {
	Properties {
		[LM_Albedo] [LM_Transparency] _MainTex ("Base (RGB)", 2D) = "white" {}
		[LM_TransparencyCutOff] _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
		//[KeywordEnum(Vertex Colors, Vertex Colors And UV4)] _BendingControls ("Bending Parameters", Float) = 0 // 0 = vertex colors, 1 = uv4
		[HideInInspector] _BendingControls ("Bending Parameters", Float) = 0
	}

	SubShader {
		Tags {
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="AFSFoliageBendingVertexLit"
			"AfsMode"="Foliage"
		}
		LOD 200
		
		CGPROGRAM
		
		// Use our own early alpha testing: so no alphatest:_Cutoff
		#pragma surface surf AFSSpecular vertex:AfsFoligeBendingGSFull addshadow fullforwardshadows exclude_path:prepass exclude_path:deferred
		#pragma target 3.0

		// Overwrites for the lighting function
		#define _TranslucencyColor _AfsTranslucencyColor
		#define _TranslucencyViewDependency _AfsTranslucencyViewDependency


		#include "../Includes/AfsPBSLighting.cginc"
		#pragma shader_feature _AFS_DEFERRED

		// Vertex Functions
		#include "TerrainEngine.cginc"
		#include "../Includes/AfsFoliageBending.cginc"

		#ifdef _AFS_DEFERRED
			#undef UNITY_APPLY_FOG
			#define UNITY_APPLY_FOG(coord,col) /**/
		#endif
		
		sampler2D _MainTex;
		float4 _MainTex_ST;
		//fixed _AfsTerrainBendingMode;
		fixed _AfsAlphaCutOff;

		#if !defined(UNITY_PASS_SHADOWCASTER)
			sampler2D _TerrianBumpTransSpecMap;
			half3 _AfsSpecularReflectivity;
			// Global vars
			float _AfsRainamount;
			float2 _AfsSpecFade;
		#endif

		struct Input {
			float4 myuv_MainTex;	// here we need float4
			fixed4 color : COLOR;	// color.a = AO
			float3 worldNormal;
			INTERNAL_DATA
		};

		void AfsFoligeBendingGSFull (inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.myuv_MainTex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			
		//	Supply the shader with tangents
			float3 newBinormal = cross(float3(1, 0, 1), v.normal);
			float3 newTangent = normalize(cross(v.normal.xyz, newBinormal.xyz));
			float3 newBinormal1 = cross(v.normal, newTangent);
			// v.tangent.xyz = normalize(-newTangent); // do we have to reverse it? / Fixed since RC2
			v.tangent.xyz = normalize(newTangent);
			// v.tangent.xyz = normalize(newTangent);
			v.tangent.w = (dot(newBinormal1, newBinormal) < 0) ? -1.0 : 1.0;

			float4 bendingCoords;

		//	As dx "changes" vertex colors....
			#if (SHADER_API_D3D9) || (SHADER_API_D3D11)
				bendingCoords.xyzw = v.color.zyxx;
			#else
				bendingCoords.xyzw = v.color.xyzz;
			#endif

		//	Terrain engine supports vertex colors only – no uv4

		//	Classic Bending:	Primary and secondary bending stored in vertex color blue
		//	New Bending:		Primary and secondary bending stored in uv4
			//bendingCoords.zw = (_AfsTerrainBendingMode == 0) ? v.color.zz : v.texcoord3.xy;

			// early exit if there is nothing to animate (pivots or even rocks)
			if (bendingCoords.b + bendingCoords.g > 0) {
				v.vertex = AfsAnimateVertex (v.vertex, v.normal, bendingCoords, 1.0f );
				v.normal = normalize(v.normal);
				v.tangent.xyz = normalize(v.tangent.xyz);
			}

		//	v.color.a *= v.color.a; // * v.color.a; // * v.color.a;
			
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
			clip(o.Alpha - _AfsAlphaCutOff);

			#if !defined(UNITY_PASS_SHADOWCASTER)
				fixed4 trngls = tex2D(_TerrianBumpTransSpecMap, IN.myuv_MainTex.xy);
				o.Smoothness = trngls.b;
				o.Specular = _AfsSpecularReflectivity;
				o.Normal = UnpackNormalDXT5nm(trngls);
				
/*
				o.Normal.xy = (trngls.wy * 2 - 1);
				#if (SHADER_TARGET >= 30)
					// SM2.0: instruction count limitation
					// SM2.0: normal scaler is not supported
					o.Normal.xy *= 2; //bumpScale;
				#endif
				o.Normal.z = sqrt(1.0 - saturate(dot(o.Normal.xy, o.Normal.xy)));
*/


				o.Translucency = trngls.r * IN.color.a * IN.color.a; // as we do not have any selfshadowing
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
					o.Specular = lerp(o.Specular, 0.02, Rainamount); 						// 0.02 = sebastien lagarde / 0.255 = unreal
				}
				o.Smoothness *= IN.myuv_MainTex.w; // specfade

//o.Albedo = _TranslucencyViewDependency;

			#endif
		}
		ENDCG
	}
}
