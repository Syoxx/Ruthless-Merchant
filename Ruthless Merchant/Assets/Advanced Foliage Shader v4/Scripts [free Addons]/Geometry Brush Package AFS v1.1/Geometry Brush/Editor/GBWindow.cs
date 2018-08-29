using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
 
public class GBWindow : EditorWindow {
	 
	[MenuItem("Geometry Brush/Open Window %g")]
	public static void OpenWindow(){
		// open the window, and set it as the focus.
		GBWindow window = (GBWindow)EditorWindow.GetWindow(typeof(GBWindow), true, "Geometry Brush Manager", true ); 
		window.minSize = new Vector2(390.0f,580.0f);
	} 
	
	void OnFocus() {
		// register for the OnSceneGUI event. (clear any previous registrations)
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}
	
	void OnDestroy() {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	
	public static void AddSelectionsToList(){
		GBSettings gbSettingsScript;
		if ( GBUtils.GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		
		for ( int i = 0; i < Selection.gameObjects.Length; i++ ){
			gbSettingsScript.activeGeometry.Add ( Selection.gameObjects[i] );	
		}
		//// save path to objects as well
		int i1 = 0;
		foreach ( UnityEngine.Object go in Selection.objects ){
			string assetPath = UnityEditor.AssetDatabase.GetAssetPath(go);
			gbSettingsScript.activeGeometryPaths.Add (assetPath);
			i1++;	
		}
	}
	
	private static bool ValidateAddSelectionsToList() {
		if ( Selection.gameObjects.Length == 0 ){
			return false;	
		}
		
		foreach ( UnityEngine.Object go in Selection.objects ){
			// make sure the file is an .fbx, .ma, or .prefab.
			string assetPath = UnityEditor.AssetDatabase.GetAssetPath(go);
			string[] str = assetPath.Split( new char[]{'/'} );
			if ( str[str.Length-1].EndsWith(".ma") == false && 
				str[str.Length-1].EndsWith(".fbx") == false && 
				str[str.Length-1].EndsWith(".prefab") == false ){
				return false;	
			} 
		}
		
		// if it made it here, return true.
		return true; 
	}
	
	public void OnGUI(){
		GBSettings gbSettingsScript;
		if ( GBUtils.GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		
		// Get the window.
		Rect thisRect = this.position;
		
		Color myback = GUI.backgroundColor;
		//Color myCol = Color.green; //new Color(0.8f,0.60f,1.3f,1.0f);
		Color myCol = new Color(.5f, .8f, .0f, 1f); // Light Green
		if (!EditorGUIUtility.isProSkin) {
			myCol = new Color(0.05f,0.45f,0.0f,1.0f); // Dark Green
		}
		//Color myCol  = new Color(2.0f,1.1f,0.0f,1.0f); //orig. yello orange
		//Color myCol = new Color(0.4f,0.62f,1.1f,1.0f); // orig. blue
		//Color myCol  = new Color(1.0f,6.0f,0.0f,1.0f); // crazy

		// Custom Label
		GUIStyle myLabel = new GUIStyle(EditorStyles.label);
		myLabel.normal.textColor = myCol;
		myLabel.onNormal.textColor = myCol;
		myLabel.fontStyle = FontStyle.Bold;


		//
		GUILayout.Space(10);
		EditorGUILayout.LabelField("Parent Game Object", myLabel);
		GUILayout.Space(5);
		gbSettingsScript.parentObject = (GameObject)EditorGUILayout.ObjectField(gbSettingsScript.parentObject, typeof(GameObject), true);
		EditorGUILayout.HelpBox("Select a game object from the hierarchy which will become parent of all drawn objects.", MessageType.Info, true);

		GUILayout.Space(15);
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Geometry Brush Settings", myLabel);
		GUILayout.Space(10);
			EditorGUI.BeginDisabledGroup (gbSettingsScript.raycastMode == GBSettings.RaycastMode.sphereCast);
				gbSettingsScript.brushSize = EditorGUILayout.Slider("Brush Size", gbSettingsScript.brushSize, gbSettingsScript.minBrushSize, gbSettingsScript.maxBrushSize);
			EditorGUI.EndDisabledGroup ();
		GUILayout.Space(5);
		gbSettingsScript.minScale = EditorGUILayout.Slider("Minimum Scale", gbSettingsScript.minScale, gbSettingsScript.minMinScale, gbSettingsScript.maxMinScale);
		gbSettingsScript.maxScale = EditorGUILayout.Slider("Maximum Scale", gbSettingsScript.maxScale, gbSettingsScript.minMaxScale, gbSettingsScript.maxMaxScale);
		gbSettingsScript.yOffset = EditorGUILayout.Slider("Ground Offset", gbSettingsScript.yOffset, gbSettingsScript.minYOffset, gbSettingsScript.maxYOffset);
		GUILayout.Space(5);
		GUILayout.Label ("Random Rotation", GUILayout.Width(146));
		GUILayout.Space(-18);
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label ("", GUILayout.Width(136));
			gbSettingsScript.randomRotation = EditorGUILayout.Vector3Field("", gbSettingsScript.randomRotation);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(18);
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label ("Raycast Mode", GUILayout.Width(146));
			bool coneCast = ( gbSettingsScript.raycastMode == GBSettings.RaycastMode.coneCast );
			coneCast = EditorGUILayout.Toggle("", coneCast, GUILayout.Width(14) );
			GUILayout.Label ("Cone", GUILayout.Width(100));
			if ( coneCast ) gbSettingsScript.raycastMode = GBSettings.RaycastMode.coneCast;
			
			bool sphereCast = ( gbSettingsScript.raycastMode == GBSettings.RaycastMode.sphereCast );
			sphereCast = EditorGUILayout.Toggle("", sphereCast, GUILayout.Width(14) );
			if ( sphereCast ) gbSettingsScript.raycastMode = GBSettings.RaycastMode.sphereCast;
			GUILayout.Label ("Sphere", GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5);
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label ("", GUILayout.Width(146));
			gbSettingsScript.alignToNormal = EditorGUILayout.Toggle("", gbSettingsScript.alignToNormal, GUILayout.Width(14));
			GUILayout.Label ("Align to Normal", GUILayout.Width(100));
			gbSettingsScript.preventOverlap = EditorGUILayout.Toggle("", gbSettingsScript.preventOverlap, GUILayout.Width(14) );
			GUILayout.Label ("Prevent Overlap");
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5);
		if ( gbSettingsScript.preventOverlap ){
			float geometrySpacingFloat = gbSettingsScript.spacing;
			geometrySpacingFloat = gbSettingsScript.spacing;
			geometrySpacingFloat = EditorGUILayout.Slider("Spacing", geometrySpacingFloat, gbSettingsScript.minSpacing, gbSettingsScript.maxSpacing );
			GBUtils.RoundToNearestDivisibleNumber( ref geometrySpacingFloat, 0.1f );
			gbSettingsScript.spacing = geometrySpacingFloat;
			EditorGUILayout.HelpBox("Spacing needs colliders attached to the objects which are drawn.", MessageType.Info, true);
		}
		
		EditorGUILayout.EndVertical();
		
		GUILayout.Space(10);
		
		EditorGUILayout.BeginVertical();
		if (gbSettingsScript.brushActive) {
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button( "Deactivate Brush", GUILayout.Height(36) ) ) {
				gbSettingsScript.brushActive = false;
			}
			GUI.backgroundColor = myback;
		}
		else {
			if (GUILayout.Button( "Activate Brush", GUILayout.Height(36) ) ) {
				gbSettingsScript.brushActive = true;
			}
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.HelpBox("While active, Left Click to place objects, Ctrl + Left Click to delete.", MessageType.Info, true);
		
		GUILayout.Space(15);
		EditorGUILayout.LabelField("Assigned Objects", myLabel);
		GUILayout.Space(5);
		
		for ( int i = 0; i < gbSettingsScript.activeGeometry.Count; i++ ){
			GUI.backgroundColor = myCol;
			EditorGUILayout.BeginVertical("Box", GUILayout.MaxWidth(thisRect.width - 9.0f) );
			EditorGUILayout.BeginHorizontal();
			GUI.backgroundColor = myback;
			
			// preview is not worth much as it is simply too dark
			//GUILayout.Box( EditorUtility.GetAssetPreview(gbSettingsScript.activeGeometry[i]), GUILayout.Width(46), GUILayout.Height(46) ); 
			
			EditorGUILayout.LabelField(gbSettingsScript.activeGeometry[i].name, GUILayout.MinWidth(thisRect.width - 94.0f));
			if (GUILayout.Button( "Remove", GUILayout.Width(75) ) ) {
				// remove this element from the list.
				gbSettingsScript.activeGeometry.RemoveAt( i );
				//
				gbSettingsScript.activeGeometryPaths.RemoveAt( i );
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
		}
		
		GUILayout.Space(5);
		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginDisabledGroup ( !ValidateAddSelectionsToList() );
			if (GUILayout.Button( "Add Selections to Brush", GUILayout.Height(28), GUILayout.Width(thisRect.width - 90.0f)) ) {
				AddSelectionsToList();	
			}
		EditorGUI.EndDisabledGroup();
		
		if (GUILayout.Button( "Remove all", GUILayout.Height(28), GUILayout.Width(77) ) ) {
				gbSettingsScript.activeGeometry.Clear();
				//
				gbSettingsScript.activeGeometryPaths.Clear();
			}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox("Select a prefab, .fbx or .ma in the Project Tab and click 'Add Selections to Brush'.", MessageType.Info, true);

		// Update the GBSettings object.
		if (GUI.changed){
			EditorUtility.SetDirty (gbSettingsScript);
		}
	}
	
	private bool m_ControlHeld = false;
	private void OnSceneGUI( SceneView sceneView ){
		
		GBSettings gbSettingsScript;
		if ( GBUtils.GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		
		if ( gbSettingsScript.brushActive == false ){
			gbSettingsScript.gizmoActive = false;
			return;
		}
		
		if (Event.current.button == 1) {
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			return;
		}
		
		// enable rotation
		if (Event.current.alt) {
			gbSettingsScript.gizmoActive = false;
			return;
		}
		
		// delete?
		if (Event.current.control ) {
			m_ControlHeld = true;
			gbSettingsScript.delete = true;
		}
		else {
			m_ControlHeld = false;
			gbSettingsScript.delete = false;
		}
		
		switch(Event.current.type){
			case EventType.Layout:
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
				break;
			case EventType.MouseMove:
				HandleUtility.Repaint();
				break;
			// start painting
			case EventType.MouseDrag:
				if(m_ControlHeld == false) {
					GBUtils.ApplyGeometryBrush( Event.current.mousePosition );
				}
				else {
					GBUtils.EraseGeometryBrush( Event.current.mousePosition );
				}
				break;
			case EventType.MouseUp:
				break;
			case EventType.MouseDown:
				if(m_ControlHeld == false) {
					GBUtils.ApplyGeometryBrush( Event.current.mousePosition );
				}
				else {
					GBUtils.EraseGeometryBrush( Event.current.mousePosition );
				}
				break;
			
		}
		
		// While hitting something, display the brush range.
		RaycastHit m_RaycastHit;
		Ray m_Ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
		if ( Physics.Raycast(m_Ray, out m_RaycastHit ) ){
			gbSettingsScript.gizmoPos = m_RaycastHit.point;
			gbSettingsScript.gizmoActive = true;
			HandleUtility.Repaint();
		} else {
			gbSettingsScript.gizmoActive = false;	
		}
	}
}
