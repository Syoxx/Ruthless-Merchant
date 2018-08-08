// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Nature/Afs Tree Creator Leaves Rendertex" {
Properties {
	_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1) // (187,219,106,255)
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_HalfOverCutoff ("0.5 / alpha cutoff", Range(0,1)) = 1.0
	_TranslucencyViewDependency ("View dependency", Range(0,1)) = 0.7
	
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpSpecMap ("Normalmap (GA) Spec (R) Shadow Offset (B)", 2D) = "bump" {}
	_TranslucencyMap ("Trans (B) Gloss(A)", 2D) = "white" {}
}

SubShader {  
	
	Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile AFS_SH_AMBIENT AFS_GRADIENT_AMBIENT AFS_COLOR_AMBIENT
#include "UnityCG.cginc"
#include "UnityBuiltin3xTreeLibrary.cginc"

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 color : TEXCOORD1; 
	//float3 backContrib : TEXCOORD2;	// skipped
	float3 nl : TEXCOORD3;
	float3 nh : TEXCOORD4;
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
	ExpandBillboard (UNITY_MATRIX_IT_MV, v.vertex, v.normal, v.tangent);
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord.xy;
	float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));

	for (int j = 0; j < 3; j++)
	{
		float3 lightDir = _TerrainTreeLightDirections[j];
		half nl = dot (v.normal, lightDir);
		// view dependent back contribution for translucency
	//	half backContrib = saturate(dot(viewDir, -lightDir));
		// normally translucency is more like -nl, but looks better when it's view dependent
	//	backContrib = lerp(saturate(-nl), backContrib, _TranslucencyViewDependency);			// skipped
	//	o.backContrib[j] = backContrib * 2;														// skipped
		// wrap-around diffuse
		nl = max (0, nl * 0.6 + 0.4 );
		o.nl[j] = nl;
		half3 h = normalize (lightDir + viewDir);
		float nh = max (0, dot (v.normal, h));
		o.nh[j] = nh;
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
fixed _Cutoff;

fixed4 frag (v2f i) : SV_Target {
	fixed4 col = tex2D (_MainTex, i.uv);
	clip (col.a - _Cutoff);
	fixed3 albedo = col.rgb * i.color;
	// half specular = tex2D (_BumpSpecMap, i.uv).r * 128.0;	// skipped
	// fixed4 trngls = tex2D (_TranslucencyMap, i.uv);			// skipped
	// half gloss = trngls.a;
	// normal should be normalized, w=1.0
	half3 light = i.vlight * albedo;
	// half3 backContribs = i.backContrib * trngls.b;			// skipped

	for (int j = 0; j < 3; j++)
	{
		half3 lightColor = _TerrainTreeLightColors[j].rgb;

	//	We skip translucency as it does not really make any sense without real time shadows
	//	half3 translucencyColor = backContribs[j] * _TranslucencyColor / 1.25;
		half3 nl = i.nl[j];
	//	half nh = i.nh[j];
	//	We skip specular lighting here as it just ruins everything
	//	half spec = pow (nh, specular) * gloss;
	//	light += (albedo * (translucencyColor + nl) + _SpecColor.rgb * spec) * lightColor;
	
	//	light += albedo * (translucencyColor + nl) * lightColor;
		light += albedo * nl * lightColor;
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
