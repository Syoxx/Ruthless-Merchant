// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Nature/Afs Tree Creator Bark Rendertex" {
Properties {
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpSpecMap ("Normalmap (GA) Spec (R)", 2D) = "bump" {}
	_TranslucencyMap ("Trans (RGB) Gloss(A)", 2D) = "white" {}
	
	// These are here only to provide default values
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
}

SubShader {  
	Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile AFS_SH_AMBIENT AFS_GRADIENT_AMBIENT AFS_COLOR_AMBIENT
#include "UnityCG.cginc"

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 color : TEXCOORD1;
	//float2 params[3]: TEXCOORD2;
	float3 params[3]: TEXCOORD2;

	//float3 viewDir : TEXCOORD3;
	//float3 normal : TEXCOORD4;
	fixed3 vlight : TEXCOORD5;
};

CBUFFER_START(AfsTerrainImposter)
	float3 _TerrainTreeLightDirections[4];
	float4 _TerrainTreeLightColors[4];
	fixed4 _AfsSkyColor;
	fixed4 _AfsGroundColor;
	fixed4 _AfsEquatorColor;
	half4 afs_SHAr;
	half4 afs_SHAg;
	half4 afs_SHAb;
	half4 afs_SHBr;
	half4 afs_SHBg;
	half4 afs_SHBb;
	half4 afs_SHC;
CBUFFER_END

half3 AFSShadeSH9 (half4 normal)
{
	normal = normalize(normal);
	half3 x1, x2, x3;
	// Linear + constant polynomial terms
	x1.r = dot(afs_SHAr,normal);
	x1.g = dot(afs_SHAg,normal);
	x1.b = dot(afs_SHAb,normal);
	// 4 of the quadratic polynomials
	half4 vB = normal.xyzz * normal.yzzx;
	x2.r = dot(afs_SHBr,vB);
	x2.g = dot(afs_SHBg,vB);
	x2.b = dot(afs_SHBb,vB);
	// Final quadratic polynomial
	float vC = normal.x*normal.x - normal.y*normal.y;
	x3 = afs_SHC.rgb * vC;
	return x1 + x2 + x3;
}

// The Trilight Model: res = colour0 * clamp(N.L) + colour1 * (1-abs(N.L)) + colour2 * clamp(-N.L)
half3 AFSTrilight (float3 normal) {
	return (_AfsSkyColor * saturate(normal.y) + _AfsGroundColor * saturate(normal.y*(-1)) + _AfsEquatorColor * (1-abs(normal.y)) ) ; // * 0.5
}

v2f vert (appdata_full v) {
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord.xy;
	float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
	//o.viewDir = viewDir;
	//o.normal = v.normal;
	for (int j = 0; j < 3; j++)
	{
		float3 lightDir = _TerrainTreeLightDirections[j];
		half nl = dot (v.normal, lightDir);
		o.params[j].r = max (0, nl);
		half3 h = normalize (lightDir + viewDir);
		float nh = max (0, dot (v.normal, h));
		o.params[j].g = nh;
		half nv = max(0, dot(v.normal, viewDir));
		o.params[j].b = nv;
	}
	o.color = v.color.a;
	#if defined(AFS_SH_AMBIENT)
		o.vlight = AFSShadeSH9(half4(v.normal * float3(-1,-1,1), 1.0)) * 10; 
	#elif defined (AFS_GRADIENT_AMBIENT)
		o.vlight = AFSTrilight (normalize(v.normal)) * 2.0;
	#elif defined (AFS_COLOR_AMBIENT)
		o.vlight = UNITY_LIGHTMODEL_AMBIENT;
	#endif
	return o;
}

sampler2D _MainTex;
sampler2D _BumpSpecMap;
sampler2D _TranslucencyMap;
fixed4 _SpecColor;

fixed4 frag (v2f i) : SV_Target
{
	fixed3 albedo = tex2D (_MainTex, i.uv).rgb * i.color;
	half gloss = tex2D(_TranslucencyMap, i.uv).a;
	half specular = tex2D (_BumpSpecMap, i.uv).r * 128.0;
	
	half3 light = i.vlight * albedo;

	for (int j = 0; j < 3; j++)
	{
		half3 lightColor = _TerrainTreeLightColors[j].rgb;
		
		half nl = i.params[j].r;
		light += albedo * lightColor * nl;
		
		float nh = i.params[j].g;
		float spec = pow (nh, specular) * gloss * nl;
		light += lightColor * _SpecColor.rgb * spec;
	}
	
	fixed4 c;
	c.rgb = light;
	c.a = 1.0;
	return c;
}
ENDCG
	}
}

FallBack Off
}
