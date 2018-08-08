// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Nature/Afs Tree Creator Leaves Optimized" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1)
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
	_TranslucencyViewDependency ("View dependency", Range(0,1)) = 0.7
	_ShadowStrength("Shadow Strength", Range(0,1)) = 0.8
	_ShadowOffsetScale ("Shadow Offset Scale", Float) = 1
	
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpSpecMap ("Normalmap (GA) Spec (R) Shadow Offset (B)", 2D) = "bump" {}
	_TranslucencyMap ("Trans (B) Gloss(A)", 2D) = "white" {}

	_AfsXtraBending ("Afs Xtra Bending", Range(0,5)) = 0

	_TumbleStrength("Tumble Strength", Range(0,1)) = 0.1
	_TumbleFrequency("Tumble Frequency", Range(0,2)) = 1

	// These are here only to provide default values
	[HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
	[HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
	[HideInInspector] _SquashAmount ("Squash", Float) = 1
}

SubShader { 
	Tags {
		"IgnoreProjector"="True"
		"RenderType"="AFSTreeLeaf"
	}
	LOD 200
	CGPROGRAM
		// Use our own early alpha testing: so no alphatest:_Cutoff
		#pragma surface surf TreeLeaf vertex:AfsTreeVertLeaf nolightmap fullforwardshadows addshadow
		#pragma target 3.0
		#pragma shader_feature _AFS_DEFERRED
		
		#define XLEAFBENDING
		//#define LEAFTUMBLING

		#include "Includes/AfsBuiltin3xTreeLibrary.cginc"

		#ifdef _AFS_DEFERRED
			#undef UNITY_APPLY_FOG
			#define UNITY_APPLY_FOG(coord,col) /**/
		#endif

		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _Cutoff;

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
			fixed4 color : COLOR; // color.a = AO
		};

		// void (inout, out) has to be in the shader (not shifted to cginc) !?!
		void AfsTreeVertLeaf (inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.AFSuv_MainTex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			float4 TreeWorldPos = float4(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w, 0);
			float fadeState = saturate( ( _AfsTerrainTrees.x - distance(_WorldSpaceCameraPos, TreeWorldPos)) / _AfsTerrainTrees.y );
			TreeWorldPos.w = clamp(fadeState * _SquashAmount, 0.0, 1.0);
		//	Apply Billboard Extrusion (if any)
			ExpandBillboard (UNITY_MATRIX_IT_MV, v.vertex, v.normal, v.tangent);
		//	Scale
			v.vertex.xyz *= _TreeInstanceScale.xyz;
		//	Decode UV3
			float3 pivot;
			#if defined(LEAFTUMBLING)
				//pivot = (frac(float3(1.0f, 256.0f, 65536.0f) * v.texcoord2.x) * 2) - 1;
				pivot = (frac(float3(1.0f, 1024.0f, 1048576.0f) * v.texcoord2.x) * 2) - 1;
				pivot *= v.texcoord2.y;
				pivot *= _TreeInstanceScale.xyz;
			#endif
		//	Add extra animation to make it fit speedtree
			TreeWorldPos.xyz = abs(TreeWorldPos.xyz);
			float sinuswave = _SinTime.z;
			float4 vOscillations = AfsSmoothTriangleWave(float4(TreeWorldPos.x + sinuswave , TreeWorldPos.z + sinuswave * 0.8, 0.0, 0.0));
			float fOsc = vOscillations.x + (vOscillations.y * vOscillations.y);
			fOsc = (fOsc + 3.0) * 0.33;
		//	Apply Wind
			v.vertex = afsAnimateVertex( float4(v.vertex.xyz, v.color.b), float4(v.normal.xyz,fOsc), float4(v.color.xy, v.texcoord1.xy), float4(pivot,_SquashAmount));
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
 
		void surf (Input IN, inout LeafSurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.AFSuv_MainTex.xy);
			o.Alpha = c.a;
			// Do early alpha test
			clip(o.Alpha - _Cutoff);
			
			#if !defined(UNITY_PASS_SHADOWCASTER)
				o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
				fixed4 trngls = tex2D (_TranslucencyMap, IN.AFSuv_MainTex.xy);
				o.Gloss = trngls.a * _Color.r;
				half4 norspc = tex2D (_BumpSpecMap, IN.AFSuv_MainTex.xy);
				o.Normal = UnpackNormalDXT5nm(norspc);
				// This does not cause "dynamic" branching as it is always the same for all processed pixels
				if (_SquashAmount < 1.0) {
					o.Normal = lerp(float3(0,0,1), o.Normal, _SquashAmount); 	// Fade in normal
					o.Specular = norspc.r * _SquashAmount;						// Fade in specular Highlights
				}
				else {
					o.Specular = norspc.r;
				}
				o.Translucency = trngls.b * IN.AFSuv_MainTex.z;					// Fade in Translucency (delayed)
				o.ShadowCutOff = IN.AFSuv_MainTex.z;							// Fade in Shadows (delayed)
			#endif
		}
	ENDCG

	}
Dependency "BillboardShader" = "Hidden/Nature/Afs Tree Creator Leaves Rendertex"
}
