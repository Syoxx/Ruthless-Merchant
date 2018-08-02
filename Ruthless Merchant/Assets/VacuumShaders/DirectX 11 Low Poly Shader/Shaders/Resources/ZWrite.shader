Shader "Hidden/VacuumShaders/DirectX 11 Low Poly/ZWrite" 
{
	Properties   
	{
		[VacuumShadersShaderType] _SHADER_TYPE_LABEL("", float) = 0
		[VacuumShadersRenderingMode] _RENDERING_MODE_LABEL("", float) = 0

		[VacuumShadersLabel] _VERTEX_LABEL("Low Poly", float) = 0
		[Enum(Triangle,0,Quad,1)] _SamplingType("Sampling Type", Float) = 0
		_MainTex ("Texture #1", 2D) = "white" {}
		[VacuumShadersUVScroll] _MainTex_Scroll("    ", vector) = (0, 0, 0, 0)		
		  
		[VacuumShadersSecondVertexTexture] _SecondTextureID("", Float) = 0
		[HideInInspector] _SecondTex_BlendType("", Float) = 0
		[HideInInspector] _SecondTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _SecondTex ("", 2D) = "white" {}
		[HideInInspector] _SecondTex_Scroll("", vector) = (0, 0, 0, 0)  
		 
		[VacuumShadersLabel] _PIXEL_LABEL("Fragment", float) = 0
		_Color ("Tint Color", Color) = (1,1,1,1)	
		[VacuumShadersToggleSimple] _VertexColor("Mesh Vertex Color", Float) = 0

		[VacuumShadersPixelTexture] _PixelTextureID("", Float) = 0
		[HideInInspector] _PixelTex_BlendType("Blend Type", Float) = 0
		[HideInInspector] _PixelTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _PixelTex ("  Texture", 2D) = "white" {}
		[HideInInspector] _PixelTex_Scroll("    ", vector) = (0, 0, 0, 0)

		[VacuumShadersToggleEffect] _SPECULAR_LABEL("Specular", float) = 0
		[VacuumShadersToggleEffect] _BUMP_LABEL("Bump", float) = 0	
		[VacuumShadersReflection] _REFLECTION_LABEL("Reflective", float) = 0	
		
		[VacuumShadersLabel] _Dsiplace_LABEL("Displace", float) = 0	
		[VacuumShadersDisplaceType] _DisplaceType("", Float) = 0
		[HideInInspector] _DisplaceTex_1 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_1_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _DisplaceTex_2 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_2_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _DisplaceBlendType ("Blend Type", Float) = 1
		[HideInInspector] _DisplaceStrength ("", float) = 1		

		[HideInInspector] _DisplaceDirection("", Range(0, 360)) = 45
		[HideInInspector] _DisplaceScriptSynchronize("", Float) = 0
		[HideInInspector] _DisplaceSpeed("", Float) = 1
		[HideInInspector] _DisplaceAmplitude ("", Float) = 0.5
		[HideInInspector] _DisplaceFrequency("", Float) = 0.2
		[HideInInspector] _DisplaceNoiseCoef("", Float) = -0.5	

		[VacuumShadersLabel] _UNITY_ARO("Unity Advanced Rendering Options", float) = 0
	}

	SubShader 
	{	
		Pass
		{	
			Name "Main"
			Tags{ "QUEUE" = "Transparent" "IGNOREPROJECTOR" = "true" "RenderType" = "Transparent" }
			ColorMask 0

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
			#pragma target 4.0
			#include "UnityCG.cginc"
			#pragma multi_compile_fog
			#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))
					
			#define UNITY_PASS_ZWRITE

			// vertex shader input data
			struct appdata 
			{
				float4 vertex : POSITION; 
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				half4 color : COLOR;
			};

			// vertex-to-fragment interpolators
			struct v2f_surf 
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
				#if USING_FOG
					fixed fog : TEXCOORD0;
				#endif			
			}; 


#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"

			// vertex shader
			v2f_surf vert(appdata v)
			{
				SET_UP_LOW_POLY_DATA(v)

				half4 color = v.color;
				float3 eyePos = UnityObjectToViewPos(float4(v.vertex.xyz,1)).xyz;
				o.color = saturate(color);
			
				// fog
				#if USING_FOG
					float fogCoord = length(eyePos.xyz); // radial fog distance
					UNITY_CALC_FOG_FACTOR(fogCoord);
					o.fog = saturate(unityFogFactor); 
				#endif

				return o;
			}

			// fragment shader
			fixed4 frag(v2f_surf IN) : SV_Target
			{
				fixed4 col;
				col = IN.color;

				// fog
				#if USING_FOG
					col.rgb = lerp(unity_FogColor.rgb, col.rgb, IN.fog);
				#endif

				return col;
			}

			ENDCG
		}
		
	}
}