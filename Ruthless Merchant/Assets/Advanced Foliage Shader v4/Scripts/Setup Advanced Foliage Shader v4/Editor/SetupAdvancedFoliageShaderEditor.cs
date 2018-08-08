#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor (typeof(SetupAdvancedFoliageShader))]
public class SetupAdvancedFoliageShaderEditor : Editor {
	// Serialize
	private SerializedObject SetupAFS;
	// Lighting
	private SerializedProperty DirectionalLightReference;
	private SerializedProperty GrassApproxTrans;
	// Specular Lighting
	private SerializedProperty AfsSpecFade;
	// Fog
	private SerializedProperty disableFoginShader;
	// Wind
	private SerializedProperty Wind;
	private SerializedProperty WindFrequency;
	private SerializedProperty WaveSizeFoliageShader;
	private SerializedProperty LeafTurbulenceFoliageShader;
	private SerializedProperty WindMultiplierForGrassshader;
	private SerializedProperty WaveSizeForGrassshader;
	private SerializedProperty WindJitterFrequencyForGrassshader;
	private SerializedProperty WindJitterScaleForGrassshader;
	private SerializedProperty WindMuliplierForTreeShaderPrimary;
	private SerializedProperty WindMuliplierForTreeShaderSecondary;
	private SerializedProperty WindMuliplierForTreeShader;
	private SerializedProperty SyncWindDir;
	// Rain
	private SerializedProperty RainAmount;
	// Terrain Detail Vegetation Settings
	private SerializedProperty VertexLitAlphaCutOff;
	private SerializedProperty VertexLitTranslucencyColor;
	private SerializedProperty VertexLitTranslucencyViewDependency;
	//private SerializedProperty VertexLitShadowStrength;
	private SerializedProperty VertexLitSpecularReflectivity;
	private SerializedProperty TerrainFoliageNrmSpecMap;
	// Grass, Tree and Billboard settings
	private SerializedProperty AutoSyncToTerrain;
	private SerializedProperty SyncedTerrain;
	private SerializedProperty DetailDistanceForGrassShader;
	private SerializedProperty BillboardStart;
	private SerializedProperty BillboardFadeLenght;
	private SerializedProperty GrassWavingTint;
	// Tree Render settings
	private SerializedProperty TreeColor;
	//private SerializedProperty TreeBillboardShadows;
	private SerializedProperty BillboardFadeOutLength;
	private SerializedProperty BillboardAdjustToCamera;
	private SerializedProperty BillboardAngleLimit;
	// Shaded Billboards
	//private SerializedProperty AutosyncShadowColor;
	//private SerializedProperty BillboardShadowColor;
	//private SerializedProperty BillboardAmbientLightFactor;
	//private SerializedProperty BillboardAmbientLightDesaturationFactor;
	// Culling
	private SerializedProperty EnableCameraLayerCulling;
	private SerializedProperty SmallDetailsDistance;
	private SerializedProperty MediumDetailsDistance;
	// Special Render Settings
	private SerializedProperty AllGrassObjectsCombined;
//

//	Temp variables
	private Cubemap tempCube;
	private Texture2D tempTex;
	private Vector2 specFade;
	private Vector4 tempWind;
	//private bool fadeLenghtReseted = false;
	private Terrain[] allTerrains;

//	private Light[] DirLights;

	private Camera MainCamera;
	
	private bool SkyshopSHEnabled;

	private string toolTip;
	private bool isProSkin;

	public enum fogMode {
		Linear = 0,
        Exponential = 1,
        Exp2 = 2
	}

	private fogMode m_fogMode;


//	Icons
	private Texture icnLight;
	private Texture icnSpec;
	private Texture icnFog;
	private Texture icnWind;
	private Texture icnRain;
	private Texture icnTerr;
	private Texture icnTree;
	private Texture icnCulling;
	private Texture icnCamera;

