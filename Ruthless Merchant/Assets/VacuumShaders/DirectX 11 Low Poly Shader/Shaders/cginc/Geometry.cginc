#ifndef VACUUM_SHADERS_DIRECTX_11_LOWPOLY_GEOMETRY_CGINC
#define VACUUM_SHADERS_DIRECTX_11_LOWPOLY_GEOMETRY_CGINC


inline float4 CalcTangent(float3 v1, float3 v2, float3 v3, float2 w1, float2 w2, float2 w3, float3 _n)
{
	float x1 = v2.x - v1.x;
	float x2 = v3.x - v1.x;
	float y1 = v2.y - v1.y;
	float y2 = v3.y - v1.y;
	float z1 = v2.z - v1.z;
	float z2 = v3.z - v1.z;

	float s1 = w2.x - w1.x;
	float s2 = w3.x - w1.x;
	float t1 = w2.y - w1.y;
	float t2 = w3.y - w1.y;

	float r = 0.0001f;
	if (s1 * t2 - s2 * t1 != 0)
		r = 1.0f / (s1 * t2 - s2 * t1);

	float3 tan1 = normalize(float3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r));
    float3 tan2 = normalize(float3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r));


	float w = (dot(cross(_n, tan1), tan2) < 0.0f) ? -1.0f : 1.0f;

	return float4(tan1, w);	 
} 


