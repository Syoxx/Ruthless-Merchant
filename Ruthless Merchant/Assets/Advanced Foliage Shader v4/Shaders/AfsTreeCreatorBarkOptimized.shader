// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Nature/Afs Tree Creator Bark Optimized" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	[LM_Albedo] _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpSpecMap ("Normalmap (GA) Spec (R)", 2D) = "bump" {}
	_TranslucencyMap ("Trans (RGB) Gloss(A)", 2D) = "white" {}

	_AfsXtraBending ("Afs Xtra Bending", Range(0,5)) = 0
	
	// These are here only to provide default values
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	[HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
	[HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
	[HideInInspector] _SquashAmount ("Squash", Float) = 1
}

SubShader { 
	Tags { "RenderType"="AfsTreeBark" }
	LOD 200
	
	CGPROGRAM
	// We use our own lighting here as unity maps the outputs for deferred pretty weird...
	#pragma surface surf AFSBlinnPhong vertex:AfsTreeVertBark addshadow fullforwardshadows nolightmap

	// nolightmap will not let us use GI 
	#pragma target 3.0
	#pragma shader_feature _AFS_DEFERRED

	#define XLEAFBENDING
	// We do NOT define LEAFTUMBLING here

	#include "Includes/AfsBuiltin3xTreeLibrary.cginc"
	#include "Includes/AfsBlinnPhongLighting.cginc"
	
	#ifdef _AFS_DEFERRED
		#undef UNITY_APPLY_FOG
		#define UNITY_APPLY_FOG(coord,col) /**/
	#endif

	// Prevent the shader from sampling sh ambient lighting in the vertex function
	#undef UNITY_SHOULD_SAMPLE_SH

	sampler2D _MainTex;
	float4 _MainTex_ST;

	#if !defined(UNITY_PASS_SHADOWCASTER)
		sampler2D _BumpSpecMap;
		sampler2D _TranslucencyMap;

		// AFS Billboard Shadow Color
		// fixed4 _AfsAmbientBillboardLight;
		// AFS Tree Color
		fixed4 _AfsTreeColor;
	#endif

	struct Input {
		float3 AFSuv_MainTex;
		fixed4 color : COLOR;
	};

	// void (inout, out) has to be in the shader (not shifted to cginc) !?!
	void AfsTreeVertBark (inout appdata_full v, out Input o) 
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
		o.AFSuv_MainTex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		float4 TreeWorldPos = float4(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w, 1);
		float fadeState = saturate( ( _AfsTerrainTrees.x - distance(_WorldSpaceCameraPos, TreeWorldPos)) / _AfsTerrainTrees.y );
		TreeWorldPos.w = clamp(fadeState * _SquashAmount, 0.0, 1.0);
	//	Scale
		v.vertex.xyz *= _TreeInstanceScale.xyz;
	//	Add extra animation to make it fit speedtree
		TreeWorldPos.xyz = abs(TreeWorldPos.xyz);
		float sinuswave = _SinTime.z;
		float4 vOscillations = AfsSmoothTriangleWave(float4(TreeWorldPos.x + sinuswave , TreeWorldPos.z + sinuswave * 0.8, 0.0, 0.0));
		float fOsc = vOscillations.x + (vOscillations.y * vOscillations.y);
		fOsc = (fOsc + 3.0) * 0.33;
	//	Apply Wind
		v.vertex = afsAnimateVertex( float4(v.vertex.xyz, v.color.b), float4(v.normal.xyz,fOsc), float4(v.color.xy, v.texcoord1.xy), float4(0,0,0,_SquashAmount) );
		//v.vertex = AfsSquashNew(v.vertex, TreeWorldPos.w);
		v.vertex = AfsSquashNew(v.vertex, _SquashAmount);
		#if !defined(UNITY_PASS_SHADOWCASTER)
			v.normal = normalize(v.normal);
			v.tangent.xyz = normalize(v.tangent.xyz);
		//	Apply tree color
			v.color.rgb = lerp(_AfsTreeColor, 1.0, _TreeInstanceColor.g) * _Color.rgb;
		//	Billboard shadow color (currently not supported)
			// v.color.rgb = lerp(_AfsAmbientBillboardLight.rgb, v.color.rgb, saturate(TreeWorldPos.w + _TreeInstanceColor.a) );
		//	Store fadestate (delayed)
			o.AFSuv_MainTex.z = TreeWorldPos.w;
		#endif
	}

	void surf (Input IN, inout BarkSurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.AFSuv_MainTex.xy);
		o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
		o.Alpha = c.a;
		
		#if !defined(UNITY_PASS_SHADOWCASTER)
			fixed4 trngls = tex2D (_TranslucencyMap, IN.AFSuv_MainTex.xy);
			o.Gloss = trngls.a * _Color.r;

			half4 norspc = tex2D (_BumpSpecMap, IN.AFSuv_MainTex.xy);
			o.Specular = norspc.r;
			o.Normal = UnpackNormalDXT5nm(norspc);

			// Although the rendertex shader uses specular lighting we fade it out here
			if (_SquashAmount < 1.0) {
				o.Normal = lerp(float3(0,0,1), o.Normal, _SquashAmount);	// Fade in normal
				#if defined (UNITY_PASS_DEFERRED)
					o.Specular *= _SquashAmount; 							// Fade in specular Highlights: deferred
				#else
					o.Gloss *= _SquashAmount;								// Fade in specular Highlights: forward
				#endif
			}
			o.ShadowCutOff = IN.AFSuv_MainTex.z;							// Fade in Shadows (delayed)
		#endif
	}
	ENDCG
}

Dependency "BillboardShader" = "Hidden/Nature/Afs Tree Creator Bark Rendertex"
}
