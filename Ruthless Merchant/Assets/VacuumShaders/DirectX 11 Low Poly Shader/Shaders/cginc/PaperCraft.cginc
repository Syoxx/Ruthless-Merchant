#ifndef VACUUM_SHADERS_DIRECTX_11_LOWPOLY_PAPERCRAFT_CGINC
#define VACUUM_SHADERS_DIRECTX_11_LOWPOLY_PAPERCRAFT_CGINC

#include "Assets/VacuumShaders/The Amazing Wireframe Shader/Shaders/cginc/Wireframe_Core.cginc"

void MakePaperCraft(float3 mass, float3 worldPos, inout float4 finalColor)
{
	float value = ExtructWireframeFromMass(mass, distance(_WorldSpaceCameraPos, worldPos));

	finalColor.rgb = lerp(lerp(finalColor.rgb, _V_WIRE_Color.rgb, _V_WIRE_Color.a), finalColor.rgb, value);
}

#endif
