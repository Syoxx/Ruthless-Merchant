// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/TerrainEngine/Details/WavingDoublePass"  {
Properties {
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
}

SubShader {
	Tags {
		"Queue" = "Geometry+200"
		"IgnoreProjector"="True"
		"RenderType"="AFSGrassTerrain"
		"DisableBatching"="True"	// otherwise it might pop!
	}
	Cull Off
	LOD 200

CGPROGRAM
#pragma surface surf Lambert vertex:AfsWavingGrassVert addshadow

#include "TerrainEngine.cginc"
#include "../Includes/AfsWavingGrass.cginc"

#pragma shader_feature _AFS_GRASS_APPROXTRANS

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
	float waveAmount = v.color.a * _AfsWaveAndDistance.z;
	v.color = WaveGrassTerrain (v.vertex, v.normal, waveAmount, v.color, v.texcoord1);

	#if defined (_AFS_GRASS_APPROXTRANS)
		//	Calculate per vertex translucency according to _AfsDirectSunDir
		float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
		half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));
		// o.translucency = saturate(dot(worldViewDir, _AfsDirectSunDir.xyz)) * _AfsDirectSunDir.w;
		// Take viewdir and slope into account to to handle low-lying sun conditions
		o.translucency = saturate(dot(worldViewDir, _AfsDirectSunDir.xyz)) * _AfsDirectSunDir.w * saturate (dot(-v.normal, _AfsDirectSunDir.xyz));
	#endif
}

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Alpha = c.a * IN.color.a;
	// Do early alpha test
	clip (o.Alpha - _Cutoff);

	o.Albedo = c.rgb * IN.color.rgb;
	#if defined (_AFS_GRASS_APPROXTRANS)
	//	o.Albedo += lerp(o.Albedo, _AfsTranslucencyColor, o.Albedo.g) * IN.translucency;
		o.Albedo += tex2D(_TerrianBumpTransSpecMap, IN.uv_MainTex).rgb * IN.translucency * IN.color.rgb;
	#endif

//	o.Albedo = IN.translucency;
}
ENDCG
}
	Fallback Off
}