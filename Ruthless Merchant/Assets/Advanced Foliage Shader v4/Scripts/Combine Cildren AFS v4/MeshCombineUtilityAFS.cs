// optimized version of the original "MeshCombineUtility"
// 

using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif
using System.Collections;

public class MeshCombineUtilityAFS {
	
	public struct MeshInstance
	{
		public Mesh      mesh;
		public int       subMeshIndex;
		public Matrix4x4 transform;
		
		public Vector3   groundNormal;
		public float     scale;
		public Vector3   pivot;
	}
	
	public static Mesh Combine (MeshInstance[] combines, bool bakeGroundLightingGrass, bool bakeGroundLightingFoliage, float randomBrightness, float randomPulse, float randomBending, float randomFluttering, Color HealthyColor, Color DryColor, float NoiseSpread, bool bakeScale, bool simplyCombine, float NoiseSpreadFoliage, bool createUniqueUV2, bool useUV4
)
	{
		int vertexCount = 0;
		int triangleCount = 0;
		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
			{
				vertexCount += combine.mesh.vertexCount;
				triangleCount += combine.mesh.GetTriangles(combine.subMeshIndex).Length;
			}
		}

		Vector3[] vertices = new Vector3[vertexCount] ;
		Vector3[] normals = new Vector3[vertexCount] ;
		Vector4[] tangents = new Vector4[vertexCount] ;
		Vector2[] uv = new Vector2[vertexCount];
//		
		Vector2[] uv1 = new Vector2[vertexCount];
		Color[] colors = new Color[vertexCount];
//
		Vector2[] uv4 = new Vector2[vertexCount];
		
		int[] triangles = new int[triangleCount];
		int offset;
		offset=0;

		bool copyUV4 = false;

		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
				Copy(combine.mesh.vertexCount, combine.mesh.vertices, vertices, ref offset, combine.transform);
		}
		offset=0;

		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
			{
				Matrix4x4 invTranspose = combine.transform;
				invTranspose = invTranspose.inverse.transpose;
				
				//if (bakeGroundLightingGrassTrans) {
				//	CopyNormalGroundTrans (combine.mesh.vertexCount, combine.mesh.normals, normals, ref offset, invTranspose, combine.groundNormal);
				//}
				if (bakeGroundLightingGrass) {
					CopyNormalGround (combine.mesh.vertexCount, combine.mesh.normals, normals, ref offset, invTranspose, combine.groundNormal);
				}
				//else if (!bakeGroundLightingGrassTrans) {
				else {
					CopyNormal(combine.mesh.vertexCount, combine.mesh.normals, normals, ref offset, invTranspose);
				}
			}
		}
		offset=0;

		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
			{
				Matrix4x4 invTranspose = combine.transform;
				invTranspose = invTranspose.inverse.transpose;
				CopyTangents(combine.mesh.vertexCount, combine.mesh.tangents, tangents, ref offset, invTranspose);
			}
				
		}
		offset=0;

		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
				Copy(combine.mesh.vertexCount, combine.mesh.uv, uv, ref offset);
		}
		offset=0;
		
		// only needed when using the foliage shader ground lighting version
		if (bakeGroundLightingFoliage) {
			foreach( MeshInstance combine in combines )
			{
				if (combine.mesh)
					Copy_uv1(combine.mesh.vertexCount, combine.mesh.uv, uv1, ref offset, new Vector2(combine.groundNormal.x, combine.groundNormal.z));
			}
			offset=0;
		}

		// Copy uv4 
		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh.uv4 != null && useUV4)
			{
				//Debug.Log("UV4 found");
				copyUV4 = true;
				Copy_uv4(combine.mesh.vertexCount, combine.mesh.uv4, uv4, ref offset, combine.scale, bakeScale, combine.pivot, NoiseSpreadFoliage, randomBending);
			}
		}
		offset=0;

		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh) {
				// either add healthy and dry colors (grass shader)
				if (bakeGroundLightingGrass) {
					CopyColors_grass(combine.mesh.vertexCount, combine.mesh.colors, colors, ref offset, HealthyColor, DryColor, NoiseSpread, combine.pivot );
				}
				// or simply add random color to create more variety (r,a and b) and bake scale
				else {
					//CopyColors(combine.mesh.vertexCount, combine.mesh.colors, colors, ref offset, new Color (Random.Range(0.0f, randomPulse), Random.Range(-randomFluttering, randomFluttering), Random.Range(-randomBending, randomBending), Random.Range(randomBrightness, -randomBrightness)), combine.scale, bakeScale);
					CopyColors(combine.mesh.vertexCount, combine.mesh.colors, colors, ref offset, combine.scale, bakeScale, combine.pivot, NoiseSpreadFoliage, randomPulse, randomFluttering, randomBrightness, randomBending, copyUV4);
				}
			}
		}
		

		int triangleOffset=0;
		int vertexOffset=0;
		foreach( MeshInstance combine in combines )
		{
			if (combine.mesh)
			{
				int[]  inputtriangles = combine.mesh.GetTriangles(combine.subMeshIndex);
				for (int i=0;i<inputtriangles.Length;i++)
				{
					triangles[i+triangleOffset] = inputtriangles[i] + vertexOffset;
				}
				triangleOffset += inputtriangles.Length;
				vertexOffset += combine.mesh.vertexCount;
				// Clean up
				inputtriangles = null;
			}
		}
		
		Mesh mesh = new Mesh();
		mesh.name = "Combined Mesh";
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.colors = colors;
		mesh.uv = uv;
		// only needed for foliage shader, skip it on grass and plants using the regular shader
		if (bakeGroundLightingFoliage) {
			mesh.uv2 = uv1;
		}

		if (copyUV4) {
			mesh.uv4 = uv4;
		}

		mesh.tangents = tangents;
		mesh.triangles = triangles;
		;

		#if UNITY_EDITOR
			if(createUniqueUV2 && !bakeGroundLightingFoliage) {
				Unwrapping.GenerateSecondaryUVSet(mesh);
			}
		#endif



		// Clean up
		vertices = null;
		normals = null;
		tangents = null;
		uv = null;
		uv1 = null;
		colors = null;
		triangles = null;

		return mesh;
	}
	
	static void Copy (int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i=0;i<src.Length;i++)
			dst[i+offset] = transform.MultiplyPoint(src[i]);
		offset += vertexcount;
	}