	void OnEnable() {

		if (EditorGUIUtility.isProSkin) {
			if (icnLight == null) icnLight = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnLight.png", typeof(Texture)) as Texture;
			if (icnSpec == null) icnSpec = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnSpec.png", typeof(Texture)) as Texture;
			if (icnFog == null) icnFog = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnFog.png", typeof(Texture)) as Texture;
			if (icnWind == null) icnWind = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnWind.png", typeof(Texture)) as Texture;
			if (icnRain == null) icnRain = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnRain.png", typeof(Texture)) as Texture;
			if (icnTerr == null) icnTerr = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnTerrain.png", typeof(Texture)) as Texture;
			if (icnTree == null) icnTree = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnTree.png", typeof(Texture)) as Texture;
			if (icnCulling == null) icnCulling = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnCulling.png", typeof(Texture)) as Texture;
			if (icnCamera == null) icnCamera = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnCamera.png", typeof(Texture)) as Texture;
		}
		else {
			if (icnLight == null) icnLight = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnLight_br.png", typeof(Texture)) as Texture;
			if (icnSpec == null) icnSpec = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnSpec_br.png", typeof(Texture)) as Texture;
			if (icnFog == null) icnFog = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnFog_br.png", typeof(Texture)) as Texture;
			if (icnWind == null) icnWind = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnWind_br.png", typeof(Texture)) as Texture;
			if (icnRain == null) icnRain = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnRain_br.png", typeof(Texture)) as Texture;
			if (icnTerr == null) icnTerr = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnTerrain_br.png", typeof(Texture)) as Texture;
			if (icnTree == null) icnTree = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnTree_br.png", typeof(Texture)) as Texture;
			if (icnCulling == null) icnCulling = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnCulling_br.png", typeof(Texture)) as Texture;
			if (icnCamera == null) icnCamera = AssetDatabase.LoadAssetAtPath("Assets/Advanced Foliage Shader v4/Scripts/Icons/icnCamera_br.png", typeof(Texture)) as Texture;
		}

	}

	
	public override void OnInspectorGUI () {
        SetupAFS = new SerializedObject(target);
		GetProperties();
		SetupAdvancedFoliageShader script = (SetupAdvancedFoliageShader)target;

//	///////////////////////////////////////////////////
//	Style Settings
		// Colors for Pro
		Color myCol = new Color(.5f, .8f, .0f, 1f); // Light Green // new Color(.6f, .9f, .22f, 1f); // Light Green
		Color myBgCol = myCol; //new Color(.5f, .75f, .24f, 1f);
		Color SplitterCol = new Color(1f, 1f, 1f, 0.075f);
		Color SplitterCol1 = new Color(.6f, .9f, .22f, .005f);
		// Colors for Indie
		if (!EditorGUIUtility.isProSkin) {
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); // Dark Green
			myBgCol = new Color(0.94f,0.94f,0.94f,1.0f);
			SplitterCol = new Color(0f, 0f, 0f, 0.125f);
			SplitterCol1 = new Color(1f, 1f, 1f, 0.5f);
		}
		//Color myBlue = new Color(0.5f,0.7f,1.0f,1.0f);

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

		GUIStyle myRegularFoldoutStyle = new GUIStyle(myFoldoutStyle);
		myRegularFoldoutStyle.fontStyle = FontStyle.Normal;

		// Custom Label
		GUIStyle myLabel = new GUIStyle(EditorStyles.label);
		myLabel.normal.textColor = myCol;
		myLabel.onNormal.textColor = myCol;

		// Default icon Size
		EditorGUIUtility.SetIconSize( new Vector2(16,16));

		// Help Boxes
		//GUIStyle helpBoxes = GUI.skin.GetStyle("HelpBox");
		//RectOffset helpOffeset = new RectOffset(2, 3, 10, 10);
		//helpBoxes.richText = true;
		//helpBoxes.fontSize = 10;
		//helpBoxes.padding = helpOffeset;


//	///////////////////////////////////////////////////
		//CheckShaderFeatures ();

