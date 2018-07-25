#ifndef VACUUM_SHADERS_DIRECTX_11_LOWPOLY_CORE_CGINC
#define VACUUM_SHADERS_DIRECTX_11_LOWPOLY_CORE_CGINC


#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Variables.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Geometry.cginc"



#define TRANSFORM_TEX_LOWPOLY(texUV,texName) (texUV.xy * texName##_ST.xy + texName##_ST.zw + texName##_Scroll.xy * _Time.x)

#ifdef V_LP_DISPLACE_TEXTURE
inline float VertexDisplace(float2 uv)
#else
inline float3 VertexDisplace(float4 vertex)
#endif
{
	#if defined(V_LP_DISPLACE_TEXTURE)

		fixed4 d1 = tex2Dlod(_DisplaceTex_1, float4(TRANSFORM_TEX_LOWPOLY(uv, _DisplaceTex_1), 0, 0)).r;
		fixed4 d2 = tex2Dlod(_DisplaceTex_2, float4(TRANSFORM_TEX_LOWPOLY(uv, _DisplaceTex_2), 0, 0)).r;
		
		return lerp(d1 + d2, d1 * d2, _DisplaceBlendType) * _DisplaceStrength;

	#elif defined(V_LP_DISPLACE_PARAMETRIC)

		float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;

	    float s, c;
	    sincos((_DisplaceDirection - 45) * 0.0174533, s, c);
		float2 dir = mul(float2x2(c, -s, s, c), worldPos.xz);

		float n = frac(sin(dot(worldPos.xzz, float3(12.9898, 78.233, 63.9137))));
	    n *= _DisplaceNoiseCoef;

	    float displace =  (_DisplaceAmplitude * (n + 1)) * sin(dot(dir, _DisplaceFrequency) + lerp(_Time.x, _V_LP_Time, _DisplaceScriptSynchronize) * _DisplaceSpeed + n);
	    
		return mul(unity_WorldToObject, float4(worldPos.x, worldPos.y + displace, worldPos.z, 1));

	#endif
}
 
inline fixed4 GetLowpolyVertexColor(float2 vTexcoord, fixed4 vColor)
{
	#ifdef V_LP_SECOND_TEXTURE_ON
		fixed4 c1 = tex2Dlod(_MainTex, float4(TRANSFORM_TEX_LOWPOLY(vTexcoord, _MainTex), 0, 0));
		fixed4 c2 = tex2Dlod(_SecondTex, float4(TRANSFORM_TEX_LOWPOLY(vTexcoord, _SecondTex), 0, 0));
	
		fixed4 detail = c1 * c2;
		detail.rgb *= unity_ColorSpaceDouble.r;

		fixed4 decal = lerp(c1, c2, saturate(c2.a + _SecondTex_AlphaOffset));

		fixed4 c = lerp(detail, decal, _SecondTex_BlendType);
	#else
		fixed4 c = tex2Dlod(_MainTex, float4(TRANSFORM_TEX_LOWPOLY(vTexcoord, _MainTex), 0, 0));
	#endif 

	return lerp(c, c * vColor, _VertexColor);
}


inline fixed GetLowpolyAlphaValue(fixed vA, fixed mA, fixed sA, fixed pA)
{
	#ifdef V_LP_PIXEL_TEXTURE_ON
		#ifdef V_LP_SECOND_TEXTURE_ON			
			fixed cV = lerp(mA * sA, lerp(mA, sA, saturate(sA + _SecondTex_AlphaOffset)), _SecondTex_BlendType);
			fixed cP = lerp(cV * pA, lerp(cV, pA, saturate(pA + _PixelTex_AlphaOffset)), _PixelTex_BlendType);

			fixed retValue = lerp(cP, vA, int(_AlphaFromVertex));
		#else
			fixed c = lerp(mA * pA, lerp(mA, pA, saturate(pA + _PixelTex_AlphaOffset)), _PixelTex_BlendType);

			fixed retValue = lerp(c, vA, int(_AlphaFromVertex));
		#endif
	#else
		#ifdef V_LP_SECOND_TEXTURE_ON			
			fixed c = lerp(mA * sA, lerp(mA, sA, saturate(sA + _SecondTex_AlphaOffset)), _SecondTex_BlendType);

			fixed retValue = lerp(c, vA, int(_AlphaFromVertex));
		#else
			fixed retValue = lerp(mA, vA, int(_AlphaFromVertex));
		#endif
	#endif


	#ifdef V_LP_CUTOUT	
		return retValue * _Color.a - _Cutoff * 1.01;
	#else
		return retValue * _Color.a;
	#endif
}

inline fixed4 GetLowpolyPixelColor(float2 uv, fixed4 vColor)
{
	fixed4 pColor = 0;
	#ifdef V_LP_PIXEL_TEXTURE_ON
		pColor = tex2D(_PixelTex, TRANSFORM_TEX_LOWPOLY(uv,_PixelTex));
	#endif

	fixed4 lowpolyColor;
	#ifdef V_LP_PIXEL_TEXTURE_ON
		fixed4 c = lerp(vColor *  pColor * unity_ColorSpaceDouble.r, lerp(vColor, pColor, saturate(pColor.a + _PixelTex_AlphaOffset)), _PixelTex_BlendType);
		c.a = vColor.a;

		lowpolyColor = c * _Color;
	#else
		lowpolyColor = vColor * _Color;
	#endif


	#if defined(V_LP_CUTOUT) || defined(V_LP_TRANSPARENT)
		fixed mA = tex2D(_MainTex, TRANSFORM_TEX_LOWPOLY(uv,_MainTex)).a;

		fixed sA = 1;
		#ifdef V_LP_SECOND_TEXTURE_ON
			sA = tex2D(_SecondTex, TRANSFORM_TEX_LOWPOLY(uv,_SecondTex)).a;
		#endif

		lowpolyColor.a = GetLowpolyAlphaValue(vColor.a, mA, sA, pColor.a);
	#endif	
		
	return lowpolyColor;
}