// copy mesh normals
	static void CopyNormal (int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i=0;i<src.Length;i++)
			dst[i+offset] = transform.MultiplyVector(src[i]).normalized;
		offset += vertexcount;
	}
	
// overwrite the mesh’s normals by the groundNormal (grass shader)
	static void CopyNormalGround (int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform, Vector3 groundNormal)
	{
		for (int i=0;i<src.Length;i++)
			dst[i+offset] = groundNormal;
		offset += vertexcount;
	}

// copy uvs
	static void Copy (int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
		for (int i=0;i<src.Length;i++)
			dst[i+offset] = src[i];
		offset += vertexcount;
	}

// copy uv4s
	static void Copy_uv4 (int vertexcount, Vector2[] src, Vector2[] dst, ref int offset, float scale, bool bakeScale, Vector3 pivot, float NoiseSpread, float randomBending)
	{
		float noise = Mathf.PerlinNoise(pivot.x, pivot.y );
		if (!bakeScale) {
			scale = 1.0f;
		}
		for (int i=0;i<src.Length;i++) {
			dst[i+offset] = new Vector2(src[i].x * scale * (1.0f - randomBending * noise ), src[i].y);
			//dst[i+offset] = new Vector2(src[i].x * scale, src[i].y);
		}
		offset += vertexcount;
	}
	
// store ground normal in uv1 (foliage shader)
	static void Copy_uv1 (int vertexcount, Vector2[] src, Vector2[] dst, ref int offset, Vector2 groundNormal)
	{
		for (int i=0;i<src.Length;i++)
			dst[i+offset] = groundNormal;
		offset += vertexcount;
	}
	
// add random colors (rgba) to create more variety (foliage shader)
	static void CopyColors (int vertexcount, Color[] src, Color[] dst, ref int offset, float scale, bool bakeScale, Vector3 pivot, float NoiseSpread, float randomPulse, float randomFluttering, float randomBrightness, float randomBending, bool copyUV4)
	{
		float noise = Mathf.PerlinNoise(pivot.x * pivot.x, pivot.z * pivot.z );
		float noise1 = Mathf.PerlinNoise(pivot.x * pivot.x * pivot.x, pivot.z * pivot.z * pivot.z );
		for (int i=0;i<src.Length;i++) {
			// compress ambient occlusion value and add scale
			// src[i].a = (4.0f * Mathf.Clamp(src[i].a * 255.0f / 4.0f, 0.0f, 63.0f) + Scale ) / 255.0f;
			// phase (only plus)
			src[i].r += randomPulse * noise * 0.25f;
			//fluttering (plus / minus)
			src[i].g = src[i].g * (1 + randomFluttering * (noise - 0.5f) );
			//bending (only plus)
			if (bakeScale) {
				// scale
				src[i].b = src[i].b * scale * (1 + randomBending * noise);
			}
			else {
				src[i].b = src[i].b * (1 + randomBending * noise);	
			}
			//	add variation per mesh 
			if (copyUV4){
				src[i].b = randomPulse * noise1 * noise1;
			}
			//brightness (only darken)
			src[i].a = src[i].a - (noise * randomBrightness);
			dst[i+offset] = src[i];
			}
		offset += vertexcount;
	}
	
// deprecated: store ground normal (grass shader translucency) and add random colors (r and a) to create more variety
	static void CopyColors_groundNormal_old (int vertexcount, Color[] src, Color[] dst, ref int offset, Color RandColor, Vector2 groundNormal)
	{
		for (int i=0;i<src.Length;i++) {
			dst[i+offset] = src[i] + RandColor;
			dst[i+offset].r = groundNormal.x;
			dst[i+offset].g = groundNormal.y;
		}
		offset += vertexcount;
	}
	
// store dry and healthy colors (grass shader) / src.color.b contains ambient occlusion
	static void CopyColors_grass (int vertexcount, Color[] src, Color[] dst, ref int offset, Color HealthyColor, Color DryColor, float NoiseSpread, Vector3 pivot)
	{
		Color BlendColor = Color.Lerp (HealthyColor, DryColor, Mathf.PerlinNoise(pivot.x * NoiseSpread, pivot.y * NoiseSpread) );
		
		for (int i=0;i<src.Length;i++) {
			dst[i+offset].a = src[i].a;
			dst[i+offset].r = Mathf.Lerp (1.0f, BlendColor.r, BlendColor.a) * src[i].b;
			dst[i+offset].g = Mathf.Lerp (1.0f, BlendColor.g, BlendColor.a) * src[i].b;
			dst[i+offset].b = Mathf.Lerp (1.0f, BlendColor.b, BlendColor.a) * src[i].b;
		}
		offset += vertexcount;
	}
	
	static void CopyTangents (int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i=0;i<src.Length;i++)
		{
			Vector4 p4 = src[i];
			Vector3 p = new Vector3(p4.x, p4.y, p4.z);
			p = transform.MultiplyVector(p).normalized;
			dst[i+offset] = new Vector4(p.x, p.y, p.z, p4.w);
		}
			
		offset += vertexcount;
	}
}
