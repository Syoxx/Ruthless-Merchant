// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Do not use CBUFFER here: dx11 does not like that
CBUFFER_START(AfsGrass)
	float4 _AfsWaveAndDistance;
	float _AfsWindJitterScale;
	fixed4 _AfsWavingTint;
	float4 _AfsGrassWind;
CBUFFER_END

// Is defined by terrain engine:
// float4 _CameraPosition;
// float4 _Wind;

fixed4 WaveGrassMesh (inout float4 vertex, float3 normal, float waveAmount, fixed4 color)
{
//	Start bending

	// doing the animation in worldspace will give us less contrast between manually placed grass
	// and grass within the terrain engine
	float4 worldPos = mul(unity_ObjectToWorld, vertex);

	float4 _waveXSize = float4 (0.012, 0.02, 0.06, 0.024) * _AfsWaveAndDistance.y;
	float4 _waveZSize = float4 (0.006, .02, 0.02, 0.05) * _AfsWaveAndDistance.y;
	float4 waveSpeed = float4 (1.2, 2, 1.6, 4.8) * 2;
	
	float4 _waveXMove = float4(0.024, 0.04, -0.12, 0.096) * _Wind.x; 	// add dir;
	float4 _waveZMove = float4 (0.006, .02, -0.02, 0.1) * _Wind.z; 		// add dir;

	float4 waves;
	waves = worldPos.x * _waveXSize;
	waves += worldPos.z * _waveZSize;
	
	// Add time to model
	// waves += _AfsWaveAndDistance.x * waveSpeed;
	waves += (_AfsWaveAndDistance.x + frac( (worldPos.x + worldPos.z) * 0.0175 )) * waveSpeed;

	float4 s, c;
	waves = frac (waves);
	FastSinCos (waves, s,c);
	
	// It already comes in like this...
	// waveAmount = color.a * _AfsWaveAndDistance.z;

	s = s * s;
	s = s * s; //(s + c.wzyx);

	float lighting = dot (s, normalize (float4 (1,1,.4,.2))) * .7;
	
	s *= waveAmount;

	fixed3 waveColor = lerp (fixed3(1,1,1), _AfsWavingTint.rgb, lighting);
	color.rgb *= waveColor;

	float3 waveMove = float3 (0,0,0);
	waveMove.x = dot (s * c, _waveXMove);
	waveMove.z = dot (s * c, _waveZMove);
	worldPos.xz += _AfsGrassWind.xz * color.a * lighting + _AfsWindJitterScale * waveMove.xz * _AfsWaveAndDistance.z; //waveAmount;
//	End bending

	// Fade the grass out before detail distance.
	// Saturate because Radeon HD drivers on OS X 10.4.10 don't saturate vertex colors properly.
	float3 offset = worldPos.xyz - _WorldSpaceCameraPos;
	color.a = saturate (2 * (_AfsWaveAndDistance.w - dot (offset, offset)) * ( 1 / _AfsWaveAndDistance.w) );
	vertex.xz = mul(unity_WorldToObject, worldPos).xz;
	return fixed4(color.rgb, color.a);
}

fixed4 WaveGrassTerrain (inout float4 vertex, float3 normal, float waveAmount, fixed4 color, float2 UV2)
{
//	Start bending
	float4 _waveXSize = float4 (0.012, 0.02, 0.06, 0.024) * _AfsWaveAndDistance.y;
	float4 _waveZSize = float4 (0.006, .02, 0.02, 0.05) * _AfsWaveAndDistance.y;
	float4 waveSpeed = float4 (1.2, 2, 1.6, 4.8) * 2;
	
	float4 _waveXMove = float4(0.024, 0.04, -0.12, 0.096) * _Wind.x; 	// add dir
	float4 _waveZMove = float4 (0.006, .02, -0.02, 0.1) * _Wind.z;		// add dir

	float4 waves;
	waves = vertex.x * _waveXSize;
	waves += vertex.z * _waveZSize;
	
	// Add time to model

	//waves += _AfsWaveAndDistance.x * waveSpeed;
//	waves += (_AfsWaveAndDistance.x + frac( (vertex.x + vertex.z) * 0.005 )) * waveSpeed; // 0.0375 / 0.0175 / 0.0125

waves += (_AfsWaveAndDistance.x + frac( (UV2.x + UV2.y) * 33.3 )) * waveSpeed; // 0.0375 / 0.0175 / 0.0125
	
	float4 s, c;
	waves = frac (waves);
	FastSinCos (waves, s,c);
	
	// It already comes in like this...
	// waveAmount = color.a * _AfsWaveAndDistance.z;

	s = s * s;
	s = s * s; //(s + c.wzyx);

	float lighting = dot (s, normalize (float4 (1,1,.4,.2))) * .7;
	
	s *= waveAmount;

	fixed3 waveColor = lerp (fixed3(1,1,1), _AfsWavingTint.rgb, lighting);
	color.rgb *= waveColor;

	float3 waveMove = float3 (0,0,0);
	waveMove.x = dot (s * c, _waveXMove);
	waveMove.z = dot (s * c, _waveZMove);
	vertex.xz += _AfsGrassWind.xz * color.a * lighting + _AfsWindJitterScale * waveMove.xz * _AfsWaveAndDistance.z; //waveAmount;

//	End bending
	// Fade the grass out before detail distance.
	// Saturate because Radeon HD drivers on OS X 10.4.10 don't saturate vertex colors properly. // We use original values from the terrain engine here	
	float3 offset = vertex.xyz - _CameraPosition.xyz;
	color.a = saturate (2 * (_WaveAndDistance.w - dot (offset, offset)) * _CameraPosition.w); 
	return fixed4(color.rgb, color.a);
}

void AfsWavingGrassVert_DepthNormal (inout appdata_full v)
{
	float waveAmount = v.color.a * _AfsWaveAndDistance.z;
	v.color = WaveGrassMesh (v.vertex, v.normal, waveAmount, v.color);
}

void AfsWavingGrassVertTerrain_DepthNormal (inout appdata_full v)
{
	float waveAmount = v.color.a * _AfsWaveAndDistance.z;
	v.color = WaveGrassTerrain (v.vertex, v.normal, waveAmount, v.color, v.texcoord1);
}
