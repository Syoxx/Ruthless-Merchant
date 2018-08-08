// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// unity 5 changed the rules: we can not have different "squash" animations on the mesh and the shadow caster... both have to match
// --> no real time shadows casted by billboards? or no transition using squash?


#ifndef UNITY_BUILTIN_3X_TREE_LIBRARY_INCLUDED
#define UNITY_BUILTIN_3X_TREE_LIBRARY_INCLUDED

// Shared tree shader functionality for Unity 3.x Tree Creator shaders

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "TerrainEngine.cginc"

#define halfPI 1.5707963267949

fixed4 _Color;
fixed3 _TranslucencyColor;
fixed _TranslucencyViewDependency;
half _ShadowStrength;

//
float4 _AfsTerrainTrees;
float4 _AFSWindMuliplier;
float4 _AfsBillboardCameraForward;
float _AfsXtraBending;

fixed _BendingControls;

#if defined (LEAFTUMBLING)
	float _TumbleStrength;
	float _TumbleFrequency;
#endif

struct LeafSurfaceOutput {
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	fixed Translucency;
	half Specular;
	fixed Gloss;
	fixed Alpha;
	//
	fixed ShadowCutOff;
};

inline half4 LightingTreeLeaf (LeafSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
{
	half3 h = normalize (lightDir + viewDir);

//	Fix for Unity 5 / not needed anymore?
	//half3 fixedNormal = normalize(s.Normal);
	half nl = dot (s.Normal, lightDir);
	half nh = max (0, dot (s.Normal, h));
	float n = s.Specular * 128.0;
	half spec = pow (nh, n) * s.Gloss * s.ShadowCutOff * s.ShadowCutOff ; // fade out specular
//	Normalize BlinnPhong: http://www.thetenthplanet.de/archives/255
//	spec *= (n + 1.0) / (UNITY_PI * 2);
	// view dependent back contribution for translucency
	fixed backContrib = saturate(dot(viewDir, -lightDir));
	// normally translucency is more like -nl, but looks better when it's view dependent
	backContrib = lerp(saturate(-nl), backContrib, _TranslucencyViewDependency);
	fixed3 translucencyColor = backContrib * s.Translucency * _TranslucencyColor;
	// wrap-around diffuse
	half diff = max(0, nl * 0.6 + 0.4);
	
	fixed4 c;
	/////@TODO: what is is this multiply 2x here???
	c.rgb = s.Albedo * (translucencyColor * 2 + diff);
	c.rgb = c.rgb * _LightColor0.rgb + spec * _LightColor0.rgb * saturate(nl);
	// For directional lights, apply less shadow attenuation
	// based on shadow strength parameter.
	#if defined(DIRECTIONAL) || defined(DIRECTIONAL_COOKIE)
		atten = lerp(fixed(1), atten, s.ShadowCutOff);
		c.rgb *= lerp(1, atten, _ShadowStrength);
	#else
		atten = lerp(fixed(0), atten, s.ShadowCutOff);
		c.rgb *= atten;
	#endif
	return c;
}

float4 AfsSmoothTriangleWave( float4 x ) {   
	return (SmoothCurve( TriangleWave( x )) - 0.5) * 2.0;   
}

// see http://www.neilmendoza.com/glsl-rotation-about-an-arbitrary-axis/
float3x3 AfsRotationMatrix(float3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;

    return float3x3	(	oc * axis.x * axis.x + c,			oc * axis.x * axis.y - axis.z * s,	oc * axis.z * axis.x + axis.y * s,
                		oc * axis.x * axis.y + axis.z * s,	oc * axis.y * axis.y + c,			oc * axis.y * axis.z - axis.x * s,
                		oc * axis.z * axis.x - axis.y * s,	oc * axis.y * axis.z + axis.x * s,	oc * axis.z * axis.z + c);   
}

// taken from afs3
inline float4 AfsSquashNew(in float4 pos, float SquashNew)
{
	float3 planeNormal = _SquashPlaneNormal.xyz;
	float3 projectedVertex = pos.xyz - (dot(planeNormal, pos.xyz) + _SquashPlaneNormal.w) * planeNormal + _AfsBillboardCameraForward.xyz * halfPI * pos.y;
	pos = float4(lerp(projectedVertex, pos.xyz, max(0.05f,SquashNew)), 1.0f);
	return pos;
}

// Detail bending
// pos.w = per leaf tumble strength
inline float4 afsAnimateVertex(float4 pos, float4 normal_fOsc, float4 animParams, float4 i_pivot_fade)
{	
	// animParams stored in color
	// animParams.x = branch phase
	// animParams.y = edge flutter factor
	// animParams.z = primary factor
	// animParams.w = secondary factor

	#define branchOrigin i_pivot_fade.xyz
	#define vertexnormal normal_fOsc.xyz
	#define Oscillation normal_fOsc.w
	#define tumbleStrength pos.w

//	Adjust and fade in wind
	float _SquashTwo = i_pivot_fade.w; // * i_TreeWorldPos.w; //_SquashAmount * _SquashAmount;
	_Wind.w *= _AFSWindMuliplier.w;
	_Wind.xyz = (_Wind.xyz * Oscillation) * _AFSWindMuliplier.xyz;
	_Wind *= _SquashTwo;

	float fDetailAmp = 0.1f;
	float fBranchAmp = 0.3f;
	
	// Phases (object, vertex, branch)
	float fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
	float fBranchPhase = fObjPhase + animParams.x;
	float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
	
	// x is used for edges; y is used for branches
	float2 vWavesIn = _Time.yy + float2(fVtxPhase, fBranchPhase );
	// 1.975, 0.793, 0.375, 0.193 are good frequencies
	float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
	vWaves = SmoothTriangleWave( vWaves );
	float2 vWavesSum = vWaves.xz + vWaves.yw;

//	Tumbling / Has to be done befor all other deformations
	#if defined (LEAFTUMBLING)
		if(_TumbleStrength > 0 && tumbleStrength > 0 && _SquashTwo > 0) {
			// _Wind.w is turbulence
			// Move point to 0,0,0
			pos.xyz -= branchOrigin;
			// Add variance to the different leaf planes
			float3 fracs = frac(branchOrigin * 33.3 + pos.w);
	 		float offset = fracs.x + fracs.y + fracs.z;
			float tFrequency = _TumbleFrequency * _Time.y;
			float4 vWaves1 = SmoothTriangleWave( float4(tFrequency + offset, tFrequency * 0.75 - offset, tFrequency * 0.1 + offset, tFrequency * 1.0 + offset));
			// float4 vWaves1 = SmoothTriangleWave( float4(tFrequency + offset, tFrequency * 0.75 - offset, 1, 1));
			// TODO: check this!
			// float3 windTangent = cross(normalize(branchOrigin), _Wind.xyz);
			float3 windTangent = cross(float3(0,1,0), _Wind.xyz);
			float twigPhase = vWaves1.x + vWaves1.y * vWaves1.y;
			float windStrength = dot(_Wind.xyz, 1) * pos.w;
			pos.xyz = mul(AfsRotationMatrix(windTangent, windStrength * (twigPhase + fBranchPhase * 0.1) * _TumbleStrength * tumbleStrength * _SquashTwo ), pos.xyz);
			// Move point back to origin
			pos.xyz += branchOrigin;
		}
	#endif

//	Preserve Length
	float origLength = length(pos.xyz);

	// Edge (xz) and branch bending (y)
	float3 bend = animParams.y * fDetailAmp * vertexnormal * sign(vertexnormal);

	#if defined (XLEAFBENDING)
		// As bark might be effected too we should simplify it
		bend.y = (animParams.w + animParams.y * _AfsXtraBending) * (fBranchAmp * ( (vWaves.y - vWaves.z) * 0.5 * frac( (pos.x + pos.z)) ) );
		// bend.y = (animParams.w + animParams.y * _AfsXtraBending) * fBranchAmp;
	#else
		bend.y = animParams.w * fBranchAmp;
	#endif
//	Apply secondary bending and edge fluttering
	pos.xyz += ((vWavesSum.xyx * bend) + (_Wind.xyz * vWavesSum.y * animParams.w)) * _Wind.w;

//	Primary bending / Displace position
	pos.xyz += animParams.z * _Wind.xyz;

//	Preserve Length
	pos.xyz = normalize(pos.xyz) * origLength;
	
	return pos;
}


void AfsTreeVertBark_DepthNormal (inout appdata_full v)
{
	float3 TreeWorldPos = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);
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
	v.vertex = AfsSquashNew(v.vertex, _SquashAmount);
	//v.color.rgb = _TreeInstanceColor.rgb * _Color.rgb;
	v.normal = normalize(v.normal);
	v.tangent.xyz = normalize(v.tangent.xyz); 
}