#ifdef V_LP_BUMP 
inline float3 GetLowpolyBump(float2 uv)
{
    float3 normal = UnpackNormal(tex2D(_BumpTex, TRANSFORM_TEX_LOWPOLY(uv, _BumpTex)));
    normal = normalize(float3(normal.x * _BumpStrength, normal.y * _BumpStrength, normal.z));

    return normal;
}
#endif



#if defined(V_LP_REFLECTIVE_CUBE_MAP) || defined(V_LP_REFLECTIVE_PROBE) || defined(V_LP_REFLECTIVE_REALTIME)
inline fixed3 GetLowpolyReflectionColor(float3 worldRefl, float3 oNormal, float3 wNormal, float3 fresnelViewDir, fixed strength, float4 screenPos)
{	
	#if defined(V_LP_REFLECTIVE_REALTIME)
				
		#ifdef V_LP_BUMP
			half4 screenWithOffset = screenPos + half4((oNormal.xy + wNormal.xz) * _ReflectionDistortion, 0, 0);
		#else
			half4 screenWithOffset = screenPos + half4(oNormal.xz * _ReflectionDistortion, 0, 0);
		#endif		 
	   
	    half4 reflTex = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(screenWithOffset));
	#elif defined(V_LP_REFLECTIVE_PROBE)
		float4 reflMap = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, worldRefl, _ReflectionRoughness * 10);
		fixed3 reflTex = DecodeHDR(reflMap, unity_SpecCube0_HDR);
	#else
		float4 reflMap = UNITY_SAMPLE_TEXCUBE_LOD(_Cube, worldRefl, _ReflectionRoughness * 10);
		fixed3 reflTex = lerp(reflMap, DecodeHDR(reflMap, _Cube_HDR), _CubeIsHDR);		
	#endif

	reflTex *= _ReflectColor * saturate(strength + _ReflectionStrengthOffset) * unity_ColorSpaceDouble * 0.5;

	float fresnel = _ReflectionFresnel < 0.01 ? 1 : pow(1.0 - saturate(dot (fresnelViewDir, oNormal)), _ReflectionFresnel);

	return reflTex * fresnel;
}
#endif

inline float3 GetLowpolyEmission(float2 uv)
{
	return tex2D(_EmissionTex, TRANSFORM_TEX_LOWPOLY(uv, _EmissionTex)).rgb * _EmissionColor.rgb * _EmissionStrength * (_EMISSION_TOGGLE > 0.5 ? 1 : 0);
}


#if defined(V_GEOMETRY_SAVE_LOWPOLY_COLOR) || defined(V_LP_BUMP)
#define SETUP_LP_PIXEL_UV(o,v) o.pixelTexUV = v.texcoord.xy;
#else
#define SETUP_LP_PIXEL_UV(o,v)  
#endif

#ifdef V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define SETUP_LP_COLOR(o,v) o.color = GetLowpolyVertexColor(v.texcoord, v.color);
#else
#define SETUP_LP_COLOR(o,v)  
#endif
  
#if defined(V_LP_DISPLACE_TEXTURE)
#define SETUP_LP_DISPLACE(v) v.vertex.xyz += v.normal * VertexDisplace(v.texcoord.xy);
#elif defined(V_LP_DISPLACE_PARAMETRIC)
#define SETUP_LP_DISPLACE(v) v.vertex.xyz = VertexDisplace(v.vertex);
#else
#define SETUP_LP_DISPLACE(v)  
#endif
 
#if defined(UNITY_PASS_META)
#define SETUP_LP_OPOS o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
#elif defined(UNITY_PASS_SHADOWCAST)
#define SETUP_LP_OPOS TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
#else 
#define SETUP_LP_OPOS o.pos = UnityObjectToClipPos(v.vertex); 
#endif 
 
#if defined(V_LP_REFLECTIVE_CUBE_MAP) || defined(V_LP_REFLECTIVE_PROBE)
#define SETUP_LP_SCREEN_POS o.screenPos = 0; 
#elif defined(V_LP_REFLECTIVE_REALTIME)
#define SETUP_LP_SCREEN_POS o.screenPos = ComputeScreenPos(o.pos);
#else 
#define SETUP_LP_SCREEN_POS
#endif
   
#if defined(UNITY_PASS_SHADOWCASTER) || defined(UNITY_PASS_META) || defined(UNITY_PASS_PREPASSBASE) || defined(UNITY_PASS_ZWRITE)
#define SETUP_LP_WIRE
#else
#define SETUP_LP_WIRE o.objectPos = v.vertex.xyz;
#endif

#define SET_UP_LOW_POLY_DATA(v) v2f_surf o; \
								UNITY_INITIALIZE_OUTPUT(v2f_surf,o); \
								UNITY_SETUP_INSTANCE_ID(v); \
								UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); \
								SETUP_LP_PIXEL_UV(o,v) \
								SETUP_LP_COLOR(o,v) \
								SETUP_LP_DISPLACE(v) \
								SETUP_LP_OPOS \
								SETUP_LP_SCREEN_POS \
								SETUP_LP_WIRE


#endif