		MainCamera = Camera.main;
		if (MainCamera != null) {
			if(MainCamera.actualRenderingPath == RenderingPath.DeferredShading && !disableFoginShader.boolValue) {
				GUI.backgroundColor = Color.red;
				EditorGUILayout.HelpBox("Your Main Camera uses deferred shading but the AFS shaders still apply fog. That might end up in fog being applied two times to all plants and leaves.\nYou may want to update the shaders using the Fog Settings.", MessageType.Error, true);
				GUI.backgroundColor = Color.white;
			}
			GUILayout.Space(4);
		}

//	///////////////////////////////////////////////////
//	Lighting settings
		GUILayout.Space(4);
		GUI.backgroundColor = myBgCol;
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldLighting = EditorGUILayout.Foldout(script.FoldLighting, "Lighting Settings", myFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnLight), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		// FoldoutContent
		if (script.FoldLighting) {
			GUILayout.Space(-5);
			EditorGUILayout.BeginVertical("Box");
				DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(13);
					EditorGUILayout.BeginVertical();
						EditorGUILayout.LabelField("General Lighting Settings", myLabel);
						GUILayout.Space(2);
						EditorGUILayout.PropertyField(DirectionalLightReference, new GUIContent("Sun") );
						GUILayout.Space(6);
						EditorGUILayout.LabelField("Grass Lighting Settings", myLabel);
						if (DirectionalLightReference.objectReferenceValue == null) {
							GUI.enabled = false;
							GrassApproxTrans.boolValue = false;	
						}
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(GrassApproxTrans, GUIContent.none, GUILayout.Width(14) );
							EditorGUILayout.LabelField("Approximate Translucency on Grass");
						EditorGUILayout.EndHorizontal();
						if(GrassApproxTrans.boolValue){
							EditorGUILayout.HelpBox("Make sure you have assigned the 'Combined Normal/Translucency/Smoothness map' in the 'Terrain Vegetation Settings' below from which the translucency values will be picked.", MessageType.Warning, true);
						}
						GUI.enabled = true;

						//
						GUILayout.Space(6);
						EditorGUILayout.LabelField("Foliage specular Lighting Settings", myLabel);
						EditorGUI.BeginChangeCheck();
						specFade.x = EditorGUILayout.Slider("Specular Range", AfsSpecFade.vector2Value.x, 0.0f, 100.0f);
						specFade.y = EditorGUILayout.Slider("Specular Fade Lenght", AfsSpecFade.vector2Value.y, 0.0f, 100.0f);
						if (EditorGUI.EndChangeCheck()) {
							AfsSpecFade.vector2Value = specFade;
						}
						//
						GUILayout.Space(6);
						EditorGUILayout.LabelField("Tree Lighting Settings", myLabel);
						toolTip = "Fade length for translucency and real time shadows.";
						EditorGUILayout.PropertyField(BillboardFadeLenght, new GUIContent("Lighting Fade Length", toolTip) );
						//
					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(2);
				EditorGUILayout.EndVertical();
		}

//	///////////////////////////////////////////////////
//	Wind settings
		GUILayout.Space(4);
		GUI.backgroundColor = myBgCol;
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldWind  = EditorGUILayout.Foldout(script.FoldWind, "Wind Settings", myFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnWind), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		// FoldoutContent
		if (script.FoldWind) {
			GUILayout.Space(-5);
			EditorGUILayout.BeginVertical("Box");
				DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(13);
					EditorGUILayout.BeginVertical();
						EditorGUILayout.LabelField("General Wind Settings", myLabel);
						GUILayout.Space(2);
						GUILayout.Label ("Wind Direction (XYZ) Strength (W)");
						tempWind = Wind.vector4Value;
						if (SyncWindDir.boolValue) {
							//GUILayout.Space(2);
							EditorGUILayout.BeginHorizontal();
								GUI.enabled = false;
									Vector3 temtempwind = new Vector3(tempWind.x, tempWind.y, tempWind.z);
										temtempwind = EditorGUILayout.Vector3Field("", temtempwind);
								GUI.enabled = true;
								EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(120));
									GUILayout.Label ("W", GUILayout.MaxWidth(14));
										EditorGUI.BeginChangeCheck();
										//tempWind.w = EditorGUILayout.FloatField("", tempWind.w, GUILayout.MaxWidth(104));
										tempWind.w = EditorGUILayout.Slider("", tempWind.w, 0.0f, 1.0f, GUILayout.MaxWidth(115));
										if (EditorGUI.EndChangeCheck()) {
											Wind.vector4Value = tempWind;
										}
								EditorGUILayout.EndHorizontal();	
							EditorGUILayout.EndHorizontal(); 
						}
						else {
							GUILayout.Space(-14);
							EditorGUI.BeginChangeCheck();
							tempWind = EditorGUILayout.Vector4Field("", Wind.vector4Value);
							if (EditorGUI.EndChangeCheck()) {
								Wind.vector4Value = tempWind;
							}
							GUILayout.Space(2);
						}
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(SyncWindDir, GUIContent.none, GUILayout.Width(14) );
							EditorGUILayout.LabelField("Sync Wind Direction to Rotation");
						EditorGUILayout.EndHorizontal();

						GUILayout.Space(4);
						toolTip = "Frequency the wind changes over time. Effects grass and foliage.";
						EditorGUILayout.PropertyField(WindFrequency, new GUIContent("Wind Frequency", toolTip) );
						
						GUILayout.Space(6);
						GUILayout.Label ("Wind Settings for Foliage Shaders", myLabel);
						GUILayout.Space(2);
						toolTip = "The shader adds some variation to the bending taking the vertex position in world space and the 'Wave Size Foliage' "+
							"parameter into account. So smaller wave sizes will add more variety to a given area but also lead to slightly different amounts of bending on each vertex even of a single mesh. This might cause some strange distortion of your models – especially large models. " +
							"For this reason you should set the 'Wave Size' to at least 2 or even 3 times the bounding box size of the largest model.";
						EditorGUILayout.PropertyField(WaveSizeFoliageShader, new GUIContent("Wave Size Foliage", toolTip) );
						toolTip = "Factor the original frequency of the secondary bending gets multiplied with. It determines the max frequency leaves might bend in when effected by strong winds.";
						EditorGUILayout.PropertyField(LeafTurbulenceFoliageShader, new GUIContent("Leaf Turbulence", toolTip) );
						GUILayout.Space(6);
						GUILayout.Label ("Wind Settings for Grass Shaders", myLabel);
						GUILayout.Space(2);
						
						toolTip = "Factor to make the main bending of the grass fit the bending of the foliage. " +
							"Effects both: grass placed manually and grass placed within the terrain engine (if you use the 'atsVxx.WavingGrass vertexAlpha' shader).";
						EditorGUILayout.PropertyField(WindMultiplierForGrassshader, new GUIContent("Main Wind Strength", toolTip) );

