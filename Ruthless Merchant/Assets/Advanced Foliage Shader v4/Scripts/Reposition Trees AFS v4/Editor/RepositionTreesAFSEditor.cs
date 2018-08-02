#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;

[CustomEditor (typeof(RepositionTreesAFS))]
public class RepositionTreesAFSEditor : Editor {
	
	public GameObject newCollider;
	public String[] treePrefabs;
	
	public String[] TreePrototypeName;
	public bool[] ExcludetreePrototype;
	
	TreePrototype[] MyTreePrototypes;

	public override void OnInspectorGUI () {
		RepositionTreesAFS script = (RepositionTreesAFS)target;
		//Color myBlue = new Color(0.5f,0.7f,1.0f,1.0f);
		Color myCol = new Color(.5f, .8f, .0f, 1f); 		// Light Green
		if (!EditorGUIUtility.isProSkin) {
			// myCol = new Color(0.0f,0.15f,0.55f,1.0f); 	// Dark Blue
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); 		// Dark Green
		}

		GUIStyle myBoldLabel = new GUIStyle(EditorStyles.label);
		myBoldLabel.fontStyle = FontStyle.Bold;
		myBoldLabel.normal.textColor = myCol;
		myBoldLabel.onNormal.textColor = myCol;

//	Assign additional Colliders	
		GUILayout.Space(10);
		GUILayout.Label ("Additional Colliders", myBoldLabel);
		GUILayout.Space(4);
		
		if(script.additionalTerrainObjects != null) {
			for(int i = 0; i < script.additionalTerrainObjects.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				script.additionalTerrainObjects[i] = (GameObject)EditorGUILayout.ObjectField("Additional Collider " + (i+1), script.additionalTerrainObjects[i], typeof(GameObject), true);
				if (GUILayout.Button("Remove", GUILayout.Width(60) )) {
					script.additionalTerrainObjects.RemoveAt(i);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		
		GUILayout.Space(9);
		EditorGUILayout.BeginHorizontal();
		newCollider = (GameObject)EditorGUILayout.ObjectField("Add new Collider", newCollider, typeof(GameObject), true);
		if (GUILayout.Button("Add", GUILayout.Width(60) )) {
			if (newCollider != null) {
				script.additionalTerrainObjects.Add(newCollider);
				newCollider = null;
			}
		}
		EditorGUILayout.EndHorizontal();
		
//	Exclude Prototypes
		GUILayout.Space(10);
		GUILayout.Label ("Exclude Tree Prototypes from Repositioning", myBoldLabel);
		GUILayout.Space(4);
		// Get terrain
		script.UpdatePrototypes();
		// Get prototypes
		MyTreePrototypes = script.myterrainComp.terrainData.treePrototypes;
		for(int k = 0; k < MyTreePrototypes.Length; k++) {
				EditorGUILayout.BeginHorizontal();
				script.ExcludetreePrototype[k] = EditorGUILayout.Toggle("", script.ExcludetreePrototype[k], GUILayout.Width(14) );
				GUILayout.Label (MyTreePrototypes[k].prefab.name);
				EditorGUILayout.EndHorizontal();
		}

//	Update Trees
		GUILayout.Space(10);
		if (GUILayout.Button("Reposition Trees", GUILayout.Height(34) )) {
			script.RepositionTrees();
		}
		GUILayout.Space(5);
	}
	
}
#endif