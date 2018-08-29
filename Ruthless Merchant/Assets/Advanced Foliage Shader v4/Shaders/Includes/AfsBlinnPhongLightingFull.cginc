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
	//half Attenuation;
	half ShadowCutOff;
};

// NOTE: some intricacy in shader compiler on some GLES2.0 platforms (iOS) needs 'viewDir' & 'h'
// to be mediump instead of lowp, otherwise specular highlight becomes too bright.
inline fixed4 AFSBlinnPhongLight (BarkSurfaceOutput s, half3 viewDir, UnityLight light)
{
	half3 h = normalize (light.dir + viewDir);
	fixed nl = max (0, dot (s.Normal, light.dir));
	float nh = saturate (dot (s.Normal, h));
	float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;
	c.rgb = s.Albedo * light.color * nl + light.color * _SpecColor.rgb * spec * nl;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingAFSBlinnPhong (BarkSurfaceOutput s, half3 viewDir, UnityGI gi)
{
	fixed4 c;
	c = AFSBlinnPhongLight (s, viewDir, gi.light);

	#if defined(DIRLIGHTMAP_SEPARATE)
		#ifdef LIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light2);
		#endif
		#ifdef DYNAMICLIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light3);
		#endif
	#endif

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		c.rgb += s.Albedo * gi.indirect.diffuse;
	#endif

	return c;
}

inline half4 LightingAFSBlinnPhong_Deferred (BarkSurfaceOutput s, half3 viewDir, UnityGI gi, out half4 outDiffuseOcclusion, out half4 outSpecSmoothness, out half4 outNormal)
{
	outDiffuseOcclusion = half4(s.Albedo, 1);
	// original version but pretty strange mapping i think...
	// outSpecSmoothness = half4(_SpecColor.rgb, s.Specular);
	outSpecSmoothness = half4(s.Specular.xxx,s.Gloss);
	outNormal = half4(s.Normal * 0.5 + 0.5, 1);
	half4 emission = half4(s.Emission, 1);

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		emission.rgb += s.Albedo * gi.indirect.diffuse;
	#endif
	
	return emission;
}

inline void LightingAFSBlinnPhong_GI (
	inout BarkSurfaceOutput s,
	UnityGIInput data,
	inout UnityGI gi
	)
{
	gi = UnityGlobalIllumination (data, 1.0, s.Gloss, s.Normal, false);
}

inline fixed4 LightingAFSBlinnPhong_PrePass (BarkSurfaceOutput s, half4 light)
{
	fixed spec = light.a * s.Gloss;
	fixed4 c;
	c.rgb = (s.Albedo * light.rgb + light.rgb * _SpecColor.rgb * spec);
	c.a = s.Alpha;
	return c;
}



#endif