						toolTip = "Similar to the 'Wave Size Foliage' parameter, but as grass models usually are pretty small even low values might look good. "+
							"It effects both: grass placed manually and grass placed within the terrain engine (if you use the 'AfsGrassShader Terrain' shader).";
						EditorGUILayout.PropertyField(WaveSizeForGrassshader, new GUIContent("Wind Jitter Wave Size", toolTip) );

						toolTip = "Defines the Speed of the second wind animation that is added on top and gives you some more natural disturbance.";
						EditorGUILayout.PropertyField(WindJitterFrequencyForGrassshader, new GUIContent("Wind Jitter Frequency", toolTip) );

						toolTip = "Defines the Scale of the second wind animation that is added on top.";
						EditorGUILayout.PropertyField(WindJitterScaleForGrassshader, new GUIContent("Wind Jitter Amplitude", toolTip) );

						GUILayout.Space(6);
						GUILayout.Label ("Wind Multipliers for Tree Creator Shaders", myLabel);
						GUILayout.Space(2);

						toolTip = "Adjust bending of the tree creator shaders to match speedtree.";
						EditorGUILayout.PropertyField(WindMuliplierForTreeShaderPrimary, new GUIContent("Primary Bending", toolTip) );
						EditorGUILayout.PropertyField(WindMuliplierForTreeShaderSecondary, new GUIContent("Secondary Bending", toolTip) );
						Vector4 tempTreeWind = new Vector4(0.0f, 0.0f, WindMuliplierForTreeShaderPrimary.floatValue, WindMuliplierForTreeShaderSecondary.floatValue);
						WindMuliplierForTreeShader.vector4Value = tempTreeWind;


					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(2);
				EditorGUILayout.EndVertical();
		}

