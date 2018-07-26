#ifndef VACUUM_SHADERS_DIRECTX_11_LOWPOLY_VARIABLES_CGINC
#define VACUUM_SHADERS_DIRECTX_11_LOWPOLY_VARIABLES_CGINC


float _SamplingType;
float _SmoothLighting;

fixed4 _Color;
float _VertexColor;

sampler2D _MainTex;
float4 _MainTex_ST;
float2 _MainTex_Scroll;

#ifdef V_LP_PBS
	half _Glossiness;
	half _Metallic;
#endif

#ifdef V_LP_SECOND_TEXTURE_ON
	float _SecondTex_BlendType; //0 - Detail, 1 - Decal

	sampler2D _SecondTex;
	float4 _SecondTex_ST;
	float2 _SecondTex_Scroll;

	float _SecondTex_AlphaOffset;
#endif


#ifdef V_LP_PIXEL_TEXTURE_ON
	float _PixelTex_BlendType; //0 - Detail, 1 - Decal

	sampler2D _PixelTex;
	float4 _PixelTex_ST;
	float2 _PixelTex_Scroll;

	float _PixelTex_AlphaOffset;
#endif

#ifdef V_LP_BUMP
	sampler2D _BumpTex;
	float4 _BumpTex_ST;
	float2 _BumpTex_Scroll;

	half _BumpStrength;
#endif

#ifdef V_LP_SPECULAR
	half _Shininess;
	half _GlossOffset;
#endif

#if defined(V_LP_REFLECTIVE_CUBE_MAP) || defined(V_LP_REFLECTIVE_PROBE) || defined(V_LP_REFLECTIVE_REALTIME)
	fixed4 _ReflectColor;
	fixed _ReflectionRoughness;
	fixed _ReflectionFresnel;
	fixed _ReflectionStrengthOffset;
	fixed _ReflectionDistortion;

	#ifdef V_LP_REFLECTIVE_REALTIME
		sampler2D _ReflectionTex;
	#else
		float _CubeIsHDR;
		UNITY_DECLARE_TEXCUBE(_Cube);
		float4 _Cube_HDR;
	#endif

#endif

float _EMISSION_TOGGLE;
sampler2D _EmissionTex;
float4 _EmissionTex_ST;
float2 _EmissionTex_Scroll;
fixed4 _EmissionColor;
float _EmissionStrength;

float _AlphaFromVertex;
#if defined(V_LP_CUTOUT)
	fixed _Cutoff;
#endif

#ifdef V_LP_DISPLACE_PARAMETRIC
	float _DisplaceDirection;
	float _DisplaceSpeed;
	float _DisplaceAmplitude;
	float _DisplaceFrequency;
	float _DisplaceNoiseCoef;

	float _DisplaceScriptSynchronize;
#endif

#ifdef V_LP_DISPLACE_TEXTURE
	sampler2D _DisplaceTex_1;
	float4 _DisplaceTex_1_ST;
	float2 _DisplaceTex_1_Scroll;

	sampler2D _DisplaceTex_2;
	float4 _DisplaceTex_2_ST;
	float2 _DisplaceTex_2_Scroll;

	float _DisplaceBlendType;
	float _DisplaceStrength;
#endif

uniform float _V_LP_Time;

#endif
