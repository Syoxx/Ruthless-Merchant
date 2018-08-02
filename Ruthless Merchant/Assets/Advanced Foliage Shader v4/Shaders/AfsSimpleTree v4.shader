// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Nature/Afs Simple Tree v4" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
	_BumpTransSpecMap ("Normal (GA) Trans(R) Smoothness(B)", 2D) = "bump" {}
	_SpecularReflectivity("Shininess", Range(0.03,1)) = 0.1
	_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1) // (187,219,106,255)
	_TranslucencyViewDependency ("View dependency", Range(0,1)) = 0.7
	_ShadowStrength("Shadow Strength", Range(0,1)) = 0.8
	_AfsXtraBending ("Afs Xtra Bending", Range(0,5)) = 0
	[KeywordEnum(Vertex Colors, Vertex Colors And UV4)] _BendingControls ("Bending Parameters", Float) = 0 // 0 = vertex colors, 1 = uv4
	// These are here only to provide default values
	[HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
	[HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
	[HideInInspector] _SquashAmount ("Squash", Float) = 1
}

SubShader { 
	Tags {
		"IgnoreProjector"="True"
		"RenderType"="AFSTreeSimple"
	}
	LOD 200
	CGPROGRAM
		// Use our own early alpha testing: so no alphatest:_Cutoff
		#pragma surface surf TreeLeaf vertex:AfsTreeVertLeaf addshadow nolightmap fullforwardshadows
		#pragma target 3.0
		#pragma shader_feature _AFS_DEFERRED

		#define XLEAFBENDING
		// #define LEAFTUMBLING
		
		#include "Includes/AfsBuiltin3xTreeLibrary.cginc"

		#ifdef _AFS_DEFERRED
			#undef UNITY_APPLY_FOG
			#define UNITY_APPLY_FOG(coord,col) /**/
		#endif

		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _Cutoff;

		sampler2D _BumpTransSpecMap;
		float _SpecularReflectivity;
		// AFS Billboard Shadow Color
		// fixed4 _AfsAmbientBillboardLight;
		// AFS Tree Color
		fixed4 _AfsTreeColor;


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
			TreeWorldPos.w = clamp(fadeState * _SquashAmount, 0.0, 1.0); // must be greater 0.0 (unity 4.3)
		//	Scale
			v.vertex.xyz *= _TreeInstanceScale.xyz;
		//	Decode UV3
			float3 pivot;
			#if defined(LEAFTUMBLING)
				pivot = (frac(float3(1.0f, 256.0f, 65536.0f) * v.texcoord2.x) * 2) - 1;
				pivot *= v.texcoord2.y;
			#endif
		//	Add extra animation to make it fit speedtree
			TreeWorldPos.xyz = abs(TreeWorldPos.xyz);
			float sinuswave = _SinTime.z;
			float4 vOscillations = AfsSmoothTriangleWave(float4(TreeWorldPos.x + sinuswave , TreeWorldPos.z + sinuswave * 0.8, 0.0, 0.0));
			float fOsc = vOscillations.x + (vOscillations.y * vOscillations.y);
			fOsc = (fOsc + 3.0) * 0.33;
		//	Apply Wind
			float4 bendingCoords;
			bendingCoords.rg = v.color.rg;
			bendingCoords.ba = (_BendingControls == 0) ? v.color.bb : v.texcoord3.xy;
			v.vertex = afsAnimateVertex( float4(v.vertex.xyz, v.color.b), float4(v.normal.xyz,fOsc), bendingCoords, float4(pivot,_SquashAmount) );
			//v.vertex = AfsSquashNew(v.vertex, TreeWorldPos.w);
			v.vertex = AfsSquashNew(v.vertex, _SquashAmount);
			#if !defined(UNITY_PASS_SHADOWCASTER)
				v.normal = normalize(v.normal);
				v.tangent.xyz = normalize(v.tangent.xyz);
			//	Apply tree color
				v.color.rgb = lerp(_AfsTreeColor, 1.0, _TreeInstanceColor.g) * _Color.rgb;
			//	Billboard shadow color (currently not supported)		
				//v.color.rgb = lerp(_AfsAmbientBillboardLight.rgb, v.color.rgb, saturate(TreeWorldPos.w + _TreeInstanceColor.a) );
			//	Store fadestate
				o.AFSuv_MainTex.z = TreeWorldPos.w;
			#endif
		}
 
		void surf (Input IN, inout LeafSurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.AFSuv_MainTex.xy);
			o.Alpha = c.a;
			// Do early alpha test
			clip(o.Alpha - _Cutoff);
			
			o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
			fixed4 trngls = tex2D (_BumpTransSpecMap, IN.AFSuv_MainTex.xy);
			o.Gloss = trngls.b * _Color.r;
			o.Normal = UnpackNormalDXT5nm(trngls);
			// This does not cause "dynamic" branching as it is always the same for all processed pixels
			if (IN.AFSuv_MainTex.z < 1.0) {
				o.Normal = lerp(float3(0,0,1), o.Normal, _SquashAmount); 	// Fade in normal
				o.Specular = _SpecularReflectivity * _SquashAmount;			// Fade in specular Highlights
			}
			else {
				o.Specular = _SpecularReflectivity;
			}
			o.Translucency = trngls.r * IN.AFSuv_MainTex.z;					// Fade in Translucency (delayed)
			o.ShadowCutOff = IN.AFSuv_MainTex.z;							// Fade in Shadows (delayed)
		}
	ENDCG

	}

Dependency "BillboardShader" = "Hidden/Nature/Afs Tree Creator Leaves Rendertex"
}