[maxvertexcount(3)]
void geom(triangle v2f_surf input[3], inout TriangleStream<v2f_surf> triStream)
{
	//Vertex
	#if defined(V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION)
		float3 v0 = input[0].worldPos;
		float3 v1 = input[1].worldPos;
		float3 v2 = input[2].worldPos;

		float3 positon = lerp((v0 + v1 + v2) / 3, v0, _SamplingType);
	#else
		float3 v0 = float3(1, 0, 0);
		float3 v1 = float3(0, 1, 0);
		float3 v2 = float3(0, 0, 1);

		float3 positon = 0;
	#endif

	
	//Normal
	#if defined(V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL) || defined(V_GEOMETRY_SAVE_NORMAL_T_SPACE) || defined(V_GEOMETRY_SAVE_REFLECTION_WORLD_REFLECTION) || defined(V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL)
		float3 normal = normalize(cross(v1 - v0, v2 - v0));
	#endif
	

	
	//Color
	#ifdef V_GEOMETRY_SAVE_LOWPOLY_COLOR
		fixed4 lowPolyColor = lerp((input[0].color + input[1].color + input[2].color) / 3.0, input[0].color, _SamplingType);
		
		input[0].color = lowPolyColor;
		input[1].color = lowPolyColor;
		input[2].color = lowPolyColor;
	#endif	 

	//Position
	#ifdef V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
		input[0].worldPos = positon;	
		input[1].worldPos = positon;	
		input[2].worldPos = positon;	
	#endif

	//Normal
	#ifdef V_GEOMETRY_SAVE_NORMAL_WORLD_NORMAL
		input[0].worldNormal = normal;	
		input[1].worldNormal = normal;	
		input[2].worldNormal = normal;	
	#endif
	#ifdef V_GEOMETRY_SAVE_NORMAL_T_SPACE
		input[0].tSpace0.z = input[1].tSpace0.z = input[2].tSpace0.z = normal.x;
		input[0].tSpace1.z = input[1].tSpace1.z = input[2].tSpace1.z = normal.y;
		input[0].tSpace2.z = input[1].tSpace2.z = input[2].tSpace2.z = normal.z;
	#endif
	

	#ifdef V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL
		float4 worldTangent = CalcTangent(v0, v1, v2, input[0].pixelTexUV, input[1].pixelTexUV, input[2].pixelTexUV, normal);

		fixed3 worldBinormal = normalize(cross(normal, worldTangent.xyz) * worldTangent.w);

		//0		
		input[0].tSpace0.xyz = float3(worldTangent.x, worldBinormal.x, normal.x);
		input[0].tSpace1.xyz = float3(worldTangent.y, worldBinormal.y, normal.y);
		input[0].tSpace2.xyz = float3(worldTangent.z, worldBinormal.z, normal.z);

		//1
		input[1].tSpace0.xyz = float3(worldTangent.x, worldBinormal.x, normal.x);
		input[1].tSpace1.xyz = float3(worldTangent.y, worldBinormal.y, normal.y);
		input[1].tSpace2.xyz = float3(worldTangent.z, worldBinormal.z, normal.z);

		//2
		input[2].tSpace0.xyz = float3(worldTangent.x, worldBinormal.x, normal.x);
		input[2].tSpace1.xyz = float3(worldTangent.y, worldBinormal.y, normal.y);
		input[2].tSpace2.xyz = float3(worldTangent.z, worldBinormal.z, normal.z);
	#endif
	

	//Reflection
	#ifdef V_GEOMETRY_SAVE_REFLECTION_WORLD_REFLECTION
		input[0].worldRefl = lerp(input[0].worldRefl, reflect(-UnityWorldSpaceViewDir(v0), normal), _ReflectionDistortion);
		input[1].worldRefl = lerp(input[1].worldRefl, reflect(-UnityWorldSpaceViewDir(v1), normal), _ReflectionDistortion);
		input[2].worldRefl = lerp(input[2].worldRefl, reflect(-UnityWorldSpaceViewDir(v2), normal), _ReflectionDistortion);
	#endif


	//SH
	#if defined(V_GEOMETRY_SAVE_SPHERICAL_HARMONICS) && defined(V_LP_LIGHT_ON)
		half3 sh = lerp((input[0].sh + input[1].sh + input[2].sh) / 3, input[0].sh, _SamplingType);

		input[0].sh = sh;	
		input[1].sh = sh;	
		input[2].sh = sh;	
	#endif
		 
	//Low poly GI for Forward Rendering
	#if defined(V_LP_LIGHT_ON) && defined(UNITY_PASS_FORWARDBASE)
		float4 lmap = lerp((input[0].lmap + input[1].lmap + input[2].lmap) / 3, input[0].lmap, _SamplingType);

		input[0].lmap.zw = lmap.zw;
		input[1].lmap.zw = lmap.zw;
		input[2].lmap.zw = lmap.zw;
	#endif



	//PaperCraft
	#if defined(UNITY_PASS_SHADOWCASTER) || defined(UNITY_PASS_META) || defined(UNITY_PASS_PREPASSBASE) || defined(UNITY_PASS_ZWRITE)
		//Empty
	#else
		/*input[0].mass = fixed3(1, 0, 0);	
		input[1].mass = fixed3(0, 1, 0);	
		input[2].mass = fixed3(1, 0, 1);	*/
		#ifdef V_WIRE_TRY_QUAD_ON 

			float e1 = length(input[0].objectPos - input[1].objectPos);
			float e2 = length(input[1].objectPos - input[2].objectPos);
			float e3 = length(input[2].objectPos - input[0].objectPos);

			float3 quad = 0;
			if (e1 > e2 && e1 > e3)
				quad.y = 1.;
			else if (e2 > e3 && e2 > e1)
				quad.x = 1;
			else
				quad.z = 1;
	
			input[0].mass.xyz = fixed3(1, 0, 0) + quad;
			input[1].mass.xyz = fixed3(0, 0, 1) + quad;
			input[2].mass.xyz = fixed3(0, 1, 0) + quad;

		#else
	
			input[0].mass.xyz = fixed3(1, 0, 0);
			input[1].mass.xyz = fixed3(0, 1, 0);
			input[2].mass.xyz = fixed3(0, 0, 1);

		#endif

	#endif


	triStream.Append( input[0] );
	triStream.Append( input[1] );
    triStream.Append( input[2] );

	triStream.RestartStrip();
} 

#endif
