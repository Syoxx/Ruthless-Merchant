#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;

[CustomEditor (typeof(CombineChildrenAFS))]
public class CombineChildrenAFSEditor : Editor {
	// Serialize
	private SerializedObject CombineChildrenAFS;
	private SerializedProperty GroundMaxDistance;
	private SerializedProperty UnderlayingTerrain;
	private SerializedProperty HealthyColor;
	private SerializedProperty DryColor;
	private SerializedProperty NoiseSpread;
	private SerializedProperty randomBrightness;
	private SerializedProperty randomPulse;
	private SerializedProperty randomBending;
	private SerializedProperty randomFluttering;
	private SerializedProperty NoiseSpreadFoliage;
	private SerializedProperty bakeScale;
	//
	private bool hTerrain = false;
	private bool sGrass = true;
	private bool sFoliage = true;

	public override void OnInspectorGUI () {
		CombineChildrenAFS = new SerializedObject(target);
		GetProperties();
		CombineChildrenAFS script = (CombineChildrenAFS)target;
		
		Color myBlue = new Color(0.5f,0.7f,1.0f,1.0f);
		Color myCol = new Color(.5f, .8f, .0f, 1f); // Light Green
		if (!EditorGUIUtility.isProSkin) {
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); // Dark Green
		}

		
		GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		myFoldoutStyle.fontStyle = FontStyle.Bold;

		myFoldoutStyle.normal.textColor = myCol;
		myFoldoutStyle.onNormal.textColor = myCol;	//	Rendering settings for when the control is turned on but lost focus
		//myFoldoutStyle.hover.textColor = Color.white;
		//myFoldoutStyle.onHover.textColor = Color.white;
		myFoldoutStyle.active.textColor = myCol;
		myFoldoutStyle.onActive.textColor = myCol;
		myFoldoutStyle.focused.textColor = myCol;
		myFoldoutStyle.onFocused.textColor = myCol;

		GUIStyle myBoldLabel = new GUIStyle(EditorStyles.label);
		myBoldLabel.fontStyle = FontStyle.Bold;
		myBoldLabel.normal.textColor = myCol;
		myBoldLabel.onNormal.textColor = myCol;


		//
		if (script.isStaticallyCombined) {
			GUI.enabled = false;
		}
		//

		EditorGUILayout.BeginVertical();
		GUILayout.Space(10);

		
		EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			script.hideChildren = EditorGUILayout.Toggle("", script.hideChildren, GUILayout.Width(14) );
			GUILayout.Label ("Hide Child Objects in Hierarchy");
			EditorGUILayout.EndHorizontal();


		if (EditorGUI.EndChangeCheck()) {
				script.ShowHideChildren();
		}

		
		//	GUI.color = myCol;
		GUILayout.Label ("Ground normal sampling", myBoldLabel); //EditorStyles.boldLabel);
		//	GUI.color = Color.white;
		GUILayout.Space(4);
		EditorGUILayout.PropertyField(GroundMaxDistance, new GUIContent("Max Ground Distance") );
		GUILayout.Space(4);
		// underlaying terrain
		//script.UnderlayingTerrain = (Terrain)EditorGUILayout.ObjectField("Underlaying Terrain", script.UnderlayingTerrain, typeof(Terrain), true);
		EditorGUILayout.PropertyField(UnderlayingTerrain, new GUIContent("Underlaying Terrain") );
		GUI.color = myBlue;
		hTerrain = EditorGUILayout.Foldout(hTerrain," Help");
		GUI.color = Color.white;
		if(hTerrain){
			EditorGUILayout.HelpBox("If you place the objects of the cluster (all or just a few of them) on top of a terrain you will have to assign the according terrain in order to make lighting fit 100% the terrain lighting.", MessageType.None, true);
		}
		
		// bake grass
		GUILayout.Space(9);
		EditorGUILayout.BeginVertical();
		//	GUI.color = myCol;
		//script.bakeGroundLigthingGrass = EditorGUILayout.Toggle("", script.bakeGroundLigthingGrass, GUILayout.Width(14) );
		sGrass = EditorGUILayout.Foldout(sGrass," Grass Shader Settings", myFoldoutStyle );
		//	GUI.color = Color.white;
		GUILayout.Space(4);
		
		if (sGrass) {
			EditorGUILayout.PropertyField(HealthyColor, new GUIContent("Healthy Color") );
			EditorGUILayout.PropertyField(DryColor, new GUIContent("Dry Color") );
			EditorGUILayout.PropertyField(NoiseSpread, new GUIContent("Noise Spread") );
		}
		EditorGUILayout.EndVertical();
		
