Shader "VacuumShaders/DirectX 11 Low Poly/Skybox" 
{
	Properties 
	{
		[VacuumShadersSkyboxType] _Skybox_Type("", Float) = 0

		_Tint ("Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
		_Rotation ("Rotation", Range(0, 360)) = 0
		_Blur("Roughness", Range(0, 1)) = 0
		[NoScaleOffset] _Tex ("Cubemap   (HDR)", Cube) = "grey" {}		

		//PaperCraft
		[VacuumShadersLabel] _PAPERCRAFT_LABEL("Wireframe", float) = 0
		[VacuumShadersPaperCraft] _PaperCrat("", float) = 0
		[HideInInspector] _V_WIRE_FixedSize("Fixed Size", float) = 0
		[HideInInspector] _V_WIRE_Size("Wire Size", Float) = 1
		[HideInInspector] _V_WIRE_Color("Wire Color", color) = (0, 0, 0, 1)

		[VacuumShadersLabel] _UNITY_ARO("Unity Advanced Rendering Options", float) = 0
	}

SubShader 
{
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" "PaperCraft"="Off" }
	Cull Off ZWrite Off

	Pass {
		
		CGPROGRAM
		#pragma vertex vert
		#pragma geometry geom
		#pragma fragment frag
		#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"

		#include "UnityCG.cginc"

		samplerCUBE _Tex;
		half4 _Tex_HDR;
		half4 _Tint;
		half _Exposure;
		float _Rotation;
		half _Blur;

		float4 RotateAroundYInDegrees (float4 vertex, float degrees)
		{
			float alpha = degrees * UNITY_PI / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			return float4(mul(m, vertex.xz), vertex.yw).xzyw;
		}
		
		struct appdata_t 
		{
			float4 vertex : POSITION;
		};

		struct v2f_surf 
		{
			float4 pos : SV_POSITION;
			fixed4 color : COLOR0;
			float3 worldPos : TEXCOORD0;
			float3 mass : TEXCOORD1;
		};


#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


    	v2f_surf vert (appdata_t v)
		{
			v2f_surf o;
			o.pos = UnityObjectToClipPos(RotateAroundYInDegrees(v.vertex, _Rotation));


			float4 rotPosition = RotateAroundYInDegrees(v.vertex, _Rotation);
			o.pos = UnityObjectToClipPos(rotPosition);
			
			o.worldPos = mul(unity_ObjectToWorld, rotPosition).xyz;

			o.color = texCUBElod (_Tex, float4(v.vertex.xyz, _Blur * 10));
			o.color.rgb = DecodeHDR (o.color, _Tex_HDR) * _Tint.rgb * unity_ColorSpaceDouble.rgb * _Exposure;
			o.color.a = 1;

			o.mass = 0;

			return o; 
		}

		fixed4 frag (v2f_surf i) : SV_Target
		{
			//PaperCraft
//			MakePaperCraft(i.mass, i.worldPos, i.color);

			return i.color;
		}
		ENDCG 
	}
} 	


Fallback Off

}
