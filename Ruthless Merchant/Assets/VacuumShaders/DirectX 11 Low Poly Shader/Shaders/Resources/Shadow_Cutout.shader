Shader "Hidden/VacuumShaders/DirectX 11 Low Poly/Shadow/Cutout" 
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

		[VacuumShadersLargeLabel] _ALPHA_CUTOFF_LABEL("Alpha Cutoff", float) = 0
		[VacuumShadersToggleSimple] _AlphaFromVertex("  Alpha From Vertex", Float) = 0		
		_Cutoff ("    Cutoff", Range(0,1)) = 0.5

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
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 100
	
	
	
		// Pass to render object as a shadow caster
		Pass 
		{
			Name "Caster"
			Tags { "LightMode" = "ShadowCaster" }
		
			CGPROGRAM 
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
			#pragma multi_compile_shadowcaster
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
				
			struct appdata_shadow 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_surf 
			{ 
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float2 pixelTexUV : TEXCOORD2;
				fixed4 color : TEXCOORD3;

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};			


			#pragma shader_feature _ V_LP_SECOND_TEXTURE_ON
			#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
			#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
			#define V_LP_CUTOUT
			#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
			#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"
			
			 
			v2f_surf vert( appdata_shadow v )
			{		
				SET_UP_LOW_POLY_DATA(v)

				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

				return o;
			}

			float4 frag( v2f_surf IN ) : SV_Target
			{			
				clip(GetLowpolyPixelColor(IN.pixelTexUV, IN.color).a);

				SHADOW_CASTER_FRAGMENT(IN)
			}
			ENDCG

		}	
	}
}
