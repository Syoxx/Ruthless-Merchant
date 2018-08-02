Shader "VacuumShaders/DirectX 11 Low Poly/Nature/Tree Creator Bark Optimized"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpSpecMap("Normalmap (GA) Spec (R)", 2D) = "bump" {}
	_TranslucencyMap("Trans (RGB) Gloss(A)", 2D) = "white" {}

	// These are here only to provide default values
	_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector] _TreeInstanceColor("TreeInstanceColor", Vector) = (1,1,1,1)
		[HideInInspector] _TreeInstanceScale("TreeInstanceScale", Vector) = (1,1,1,1)
		[HideInInspector] _SquashAmount("Squash", Float) = 1
	}

		SubShader{
		Tags{ "IgnoreProjector" = "True" "RenderType" = "TreeBark" }
		LOD 200


		// ------------------------------------------------------------
		// Surface shader code generated out of a CGPROGRAM block:


		// ---- forward rendering base pass:
		Pass{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#pragma multi_compile_instancing
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_fwdbase nodynlightmap nolightmap
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: YES
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: no
		// 1 texcoords actually used
		//   float2 _MainTex
#define UNITY_PASS_FORWARDBASE
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"



	// vertex-to-fragment interpolation data
	// no lightmaps:
#ifdef LIGHTMAP_OFF
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
#if UNITY_SHOULD_SAMPLE_SH
#define V_GEOMETRY_SAVE_SPHERICAL_HARMONICS

		half3 sh : TEXCOORD3; // SH
#endif
		UNITY_SHADOW_COORDS(4)
			UNITY_FOG_COORDS(5)
			float3 mass : TEXCOORD6;
			

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};
#endif
	// with lightmaps:
#ifndef LIGHTMAP_OFF
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
		float4 lmap : TEXCOORD3;
		UNITY_SHADOW_COORDS(4)
			UNITY_FOG_COORDS(5)
			float3 mass : TEXCOORD6;
			

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};
#endif


#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"


	sampler2D _BumpSpecMap;
	sampler2D _TranslucencyMap;

	struct Input {
		float2 uv_MainTex;
		fixed4 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutput o) {

		o.Albedo = IN.color.rgb;

		fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
		o.Gloss = trngls.a * _Color.r;
		o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

		half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
		o.Specular = norspc.r;
	}


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertBark(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);


		o.worldPos = worldPos;
		o.worldNormal = worldNormal;

		o.color = v.color;
		o.color *= tex2Dlod(_MainTex, float4(TRANSFORM_TEX(v.texcoord, _MainTex), 0, 0)) * o.color.a;

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
		UNITY_SETUP_INSTANCE_ID(IN);
	// prepare and unpack data
	Input surfIN;
	UNITY_INITIALIZE_OUTPUT(Input,surfIN);
	surfIN.uv_MainTex.x = 1.0;
	surfIN.color.x = 1.0;
	surfIN.uv_MainTex = IN.pixelTexUV.xy;
	float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
	fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
	fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
	surfIN.color = IN.color;
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

	// call surface function
	surf(surfIN, o);

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
	giInput.worldViewDir = worldViewDir;
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
	LightingBlinnPhong_GI(o, giInput, gi);

	// realtime lighting: call lighting function
	c += LightingBlinnPhong(o, worldViewDir, gi);
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
#pragma multi_compile_fwdadd nodynlightmap nolightmap
#pragma skip_variants INSTANCING_ON
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: YES
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: no
		// 1 texcoords actually used
		//   float2 _MainTex
#define UNITY_PASS_FORWARDADD
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"






	// vertex-to-fragment interpolation data
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;
		UNITY_SHADOW_COORDS(3)
			UNITY_FOG_COORDS(4)
			float3 mass : TEXCOORD5;

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};


