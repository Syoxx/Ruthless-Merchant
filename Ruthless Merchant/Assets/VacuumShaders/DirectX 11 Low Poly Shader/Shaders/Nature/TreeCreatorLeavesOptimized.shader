Shader "VacuumShaders/DirectX 11 Low Poly/Nature/Tree Creator Leaves Optimized"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_TranslucencyColor("Translucency Color", Color) = (0.73,0.85,0.41,1) // (187,219,106,255)
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.3
		_TranslucencyViewDependency("View dependency", Range(0,1)) = 0.7
		_ShadowStrength("Shadow Strength", Range(0,1)) = 0.8
		_ShadowOffsetScale("Shadow Offset Scale", Float) = 1

		_MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
		_ShadowTex("Shadow (RGB)", 2D) = "white" {}
		_BumpSpecMap("Normalmap (GA) Spec (R) Shadow Offset (B)", 2D) = "bump" {}
		_TranslucencyMap("Trans (B) Gloss(A)", 2D) = "white" {}

		// These are here only to provide default values
		[HideInInspector] _TreeInstanceColor("TreeInstanceColor", Vector) = (1,1,1,1)
		[HideInInspector] _TreeInstanceScale("TreeInstanceScale", Vector) = (1,1,1,1)
		[HideInInspector] _SquashAmount("Squash", Float) = 1
	}

	SubShader
	{
		Tags{"IgnoreProjector" = "True" "RenderType" = "TreeLeaf" }
		LOD 200
		Cull Off

		// ------------------------------------------------------------
		// Surface shader code generated out of a CGPROGRAM block:


		// ---- forward rendering base pass:
		Pass{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }
		ColorMask RGB

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
		// vertex modifier: 'TreeVertLeaf'
		// writes to per-pixel normal: no
		// writes to emission: no
		// writes to occlusion: no
		// needs world space reflection vector: no
		// needs world space normal vector: no
		// needs screen space position: no
		// needs world space position: no
		// needs view direction: no
		// needs world space view direction: no
		// needs world space position for lighting: no
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
	struct v2f_surf 
	{
		float4 pos : SV_POSITION;
		float2 pixelTexUV : TEXCOORD0; // _MainTex
		half3 worldNormal : TEXCOORD1;
		float3 worldPos : TEXCOORD2;
		fixed4 color : COLOR0;

		#define V_GEOMETRY_SAVE_SPHERICAL_HARMONICS
		fixed3 sh : TEXCOORD3; // ambient/SH/vertexlights
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
#define V_LP_CUTOUT
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


//#pragma surface surf TreeLeaf alphatest:_Cutoff vertex:TreeVertLeaf nolightmap noforwardadd
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/UnityBuiltin3xTreeLibrary.cginc"


	sampler2D _BumpSpecMap;
	sampler2D _TranslucencyMap;

	struct Input
	{
		float2 uv_MainTex;
		fixed4 color : COLOR; // color.a = AO
	};

	void surf(Input IN, inout LeafSurfaceOutput o)
	{		
		o.Albedo = IN.color * 2;

		fixed4 trngls = tex2D(_TranslucencyMap, IN.uv_MainTex);
		o.Translucency = trngls.b;
		o.Gloss = trngls.a * _Color.r;
		o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

		half4 norspc = tex2D(_BumpSpecMap, IN.uv_MainTex);
		o.Specular = norspc.r;
	} 


	// vertex shader
	v2f_surf vert_surf(appdata_full v) 
	{

		UNITY_SETUP_INSTANCE_ID(v);
		v2f_surf o;
		UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		TreeVertLeaf(v);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.pixelTexUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

		o.worldPos = worldPos;
		o.worldNormal = worldNormal;

		o.color = v.color;
		o.color *= tex2Dlod(_MainTex, float4(TRANSFORM_TEX_LOWPOLY(v.texcoord, _MainTex), 0, 0)) * o.color.a;


#ifndef LIGHTMAP_OFF
		o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

		// SH/ambient and vertex lights
		#ifdef LIGHTMAP_OFF
			#if UNITY_SHOULD_SAMPLE_SH
					float3 shlight = ShadeSH9(float4(worldNormal,1.0));
					o.sh = shlight;
			#else
					o.sh = 0.0;
			#endif

			#ifdef VERTEXLIGHT_ON
					o.sh += Shade4PointLights(
						unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
						unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
						unity_4LightAtten0, worldPos, worldNormal);
			#endif // VERTEXLIGHT_ON
		#endif // LIGHTMAP_OFF

		TRANSFER_SHADOW(o); // pass shadow coordinates to pixel shader
		UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
		return o;
	}

	// fragment shader
	fixed4 frag_surf(v2f_surf IN) : SV_Target
	{

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
			LeafSurfaceOutput o = (LeafSurfaceOutput)0;
		#else
			LeafSurfaceOutput o;
		#endif

		o.Albedo = 0.0;
		o.Emission = 0.0;
		o.Specular = 0.0;
		o.Alpha = 0.0;
		fixed3 normalWorldVertex = fixed3(0,0,1);
		o.Normal = IN.worldNormal;
		normalWorldVertex = IN.worldNormal;

		// call surface function
		surf(surfIN, o);
		

		// alpha test
		clip(o.Alpha - _Cutoff);

		// compute lighting & shadowing factor
		UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
		fixed4 c = 0;
		#ifdef LIGHTMAP_OFF
			c.rgb += o.Albedo * IN.sh;
		#endif // LIGHTMAP_OFF

		// lightmaps
		#ifndef LIGHTMAP_OFF

			#if DIRLIGHTMAP_COMBINED
				// directional lightmaps
				fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
				half3 lm = DecodeLightmap(lmtex);
			#elif DIRLIGHTMAP_SEPARATE
				// directional with specular - no support
				half4 lmtex = 0;
				half3 lm = 0;
			#else
				// single lightmap
				fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
				fixed3 lm = DecodeLightmap(lmtex);
			#endif

		#endif // LIGHTMAP_OFF


		// realtime lighting: call lighting function
		#ifdef LIGHTMAP_OFF
			c += LightingTreeLeaf(o, lightDir, worldViewDir, atten);
		#else
			c.a = o.Alpha;
		#endif

		#ifndef LIGHTMAP_OFF
		#endif // LIGHTMAP_OFF

		UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
		return c;
	}

	ENDCG

	}

		// Pass to render object as a shadow caster
		Pass{
		Name "ShadowCaster"
		Tags{ "LightMode" = "ShadowCaster" }

		CGPROGRAM


#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma multi_compile_shadowcaster
#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "Lighting.cginc"

#pragma multi_compile_instancing
#include "UnityBuiltin3xTreeLibrary.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
	struct Input {
		float2 uv_MainTex;
	};

	struct v2f_surf {
		V2F_SHADOW_CASTER;
		float2 hip_pack0 : TEXCOORD1;

		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f_surf vert_surf(appdata_full v) {
		v2f_surf o = (v2f_surf)0;

		UNITY_INITIALIZE_OUTPUT(v2f_surf, o); 
			UNITY_SETUP_INSTANCE_ID(v); 

		TreeVertLeaf(v);
		o.hip_pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			return o;
	}
	fixed _Cutoff;
	float4 frag_surf(v2f_surf IN) : SV_Target{
		half alpha = tex2D(_MainTex, IN.hip_pack0.xy).a;
	clip(alpha - _Cutoff);
	SHADOW_CASTER_FRAGMENT(IN)
	}
		ENDCG

	}

	}

		Dependency "BillboardShader" = "Hidden/Nature/Tree Creator Leaves Rendertex"
}
