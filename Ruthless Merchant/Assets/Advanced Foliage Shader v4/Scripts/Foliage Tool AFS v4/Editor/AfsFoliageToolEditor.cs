#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;

[CustomEditor (typeof(AfsFoliageTool))]
public class AfsFoliageToolEditor : Editor {
	// Serialize
	private SerializedObject AfsFoliageTool;

	private SerializedProperty BendingModeSelected;

	private SerializedProperty Set1stBendingSelected;
	private SerializedProperty Mask2ndBendingSelected;

	private SerializedProperty maxBendingValueY;
	private SerializedProperty maxBendingValueX;
	private SerializedProperty curvY;
	private SerializedProperty curvX;
	private SerializedProperty curvZ;

	private SerializedProperty adjustBlueValue;
	private SerializedProperty adjustGreenValue;
	private SerializedProperty adjustRedValue;
	private SerializedProperty adjustAlphaValue;

	private SerializedProperty TextureSplitSelected;
	
	private bool hVertexColors = false;
	private bool hSetBending = false;
	private bool hSetBendingUV = false;

	private bool showSaveBtn = true;
	
	public Color leColor;
	public string toolTip;

//	Icons
	private Texture icnBending;
	private Texture icnVertexColors;
	private Texture icnMerge;
	private Texture icnTree;

	void OnEnable() {
		if (EditorGUIUtility.isProSkin) {
			if (icnBending == null) icnBending = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnBending.png", typeof(Texture)) as Texture;
			if (icnVertexColors == null) icnVertexColors = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnVertexColors.png", typeof(Texture)) as Texture;
			if (icnMerge == null) icnMerge = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnMerge.png", typeof(Texture)) as Texture;
			if (icnTree == null) icnTree = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnConvTree.png", typeof(Texture)) as Texture;
		}
		else {
			if (icnBending == null) icnBending = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnBending_br.png", typeof(Texture)) as Texture;
			if (icnVertexColors == null) icnVertexColors = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnVertexColors_br.png", typeof(Texture)) as Texture;
			if (icnMerge == null) icnMerge = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnMerge_br.png", typeof(Texture)) as Texture;
			if (icnTree == null) icnTree = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnConvTree_br.png", typeof(Texture)) as Texture;
		}
	}

	public override void OnInspectorGUI () {
		AfsFoliageTool = new SerializedObject(target);
		GetProperties();
		AfsFoliageTool script = (AfsFoliageTool)target;
		Color myBlue = new Color(0.5f,0.7f,1.0f,1.0f);
		Color SplitterCol = new Color(1f, 1f, 1f, 0.075f);
		Color SplitterCol1 = new Color(.6f, .9f, .22f, .005f);
		Color myCol = new Color(.5f, .8f, .0f, 1f); 		// Light Green
		Color myBgCol = SplitterCol;
		myBgCol = myCol; //new Color(.5f, .75f, .24f, 1f);
		if (!EditorGUIUtility.isProSkin) {
			// myCol = new Color(0.0f,0.15f,0.55f,1.0f);	// Dark Blue
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); 		// Dark Green
			myBgCol = new Color(0.94f,0.94f,0.94f,1.0f);
			SplitterCol = new Color(0f, 0f, 0f, 0.125f);
			SplitterCol1 = new Color(1f, 1f, 1f, 0.5f);
		}

		// Custom Foldout
		GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		myFoldoutStyle.fontStyle = FontStyle.Bold;
		myFoldoutStyle.fontSize = 12;

		myFoldoutStyle.normal.textColor = myCol;
		myFoldoutStyle.onNormal.textColor = myCol;
		//myFoldoutStyle.hover.textColor = Color.white;
		//myFoldoutStyle.onHover.textColor = Color.white;
		myFoldoutStyle.active.textColor = myCol;
		myFoldoutStyle.onActive.textColor = myCol;
		myFoldoutStyle.focused.textColor = myCol;
		myFoldoutStyle.onFocused.textColor = myCol;

		// Help Foldout
		GUIStyle helpFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		//helpFoldoutStyle.fixedWidth = 40;
		//helpFoldoutStyle.alignment = TextAnchor.MiddleRight;
		helpFoldoutStyle.normal.textColor = myBlue;
		helpFoldoutStyle.onNormal.textColor = myBlue;
		helpFoldoutStyle.active.textColor = myBlue;
		helpFoldoutStyle.onActive.textColor = myBlue;
		helpFoldoutStyle.focused.textColor = myBlue;
		helpFoldoutStyle.onFocused.textColor = myBlue;

		// Custom Label
		GUIStyle myLabel = new GUIStyle(EditorStyles.label);
		myLabel.normal.textColor = myCol;
		myLabel.onNormal.textColor = myCol;

		// Custom Label Bold
		GUIStyle myLabelBold = new GUIStyle(EditorStyles.label);
		myLabelBold.fontStyle = FontStyle.Bold;
		myLabelBold.normal.textColor = myCol;
		myLabelBold.onNormal.textColor = myCol;

		// Custom Label Big Bold
		GUIStyle myLabelBigBold = new GUIStyle(EditorStyles.label);
		myLabelBigBold.fontStyle = FontStyle.Bold;
		myLabelBigBold.fontSize = 12;
		myLabelBigBold.normal.textColor = myCol;

		// Default icon Size
		EditorGUIUtility.SetIconSize( new Vector2(16,16));

