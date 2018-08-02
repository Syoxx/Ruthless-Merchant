#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;

[CustomEditor (typeof(touchBendingCollisionGS))]
public class touchBendingCollisionGSEditor : Editor {

	public override void OnInspectorGUI () {
		touchBendingCollisionGS script = (touchBendingCollisionGS)target;
		
		// Styles
		Color myCol = new Color(.5f, .8f, .0f, 1f); // Light Green
		if (!EditorGUIUtility.isProSkin) {
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); // Dark Green
		}

		// Custom Label
		GUIStyle myLabel = new GUIStyle(EditorStyles.label);
		myLabel.fontStyle = FontStyle.Bold;
		myLabel.normal.textColor = myCol;
		myLabel.onNormal.textColor = myCol;

		//
		// EditorGUILayout.BeginVertical("Box");
		GUILayout.Space(10);
		GUILayout.Label ("Set up Touch Bending", myLabel);
		GUILayout.Space(4);
		
		script.simpleBendingMaterial = (Material)EditorGUILayout.ObjectField("Regular material", script.simpleBendingMaterial, typeof(Material), false);
		script.touchBendingMaterial = (Material)EditorGUILayout.ObjectField("Touch bending material", script.touchBendingMaterial, typeof(Material), false);
		
		GUILayout.Space(5);
		if (GUILayout.Button("Sync Touch bending Material")) {
			syncTouchBendingMaterial();
		}
		
		GUILayout.Space(10);
		script.stiffness = EditorGUILayout.Slider("Bendability", script.stiffness, 0.01f, 50.0f);
		script.disturbance = EditorGUILayout.Slider("Disturbance", script.disturbance, 0.01f, 10.0f); 
		script.duration = EditorGUILayout.Slider("Duration", script.duration, 0.1f, 20.0f); 
		
		//EditorGUILayout.EndVertical();
		EditorUtility.SetDirty(script);
	}
	
	public void syncTouchBendingMaterial() {
		touchBendingCollisionGS script = (touchBendingCollisionGS)target;
		script.touchBendingMaterial.SetTexture("_MainTex", script.simpleBendingMaterial.GetTexture ("_MainTex") );
		if (script.simpleBendingMaterial.GetTexture ("_BumpTransSpecMap")){
			script.touchBendingMaterial.SetTexture("_BumpTransSpecMap", script.simpleBendingMaterial.GetTexture ("_BumpTransSpecMap") );	
		}
		
		script.touchBendingMaterial.SetFloat("_Cutoff", script.simpleBendingMaterial.GetFloat ("_Cutoff") );
		// Regular Shader
		if (script.touchBendingMaterial.HasProperty("_Shininess") && script.simpleBendingMaterial.HasProperty("_Shininess")) {
			script.touchBendingMaterial.SetFloat("_Shininess", script.simpleBendingMaterial.GetFloat ("_Shininess") );
		}
		// Specular Reflectivity
		if (script.touchBendingMaterial.HasProperty("_SpecularReflectivity") && script.simpleBendingMaterial.HasProperty("_SpecularReflectivity")) {
			script.touchBendingMaterial.SetColor("_SpecularReflectivity", script.simpleBendingMaterial.GetColor ("_SpecularReflectivity") );
		}
		script.touchBendingMaterial.SetColor("_TranslucencyColor", script.simpleBendingMaterial.GetColor ("_TranslucencyColor") );
		script.touchBendingMaterial.SetFloat("_TranslucencyViewDependency", script.simpleBendingMaterial.GetFloat ("_TranslucencyViewDependency") );
		//script.touchBendingMaterial.SetFloat("_LeafTurbulence", script.simpleBendingMaterial.GetFloat ("_LeafTurbulence") );
		script.touchBendingMaterial.SetFloat("_BendingControls", script.simpleBendingMaterial.GetFloat ("_BendingControls") );
		// Rain Drops
		if (script.touchBendingMaterial.HasProperty("_RainTexScale") && script.simpleBendingMaterial.HasProperty("_RainTexScale")) {
			script.touchBendingMaterial.SetFloat("_RainTexScale", script.simpleBendingMaterial.GetFloat ("_RainTexScale") );
			if (script.simpleBendingMaterial.GetTexture ("_BumpRain") != null) {
				script.touchBendingMaterial.SetTexture("_BumpRain", script.simpleBendingMaterial.GetTexture ("_BumpRain") );
			}
			if (script.simpleBendingMaterial.GetTexture ("_MaskRain") != null) {
				script.touchBendingMaterial.SetTexture("_MaskRain", script.simpleBendingMaterial.GetTexture ("_MaskRain") );
			}
		}
		
		EditorUtility.SetDirty(script);
	}
}
#endif
