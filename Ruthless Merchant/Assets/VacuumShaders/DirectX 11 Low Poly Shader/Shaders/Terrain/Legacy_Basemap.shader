Shader "Hidden/VacuumShaders/DirectX 11 Low Poly/Nature/Basemap"
{
	Properties
	{
		[VacuumShadersShaderType] _SHADER_TYPE_LABEL("", float) = 0
		[VacuumShadersRenderingMode] _RENDERING_MODE_LABEL("", float) = 0

		[VacuumShadersLabel] _VERTEX_LABEL("Low Poly", float) = 0
		[Enum(Triangle,0,Quad,1)] _SamplingType("Sampling Type", Float) = 0
		[HideInInspector]_MainTex("Texture #1", 2D) = "white" {}

		[VacuumShadersLabel] _PIXEL_LABEL("Fragment", float) = 0
		_Color("Tint Color", Color) = (1,1,1,1)

		[VacuumShadersPixelTexture] _PixelTextureID("", Float) = 0
		[HideInInspector] _PixelTex_BlendType("Blend Type", Float) = 0
		[HideInInspector] _PixelTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _PixelTex("  Texture", 2D) = "white" {}
		[HideInInspector] _PixelTex_Scroll("    ", vector) = (0, 0, 0, 0)

		[VacuumShadersToggleEffect] _SPECULAR_LABEL("Specular", float) = 0
		[VacuumShadersToggleEffect] _BUMP_LABEL("Bump", float) = 0
		[VacuumShadersReflection] _REFLECTION_LABEL("Reflective", float) = 0


		[VacuumShadersLabel] _FORWARD_RENDERING_LABEL("Forward Rendering Options", float) = 0
		[VacuumShadersLowPolyLight] _LowPolyLightID("", Float) = 0

		//PaperCraft
		[VacuumShadersLabel] _PAPERCRAFT_LABEL("Wireframe", float) = 0
		[VacuumShadersPaperCraft] _PaperCrat("", float) = 0
		[HideInInspector] _V_WIRE_FixedSize("Fixed Size", float) = 0
		[HideInInspector] _V_WIRE_Size("Wire Size", Float) = 1
		[HideInInspector] _V_WIRE_Color("Wire Color", color) = (0, 0, 0, 1)
		[HideInInspector] _V_WIRE_TryQuad("Try Quad", Float) = 0

		[VacuumShadersLabel] _UNITY_ARO("Unity Advanced Rendering Options", float) = 0
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "PaperCraft"="Off" }
		LOD 200

		// ---- forward rendering base pass:
		Pass{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_fwdbase
#include "HLSLSupport.cginc" 
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_FORWARDBASE
#include "UnityCG.cginc" 
#include "Lighting.cginc"
#include "AutoLight.cginc"





		// vertex-to-fragment interpolation data
		// no lightmaps:
#ifdef LIGHTMAP_OFF
		struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
#if UNITY_SHOULD_SAMPLE_SH
#define V_GEOMETRY_SAVE_SPHERICAL_HARMONICS

		half3 sh : TEXCOORD3; // SH
#endif
		UNITY_SHADOW_COORDS(4)
			UNITY_FOG_COORDS(5)
			float4 lmap : TEXCOORD6;

		float3 mass : TEXCOORD7;
		float3 objectPos : TEXCOORD8;

	};
#endif
	// with lightmaps:
#ifndef LIGHTMAP_OFF
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;

		float4 lmap : TEXCOORD3;
		UNITY_SHADOW_COORDS(4)
			UNITY_FOG_COORDS(5)
			float3 mass : TEXCOORD6;
		float3 objectPos : TEXCOORD7;
	};
#endif    


#pragma shader_feature _ V_LP_LIGHT_ON
#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#ifdef V_LP_LIGHT_ON
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#endif
//	#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader 
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
#if !defined(LIGHTMAP_OFF) && defined(DIRLIGHTMAP_COMBINED)
			fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
			fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
			fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
#endif

			o.worldPos = worldPos;
			o.worldNormal = worldNormal;

