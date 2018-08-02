// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Detail bending
float _AfsWaveSize;
float _WindFrequency;
float4 _AfsTimeFrequency; // x: time * frequency, y: time, zw: turbulence for 2nd bending
float _LeafTurbulence;
//float _GroundLightingAttunation;
#if defined(AFS_TOUCHBENDING)
	float4 _TouchBendingPosition;
	float4 _TouchBendingForce;
	float4x4 _RotMatrix;
#endif
fixed _BendingControls;

inline float4 AfsAnimateVertex (float4 pos, float3 normal, float4 animParams, float variation)
{	
	// animParams.r = branch phase
	// animParams.g = edge flutter factor
	// animParams.b = primary factor
	// animParams.a = secondary factor

//	Preserve Length
	//float origLength = length(pos.xyz);

// 	All computation is done in worldspace
	pos.xyz = mul(unity_ObjectToWorld, pos).xyz;

//	based on original wind bending
	float fDetailAmp = 0.1;
	float fBranchAmp = 0.3;

//	Phases (object, vertex, branch)
	float fObjPhase = frac( (pos.x + pos.z) * _AfsWaveSize ) + variation;
	float fBranchPhase = fObjPhase.x + animParams.r; //---> fObjPhase + vertex color red
	float fVtxPhase = dot(pos.xyz, animParams.g + fBranchPhase); // controled by vertex color green

//	Animate Wind
	//float sinuswave = _Time.z * _WindFrequency + variation;
	float sinuswave = _AfsTimeFrequency.x + variation;

	float4 TriangleWaves = SmoothTriangleWave(float4( frac( (pos.x ) * _AfsWaveSize) + sinuswave , frac( (pos.z) * _AfsWaveSize) + sinuswave * 0.8, 0.0, 0.0));
	float Oscillation = TriangleWaves.x + (TriangleWaves.y * TriangleWaves.y);
	Oscillation = (Oscillation + 3.0) * 0.33 * _Wind.w;	

	//	x is used for edges; y is used for branches float2(_Time.y, _Time.z) // 0.193
	// float2 vWavesIn = _Time.yy + float2(fVtxPhase, fBranchPhase);
	float2 vWavesIn = _AfsTimeFrequency.y + float2(fVtxPhase, fBranchPhase);

	//float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
	float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, lerp(float2(0.375, 0.193), _AfsTimeFrequency.zw, _LeafTurbulence )) ) * 2.0 - 1.0);
	vWaves = SmoothTriangleWave( vWaves );
	float2 vWavesSum = vWaves.xz + vWaves.yw;

//	Edge (xz) controlled by vertex green and branch bending (y) controled by vertex alpha
	float3 bend = animParams.g * fDetailAmp * normal.xyz * sign(normal.xyz); // sign important to match normals of both faces!!! otherwise edge fluttering might be corrupted.
	bend.y = animParams.a * fBranchAmp;

//	Secondary bending
	pos.xyz += ( (vWavesSum.xyx * bend) + (_Wind.xyz * vWavesSum.y * animParams.a) ) * Oscillation;

//	Preserve Length / would need single game objects...
//	pos.xyz = normalize(pos.xyz) * origLength;

//	Primary bending / Displace position
	pos.xyz += animParams.b * _Wind.xyz * Oscillation;

//	Preserve Length // Does not work in worldspace...
//	pos.xyz = normalize(pos.xyz) * origLength;

//	Touch bending
	#if defined(AFS_TOUCHBENDING)
	//	Primary displacement by touch bending
		pos.xyz += animParams.a * _TouchBendingForce.xyz * _TouchBendingForce.w;
	//	Touch rotation
		pos.xyz = lerp( pos.xyz, mul(_RotMatrix, float4(pos.xyz - _TouchBendingPosition.xyz, 0)).xyz + _TouchBendingPosition.xyz, animParams.b * 10 * (1 + animParams.r ) );

	#endif

//	bring pos back to local space
	pos.xyz = mul(unity_WorldToObject, pos).xyz;
	return pos;	
}

void AfsFoligeBending_DepthNormal (inout appdata_full v)
{
	float4 bendingCoords;

	#if defined(VERTEXLIT)
		#if (SHADER_API_D3D9) || (SHADER_API_D3D11)
				bendingCoords.xyzw = v.color.zyxx;
		#else
			bendingCoords.xyzw = v.color.xyzz;
		#endif
	#else
		bendingCoords.rg = v.color.rg;

	//	Legacy Bending:	Primary and secondary bending stored in vertex color blue
	//	New Bending:	Primary and secondary bending stored in uv4
	//	x = primary bending = blue
	//	y = secondary = alpha
		bendingCoords.ba = (_BendingControls == 0) ? v.color.bb : v.texcoord3.xy;
	#endif

//	Add variation only if the shader uses UV4
	float variation = (_BendingControls == 0) ? 1.0 : v.color.b * 2;
//	
	v.vertex = AfsAnimateVertex (v.vertex, v.normal, bendingCoords, variation);
	
//	also animate the normal
	#if defined(AFS_TOUCHBENDING)
		v.normal = lerp( v.normal , mul(_RotMatrix, float4(v.normal, 0)).xyz, bendingCoords.b * 10 * (1 + bendingCoords.r ) );
	#endif
	v.normal = normalize(v.normal);
	v.tangent.xyz = normalize(v.tangent.xyz);
}