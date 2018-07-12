// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "AFS/Grass Shader v4" {
Properties {
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
}

SubShader {
	Tags {
//		"Queue" = "Geometry+200"
		"Queue"="AlphaTest" // needed for lightmapper
		"IgnoreProjector"="True"
		"RenderType"="AFSGrass"
		"AfsMode"="Grass"
	}
	Cull Off
	LOD 200

CGPROGRAM
#pragma surface surf Lambert vertex:AfsWavingGrassVert addshadow

#include "TerrainEngine.cginc"
#include "Includes/AfsWavingGrass.cginc"

#pragma shader_feature _AFS_GRASS_APPROXTRANS
#pragma multi_compile IN_EDITMODE IN_PLAYMODE

sampler2D _MainTex;
float _Cutoff;

#if defined (_AFS_GRASS_APPROXTRANS)
	float4 _AfsDirectSunDir;
	fixed3 _AfsDirectSunCol;
	//fixed3 _AfsTranslucencyColor;
	sampler2D _TerrianBumpTransSpecMap;
#endif

struct Input {
	float2 uv_MainTex;
	float4 color : COLOR;
	float3 normal;
	float translucency;
};

void AfsWavingGrassVert (inout appdata_full v, out Input o) 
{
	UNITY_INITIALIZE_OUTPUT(Input,o);
	#ifdef IN_EDITMODE
		v.normal = UnityObjectToWorldNormal (float3(0,1,0));
	#endif
	float waveAmount = v.color.a * _AfsWaveAndDistance.z;
	v.color = WaveGrassMesh (v.vertex, v.normal, waveAmount, v.color);

	#if !defined(UNITY_PASS_SHADOWCASTER)

		#ifdef IN_EDITMODE
			v.color.rgb = half3(1,1,1);
		#endif

		#if defined (_AFS_GRASS_APPROXTRANS)
			//	Calculate per vertex translucency according to _AfsDirectSunDir
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));
			o.translucency = saturate(dot(worldViewDir, _AfsDirectSunDir.xyz)) * _AfsDirectSunDir.w;
		#endif
	#endif
}

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Alpha = c.a * IN.color.a;
	// Do early alpha test
	clip (o.Alpha - _Cutoff);

	#if !defined(UNITY_PASS_SHADOWCASTER)
		o.Albedo = c.rgb * IN.color.rgb;	
		#if defined (_AFS_GRASS_APPROXTRANS)
			//	o.Albedo += lerp(o.Albedo, _AfsTranslucencyColor, o.Albedo.g) * IN.translucency;
			o.Albedo += tex2D(_TerrianBumpTransSpecMap, IN.uv_MainTex).rgb * IN.translucency * IN.color.rgb;
		#endif
	#endif
}
ENDCG
}
	Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
}