#ifndef DYNAMICLIGHTMAP_OFF 
			o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
#ifndef LIGHTMAP_OFF
			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif     

			// SH/ambient and vertex lights
#ifdef LIGHTMAP_OFF
#if UNITY_SHOULD_SAMPLE_SH
			o.sh = 0;
			// Approximated illumination from non-important point lights
#ifdef VERTEXLIGHT_ON
			o.sh += Shade4PointLights(
				unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
				unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
				unity_4LightAtten0, worldPos, worldNormal);
#endif
			o.sh = ShadeSHPerVertex(worldNormal, o.sh);
#endif
#endif // LIGHTMAP_OFF

			TRANSFER_SHADOW(o); // pass shadow coordinates to pixel shader
			UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			return o;
	}


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data
		float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
		fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
		fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
#ifdef UNITY_COMPILER_HLSL
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
		o.Albedo = 0.0;
		o.Emission = 0.0;
		o.Specular = 0.0;
		o.Alpha = 0.0;
		o.Gloss = 0.0;
		fixed3 normalWorldVertex = fixed3(0,0,1);
		o.Normal = IN.worldNormal;
		normalWorldVertex = IN.worldNormal;


		//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		fixed4 lowpolyColor = IN.color;

		//PaperCraft
//		MakePaperCraft(IN.mass, IN.worldPos, lowpolyColor);

		//Albedo & Alpha
		o.Albedo = lowpolyColor.rgb;
		o.Alpha = lowpolyColor.a;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		// compute lighting & shadowing factor
		UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
			fixed4 c = 0;

		// Setup lighting environment
		UnityGI gi;
		UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
		gi.indirect.diffuse = 0;
		gi.indirect.specular = 0;
#if !defined(LIGHTMAP_ON)
		gi.light.color = _LightColor0.rgb;
		gi.light.dir = lightDir;
		gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);
#endif
		// Call GI (lightmaps/SH/reflections) lighting function
		UnityGIInput giInput;
		UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
		giInput.light = gi.light;
		giInput.worldPos = worldPos;
		giInput.atten = atten;
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
		giInput.lightmapUV = IN.lmap;
#else
		giInput.lightmapUV = 0.0;
#endif

giInput.ambient.rgb = 0.0;
#ifdef LIGHTMAP_OFF
	#if UNITY_SHOULD_SAMPLE_SH
			giInput.ambient = IN.sh;		
	#endif
#endif


		giInput.probeHDR[0] = unity_SpecCube0_HDR;
		giInput.probeHDR[1] = unity_SpecCube1_HDR;
#if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
		giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
#endif
#if UNITY_SPECCUBE_BOX_PROJECTION
		giInput.boxMax[0] = unity_SpecCube0_BoxMax;
		giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
		giInput.boxMax[1] = unity_SpecCube1_BoxMax;
		giInput.boxMin[1] = unity_SpecCube1_BoxMin;
		giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
#endif
		LightingLambert_GI(o, giInput, gi);

		// realtime lighting: call lighting function
		c += LightingLambert(o, gi);
		UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
		UNITY_OPAQUE_ALPHA(c.a);
		return c;
	}

		ENDCG

	}

		// ---- forward rendering additive lights pass:
		Pass{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardAdd" }
		ZWrite Off Blend One One

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf 
#pragma geometry geom 
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_fwdadd
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_FORWARDADD
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"





		// vertex-to-fragment interpolation data 
		struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
		UNITY_SHADOW_COORDS(3)
			UNITY_FOG_COORDS(4)
			float3 mass : TEXCOORD5;
		float3 objectPos : TEXCOORD6;
	};

#pragma shader_feature _ V_LP_LIGHT_ON
#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#ifdef V_LP_LIGHT_ON
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#endif
//		#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
			o.worldPos = worldPos;
			o.worldNormal = worldNormal;

			TRANSFER_SHADOW(o); // pass shadow coordinates to pixel shader
			UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			return o;
	}


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data
		float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
		fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
		fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
