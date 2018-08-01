// AFS GRASSMESHPOSTPROCESSOR
// Adds vertex color and stores bending in vertex color alpha according to the local position of the given vertex

#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

internal class AFSGrassMeshPostprocessor : AssetPostprocessor {

	public const string GMSuffix = "_AfsGM";

	public void OnPostprocessModel(GameObject GrassMesh) {
		if (assetPath.Contains(GMSuffix)) {
			string filename = Path.GetFileNameWithoutExtension(assetPath);
			Debug.Log("Processing Mesh: " + filename);

			int index = filename.IndexOf(GMSuffix, 0);
			string filename_Remainder = filename.Substring(index+GMSuffix.Length, 2);
			float maxBending = float.Parse(filename_Remainder);
			maxBending *= 0.1f;
			maxBending = Mathf.Clamp(maxBending, 0.0f, 1.0f);

			Debug.Log("Max Bending: " + maxBending);

			Mesh currentMesh = GrassMesh.GetComponent<MeshFilter>().sharedMesh;
			if (currentMesh.subMeshCount < 2) {
				Vector3[] vertices = currentMesh.vertices;
				Color[] colors = currentMesh.colors;
				// Create vertex color in case there are no
				if (colors.Length == 0) {
					colors = new Color[vertices.Length];
					for (int i = 0; i < vertices.Length; i++) {
						colors[i] = new Color(1.0f,1.0f,1.0f,0.0f);
					}
				}

				Bounds bounds = currentMesh.bounds;
				for (int i = 0; i < vertices.Length; i++) {
					colors[i].r = 1.0f;
					colors[i].g = 1.0f;
					colors[i].b = 1.0f;
					if (vertices[i].y <= 0.0f) {
						colors[i].a = 0.0f;
					}
					else {
						colors[i].a = Mathf.Lerp (0.0f, maxBending, vertices[i].y/bounds.size.y );
					}
				}
				// Update mesh
				currentMesh.colors = colors;
			}
		}	
	}
}

#endif