//	///////////////////////////////////////////////////
//	Rain settings
		GUILayout.Space(4);
		GUI.backgroundColor = myBgCol;
		EditorGUILayout.BeginVertical("Box");
			// Foldout incl. Icon
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(-2);
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical();
					script.FoldRain  = EditorGUILayout.Foldout(script.FoldRain, "Rain Settings", myFoldoutStyle);
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
				EditorGUILayout.BeginVertical(GUILayout.Width(20) );
					// Label needs width!
					EditorGUILayout.LabelField(new GUIContent(icnRain), GUILayout.Width(20), GUILayout.Height(20));
				EditorGUILayout.EndVertical();
				GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		// FoldoutContent
		if (script.FoldRain) {
			GUILayout.Space(-5);
			EditorGUILayout.BeginVertical("Box");
				DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(13);
					EditorGUILayout.PropertyField(RainAmount, new GUIContent("Rain Amount") );
				EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

//	///////////////////////////////////////////////////
//	Terrain Vegetation Settings	
		GUILayout.Space(4);
		GUI.backgroundColor = myBgCol;
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldVegTerrain  = EditorGUILayout.Foldout(script.FoldVegTerrain, "Terrain Vegetation Settings", myFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				// GUILayout.Space(2);
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnTerr), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		// FoldoutContent
		if (script.FoldVegTerrain) {
			GUILayout.Space(-5);
			EditorGUILayout.BeginVertical("Box");
				DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(13);

					EditorGUILayout.BeginVertical();
						// General Billboard settings
						EditorGUILayout.LabelField("Sync Settings to Terrain", myLabel);
						GUILayout.Space(2);
					
						EditorGUILayout.BeginHorizontal();
							//script.AutoSyncToTerrain = EditorGUILayout.Toggle("", script.AutoSyncToTerrain, GUILayout.Width(14) );
							EditorGUILayout.PropertyField(AutoSyncToTerrain, GUIContent.none, GUILayout.Width(14) );
							EditorGUILayout.LabelField("Automatically sync with Terrain");
						EditorGUILayout.EndHorizontal();
					
						if(AutoSyncToTerrain.boolValue) {
							EditorGUILayout.PropertyField(SyncedTerrain, new GUIContent("Specify Terrain") );
							if(SyncedTerrain.objectReferenceValue != null){
								GUI.enabled = false;
							}
							else {
								EditorGUILayout.HelpBox("Please assign a terrain.", MessageType.Warning, true);
							}
						}
						GUILayout.Space(4);
							toolTip = "The distance (from camera) at which 3D tree objects will be replaced by billboard images (should fit your terrain’s settings).";
						EditorGUILayout.PropertyField(BillboardStart, new GUIContent("Billboard Start", toolTip));
						//if(TreeBillboardShadows.boolValue){
						//	GUI.enabled = true;
						//}
						GUI.enabled = true;
						
						if(AutoSyncToTerrain.boolValue) {
							GUI.enabled = false;
						}
							toolTip = "The distance (from camera) towards which grass will fade out (should fit your terrain’s settings).";
						EditorGUILayout.PropertyField(DetailDistanceForGrassShader, new GUIContent("Grass Fadeout Distance", toolTip));
							toolTip = "Overall color tint applied to grass objects (should fit your terrain’s settings).";
						EditorGUILayout.PropertyField(GrassWavingTint, new GUIContent("Grass Waving Tint", toolTip));
						if(AutoSyncToTerrain.boolValue) {
							GUI.enabled = true;
						}

						EditorGUILayout.BeginVertical();
							// General Vegetation settings
							GUILayout.Space(6);
							EditorGUILayout.LabelField("Terrain Detail Vegetation Settings", myLabel);
							GUILayout.Space(2);
								toolTip = "Cut off value for all models placed as 'detail mesh' using the built in terrain engine.";
							EditorGUILayout.PropertyField(VertexLitAlphaCutOff, new GUIContent("Alpha Cut Off", toolTip));
								toolTip = "Translucency Color for all models placed as 'detail mesh' using the built in terrain engine.";
							EditorGUILayout.PropertyField(VertexLitTranslucencyColor, new GUIContent("Translucency Color", toolTip));
								toolTip = "Translucency view dependency for all models placed as 'detail mesh' using the built in terrain engine.";
							EditorGUILayout.PropertyField(VertexLitTranslucencyViewDependency, new GUIContent("Translucency View Dependency", toolTip));
							//EditorGUILayout.PropertyField(VertexLitShadowStrength, new GUIContent("Shadow Strength") );
								toolTip = "Specular Reflectivity for all models placed as 'detail mesh' using the built in terrain engine.";
							EditorGUILayout.PropertyField(VertexLitSpecularReflectivity, new GUIContent("Specular Reflectivity", toolTip));
							//script.TerrainBendingMode = (TerrainBendingModes)EditorGUILayout.EnumPopup("Bending Parameters", script.TerrainBendingMode);
							GUILayout.Space(4);
							EditorGUILayout.BeginHorizontal();
								EditorGUI.BeginChangeCheck();
								tempTex = (Texture2D)EditorGUILayout.ObjectField(TerrainFoliageNrmSpecMap.objectReferenceValue, typeof(Texture2D), false, GUILayout.MinHeight(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64));
								if (EditorGUI.EndChangeCheck()) {
									TerrainFoliageNrmSpecMap.objectReferenceValue = tempTex;
								}
								EditorGUILayout.LabelField("Combined Normal/Translucency/Smoothness Map");
							EditorGUILayout.EndHorizontal();
							GUILayout.Space(2);
						EditorGUILayout.EndVertical();
					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
		
//	///////////////////////////////////////////////////
//	Tree and Billboard settings
		GUILayout.Space(4);
		GUI.backgroundColor = myBgCol;
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldBillboard  = EditorGUILayout.Foldout(script.FoldBillboard, "Tree and Billboard Settings", myFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				// GUILayout.Space(2);
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnTree), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		// FoldoutContent
		if (script.FoldBillboard) {
			GUILayout.Space(-5);
			EditorGUILayout.BeginVertical("Box");
				DrawSplitter1(EditorGUILayout.GetControlRect(false, 4), SplitterCol1);
				EditorGUILayout.BeginHorizontal();
					GUILayout.Space(13);
					EditorGUILayout.BeginVertical();
					
						// Tree and Billboard Render settings
						// GUILayout.Space(4);
						EditorGUILayout.LabelField("Tree and Billboard Render Settings", myLabel);
						GUILayout.Space(2);
						//
						EditorGUILayout.BeginHorizontal();
								toolTip = "Usually tree are simply tinted with black if you set the „color variation“ > 0 while painting trees. You may replace this color by any one you like using this color field.";
							EditorGUILayout.PropertyField(TreeColor, new GUIContent("Tint Color", toolTip));
						EditorGUILayout.EndHorizontal();
						GUILayout.Space(2);
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(BillboardAdjustToCamera, GUIContent.none, GUILayout.Width(14) );
							EditorGUILayout.LabelField("Align Billboards to Camera");
						EditorGUILayout.EndHorizontal();
						if (BillboardAdjustToCamera.boolValue) {
								toolTip = "Defines the angle (when viewed from above) the billboard shader will fade to the 'classical' upright oriented billboards in order to reduce distortion artifacts. The standard value is '30'.";
							EditorGUILayout.PropertyField(BillboardAngleLimit, new GUIContent("Angle Limit", toolTip));
						}
						GUILayout.Space(2);
						//
							toolTip = "Instead of simply culling the billboards beyond the 'Tree distance' specified in the terrain settings you may define a length over which the billboards will be smoothly faded out.";
						EditorGUILayout.PropertyField(BillboardFadeOutLength, new GUIContent("Billboard Fade Out Length", toolTip));
						GUILayout.Space(4);
						
						// "Shaded" Billboard settings
/*						GUILayout.Space(4);
						EditorGUILayout.LabelField("Shaded Billboard Settings", myLabel);
						GUILayout.Space(2);

						EditorGUILayout.PropertyField(DirectionalLightReference, new GUIContent("Light Reference") );
						EditorGUILayout.PropertyField(BillboardShadowColor, new GUIContent("Shadow Color") );
*/

					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

		}

//	///////////////////////////////////////////////////
//	Fog Settings
		GUILayout.Space(4);
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldFog  = EditorGUILayout.Foldout(script.FoldFog, "Fog Settings", myRegularFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				GUILayout.Space(3);
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnFog), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		// FoldoutContent
		if (script.FoldFog) {
			GUILayout.Space(-2);
			DrawSplitter(EditorGUILayout.GetControlRect(false, 4), SplitterCol);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(13);

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(disableFoginShader, new GUIContent(""), GUILayout.Width(14));
					GUILayout.Label("Disable Fog in all AFS Shaders using the forward path.");
				EditorGUILayout.EndHorizontal();


			/*	EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(AFSShader_Folder, new GUIContent("AFS Shader Path"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.BeginVertical();
							GUILayout.Space(4);
							EditorGUI.BeginChangeCheck();
							m_fogMode = (fogMode)EditorGUILayout.EnumPopup("Fog Mode", (fogMode)AFSFog_Mode.intValue );
							if (EditorGUI.EndChangeCheck()) {
								AFSFog_Mode.intValue = (int)m_fogMode;
							}
						EditorGUILayout.EndVertical();
						if (GUILayout.Button("Update Shaders", GUILayout.MaxWidth(110) )) {
								SetFogFeatures ();
							}
					EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical(); */
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(6);
		}
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();


//	///////////////////////////////////////////////////
//	Layer Culling Settings
		GUILayout.Space(4);
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldCulling  = EditorGUILayout.Foldout(script.FoldCulling, "Layer Culling Settings", myRegularFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				GUILayout.Space(3);
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnCulling), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		// FoldoutContent
		if (script.FoldCulling) {
			DrawSplitter(EditorGUILayout.GetControlRect(false, 4), SplitterCol);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(13);
				EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(EnableCameraLayerCulling, GUIContent.none, GUILayout.Width(14) );
						EditorGUILayout.LabelField("Enable Layer Culling");
					EditorGUILayout.EndHorizontal();
					if (EnableCameraLayerCulling.boolValue) {
						EditorGUILayout.PropertyField(SmallDetailsDistance, new GUIContent("Small Detail Distance") );
						EditorGUILayout.PropertyField(MediumDetailsDistance, new GUIContent("Medium Detail Distance") );
					}
				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4);
		}
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();

	//	Special Render Settings
		GUILayout.Space(4);
		EditorGUILayout.BeginVertical("Box");
		// Foldout incl. Icon
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space(-2);
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical();
				script.FoldRender  = EditorGUILayout.Foldout(script.FoldRender, "Special Render Settings", myRegularFoldoutStyle);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginVertical(GUILayout.Width(20) );
				GUILayout.Space(3);
				// Label needs width!
				EditorGUILayout.LabelField(new GUIContent(icnCamera), GUILayout.Width(20), GUILayout.Height(20));
			EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		// FoldoutContent
		if (script.FoldRender) {
			DrawSplitter(EditorGUILayout.GetControlRect(false, 4), SplitterCol);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(13);
				EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(AllGrassObjectsCombined, GUIContent.none, GUILayout.Width(14) );
						EditorGUILayout.LabelField("All Grass Objects Combined");
					EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4);
		}
		GUILayout.Space(-2);
		EditorGUILayout.EndVertical();
		GUILayout.Space(4);

