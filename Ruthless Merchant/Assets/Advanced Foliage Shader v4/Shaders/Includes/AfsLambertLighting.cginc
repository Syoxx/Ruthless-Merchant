#ifndef AFS_LAMBERT_LIGHTING_INCLUDED
#define AFS_LAMBERT_LIGHTING_INCLUDED

#include "UnityLightingCommon.cginc"
#include "UnityGlobalIllumination.cginc"

struct SurfaceOutputLambert {
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half Specular;
	fixed Gloss;
	fixed Alpha;
};

inline fixed4 UnityAFSLambertLight (SurfaceOutputLambert s, UnityLight light)
{
	fixed diff = max (0, dot (s.Normal, light.dir));
	fixed4 c;
	c.rgb = s.Albedo * light.color * diff;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingAFSLambert (SurfaceOutputLambert s, UnityGI gi)
{
	fixed4 c;
	c = UnityAFSLambertLight (s, gi.light);

	#if defined(DIRLIGHTMAP_SEPARATE)
		#ifdef LIGHTMAP_ON
			c += UnityAFSLambertLight (s, gi.light2);
		#endif
		#ifdef DYNAMICLIGHTMAP_ON
			c += UnityAFSLambertLight (s, gi.light3);
		#endif
	#endif

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		c.rgb += s.Albedo * gi.indirect.diffuse;
	#endif

	return c;
}

// Only Deferred has to be tweaked

inline half4 LightingAFSLambert_Deferred (SurfaceOutputLambert s, UnityGI gi, out half4 outDiffuseOcclusion, out half4 outSpecSmoothness, out half4 outNormal)
{
	outDiffuseOcclusion = half4(s.Albedo, 1);
	outSpecSmoothness = 0.0;
	outNormal = half4(s.Normal * 0.5 + 0.5, 1);
	half4 emission = half4(s.Emission, 1);

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		emission.rgb += s.Albedo * gi.indirect.diffuse;
	#endif

	return emission;
}

inline void LightingAFSLambert_GI (
	SurfaceOutputLambert s,
	UnityGIInput data,
	inout UnityGI gi)
{
	gi = UnityGlobalIllumination (data, 1.0, 0.0, s.Normal, false);
}

inline fixed4 LightingAFSLambert_PrePass (SurfaceOutputLambert s, half4 light)
{
	fixed4 c;
	c.rgb = s.Albedo * light.rgb;
	c.a = s.Alpha;
	return c;
}


#endif