#ifdef UNITY_COMPILER_HLSL
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
		o.Albedo = 0.0;
		o.Emission = 0.0;
		o.Specular = 0.0;
		o.Alpha = 0.0;
		o.Gloss = 0.0;
		fixed3 normalWorldVertex = fixed3(0,0,1);
		o.Normal = IN.worldNormal;
		normalWorldVertex = IN.worldNormal;


		//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		fixed4 lowpolyColor = IN.color;

		//PaperCraft
//		MakePaperCraft(IN.mass, IN.worldPos, lowpolyColor);

		//Albedo & Alpha
		o.Albedo = lowpolyColor.rgb;
		o.Alpha = lowpolyColor.a;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
			fixed4 c = 0;

		// Setup lighting environment
		UnityGI gi;
		UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
		gi.indirect.diffuse = 0;
		gi.indirect.specular = 0;
#if !defined(LIGHTMAP_ON)
		gi.light.color = _LightColor0.rgb;
		gi.light.dir = lightDir;
		gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);
#endif
		gi.light.color *= atten;
		c += LightingLambert(o, gi);
		c.a = 0.0;
		UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
		UNITY_OPAQUE_ALPHA(c.a);
		return c;
	}

		ENDCG

	}

		// ---- deferred lighting base geometry pass:
		Pass{
		Name "PREPASS"
		Tags{ "LightMode" = "PrePassBase" }

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_PREPASSBASE
#include "UnityCG.cginc"
#include "Lighting.cginc"






		// vertex-to-fragment interpolation data
		struct v2f_surf {
		float4 pos : SV_POSITION;
		half3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;

	};


#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
			o.worldPos = worldPos;
			o.worldNormal = worldNormal;
			return o;
	}


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data
#ifdef UNITY_COMPILER_HLSL
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
	o.Albedo = 1.0;
	o.Emission = 0.0;
	o.Specular = 0.0;
	o.Alpha = 1.0;
	o.Gloss = 0.0;
	fixed3 normalWorldVertex = fixed3(0,0,1);
	o.Normal = IN.worldNormal;
	normalWorldVertex = IN.worldNormal;


	//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Empty   
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	// output normal and specular
	fixed4 res;
	res.rgb = o.Normal * 0.5 + 0.5;
	res.a = o.Specular;
	return res;
	}

		ENDCG

	}

		// ---- deferred lighting final pass:
		Pass{
		Name "PREPASS"
		Tags{ "LightMode" = "PrePassFinal" }
		ZWrite Off

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_prepassfinal
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_PREPASSFINAL
#include "UnityCG.cginc"
#include "Lighting.cginc"





		// vertex-to-fragment interpolation data
		struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		fixed4 color : COLOR0;
		float4 screen : TEXCOORD2;
		float4 lmap : TEXCOORD3;
#ifdef LIGHTMAP_OFF
		float3 vlight : TEXCOORD4;
#else
#ifdef DIRLIGHTMAP_OFF
		float4 lmapFadePos : TEXCOORD4;
#endif
#endif
		UNITY_FOG_COORDS(5)
			float3 mass : TEXCOORD6;
		float3 objectPos : TEXCOORD7;
	};


#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
//		#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

			o.worldPos = worldPos;

			o.screen = ComputeScreenPos(o.pos);
#ifndef DYNAMICLIGHTMAP_OFF
			o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#else
			o.lmap.zw = 0;