		// bake foliage
		GUILayout.Space(9);
		EditorGUILayout.BeginVertical();
		//script.bakeGroundLightingFoliage = EditorGUILayout.Toggle("", script.bakeGroundLightingFoliage, GUILayout.Width(14) );
		//GUI.color = myCol;
		sFoliage = EditorGUILayout.Foldout(sFoliage," Foliage Shader Settings", myFoldoutStyle);
		//GUI.color = Color.white;
		GUILayout.Space(4);
		if (sFoliage) {
			EditorGUILayout.PropertyField(randomBrightness, new GUIContent("Random Brightness") );
			EditorGUILayout.PropertyField(randomPulse, new GUIContent("Random Pulse") );
			EditorGUILayout.PropertyField(randomBending, new GUIContent("Random Bending") );
			EditorGUILayout.PropertyField(randomFluttering, new GUIContent("Random Fluttering") );
			EditorGUILayout.PropertyField(NoiseSpreadFoliage, new GUIContent("Noise Spread") );
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(bakeScale, GUIContent.none, GUILayout.Width(14) );
				GUILayout.Label ("Bake Scale");
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		
		// overall settings
		GUILayout.Space(9);
		//GUI.color = myCol;
		GUILayout.Label ("Overall Settings", myBoldLabel);
		//GUI.color = Color.white;
		GUILayout.Space(4);
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginHorizontal();
			script.CastShadows = EditorGUILayout.Toggle("", script.CastShadows, GUILayout.Width(14) );
			GUILayout.Label ("Cast Shadows" );
			EditorGUILayout.EndHorizontal();
		GUILayout.Space(4);
			EditorGUILayout.BeginHorizontal();
			script.destroyChildObjectsInPlaymode = EditorGUILayout.Toggle("", script.destroyChildObjectsInPlaymode, GUILayout.Width(14) );
			GUILayout.Label ("Destroy Children" );
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(4);
			EditorGUILayout.BeginHorizontal();
			script.UseLightprobes = EditorGUILayout.Toggle("", script.UseLightprobes, GUILayout.Width(14) );
			GUILayout.Label ("Use Lightprobes" );
			EditorGUILayout.EndHorizontal();
			//if (script.UseLightprobes) {
			//	EditorGUILayout.HelpBox("Please make sure that 'Enable IBL' is disabled in the 'Setup Advanced Foliage Shader' script. Otherwise Lightprobes are not supported.", MessageType.Warning, true);
			//}
		GUILayout.Space(4);
		//if (script.destroyChildObjectsInPlaymode) {
		//	EditorGUILayout.BeginHorizontal();
		//	GUILayout.Label ("", GUILayout.Width(16) );
		//	EditorGUILayout.HelpBox("If this option is checked, child objects will also be destroyed if you hit 'Combine statically'.", MessageType.Warning, true);	
		//	EditorGUILayout.EndHorizontal();
		//}
		
	
		
		
		// debugging settings
		GUILayout.Space(9);
		//GUI.color = myCol;
		GUILayout.Label ("Debugging", myBoldLabel);
		//GUI.color = Color.white;
		GUILayout.Space(4);
		EditorGUILayout.BeginHorizontal();
		script.debugNormals = EditorGUILayout.Toggle("", script.debugNormals, GUILayout.Width(14) );
		GUILayout.Label ("Debug sampled Ground Normals" );
		EditorGUILayout.EndHorizontal();
		if (script.debugNormals) {
			script.EnableDebugging();
		}
		else {
			script.DisableDebugging();
		}
		

		// functions
		GUILayout.Space(9);
		//GUI.color = myCol;
		GUILayout.Label ("Functions", myBoldLabel);
		//GUI.color = Color.white;
		GUILayout.Space(4);
		script.RealignGroundMaxDistance = EditorGUILayout.Slider("Realign Ground max Dist.", script.RealignGroundMaxDistance, 0.0f, 10.0f );
		EditorGUILayout.BeginHorizontal();
			script.ForceRealignment = EditorGUILayout.Toggle("", script.ForceRealignment, GUILayout.Width(14) );
			GUILayout.Label ("Force Realignment" );
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(9);
		EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button( "Realign Objects") ) {
					script.Realign();
			}
			if (GUILayout.Button( "Combine statically") ) {
				if (script.destroyChildObjectsInPlaymode) {
					if ( EditorUtility.DisplayDialog("Combine statically?", "Are you sure you want to combine and destroy all child objects? If you want to be able to edit child objects please uncheck 'Destroy Children' first.", "Combine", "Do not Combine" ) ) {
						script.Combine();
					}
				}
				else {
					if ( EditorUtility.DisplayDialog("Combine statically?", "All child objects will be deactivated.", "Combine", "Do not Combine" ) ) {
						script.Combine();
					}
				}
			}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(9);

		EditorGUILayout.BeginHorizontal();
			script.createUniqueUV2 = EditorGUILayout.Toggle("", script.createUniqueUV2, GUILayout.Width(14) );
			GUILayout.Label ("Create unique UV2 (needed by lightmapper)" );
		EditorGUILayout.EndHorizontal();

		GUI.enabled = true;

		EditorGUILayout.BeginHorizontal();
			script.isStaticallyCombined = EditorGUILayout.Toggle("", script.isStaticallyCombined, GUILayout.Width(14) );
			GUILayout.Label ("Has been statically combined" );
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(9);
		EditorGUILayout.EndVertical();

		if (GUI.changed) {
	   		//script.Update();
	   		EditorUtility.SetDirty(script);
	   		SceneView.RepaintAll();
	  	}
	  	CombineChildrenAFS.ApplyModifiedProperties();
	}
	
	// if the editor looses focus
	void OnDisable() {
		
	}

	//	///////////////////////////////////////////////////
	private void GetProperties() {
		GroundMaxDistance = CombineChildrenAFS.FindProperty("GroundMaxDistance");
		UnderlayingTerrain = CombineChildrenAFS.FindProperty("UnderlayingTerrain");
		HealthyColor = CombineChildrenAFS.FindProperty("HealthyColor");
		DryColor = CombineChildrenAFS.FindProperty("DryColor");
		NoiseSpread = CombineChildrenAFS.FindProperty("NoiseSpread");
		//
		randomBrightness = CombineChildrenAFS.FindProperty("randomBrightness");
		randomPulse = CombineChildrenAFS.FindProperty("randomPulse");
		randomBending = CombineChildrenAFS.FindProperty("randomBending");
		randomFluttering = CombineChildrenAFS.FindProperty("randomFluttering");
		NoiseSpreadFoliage = CombineChildrenAFS.FindProperty("NoiseSpreadFoliage");
		bakeScale = CombineChildrenAFS.FindProperty("bakeScale");
	}		
}
#endif