		// Enable rich text for helpboxes
		GUIStyle helpBoxes = GUI.skin.GetStyle("HelpBox");
		helpBoxes.richText = true;

		EditorGUILayout.BeginVertical();
		GUILayout.Space(10);

		if (!script.hasBeenSaved) {
			GUILayout.Label ("Get started", myLabel);
			EditorGUILayout.HelpBox("<b>Please note:</b>\nYou have to safe a new mesh before you can start editing it.", MessageType.Warning, true);
		}

		else {
		//	Call this only if the Application is not playing as static batching might produce strange messages
			if (!Application.isPlaying) {
				script.checkSubmeshes();
			}
		//	///////////////////////////
		//	///////////////////////////
		//
		//	Convert Tree & merge Submeshes
		//
			if (script.hasSubmeshes) {
				showSaveBtn = false;
				script.mergeSubMeshes = true; // flas for speedtrees...
				EditorGUILayout.HelpBox("The assigned Mesh has more than one Submesh. "+
				"So the script assumes that you want to convert a tree.\n"+
				"Please select one of the options below.", MessageType.Warning, true);

				GUILayout.Space(5);

				//	//////////////////////////////////
    			//	Convert to Simple Tree
    			GUI.backgroundColor = myBgCol;
				EditorGUILayout.BeginVertical("Box");
					GUI.color = Color.white; //myCol;
					// Foldout incl. Icon
					EditorGUILayout.BeginHorizontal();
						GUILayout.Space(-2);
						EditorGUI.indentLevel++;
						EditorGUILayout.BeginVertical();
							// Only icon gets rendered
							script.convert2simpletree = EditorGUILayout.Foldout(script.convert2simpletree, "Convert Tree into flattened Tree", myFoldoutStyle);
						EditorGUILayout.EndVertical();
						EditorGUI.indentLevel--;
						EditorGUILayout.BeginVertical(GUILayout.Width(20) );
							GUILayout.Space(3);
							// Label needs width!
							EditorGUILayout.LabelField(new GUIContent(icnTree), GUILayout.Width(20), GUILayout.Height(20));
						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(-2);
					GUI.backgroundColor = Color.white;
				EditorGUILayout.EndVertical();
				// FoldoutContent
    			if (script.convert2simpletree){
    				script.convert2simpletree = true;
	        		script.convert2foliage = false;
    				
    				GUILayout.Space(-5);
    				EditorGUILayout.BeginVertical("Box");
	    				GUILayout.Space(10);
						GUILayout.Label ("Mesh Processing", myLabel);
						EditorGUILayout.HelpBox("Converting a tree into a flattened one just flattens the submeshes so as result you will get a tree with only one material.\n"+
						"Bending parameters will be copied.", MessageType.None, true);

	    				GUILayout.Space(10);
	    				GUILayout.Label ("Texture Processing", myLabel);
	        			// Grab Textures
						if(script.sourceTexDiffuse == null) {
							script.grabDiffuse();
						}
						if(script.sourceTex1 == null) {
							script.grabTranslucencyGloss();
						}

	        			EditorGUILayout.BeginHorizontal();
							EditorGUILayout.BeginVertical();
								GUILayout.Label ("diffuse");
								script.sourceTexDiffuse = (Texture2D)EditorGUILayout.ObjectField(script.sourceTexDiffuse, typeof(Texture2D), false, GUILayout.MinHeight(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64));
							EditorGUILayout.EndVertical();
						
							EditorGUILayout.BeginVertical();
								GUILayout.Label ("translucency_gloss");
								script.sourceTex1 = (Texture2D)EditorGUILayout.ObjectField(script.sourceTex1, typeof(Texture2D), false, GUILayout.MinHeight(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64));
							EditorGUILayout.EndVertical();
						EditorGUILayout.EndHorizontal();
						GUILayout.Space(4);
						//
						EditorGUILayout.BeginVertical();
							EditorGUILayout.BeginHorizontal();
								toolTip =	"If your tree uses one of the most common texture setups the script can automatically fill the proper parts of the translucency map with black so the bark will be completely opaque.\n" +
											"Otherwise you will have to edit the generated texture manually and fill those parts of the red color channel with black where the tree should be fully opaque.";  
								//EditorGUILayout.LabelField(new GUIContent("Texture Split", toolTip), GUILayout.Width(80));
								//script.TextureSplitSelected = (TextureSplit)EditorGUILayout.EnumPopup(script.TextureSplitSelected);
								EditorGUILayout.PropertyField(TextureSplitSelected, new GUIContent("Texture Split", toolTip) );
							EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();

						GUILayout.Space(5);
						if(! EditorApplication.isPlaying && script.sourceTexDiffuse != null && script.sourceTex1 != null ) {
							if (GUILayout.Button("Convert Tree", GUILayout.Height(34) )) {
								script.hasChanged = false;
								// Save mesh
								script.SaveNewPlantMesh();
								// Save textures
								mergeTextureTree();
								// Update Materials
								script.updateSimpleTree();
								script.simpleTreeConverted = true;
							}
						}
						else {
							GUI.enabled = false;
							GUILayout.Button("Convert Tree", GUILayout.Height(34) );
							GUI.enabled = true;
						}
						GUILayout.Space(3);
					EditorGUILayout.EndVertical();	
        		}

        		GUILayout.Space(5);

        		//	//////////////////////////////////
        		//	Convert to Foliage Shader
        		GUI.backgroundColor = myBgCol;
				EditorGUILayout.BeginVertical("Box");
					GUI.color = Color.white; //myCol;
					// Foldout incl. Icon
					EditorGUILayout.BeginHorizontal();
						GUILayout.Space(-2);
						EditorGUI.indentLevel++;
						EditorGUILayout.BeginVertical();
							// Only icon gets rendered
							script.convert2foliage = EditorGUILayout.Foldout(script.convert2foliage, "Convert Tree for Foliage Shader", myFoldoutStyle);
						EditorGUILayout.EndVertical();
						EditorGUI.indentLevel--;
						EditorGUILayout.BeginVertical(GUILayout.Width(20) );
							GUILayout.Space(3);
							// Label needs width!
							EditorGUILayout.LabelField(new GUIContent(icnTree), GUILayout.Width(20), GUILayout.Height(20));
						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(-2);
					GUI.backgroundColor = Color.white;
				EditorGUILayout.EndVertical();
				// FoldoutContent
				if (script.convert2foliage){
					script.convert2simpletree = false;
    				script.convert2foliage = true;
    				script.delete2ndUV = true;

					GUILayout.Space(-5);
					EditorGUILayout.BeginVertical("Box");
    				GUILayout.Space(10);
					GUILayout.Label ("Mesh Processing", myLabel);
					//script.BendingModeSelected = (BendingModes)EditorGUILayout.EnumPopup("Bending Parameters", script.BendingModeSelected);
					EditorGUILayout.PropertyField(BendingModeSelected, new GUIContent("Bending Parameters") );
					if (script.BendingModeSelected == BendingModes.VertexColors ) {
						EditorGUILayout.HelpBox("If you chose 'Vertex Colors' you will have to set up primary and secondary bending from scratch. "+ 
						"Original bending parameters will be lost. Check 'generate UV2' if you want to support lightmapping.", MessageType.None, true);
						script.storeUV4 = false;
					}
					else {
						EditorGUILayout.HelpBox("If you chose 'Vertex Colors and UV4' the original bending parameters will simply be copied to UV4. "+ 
						"UV2 will be skipped unless you check 'generate UV2' (needed for lightmapping).", MessageType.None, true);
						script.storeUV4 = true;
					}
					EditorGUILayout.BeginHorizontal();
						script.generateUV2 = EditorGUILayout.Toggle("", script.generateUV2, GUILayout.Width(14) );
						GUILayout.Label ("Generate UV2");
					EditorGUILayout.EndHorizontal();
					// Grab Textures
					if(script.sourceTex0 == null) {
						script.grabNormalSpecular();
					}
					if(script.sourceTex1 == null) {
						script.grabTranslucencyGloss();
					}
					GUILayout.Space(10);
					GUILayout.Label ("Texture Processing", myLabel);
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.BeginVertical();
							GUILayout.Label ("normal_specular");
							script.sourceTex0 = (Texture2D)EditorGUILayout.ObjectField(script.sourceTex0, typeof(Texture2D), false, GUILayout.MinHeight(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64));
						EditorGUILayout.EndVertical();
					
						EditorGUILayout.BeginVertical();
							GUILayout.Label ("translucency_gloss");
							script.sourceTex1 = (Texture2D)EditorGUILayout.ObjectField(script.sourceTex1, typeof(Texture2D), false, GUILayout.MinHeight(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64));
						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(4);
					//
					EditorGUILayout.BeginVertical();
						EditorGUILayout.BeginHorizontal();
							toolTip =	"If your tree uses one of the most common texture setups the script can automatically fill the proper parts of the translucency map with black so the bark will be completely opaque.\n" +
										"Otherwise you will have to edit the generated texture manually and fill those parts of the red color channel with black where the tree should be fully opaque.";  
							//EditorGUILayout.LabelField(new GUIContent("Texture Split", toolTip), GUILayout.Width(80));
							//script.TextureSplitSelected = (TextureSplit)EditorGUILayout.EnumPopup(script.TextureSplitSelected);
							EditorGUILayout.PropertyField(TextureSplitSelected, new GUIContent("Texture Split", toolTip) );
						EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
					
					GUILayout.Space(5);
					if(!EditorApplication.isPlaying && script.sourceTex0 != null && script.sourceTex1 != null ) {
						if (GUILayout.Button("Convert Tree", GUILayout.Height(34) )) {
							if (EditorUtility.DisplayDialog("Convert Tree to be compatible with the foliage shader?","This will finally remove the 'tree' component from the gameobject. So please make sure that you are working on a copy.", "Convert", "Cancel")) {
								script.hasChanged = false;
								// Save mesh
								script.SaveNewPlantMesh();
								if (script.savePath !="") {
									// Save textures
									mergeTextureFoliage();
									// Update Materials
									script.updateFoliageTree();
									script.foliageTreeConverted = true;
									if (script.BendingModeSelected == BendingModes.VertexColorsAndUV4 ) {
										script.foliageTreeUV4Converted = true;
									}
								}
							}
						}
					}
					else {
						GUI.enabled = false;
						GUILayout.Button("Convert Tree", GUILayout.Height(34) );
						GUI.enabled = true;
					}
					GUILayout.Space(3);
				// End Box
				EditorGUILayout.EndVertical();
    			}
			}
			
		//	////////////////////////////////
		//	////////////////////////////////
		//
		//	Success
		//
			if(script.simpleTreeConverted) {
				EditorGUILayout.HelpBox("The tree has been converted to a simple one. "+ 
				"Textures that needed to be updated have been created and assigned as well as a new Material.", MessageType.Warning, true);
				EditorGUILayout.HelpBox("As there is nothing else you can do here, you should simply remove the script.", MessageType.Warning, true);	
			}

			if(script.foliageTreeConverted) {
				EditorGUILayout.HelpBox("The tree has been converted to be compatible with the foliage shaders. "+ 
				"Textures that needed to be updated have been created and assigned as well as a new Material.", MessageType.Warning, true);
				if(script.foliageTreeUV4Converted ) {
					EditorGUILayout.HelpBox("As there is nothing else you can do here, you should simply remove the script.", MessageType.Warning, true);	
				}
				else {
					EditorGUILayout.HelpBox("As you have chosen to store bending in vertex colors only you will have to set up primary and secondary bending below.", MessageType.Warning, true);	
				}
			}


		//	////////////////////////////////
		//	////////////////////////////////
		//
		//	Rework meshes
		//
			if (!script.mergeSubMeshes && !script.simpleTreeConverted && !script.foliageTreeUV4Converted) {
				showSaveBtn = true;
		
		//	Set Bending
			GUI.backgroundColor = myBgCol;
			EditorGUILayout.BeginVertical("Box");
				GUI.color = Color.white;
				// Foldout incl. Icon
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(-2);
					EditorGUI.indentLevel++;
					EditorGUILayout.BeginVertical();
						// Only icon gets rendered
						script.adjustBending = EditorGUILayout.Foldout(script.adjustBending, "Set Bending", myFoldoutStyle);
					EditorGUILayout.EndVertical();
					EditorGUI.indentLevel--;
					EditorGUILayout.BeginVertical(GUILayout.Width(20));
						GUILayout.Space(3);
						// Label needs width!
						EditorGUILayout.LabelField(new GUIContent(icnBending), GUILayout.Width(20), GUILayout.Height(20));
					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(-2);
				GUI.backgroundColor = Color.white;
			EditorGUILayout.EndVertical();
			// FoldoutContent
			if (script.adjustBending) {
				script.adjustVertexColors = false;
				GUILayout.Space(-5);
				EditorGUILayout.BeginVertical("Box");
					DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
					int RestoreBendingMode = BendingModeSelected.enumValueIndex;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(BendingModeSelected, new GUIContent("Bending Parameters") );
					if (EditorGUI.EndChangeCheck()) {
						if (!EditorUtility.DisplayDialog("Change Bending Mode?","This will effect all plants sharing the assigned material as soon as you hit 'Apply'.", "Change", "Cancel")) {
							BendingModeSelected.enumValueIndex = RestoreBendingMode;
						};
					}
					GUILayout.Space(5);

			//		Classic Bending
					if(script.BendingModeSelected == BendingModes.VertexColors ) {
						EditorGUILayout.BeginHorizontal();
							GUILayout.Label ("Primary and secondary Bending", myLabel);
							Rect h1rect = EditorGUILayout.GetControlRect(false, 15);
							h1rect.x += h1rect.width - 30;
							hSetBending = EditorGUI.Foldout(h1rect,hSetBending, "Help", helpFoldoutStyle);
						EditorGUILayout.EndHorizontal();
						// Help
						if(hSetBending){
							EditorGUILayout.HelpBox("Set up vertex colors for primary and secondary bending from scratch. "+ 
							"Using this function will overwrite all vertex color blue values originally baked into the mesh.", MessageType.None, true);
						}
						GUILayout.Space(2);
					}

			//		Bending stored in UV4
					else if(script.BendingModeSelected == BendingModes.VertexColorsAndUV4 ) { // use ref here
						EditorGUILayout.BeginHorizontal();
							GUILayout.Label ("Primary Bending", myLabel);
							Rect h2rect = EditorGUILayout.GetControlRect(false, 15);
							h2rect.x += h2rect.width - 30;
							hSetBendingUV = EditorGUI.Foldout(h2rect,hSetBendingUV, "Help", helpFoldoutStyle);
						EditorGUILayout.EndHorizontal();
						GUILayout.Space(2);
						// Help
						if(hSetBendingUV){
							EditorGUILayout.HelpBox("Writes primary and secondary bending values to UV4 instead of vertex color blue. "+ 
							"As soon as you hit 'Apply' the assigned material will be tweaked automatically.", MessageType.None, true);
						}

					}

					// Bending y-axis
					/*EditorGUILayout.BeginHorizontal();
						script.Set1stBendingAlongY = EditorGUILayout.Toggle("", script.Set1stBendingAlongY, GUILayout.Width(14) );
						EditorGUILayout.PropertyField(maxBendingValueY, new GUIContent("Along Y-axis"));
						curvY.animationCurveValue = EditorGUILayout.CurveField("", curvY.animationCurveValue, new Color (0.0f, 1.0f, 1.0f, 0.75f), new Rect(0, 0, 1, 1), GUILayout.Width(40));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
						script.Set1stBendingRelative2Pivot= EditorGUILayout.Toggle("", script.Set1stBendingRelative2Pivot, GUILayout.Width(14) );
						GUILayout.Label ("Relative to Pivot");
					EditorGUILayout.EndHorizontal(); */

					/*EditorGUILayout.BeginHorizontal();
						string[] options = new string[] { " Along Y-Axis", " Relative to Pivot" };
						//selected = GUILayout.SelectionGrid(selected, options, options.Length, EditorStyles.radioButton);
						selected = GUILayout.SelectionGrid(selected, options, 1, EditorStyles.radioButton);

						EditorGUILayout.PropertyField(maxBendingValueY, GUIContent.none);
						curvY.animationCurveValue = EditorGUILayout.CurveField("", curvY.animationCurveValue, new Color (0.0f, 1.0f, 1.0f, 0.75f), new Rect(0, 0, 1, 1), GUILayout.Width(40));
					EditorGUILayout.EndHorizontal();*/

					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(Set1stBendingSelected, GUIContent.none,  GUILayout.Width(112) );
						GUILayout.Space(10);
						EditorGUILayout.PropertyField(maxBendingValueY, GUIContent.none);
						curvY.animationCurveValue = EditorGUILayout.CurveField("", curvY.animationCurveValue, new Color (0.0f, 1.0f, 1.0f, 0.75f), new Rect(0, 0, 1, 1), GUILayout.Width(40));
					EditorGUILayout.EndHorizontal();

					
					if(script.BendingModeSelected == BendingModes.VertexColorsAndUV4 ) {
						GUILayout.Space(5);
						GUILayout.Label ("Secondary Bending", myLabel);
					}
					// Bending xz-axes
					EditorGUILayout.BeginHorizontal();
						//EditorGUILayout.PropertyField(maxBendingValueX, new GUIContent("Along XZ-axis") );
						GUILayout.Label ("Along XZ-Axes", GUILayout.Width(112));
						GUILayout.Space(10);
						EditorGUILayout.PropertyField(maxBendingValueX, GUIContent.none );
						curvX.animationCurveValue = EditorGUILayout.CurveField("", curvX.animationCurveValue, Color.green, new Rect(0, 0, 1, 1), GUILayout.Width(40));
					EditorGUILayout.EndHorizontal();
					
					// Mask secondary Bending
					if(script.BendingModeSelected == BendingModes.VertexColorsAndUV4 ) {
						GUILayout.Space(5);
						EditorGUILayout.BeginHorizontal();
							GUILayout.Label ("Mask secondary Bending", myLabel);
						EditorGUILayout.EndHorizontal();
						GUILayout.Space(2);
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(Mask2ndBendingSelected, GUIContent.none,  GUILayout.Width(112) );
							GUILayout.Label ("");
							if(Mask2ndBendingSelected.enumValueIndex == 0) {
								curvZ.animationCurveValue = EditorGUILayout.CurveField("", curvZ.animationCurveValue, Color.red, new Rect(0, 0, 1, 1), GUILayout.Width(40));
							}
						EditorGUILayout.EndHorizontal();	
					}
					// Draw Gizmos
					GUILayout.Space(5);
					EditorGUILayout.BeginHorizontal();
						script.showGizmos = EditorGUILayout.Toggle("", script.showGizmos, GUILayout.Width(14) );
						GUILayout.Label ("Show Gizmos", myLabel);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
						GUI.enabled = false;
						script.VertexBlueValsStored = EditorGUILayout.Toggle("", script.VertexBlueValsStored, GUILayout.Width(14) );
						GUILayout.Label ("Vertex Color Blue Values stored");
						GUI.enabled = true;
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);
					EditorGUILayout.BeginHorizontal();
						if(! EditorApplication.isPlaying ) {
							if (GUILayout.Button("Apply" )) {
								script.hasChanged = true;
								script.AdjustBending();
							}
							if (GUILayout.Button("Test", GUILayout.Width(94) )) {
								EditorApplication.isPlaying = true;
							}
						}
						else {
							GUI.enabled = false;
							GUILayout.Button("Apply");
							GUI.enabled = true;
							if (GUILayout.Button("Stop", GUILayout.Width(94) )) {
								EditorApplication.isPlaying = false;
							}
						}
				 	EditorGUILayout.EndHorizontal();
				GUILayout.Space(2);
				EditorGUILayout.EndVertical();
			}

			GUILayout.Space(5);

		//	Adjust Vertex Colors
			GUI.backgroundColor = myBgCol;
			EditorGUILayout.BeginVertical("Box");
				GUI.color = Color.white;
				// Foldout incl. Icon
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(-2);
					EditorGUI.indentLevel++;
					EditorGUILayout.BeginVertical();
						// Only icon gets rendered
						script.adjustVertexColors = EditorGUILayout.Foldout(script.adjustVertexColors, "Adjust Vertex Colors", myFoldoutStyle);
					EditorGUILayout.EndVertical();
					EditorGUI.indentLevel--;
					EditorGUILayout.BeginVertical(GUILayout.Width(20) );
						GUILayout.Space(3);
						// Label needs width!
						EditorGUILayout.LabelField(new GUIContent(icnVertexColors), GUILayout.Width(20), GUILayout.Height(20));
					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(-2);
				GUI.backgroundColor = Color.white;
			EditorGUILayout.EndVertical();
			// FoldoutContent
			if (script.adjustVertexColors) {
				//
				script.adjustBending = false;
				//
				GUILayout.Space(-5);
				EditorGUILayout.BeginVertical("Box");
					DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
					EditorGUILayout.BeginHorizontal();
						Rect h3rect = EditorGUILayout.GetControlRect(false, 20);
						h3rect.x += h3rect.width - 40;
						hVertexColors = EditorGUI.Foldout(h3rect,hVertexColors, "Help", helpFoldoutStyle);
					EditorGUILayout.EndHorizontal();
					if(hVertexColors){
						EditorGUILayout.HelpBox("This option let's you change the vertex colors applied to the original mesh in order to adjust the overall bending. " +
						"All changes are done only relatively to the original values.", MessageType.None, true);
					}
					EditorGUILayout.PropertyField(adjustBlueValue, new GUIContent("Main Bending (Blue/%)") );
					EditorGUILayout.PropertyField(adjustGreenValue, new GUIContent("Edge Flutter (Green/%)") );
					EditorGUILayout.PropertyField(adjustRedValue, new GUIContent("Phase (Red/%)") );
					EditorGUILayout.PropertyField(adjustAlphaValue, new GUIContent("AO (Alpha/%)") );

					GUILayout.Space(5);
					EditorGUILayout.BeginHorizontal();
						if(! EditorApplication.isPlaying ) {
							if (GUILayout.Button("Apply" )) {
								script.hasChanged = true;
								script.AdjustVertexColors();
							}
							if (GUILayout.Button("Test", GUILayout.Width(94) )) {
								EditorApplication.isPlaying = true;
							}
						}
						else {
							GUI.enabled = false;
							GUILayout.Button("Apply");
							GUI.enabled = true;
							if (GUILayout.Button("Stop", GUILayout.Width(94) )) {
								EditorApplication.isPlaying = false;
							}
						}
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(2);
				EditorGUILayout.EndVertical();
			}
		}	
		
		}

	//	Save Btn
		if (showSaveBtn == true && !script.simpleTreeConverted && !script.foliageTreeUV4Converted) {
			if (script.hasChanged) {
				GUI.backgroundColor = Color.red;
			}
			
			GUILayout.Space(5);
			if(! EditorApplication.isPlaying ) {
				if (GUILayout.Button("Save Mesh", GUILayout.Height(34) )) {
					//script.hasChanged = false;
					script.SaveNewPlantMesh();
				}
			}
			else {
				GUI.enabled = false;
				GUILayout.Button("Save Mesh", GUILayout.Height(34) );
				GUI.enabled = true;
			}
			GUI.backgroundColor = Color.white;
			GUILayout.Space(3);
		}
		EditorGUILayout.EndVertical();
		if (GUI.changed) {
	   		EditorUtility.SetDirty(script);
	  	}
	  	AfsFoliageTool.ApplyModifiedProperties();
	}