#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"



		sampler2D _BumpSpecMap;
		sampler2D _TranslucencyMap;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.color.rgb;

			fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
			o.Gloss = trngls.a * _Color.r;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

			half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
			o.Specular = norspc.r;
		}


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		TreeVertBark(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
		o.worldPos = worldPos;
		o.worldNormal = worldNormal;

		o.color = v.color;
		o.color *= tex2Dlod(_MainTex, float4(TRANSFORM_TEX(v.texcoord, _MainTex), 0, 0)) * o.color.a;

		TRANSFER_SHADOW(o); // pass shadow coordinates to pixel shader
		UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
		return o;
	}

	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		// prepare and unpack data
		Input surfIN;
	UNITY_INITIALIZE_OUTPUT(Input,surfIN);
	surfIN.uv_MainTex.x = 1.0;
	surfIN.color.x = 1.0;
	surfIN.uv_MainTex = IN.pixelTexUV.xy;
	float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
	fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
	fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
	surfIN.color = IN.color;
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

	// call surface function
	surf(surfIN, o);
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
	c += LightingBlinnPhong(o, worldViewDir, gi);
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
#pragma multi_compile_instancing
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: no
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: YES
		// 1 texcoords actually used
		//   float2 _MainTex
#define UNITY_PASS_PREPASSBASE
#include "UnityCG.cginc"
#include "Lighting.cginc"




	// vertex-to-fragment interpolation data
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};


#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"


		sampler2D _BumpSpecMap;
		sampler2D _TranslucencyMap;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
			o.Gloss = trngls.a * _Color.r;

			half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
			o.Specular = norspc.r;
		}


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertBark(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
		o.worldPos = worldPos;
		o.worldNormal = worldNormal;
		return o;
	}


	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		UNITY_SETUP_INSTANCE_ID(IN);
	// prepare and unpack data
	Input surfIN;
	UNITY_INITIALIZE_OUTPUT(Input,surfIN);
	surfIN.uv_MainTex.x = 1.0;
	surfIN.color.x = 1.0;
	surfIN.uv_MainTex = IN.pixelTexUV.xy;
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

	// call surface function
	surf(surfIN, o);

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
#pragma multi_compile_instancing
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_prepassfinal nodynlightmap nolightmap
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: YES
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: no
		// 1 texcoords actually used
		//   float2 _MainTex
#define UNITY_PASS_PREPASSFINAL
#include "UnityCG.cginc"
#include "Lighting.cginc"





	// vertex-to-fragment interpolation data
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
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

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};


#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"


		sampler2D _BumpSpecMap;
		sampler2D _TranslucencyMap;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = IN.color.rgb;

			fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
			o.Gloss = trngls.a * _Color.r;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

			half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
			o.Specular = norspc.r;
		}


	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertBark(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);


		o.worldPos = worldPos;

		o.color = v.color;
		o.color *= tex2Dlod(_MainTex, float4(TRANSFORM_TEX(v.texcoord, _MainTex), 0, 0)) * o.color.a;

		o.screen = ComputeScreenPos(o.pos);
		o.lmap.zw = 0;
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
		UNITY_SETUP_INSTANCE_ID(IN);
	// prepare and unpack data
	Input surfIN;
	UNITY_INITIALIZE_OUTPUT(Input,surfIN);
	surfIN.uv_MainTex.x = 1.0;
	surfIN.color.x = 1.0;
	surfIN.uv_MainTex = IN.pixelTexUV.xy;
	float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
	fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
	fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
	surfIN.color = IN.color;
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

	// call surface function
	surf(surfIN, o);
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

	half4 c = LightingBlinnPhong_PrePass(o, light);
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
#pragma multi_compile_instancing
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma fragment frag_surf
#pragma exclude_renderers nomrt
#pragma multi_compile_prepassfinal nodynlightmap nolightmap
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: YES
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: YES
		// 1 texcoords actually used
		//   float2 _MainTex
#define UNITY_PASS_DEFERRED
#include "UnityCG.cginc"
#include "Lighting.cginc"




	// vertex-to-fragment interpolation data
	struct v2f_surf {
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
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
		

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};
		 

