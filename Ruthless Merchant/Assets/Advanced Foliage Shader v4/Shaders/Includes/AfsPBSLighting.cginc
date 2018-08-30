#ifndef AFS_PBS_LIGHTING_INCLUDED
#define AFS_PBS_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"

//-------------------------------------------------------------------------------------

fixed3 _TranslucencyColor;
fixed _TranslucencyViewDependency;

#if defined(LIGHTMAP_ON)
	float4 _AfsDirectSunDir;
	fixed3 _AfsDirectSunCol;
#endif

//-------------------------------------------------------------------------------------
// Surface shader output structure to be used with physically
// based shading model.
struct SurfaceOutputAFSSpecular
{
	fixed3 Albedo;		// diffuse color
	fixed3 Specular;	// specular color
	fixed3 Normal;		// tangent space normal, if written
	half3 Emission;
	half Smoothness;	// 0=rough, 1=smooth
	half Occlusion;
	fixed Alpha;
	fixed Translucency;
};


//-------------------------------------------------------------------------------------
//	This is more or less like the "LightingStandardSpecular" but with translucency added on top ond wrapped around diffuse
//	So forward only...

inline half4 LightingAFSSpecular (SurfaceOutputAFSSpecular s, half3 viewDir, UnityGI gi)
{
	s.Normal = normalize(s.Normal);

	// energy conservation
	half oneMinusReflectivity;
	s.Albedo = EnergyConservationBetweenDiffuseAndSpecular (s.Albedo, s.Specular, /*out*/ oneMinusReflectivity);

	// shader relies on pre-multiply alpha-blend (_SrcBlend = One, _DstBlend = OneMinusSrcAlpha)
	// this is necessary to handle transparency in physically correct way - only diffuse component gets affected by alpha
	half outputAlpha;
	s.Albedo = PreMultiplyAlpha (s.Albedo, s.Alpha, oneMinusReflectivity, /*out*/ outputAlpha);

//	Add ambient occlusion – as we can’t calculate it in "gi = UnityStandardGlobalIllumination" for some unkown reasons (see below in: "LightingAFSSpecular_GI")
	gi.indirect.diffuse *= s.Occlusion;
	gi.indirect.specular *= s.Occlusion;
//	Add wrapped around diffuse lighting --> that also darkens backfaces and works against translucency...
	gi.light.ndotl = saturate (dot(s.Normal, gi.light.dir) * 0.6 + 0.4);

	half4 c = UNITY_BRDF_PBS (s.Albedo, s.Specular, oneMinusReflectivity, s.Smoothness, s.Normal, viewDir, gi.light, gi.indirect);
	c.rgb += UNITY_BRDF_GI (s.Albedo, s.Specular, oneMinusReflectivity, s.Smoothness, s.Normal, viewDir, s.Occlusion, gi);
	c.a = outputAlpha;

//	Add Translucency – needs light dir and intensity: so real time only
	#if !defined(LIGHTMAP_ON)
		// view dependent back contribution for translucency
		fixed backContrib = saturate(dot(viewDir, -gi.light.dir));
		// normally translucency is more like -gi.light.ndotl, but looks better when it's view dependent
		backContrib = lerp( saturate( -dot(s.Normal, gi.light.dir) ), backContrib, _TranslucencyViewDependency);
		c.rgb += gi.light.color * backContrib * _TranslucencyColor * s.Translucency;

//	Theoretically we could approximate translucency here too – but it looks pretty weird as shadows are simply missing
//	#else
//		float MinusDotNL = saturate (dot(s.Normal, _AfsDirectSunDir.xyz ));
		// view dependent back contribution for translucency
//		fixed backContrib = saturate(dot(viewDir, _AfsDirectSunDir.xyz));
		// normally translucency is more like 1-gi.light.ndotl, but looks better when it's view dependent
//		backContrib = lerp(MinusDotNL, backContrib, _TranslucencyViewDependency);
//		c.rgb += backContrib * _AfsDirectSunDir.w * _TranslucencyColor * s.Translucency * 0.5; //
	#endif

	return c;
}

inline void LightingAFSSpecular_GI (
	SurfaceOutputAFSSpecular s,
	UnityGIInput data,
	inout UnityGI gi)
{
//	This would be the place to add Groundatten / first pass only //data.atten *= 0.5;
	//	We have to apply occlusion ourself (!? – otherwise it all gets black, see above in: "LightingAFSSpecular")
	//	gi = UnityGlobalIllumination (data, s.Occlusion, s.Smoothness, s.Normal);
	gi = UnityGlobalIllumination (data, 1.0, s.Smoothness, s.Normal);
}


#endif // AFS_PBS_LIGHTING_INCLUDED