//	////////////////////////
		SetupAFS.ApplyModifiedProperties();
	}

//	End Editor

//	///////////////////////////////////////////////////
//	Helper Functions

	void ResetBillboardFadeLength () {
		allTerrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
		for (int i = 0; i < allTerrains.Length; i ++) {
			// treeCrossFadeLength must be > 0 otherwise the number of draw calls will explode at steep viewing angles
			allTerrains[i].treeCrossFadeLength = 0.0f; //0.0001f;
		}
	}
	
	void RestoreBillboardFadeLength (float resetValue) {
		allTerrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
		for (int i = 0; i < allTerrains.Length; i ++) {
			allTerrains[i].treeCrossFadeLength = resetValue;
		}
	}

/*	void FindFirstDirectionalLight () {
		DirLights = FindObjectsOfType(typeof(Light)) as Light[];
		for (int i = 0; i < DirLights.Length; i ++) {
			//allTerrains[i].treeCrossFadeLength = resetValue;
			//Debug.Log(DirLights.transform.name);
		//	Debug.Log(DirLights.Length);
		//	Debug.Log(DirLights[i]);
		//	Debug.Log(DirLights[i].GetComponent<Light>().type);
			if(DirLights[i].GetComponent<Light>().type == LightType.Directional) {
				TerrianLight0.objectReferenceValue = DirLights[i]; //.transform.parent;
				//SetupAFS.TerrianLight0 = DirLights[i];
			//	SetupAFS.FindProperty("TerrianLight0") = TerrianLight0;
				SetupAFS.Update();
				//TerrianLight0.CopyFromSerializedProperty();

// warum klappt das nicht??

				Debug.Log("assigned " + TerrianLight0);
				
				//TerrianLight0 = SetupAFS.FindProperty("TerrianLight0");
				//break;
			}
		}

	} */

	void CheckShaderFeatures () {
		SetupAdvancedFoliageShader script = (SetupAdvancedFoliageShader)target;

		string ShaderFolder = script.AFSShader_Folder;

		string[] ShaderPath = new string[] {
			"advancedFoliageShader IBL v3.shader"
		};
		for( int i = 0; i < ShaderPath.Length; i ++ ) {
			if (System.IO.File.Exists(ShaderFolder+ShaderPath[i])) {
				string ShaderCode = System.IO.File.ReadAllText(ShaderFolder+ShaderPath[i]);
					
					SkyshopSHEnabled = CheckShaderFeature (ref ShaderCode, "DIFFCUBE_ON");

				Debug.Log(SkyshopSHEnabled);

			}
		}
	}

	void SetFogFeatures () {
		SetupAdvancedFoliageShader script = (SetupAdvancedFoliageShader)target;
		Debug.Log("set fog");
		bool Disable = false;
		bool Enable = true;

		string ShaderFolder = script.AFSShader_Folder;

		string[] ShaderPath = new string[] {
			"advancedFoliageShader IBL Raindrops v3.shader"
			/*"advancedFoliageShader IBL v3.shader",
			"advancedFoliageShader IBL Raindrops v3.shader",
			"advancedFoliageShader IBL TouchBending v3.shader",
			"advancedFoliageShader IBL TouchBending Raindrops v3.shader",

			"advancedFoliageShader GS IBL v3.shader",
			"advancedFoliageShader GS IBL TouchBending v3.shader",

			"advancedFoliageShader IBL Groundlighting v3.shader",

			"AfsSimpleTerrainTree IBL v3.shader",
			"AfsTreeCreatorBarkOptimized IBL v3.shader",
			"AfsTreeCreatorLeafsOptimized IBL v3.shader",

			"hidden Shaders/advancedFoliageShader for Terrain Engine v3 IBL.shader"*/
		};
		
		for( int i = 0; i < ShaderPath.Length; i ++ ) {
			if (System.IO.File.Exists(ShaderFolder+ShaderPath[i])) {
				string ShaderCode = System.IO.File.ReadAllText(ShaderFolder+ShaderPath[i]);

				// Linear
				if (script.AFSFog_Mode == 0) {
					SetShaderFeature (ref ShaderCode, "FogExpo", Disable);
					SetShaderFeature (ref ShaderCode, "FogExp2", Disable);
					SetShaderFeature (ref ShaderCode, "FogLinear", Enable);
				}
				// Exponential 
				else if (script.AFSFog_Mode == 1) {
					SetShaderFeature (ref ShaderCode, "FogExpo", Enable);
					SetShaderFeature (ref ShaderCode, "FogExp2", Disable);
					SetShaderFeature (ref ShaderCode, "FogLinear", Disable);

				}
				// Exp2 
				else if (script.AFSFog_Mode == 2) {
					SetShaderFeature (ref ShaderCode, "FogExpo", Disable);
					SetShaderFeature (ref ShaderCode, "FogExp2", Enable);
					SetShaderFeature (ref ShaderCode, "FogLinear", Disable);
				}
				// Write back modified shader
				System.IO.File.WriteAllText(ShaderFolder + ShaderPath[i], ShaderCode);
			}
			else {
				Debug.Log("Shader not found: " + ShaderFolder + ShaderPath);
			}
		}
		AssetDatabase.Refresh();
	}
	
	private void SetShaderFeature(ref string ShaderCode, string DefineDefinition, bool EnableFeature) {
		int index = -1;
		string searchString = "";
		if (EnableFeature) {
			searchString = "// #define " + DefineDefinition;
			index = ShaderCode.IndexOf("// #define " + DefineDefinition, 0);
			if (index < 0) {
				searchString = "//#define " + DefineDefinition;
				index = ShaderCode.IndexOf("//#define " + DefineDefinition, 0);
			}
		}
		else {
			searchString = "#define " + DefineDefinition;
			if (ShaderCode.IndexOf("// #define " + DefineDefinition, 0) < 0) {
				index = ShaderCode.IndexOf("#define " + DefineDefinition, 0);
			}
		}
		if (index > 0) {
			string Code_Start = ShaderCode.Substring(0,index);
			string Code_Remainder = ShaderCode.Substring(index+searchString.Length);
			ShaderCode = Code_Start;
			if (EnableFeature) {
				ShaderCode +="#define " + DefineDefinition;
			} else {
				ShaderCode +="// #define " + DefineDefinition;
			}
			ShaderCode += Code_Remainder;
		}	
	}

	private bool CheckShaderFeature(ref string ShaderCode, string DefineDefinition) {
		int index = -1;
		bool result = false;
		//string searchString = "// #define " + DefineDefinition;
		index = ShaderCode.IndexOf("// #define " + DefineDefinition, 0);
		if (index < 0) {
			//searchString = "//#define " + DefineDefinition;
			index = ShaderCode.IndexOf("//#define " + DefineDefinition, 0);
		}
		if (index > 0) {
			result = true;
		}
		// return true if feature is enabled!
		return result;
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

	private void GetProperties() {
		//	Lighting
		DirectionalLightReference = SetupAFS.FindProperty("DirectionalLightReference");
		GrassApproxTrans = SetupAFS.FindProperty("GrassApproxTrans");
		// Specular
		AfsSpecFade = SetupAFS.FindProperty("AfsSpecFade");
		// Fog
		disableFoginShader = SetupAFS.FindProperty("disableFoginShader");
		// Wind
		Wind = SetupAFS.FindProperty("Wind");
		WindFrequency = SetupAFS.FindProperty("WindFrequency");
		WaveSizeFoliageShader = SetupAFS.FindProperty("WaveSizeFoliageShader");
		LeafTurbulenceFoliageShader = SetupAFS.FindProperty("LeafTurbulenceFoliageShader");
		WindMultiplierForGrassshader = SetupAFS.FindProperty("WindMultiplierForGrassshader");
		WaveSizeForGrassshader = SetupAFS.FindProperty("WaveSizeForGrassshader");
		WindJitterFrequencyForGrassshader = SetupAFS.FindProperty("WindJitterFrequencyForGrassshader");
		WindJitterScaleForGrassshader = SetupAFS.FindProperty("WindJitterScaleForGrassshader");
		WindMuliplierForTreeShaderPrimary = SetupAFS.FindProperty("WindMuliplierForTreeShaderPrimary");
		WindMuliplierForTreeShaderSecondary = SetupAFS.FindProperty("WindMuliplierForTreeShaderSecondary");
		WindMuliplierForTreeShader = SetupAFS.FindProperty("WindMuliplierForTreeShader");
		SyncWindDir = SetupAFS.FindProperty("SyncWindDir");
		// Rain
		RainAmount = SetupAFS.FindProperty("RainAmount");
		// Terrain Detail Vegetation Settings
		VertexLitAlphaCutOff = SetupAFS.FindProperty("VertexLitAlphaCutOff");
		VertexLitTranslucencyColor = SetupAFS.FindProperty("VertexLitTranslucencyColor");
		VertexLitTranslucencyViewDependency = SetupAFS.FindProperty("VertexLitTranslucencyViewDependency");
		//VertexLitShadowStrength = SetupAFS.FindProperty("VertexLitShadowStrength");
		VertexLitSpecularReflectivity = SetupAFS.FindProperty("VertexLitSpecularReflectivity");
		TerrainFoliageNrmSpecMap = SetupAFS.FindProperty("TerrainFoliageNrmSpecMap");
		// Grass, Tree and Billboard settings
		AutoSyncToTerrain = SetupAFS.FindProperty("AutoSyncToTerrain");
		SyncedTerrain = SetupAFS.FindProperty("SyncedTerrain");
		DetailDistanceForGrassShader = SetupAFS.FindProperty("DetailDistanceForGrassShader");
		BillboardStart = SetupAFS.FindProperty("BillboardStart");
		BillboardFadeLenght = SetupAFS.FindProperty("BillboardFadeLenght");
		GrassWavingTint = SetupAFS.FindProperty("GrassWavingTint");
		// Tree Render settings
		TreeColor = SetupAFS.FindProperty("AFSTreeColor");
		//TreeBillboardShadows = SetupAFS.FindProperty("TreeBillboardShadows");
		BillboardFadeOutLength = SetupAFS.FindProperty("BillboardFadeOutLength");
		BillboardAdjustToCamera = SetupAFS.FindProperty("BillboardAdjustToCamera");
		BillboardAngleLimit = SetupAFS.FindProperty("BillboardAngleLimit");
		//
		//AutosyncShadowColor = SetupAFS.FindProperty("AutosyncShadowColor");
		//BillboardShadowColor = SetupAFS.FindProperty("BillboardShadowColor");
		//BillboardAmbientLightFactor = SetupAFS.FindProperty("BillboardAmbientLightFactor");
		//BillboardAmbientLightDesaturationFactor = SetupAFS.FindProperty("BillboardAmbientLightDesaturationFactor");
		// Culling
		EnableCameraLayerCulling = SetupAFS.FindProperty("EnableCameraLayerCulling");
		SmallDetailsDistance = SetupAFS.FindProperty("SmallDetailsDistance");
		MediumDetailsDistance = SetupAFS.FindProperty("MediumDetailsDistance");
		// Special Render Settings
		AllGrassObjectsCombined = SetupAFS.FindProperty("AllGrassObjectsCombined");
	}

}
#endif
