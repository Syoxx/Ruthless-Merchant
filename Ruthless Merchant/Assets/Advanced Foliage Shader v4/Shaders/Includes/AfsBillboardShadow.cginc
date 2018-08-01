float4 _AfsBillboardCameraForward;
float4 _AfsBillboardShadowCameraForward;
float4 _AfsSunDirection;

#define halfPI 1.5707963267949

void AfsTerrainBillboardTree( inout float4 pos, float2 offset, float offsetz )
{
//	not needed here as we fade out the billboards
	//float3 treePos = pos.xyz - _TreeBillboardCameraPos.xyz;
	//float treeDistanceSqr = dot(treePos, treePos);
	//if( treeDistanceSqr > _TreeBillboardDistances.x )
	//	offset.xy = offsetz = 0.0;
		
//	positioning of billboard vertices horizontaly
	pos.xyz += _TreeBillboardCameraRight.xyz * offset.x;
	
	// _TreeBillboardCameraPos.w contains ImposterRenderTexture::billboardAngleFactor
	float billboardAngleFactor = _TreeBillboardCameraPos.w;
	
//
//		
//	this fixex the trunk on the terrain and aligns the billboard to the camera
	float copyoffset_y_factor = saturate ( floor(offset.y + 0.5));
	float copyoffset_y = offset.y * copyoffset_y_factor - 12.0 * billboardAngleFactor * (1 - copyoffset_y_factor);
	pos.xyz += _AfsBillboardCameraForward.xyz * copyoffset_y * (halfPI + billboardAngleFactor );
//

	// The following line performs two things:
	// 1) peform non-uniform scale, see "3) incorrect compensating (using lerp)" above
	// 2) blend between vertical and horizontal billboard mode
	
	float radius = lerp(offset.y, offsetz, billboardAngleFactor);	
	// positioning of billboard vertices verticaly

/////
// test: prevent from stretching? float3(1,1-billboardAngleFactor,1);

	pos.xyz += _TreeBillboardCameraUp.xyz * radius; // * float3(1,1-billboardAngleFactor,1);
		// would nearly do the trick...
		// pos.xyz += _TreeBillboardCameraUp.zyx * radius;
	
	// _TreeBillboardCameraUp.w contains ImposterRenderTexture::billboardOffsetFactor
	float billboardOffsetFactor = _TreeBillboardCameraUp.w;
	// Offsetting billboad from the ground, so it doesn't get clipped by ztest.
	// In theory we should use billboardCenterOffsetY instead of offset.x,
	// but we can't because offset.y is not the same for all 4 vertices, so 
	// we use offset.x which is the same for all 4 vertices (except sign). 
	// And it doesn't matter a lot how much we offset, we just need to offset 
	// it by some distance
	pos.xyz += _TreeBillboardCameraFront.xyz * abs(offset.x) * billboardOffsetFactor;

}

void AfsTerrainBillboardTreeShadow( inout float4 pos, float2 offset, float offsetz )
{
	//float3 crossProd = cross(_AfsSunDirection.xyz, float3(0,1,0) ) ;

//	positioning of billboard vertices horizontaly
	// scale * 1.3
	pos.xyz += _AfsSunDirection.xyz * offset.x * 1.3;
	
	
	// _TreeBillboardCameraPos.w contains ImposterRenderTexture::billboardAngleFactor
	float billboardAngleFactor = _TreeBillboardCameraPos.w;
	
//
//		
//	this fixex the trunk on the terrain and aligns the billboard to the camera
	float copyoffset_y_factor = saturate ( floor(offset.y + 0.5));
	float copyoffset_y = offset.y * copyoffset_y_factor - 12.0 * billboardAngleFactor * (1 - copyoffset_y_factor);
	pos.xyz += _AfsBillboardShadowCameraForward.xyz * copyoffset_y * (halfPI + billboardAngleFactor ); // 1.4
//

	// The following line performs two things:
	// 1) peform non-uniform scale, see "3) incorrect compensating (using lerp)" above
	// 2) blend between vertical and horizontal billboard mode
	
	float radius = lerp(offset.y, offsetz, billboardAngleFactor);	
	// positioning of billboard vertices verticaly
	pos.xyz += _TreeBillboardCameraUp.xyz * radius;
		// would nearly do the trick...
		// pos.xyz += _TreeBillboardCameraUp.zyx * radius;
	
	// _TreeBillboardCameraUp.w contains ImposterRenderTexture::billboardOffsetFactor
	float billboardOffsetFactor = _TreeBillboardCameraUp.w;
	// Offsetting billboad from the ground, so it doesn't get clipped by ztest.
	// In theory we should use billboardCenterOffsetY instead of offset.x,
	// but we can't because offset.y is not the same for all 4 vertices, so 
	// we use offset.x which is the same for all 4 vertices (except sign). 
	// And it doesn't matter a lot how much we offset, we just need to offset 
	// it by some distance
	pos.xyz += _TreeBillboardCameraFront.xyz * abs(offset.x) * billboardOffsetFactor;	
	
	
	//stretch
	//pos.xz = pos.xz + _AfsSunDirection.xz * billboardAngleFactor * 10 * copyoffset_y;
	//	pos.xyz = pos.xyz - crossProd.xyz * billboardAngleFactor * billboardAngleFactor * copyoffset_y * 0.1;
	//	pos.xyz += crossProd * billboardAngleFactor * 10;
	
}