#endif
#ifndef LIGHTMAP_OFF
			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#ifdef DIRLIGHTMAP_OFF
			o.lmapFadePos.xyz = (mul(unity_ObjectToWorld, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
			o.lmapFadePos.w = (-UnityObjectToViewPos(v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
#endif
#else
			o.lmap.xy = 0;
			float3 worldN = UnityObjectToWorldNormal(v.normal);
			o.vlight = ShadeSH9(float4(worldN,1.0));
#endif
			UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			return o;
	}
	sampler2D _LightBuffer;
#if defined (SHADER_API_XBOX360) && defined (UNITY_HDR_ON)
	sampler2D _LightSpecBuffer;
#endif
#ifdef LIGHTMAP_ON
	float4 unity_LightmapFade;
#endif
	fixed4 unity_Ambient;


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data

#ifdef UNITY_COMPILER_HLSL
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
	o.Albedo = 0.0;
	o.Emission = 0.0;
	o.Specular = 0.0;
	o.Alpha = 0.0;
	o.Gloss = 0.0;
	fixed3 normalWorldVertex = fixed3(0,0,1);


	//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 lowpolyColor = IN.color;

	//PaperCraft
//	MakePaperCraft(IN.mass, IN.worldPos, lowpolyColor);

	//Albedo & Alpha
	o.Albedo = lowpolyColor.rgb;
	o.Alpha = lowpolyColor.a;
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	half4 light = tex2Dproj(_LightBuffer, UNITY_PROJ_COORD(IN.screen));
#if defined (SHADER_API_MOBILE)
	light = max(light, half4(0.001, 0.001, 0.001, 0.001));
#endif
#ifndef UNITY_HDR_ON
	light = -log2(light);
#endif
#if defined (SHADER_API_XBOX360) && defined (UNITY_HDR_ON)
	light.w = tex2Dproj(_LightSpecBuffer, UNITY_PROJ_COORD(IN.screen)).r;
#endif
#ifndef LIGHTMAP_OFF
#ifdef DIRLIGHTMAP_OFF
	// single lightmap
	fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
	fixed3 lm = DecodeLightmap(lmtex);
	light.rgb += lm;
#elif DIRLIGHTMAP_COMBINED
	// directional lightmaps
	fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
	half4 lm = half4(DecodeLightmap(lmtex), 0);
	light += lm;
#elif DIRLIGHTMAP_SEPARATE
	// directional with specular - no support
#endif // DIRLIGHTMAP_OFF
#else
	light.rgb += IN.vlight;
#endif // !LIGHTMAP_OFF

#ifndef DYNAMICLIGHTMAP_OFF
	fixed4 dynlmtex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, IN.lmap.zw);
	light.rgb += DecodeRealtimeLightmap(dynlmtex);
#endif

	half4 c = LightingLambert_PrePass(o, light);
	UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
	UNITY_OPAQUE_ALPHA(c.a);
	return c;
	}

		ENDCG

	}

		// ---- deferred shading pass:
		Pass{
		Name "DEFERRED"
		Tags{ "LightMode" = "Deferred" }

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma exclude_renderers nomrt
#pragma multi_compile_prepassfinal
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_DEFERRED
#include "UnityCG.cginc"
#include "Lighting.cginc"




		// vertex-to-fragment interpolation data
		struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
		float4 lmap : TEXCOORD3;
#ifdef LIGHTMAP_OFF
#if UNITY_SHOULD_SAMPLE_SH
#define V_GEOMETRY_SAVE_SPHERICAL_HARMONICS

		half3 sh : TEXCOORD4; // SH
#endif
#else
#ifdef DIRLIGHTMAP_OFF
		float4 lmapFadePos : TEXCOORD4;
#endif
#endif
		float3 mass : TEXCOORD5;
		float3 objectPos : TEXCOORD6;
	};


#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
//		#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
			o.worldPos = worldPos;
			o.worldNormal = worldNormal;

#ifndef DYNAMICLIGHTMAP_OFF
			o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#else
			o.lmap.zw = 0;