void AfsTreeVertLeaf_DepthNormal (inout appdata_full v)
{
	float3 TreeWorldPos = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);

	ExpandBillboard (UNITY_MATRIX_IT_MV, v.vertex, v.normal, v.tangent);
	v.vertex.xyz *= _TreeInstanceScale.xyz;
//	Decode UV3
	float3 pivot;
	#if defined(LEAFTUMBLING)
		//pivot = (frac(float3(1.0f, 256.0f, 65536.0f) * v.texcoord2.x) * 2) - 1;
		pivot = (frac(float3(1.0f, 1024.0f, 1048576.0f) * v.texcoord2.x) * 2) - 1;
		pivot *= v.texcoord2.y;
	#endif
//	Add extra animation to make it fit speedtree
	TreeWorldPos.xyz = abs(TreeWorldPos.xyz);
	float sinuswave = _SinTime.z;
	float4 vOscillations = AfsSmoothTriangleWave(float4(TreeWorldPos.x + sinuswave , TreeWorldPos.z + sinuswave * 0.8, 0.0, 0.0));
	float fOsc = vOscillations.x + (vOscillations.y * vOscillations.y);
	fOsc = (fOsc + 3.0) * 0.33;
//	Apply Wind
	v.vertex = afsAnimateVertex( float4(v.vertex.xyz, v.color.b), float4(v.normal.xyz,fOsc), float4(v.color.xy, v.texcoord1.xy), float4(pivot,_SquashAmount) );
	v.vertex = AfsSquashNew(v.vertex, _SquashAmount);
	//v.color.rgb = _TreeInstanceColor.rgb * _Color.rgb;
	v.normal = normalize(v.normal);
	v.tangent.xyz = normalize(v.tangent.xyz);
}

void AfsTreeVertSimple_DepthNormal (inout appdata_full v)
{
	float3 TreeWorldPos = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);

	ExpandBillboard (UNITY_MATRIX_IT_MV, v.vertex, v.normal, v.tangent);
	v.vertex.xyz *= _TreeInstanceScale.xyz;
//	Decode UV3
	float3 pivot;
	#if defined(LEAFTUMBLING)
		//pivot = (frac(float3(1.0f, 256.0f, 65536.0f) * v.texcoord2.x) * 2) - 1;
		pivot = (frac(float3(1.0f, 1024.0f, 1048576.0f) * v.texcoord2.x) * 2) - 1;
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
	v.vertex = AfsSquashNew(v.vertex, _SquashAmount);
	//v.color.rgb = _TreeInstanceColor.rgb * _Color.rgb;
	v.normal = normalize(v.normal);
	v.tangent.xyz = normalize(v.tangent.xyz);
}
#endif // UNITY_BUILTIN_3X_TREE_LIBRARY_INCLUDED