//	End Editor	
//	///////////////////////////	

	public void mergeTextureTree () {
		AfsFoliageTool script = (AfsFoliageTool)target;

		if(script.sourceTexDiffuse != null && script.sourceTex1 != null) {
			// Check textures
			bool wasReadable0 = false;
			TextureImporterFormat format0;
			TextureImporterType type0;
			bool wasReadable1 = false;
			TextureImporterFormat format1;
			TextureImporterType type1;
			// Check texture0
			string path0 = AssetDatabase.GetAssetPath(script.sourceTexDiffuse);
			TextureImporter ti0 = (TextureImporter) TextureImporter.GetAtPath(path0);
			format0 = ti0.textureFormat;
			type0 = ti0.textureType;
			if (ti0.isReadable == true) {
				wasReadable0 = true;
			}
			else {
				ti0.isReadable = true;
			}
			if (ti0.textureFormat != TextureImporterFormat.AutomaticTruecolor || ti0.textureType != TextureImporterType.Default ) {
				ti0.textureType = TextureImporterType.Default; 
				ti0.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				// Refresh texture
				AssetDatabase.ImportAsset( path0, ImportAssetOptions.ForceUpdate );
			}
			// Check texture1
			string path1 = AssetDatabase.GetAssetPath(script.sourceTex1);
			TextureImporter ti1 = (TextureImporter) TextureImporter.GetAtPath(path1);
			format1 = ti1.textureFormat;
			type1 = ti1.textureType;
			if (ti1.isReadable == true) {
				wasReadable1 = true;
			}
			else {
				ti1.isReadable = true;
			}
			if (ti1.textureFormat != TextureImporterFormat.AutomaticTruecolor || ti1.textureType != TextureImporterType.Default ) {
				ti1.textureType = TextureImporterType.Default; 
				ti1.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				// Refresh texture
				AssetDatabase.ImportAsset( path1, ImportAssetOptions.ForceUpdate ); 
			}

			// Start processing
			// TODO: clear alpha of diffuse on bark if any

			Color combinedColor;

			Texture2D correctedDiffuseTex = new Texture2D( script.sourceTexDiffuse.width, script.sourceTexDiffuse.height, TextureFormat.ARGB32, true);
			for (int y = 0; y < script.sourceTexDiffuse.height; y++) {
				for (int x = 0; x < script.sourceTexDiffuse.width; x++) {
					if (x < script.sourceTexDiffuse.width * 0.5 && script.TextureSplitSelected == TextureSplit.OneByOne) {
						combinedColor.a = 1.0f;
					}
					else if (x < script.sourceTexDiffuse.width * 0.666 && script.TextureSplitSelected == TextureSplit.TwoByOne) {
						combinedColor.a = 1.0f;
					}
					else {
						combinedColor.a = script.sourceTexDiffuse.GetPixel(x,y).a; 	// a = trans
					}
					combinedColor.r = script.sourceTexDiffuse.GetPixel(x,y).r; 		
					combinedColor.g = script.sourceTexDiffuse.GetPixel(x,y).g; 		
					combinedColor.b = script.sourceTexDiffuse.GetPixel(x,y).b; 		
					correctedDiffuseTex.SetPixel(x,y,combinedColor);
				}
			}


			// Set translucency on bark to 0 // trngls.b;
			Texture2D combinedTex = new Texture2D( script.sourceTex1.width, script.sourceTex1.height, TextureFormat.ARGB32, true);
			
			for (int y = 0; y < script.sourceTex1.height; y++) {
				for (int x = 0; x < script.sourceTex1.width; x++) {
					if (x < script.sourceTex1.width * 0.5 && script.TextureSplitSelected == TextureSplit.OneByOne) {
						combinedColor.b = 0;
					}
					else if (x < script.sourceTex1.width * 0.666 && script.TextureSplitSelected == TextureSplit.TwoByOne) {
						combinedColor.b = 0;
					}
					else {
						combinedColor.b = script.sourceTex1.GetPixel(x,y).b; 	// b = trans
					}
					combinedColor.r = script.sourceTex1.GetPixel(x,y).r; 		// copy red channel
					combinedColor.g = script.sourceTex1.GetPixel(x,y).g; 		// copy green channel
					combinedColor.a = script.sourceTex1.GetPixel(x,y).a; 		// copy alpha channel
					combinedTex.SetPixel(x,y,combinedColor);
				}
			}
			// Safe texture at the same location as the mesh
			string directory;
			if (script.savePath == "") {
				directory = Application.dataPath;
			}
			else {
				directory = Path.GetDirectoryName(script.savePath);	
			}

			// write diffuse
			string filePath = directory + "/" + Path.GetFileNameWithoutExtension(script.savePath) + "_Diffuse_Alpha.png";
			var bytes = correctedDiffuseTex.EncodeToPNG(); 
			File.WriteAllBytes(filePath, bytes);
			AssetDatabase.Refresh();
			TextureImporter ti2 = AssetImporter.GetAtPath(filePath) as TextureImporter;
			ti2.anisoLevel = 7;
			ti2.textureType = TextureImporterType.Default;
			ti2.textureFormat = TextureImporterFormat.AutomaticCompressed;
			AssetDatabase.ImportAsset(filePath);
			AssetDatabase.Refresh();
			script.diffuseTexPath = filePath;
			DestroyImmediate(correctedDiffuseTex, true);

			// write translucency gloss
			filePath = directory + "/" + Path.GetFileNameWithoutExtension(script.savePath) + "_Translucency_Gloss.png";
			var bytes1 = combinedTex.EncodeToPNG(); 
			File.WriteAllBytes(filePath, bytes1); 
			
			AssetDatabase.Refresh();
			ti2 = AssetImporter.GetAtPath(filePath) as TextureImporter;
			ti2.anisoLevel = 7;
			ti2.textureType = TextureImporterType.Default;
			ti2.textureFormat = TextureImporterFormat.ARGB32;
			AssetDatabase.ImportAsset(filePath);
			AssetDatabase.Refresh();
			script.transTexPath = filePath;
			DestroyImmediate(combinedTex, true);
			
			// Reset texture settings
			ti0.textureFormat = format0;
			ti0.textureType = type0;
			ti1.textureFormat = format1;
			ti1.textureType = type1;
			if (wasReadable0 == false) {
				ti0.isReadable = false;
			}
			if (wasReadable1 == false) {
				ti1.isReadable = false;
			}
			AssetDatabase.ImportAsset( path0, ImportAssetOptions.ForceUpdate );
			AssetDatabase.ImportAsset( path1, ImportAssetOptions.ForceUpdate );
			Resources.UnloadUnusedAssets();
		}
	}

	public void mergeTextureFoliage () {
		AfsFoliageTool script = (AfsFoliageTool)target;

		if(script.sourceTex0 != null && script.sourceTex1 != null) {
			// Check textures
			bool wasReadable0 = false;
			TextureImporterFormat format0;
			TextureImporterType type0;
			bool wasReadable1 = false;
			TextureImporterFormat format1;
			TextureImporterType type1;
			// Check texture0
			string path0 = AssetDatabase.GetAssetPath(script.sourceTex0);
			TextureImporter ti0 = (TextureImporter) TextureImporter.GetAtPath(path0);
			format0 = ti0.textureFormat;
			type0 = ti0.textureType;
			if (ti0.isReadable == true) {
				wasReadable0 = true;
			}
			else {
				ti0.isReadable = true;
			}
			if (ti0.textureFormat != TextureImporterFormat.AutomaticTruecolor || ti0.textureType != TextureImporterType.Default ) {
				ti0.textureType = TextureImporterType.Default; 
				ti0.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				// Refresh texture
				AssetDatabase.ImportAsset( path0, ImportAssetOptions.ForceUpdate );
			}
			// Check texture1
			string path1 = AssetDatabase.GetAssetPath(script.sourceTex1);
			TextureImporter ti1 = (TextureImporter) TextureImporter.GetAtPath(path1);
			format1 = ti1.textureFormat;
			type1 = ti1.textureType;
			if (ti1.isReadable == true) {
				wasReadable1 = true;
			}
			else {
				ti1.isReadable = true;
			}
			if (ti1.textureFormat != TextureImporterFormat.AutomaticTruecolor || ti1.textureType != TextureImporterType.Default ) {
				ti1.textureType = TextureImporterType.Default; 
				ti1.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				// Refresh texture
				AssetDatabase.ImportAsset( path1, ImportAssetOptions.ForceUpdate ); 
			}
			// Check dimensions
			if (script.sourceTex0.width == script.sourceTex1.width)
			{
				// Start combining
				Texture2D combinedTex = new Texture2D( script.sourceTex0.width, script.sourceTex0.height, TextureFormat.ARGB32, true);
				Color combinedColor;
				for (int y = 0; y < script.sourceTex0.height; y++) {
					for (int x = 0; x < script.sourceTex0.width; x++) {
						if (x < script.sourceTex0.width * 0.5 && script.TextureSplitSelected == TextureSplit.OneByOne) {
							combinedColor.r = 0;
						}
						else if (x < script.sourceTex0.width * 0.666 && script.TextureSplitSelected == TextureSplit.TwoByOne) {
							combinedColor.r = 0;
						}
						else {
							combinedColor.r = script.sourceTex1.GetPixel(x,y).b; 	// red channel = translucency
						}
						combinedColor.g = script.sourceTex0.GetPixel(x,y).g; 		// green channel = g from normal
						combinedColor.b = script.sourceTex1.GetPixel(x,y).a; 		// blue channel = gloss
						combinedColor.a = script.sourceTex0.GetPixel(x,y).a; 		// alpha channel = a from normal
						combinedTex.SetPixel(x,y,combinedColor);
					}
				}
				// Safe texture at the same location as the mesh
				string directory;
				if (script.savePath == "") {
					directory = Application.dataPath;
				}
				else {
					directory = Path.GetDirectoryName(script.savePath);	
				}
				string filePath = directory + "/" + Path.GetFileNameWithoutExtension(script.savePath) + "_Translucency_Gloss.png";
				var bytes = combinedTex.EncodeToPNG(); 
				File.WriteAllBytes(filePath, bytes); 
				AssetDatabase.Refresh();
				TextureImporter ti2 = AssetImporter.GetAtPath(filePath) as TextureImporter;
				ti2.anisoLevel = 7;
				ti2.textureType = TextureImporterType.Default;
				ti2.textureFormat = TextureImporterFormat.ARGB32;
				AssetDatabase.ImportAsset(filePath);
				AssetDatabase.Refresh();
				script.combinedTexPath = filePath;
				DestroyImmediate(combinedTex, true);
			}
			else {
				Debug.Log ("Both Textures have to fit in size.");
			}
			// Reset texture settings
			ti0.textureFormat = format0;
			ti0.textureType = type0;
			ti1.textureFormat = format1;
			ti1.textureType = type1;
			if (wasReadable0 == false) {
				ti0.isReadable = false;
			}
			if (wasReadable1 == false) {
				ti1.isReadable = false;
			}
			AssetDatabase.ImportAsset( path0, ImportAssetOptions.ForceUpdate );
			AssetDatabase.ImportAsset( path1, ImportAssetOptions.ForceUpdate );
			Resources.UnloadUnusedAssets();
		}
	}


	//	///////////////////////////////////////////////////
	//	Editor Helper Functions

	private void DrawSplitter(Rect targetPosition, Color SplitterCol) {
		GUI.color = SplitterCol;
		targetPosition.x -= 4;
		targetPosition.height = 1;
		targetPosition.width += 8;
		GUI.DrawTexture(targetPosition, EditorGUIUtility.whiteTexture);
		GUI.color = Color.white;
		GUILayout.Space(4);
	}

	private void DrawSplitter1(Rect targetPosition, Color SplitterCol) {
		GUI.color = SplitterCol;
		targetPosition.y -= 3;
		targetPosition.x -= 3;
		targetPosition.height = 1;
		targetPosition.width += 6;
		GUI.DrawTexture(targetPosition, EditorGUIUtility.whiteTexture);
		GUI.color = Color.white;
	}

	//	///////////////////////////////////////////////////
	private void GetProperties() {
		BendingModeSelected = AfsFoliageTool.FindProperty("BendingModeSelected");

		Set1stBendingSelected = AfsFoliageTool.FindProperty("Set1stBendingSelected");
		Mask2ndBendingSelected = AfsFoliageTool.FindProperty("Mask2ndBendingSelected");

		maxBendingValueY = AfsFoliageTool.FindProperty("maxBendingValueY");
		maxBendingValueX = AfsFoliageTool.FindProperty("maxBendingValueX");
		curvY = AfsFoliageTool.FindProperty("curvY");
		curvX = AfsFoliageTool.FindProperty("curvX");
		curvZ = AfsFoliageTool.FindProperty("curvZ");
		//
		adjustBlueValue = AfsFoliageTool.FindProperty("adjustBlueValue");
		adjustGreenValue = AfsFoliageTool.FindProperty("adjustGreenValue");
		adjustRedValue = AfsFoliageTool.FindProperty("adjustRedValue");
		adjustAlphaValue = AfsFoliageTool.FindProperty("adjustAlphaValue");
		//
		TextureSplitSelected = AfsFoliageTool.FindProperty("TextureSplitSelected");
	}
}
#endif