#endif
#ifndef LIGHTMAP_OFF
			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#ifdef DIRLIGHTMAP_OFF
			o.lmapFadePos.xyz = (mul(unity_ObjectToWorld, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
			o.lmapFadePos.w = (-UnityObjectToViewPos(v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
#endif
#else
			o.lmap.xy = 0;
#if UNITY_SHOULD_SAMPLE_SH
			o.sh = 0;
			o.sh = ShadeSHPerVertex(worldNormal, o.sh);
#endif
#endif
			return o;
	}
#ifdef LIGHTMAP_ON
	float4 unity_LightmapFade;
#endif
	fixed4 unity_Ambient;

	// fragment shader
	void frag_surf(v2f_surf IN,
		out half4 outDiffuse : SV_Target0,
		out half4 outSpecSmoothness : SV_Target1,
		out half4 outNormal : SV_Target2,
		out half4 outEmission : SV_Target3) {
		// prepare and unpack data
		float3 worldPos = IN.worldPos;

#ifdef UNITY_COMPILER_HLSL 
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
		o.Albedo = 0.0;
		o.Emission = 0.0;
		o.Specular = 0.0;
		o.Alpha = 0.0;
		o.Gloss = 0.0;
		fixed3 normalWorldVertex = fixed3(0,0,1);
		o.Normal = IN.worldNormal;
		normalWorldVertex = IN.worldNormal;


		//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		fixed4 lowpolyColor = IN.color;

		//PaperCraft
//		MakePaperCraft(IN.mass, IN.worldPos, lowpolyColor);

		//Albedo & Alpha
		o.Albedo = lowpolyColor.rgb;
		o.Alpha = lowpolyColor.a;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		fixed3 originalNormal = o.Normal;
		half atten = 1;

		// Setup lighting environment
		UnityGI gi;
		UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
		gi.indirect.diffuse = 0;
		gi.indirect.specular = 0;
		gi.light.color = 0;
		gi.light.dir = half3(0,1,0);
		gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);
		// Call GI (lightmaps/SH/reflections) lighting function
		UnityGIInput giInput;
		UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
		giInput.light = gi.light;
		giInput.worldPos = worldPos;
		giInput.atten = atten;
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
		giInput.lightmapUV = IN.lmap;
#else
		giInput.lightmapUV = 0.0;
#endif

giInput.ambient.rgb = 0.0;
#ifdef LIGHTMAP_OFF
	#if UNITY_SHOULD_SAMPLE_SH
			giInput.ambient = IN.sh;		
	#endif
#endif


		giInput.probeHDR[0] = unity_SpecCube0_HDR;
		giInput.probeHDR[1] = unity_SpecCube1_HDR;
#if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
		giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
#endif
#if UNITY_SPECCUBE_BOX_PROJECTION
		giInput.boxMax[0] = unity_SpecCube0_BoxMax;
		giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
		giInput.boxMax[1] = unity_SpecCube1_BoxMax;
		giInput.boxMin[1] = unity_SpecCube1_BoxMin;
		giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
#endif
		LightingLambert_GI(o, giInput, gi);

		// call lighting function to output g-buffer
		outEmission = LightingLambert_Deferred(o, gi, outDiffuse, outSpecSmoothness, outNormal);
#ifndef UNITY_HDR_ON
		outEmission.rgb = exp2(-outEmission.rgb);
#endif
		UNITY_OPAQUE_ALPHA(outDiffuse.a);
	}

	ENDCG

	}

		// ---- meta information extraction pass:
		Pass{
		Name "Meta"
		Tags{ "LightMode" = "Meta" }
		Cull Off

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_META
#include "UnityCG.cginc"
#include "Lighting.cginc"






#include "UnityMetaPass.cginc"

		// vertex-to-fragment interpolation data
		struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0;
		fixed4 color : COLOR0;
	};


#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {

		SET_UP_LOW_POLY_DATA(v)

			return o;
	}


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data
#ifdef UNITY_COMPILER_HLSL
		SurfaceOutput o = (SurfaceOutput)0;
#else
		SurfaceOutput o;
#endif
	o.Albedo = 0.0;
	o.Emission = 0.0;
	o.Specular = 0.0;
	o.Alpha = 0.0;
	o.Gloss = 0.0;
	fixed3 normalWorldVertex = fixed3(0,0,1);


	//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 lowpolyColor = IN.color;

	//Albedo & Alpha
	o.Albedo = lowpolyColor.rgb;
	o.Alpha = lowpolyColor.a;
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	UnityMetaInput metaIN;
	UNITY_INITIALIZE_OUTPUT(UnityMetaInput, metaIN);
	metaIN.Albedo = o.Albedo;
	metaIN.Emission = o.Emission;
	return UnityMetaFragment(metaIN);
	}

		ENDCG

	}

	} 
	
	Fallback "Diffuse"
}
