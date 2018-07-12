#ifndef AFS_BP_LIGHTING_INCLUDED
#define AFS_BP_LIGHTING_INCLUDED

#include "UnityLightingCommon.cginc"
#include "UnityGlobalIllumination.cginc"

struct BarkSurfaceOutput {
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half Specular;
	fixed Gloss;
	fixed Alpha;
	half ShadowCutOff;
};

// NOTE: some intricacy in shader compiler on some GLES2.0 platforms (iOS) needs 'viewDir' & 'h'
// to be mediump instead of lowp, otherwise specular highlight becomes too bright.
inline fixed4 LightingAFSBlinnPhong (BarkSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
{
	half3 h = normalize (lightDir + viewDir);
	fixed nl = max (0, dot (s.Normal, lightDir));
	float nh = saturate(dot (s.Normal, h));
	float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * nl + _LightColor0.rgb * _SpecColor.rgb * spec * nl;
	
	#if defined(DIRECTIONAL) || defined(DIRECTIONAL_COOKIE)
		c.rgb *=  lerp(fixed(1), atten, s.ShadowCutOff);
	#else
		c.rgb *= lerp(fixed(0), atten, s.ShadowCutOff);
	#endif

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		half3 sh = ShadeSH12Order(half4(s.Normal, 1.0));
		c.rgb += s.Albedo * sh;
	#endif

	c.a = s.Alpha;
	return c;
}

#endif