#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"



		sampler2D _BumpSpecMap;
		sampler2D _TranslucencyMap;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o) {

			o.Albedo = IN.color.rgb;

			fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
			o.Gloss = trngls.a * _Color.r;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

			half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
			o.Specular = norspc.r;
		}

	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertBark(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
		o.worldPos = worldPos;
		o.worldNormal = worldNormal;

		o.color = v.color;
		o.color *= tex2Dlod(_MainTex, float4(TRANSFORM_TEX(v.texcoord, _MainTex), 0, 0)) * o.color.a;

		o.lmap.zw = 0;
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
		UNITY_SETUP_INSTANCE_ID(IN);
		// prepare and unpack data
		Input surfIN;
		UNITY_INITIALIZE_OUTPUT(Input,surfIN);
		surfIN.uv_MainTex.x = 1.0;
		surfIN.color.x = 1.0;
		surfIN.uv_MainTex = IN.pixelTexUV.xy;
		float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
		fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
		fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
		fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
		surfIN.color = IN.color;
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

		// call surface function
		surf(surfIN, o);
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
		giInput.worldViewDir = worldViewDir;
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
		LightingBlinnPhong_GI(o, giInput, gi);

		// call lighting function to output g-buffer
		outEmission = LightingBlinnPhong_Deferred(o, worldViewDir, gi, outDiffuse, outSpecSmoothness, outNormal);
#ifndef UNITY_HDR_ON
		outEmission.rgb = exp2(-outEmission.rgb);
#endif
		UNITY_OPAQUE_ALPHA(outDiffuse.a);
	}

	ENDCG

	}

		// ---- shadow caster pass:
		Pass{
		Name "ShadowCaster"
		Tags{ "LightMode" = "ShadowCaster" }
		ZWrite On ZTest LEqual

		CGPROGRAM
		// compile directives
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma multi_compile_instancing
#pragma multi_compile_shadowcaster nodynlightmap nolightmap
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
		// Surface shader code generated based on:
		// vertex modifier: 'TreeVertBark'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: YES
		// needs world space view direction for lighting: YES
		// needs world space view direction for lightmaps: no
		// needs vertex color: no
		// needs VFACE: no
		// passes tangent-to-world matrix to pixel shader: no
		// reads from normal: no
		// 0 texcoords actually used
#define UNITY_PASS_SHADOWCASTER
#include "UnityCG.cginc"
#include "Lighting.cginc"


		//#pragma surface surf BlinnPhong vertex:TreeVertBark addshadow nolightmap
#include "UnityBuiltin3xTreeLibrary.cginc"

		sampler2D _MainTex;
	sampler2D _BumpSpecMap;
	sampler2D _TranslucencyMap;

	struct Input {
		float2 uv_MainTex;
		fixed4 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb * IN.color.rgb * IN.color.a;

		fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
		o.Gloss = trngls.a * _Color.r;
		o.Alpha = c.a;

		half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
		o.Specular = norspc.r;
	}


	// vertex-to-fragment interpolation data
	struct v2f_surf {
		V2F_SHADOW_CASTER;
		float3 worldPos : TEXCOORD1;
		
		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};

	// vertex shader
	v2f_surf vert_surf(appdata_full v) {
		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertBark(v);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
		o.worldPos = worldPos;
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			return o;
	}

	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target{
		UNITY_SETUP_INSTANCE_ID(IN);
	// prepare and unpack data
	Input surfIN;
	UNITY_INITIALIZE_OUTPUT(Input,surfIN);
	surfIN.uv_MainTex.x = 1.0;
	surfIN.color.x = 1.0;
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

	// call surface function
	surf(surfIN, o);
	SHADOW_CASTER_FRAGMENT(IN)
	}

		ENDCG

	}

	}

		Dependency "BillboardShader" = "Hidden/Nature/Tree Creator Bark Rendertex"
}
