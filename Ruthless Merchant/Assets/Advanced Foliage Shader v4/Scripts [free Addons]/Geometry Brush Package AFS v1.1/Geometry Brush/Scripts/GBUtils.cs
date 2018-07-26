#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GBUtils : MonoBehaviour {
	
	public static void ApplyGeometryBrush( Vector2 mousePos ){
		RaycastHit m_RaycastHit;		
		// First, find the settings object in the scene. If missing, inform the user and stop the operation.
		GBSettings gbSettingsScript;
		if ( GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		if ( gbSettingsScript.activeGeometry.Count == 0 ) return;
		
		// the first raycast is the same regardless of our raycast mode. fire a ray from the camera to the mouse position.
		Ray m_Ray = HandleUtility.GUIPointToWorldRay( mousePos );
		
		if ( Physics.Raycast(m_Ray, out m_RaycastHit ) ){
			float positionOffset = gbSettingsScript.brushSize;

			RaycastHit firstHit = m_RaycastHit;
			
			// if a hit has been detected, spread geometry out in the area around the hit, based on the size of the current geometry brush.
			int placedObjects = 0;
			int skippedObjects = 0;
			
			for ( int i = 0; i < gbSettingsScript.brushSize; i++ ){
				float randX = Random.Range (-1.0f, 1.0f);
				float randZ = Random.Range (-1.0f, 1.0f);
				
				// based on our current mode, either fire raycasts from the edges of the sphere toward the other side (sphere), or from straight up (cone).
				Vector3 targetDirection = Vector3.zero;
				Vector3 startPos = Vector3.one;
				//float castDistance = Mathf.Infinity;
				
				if ( gbSettingsScript.raycastMode == GBSettings.RaycastMode.coneCast ){
					// pick a random point on the circle, and fire from the height of the cone to that point. (straight down)
					startPos = new Vector3(randX, 0, randZ);
					startPos *= ( (positionOffset * Random.Range (0.0f, 1.0f))/Mathf.Sqrt(randX * randX + randZ * randZ) );
					startPos += firstHit.point;
					startPos.y = m_Ray.origin.y;
					
					targetDirection = Vector3.down;
				} else if ( gbSettingsScript.raycastMode == GBSettings.RaycastMode.sphereCast ){
					// pick a random point on the sphere.
					startPos = GetRandomSpherePoint( firstHit.point, gbSettingsScript.brushSize );
					
					// also get the antipodal point.
					Vector3 targetPos = (startPos - firstHit.point);
					targetPos.Scale ( new Vector3(-1.0f,-1.0f,-1.0f) );
					
					// in reality, what we should be doing is this:
					// get the normal of the raycast.
					// find the plane using that normal.
					// split the sphere into two hemispheres
					// choose a random point on the upper hemisphere
					// cast a raycast down toward the point on the lower hemisphere
					
//					int axis = Random.Range (0,3);
//					if ( axis == 0 ){
//						targetPos = Quaternion.AngleAxis( 180.0f, new Vector3(1,0,0) ) * targetPos;
//					} else if ( axis == 1 ){
//						targetPos = Quaternion.AngleAxis( 180.0f, new Vector3(0,1,0) ) * targetPos;
//					} else if ( axis == 2 ){
//						targetPos = Quaternion.AngleAxis( 180.0f, new Vector3(0,0,1) ) * targetPos;
//					}
					
					targetPos += firstHit.point;
					
					// fire the raycast from the random point to its opposite point.
					targetDirection = targetPos - startPos;
					
					//castDistance = targetDirection.magnitude;
					targetDirection.Normalize();
				}
				
				float scaleFloat = Random.Range ( gbSettingsScript.minScale, gbSettingsScript.maxScale );
				Vector3 scale = new Vector3( scaleFloat, scaleFloat, scaleFloat );
				
				int randomID = Random.Range ( 0, gbSettingsScript.activeGeometry.Count );
				
				Bounds bounds = gbSettingsScript.activeGeometry[randomID].transform.GetComponent<Renderer>().bounds;
				float radius = bounds.size.x;
				if ( bounds.size.z > radius ){
					radius = bounds.size.z;	
				}
				radius *= (scaleFloat/2);
				
				radius *= gbSettingsScript.spacing;
				
	

// if ( Physics.SphereCast) does not work in Unity 5
// "Smooth sphere collisions are removed both from terrain and meshes."?
// quick hack: simply removed
				//if ( Physics.SphereCast( startPos, radius, targetDirection, out m_RaycastHit, castDistance ) ){
					Transform targetTile = m_RaycastHit.transform;
					if ( gbSettingsScript.preventOverlap ){
						bool skipObject = false;
						for ( int j = 0; j < gbSettingsScript.activeGeometry.Count; j++ ){
							if ( NamesAreEquivalent( targetTile.name, gbSettingsScript.activeGeometry[j].name ) ){
								// skip this one.
								skippedObjects++;
								if ( skippedObjects < 10 ){
									// only try again if we haven't run into an infinite-loop situation.
									i = placedObjects - 1;
								}
								skipObject = true;
							}
						}
						if ( skipObject ){
							continue;
						}
					}
					
					float rotX = ( gbSettingsScript.randomRotX ? Random.Range (0.0f, 360.0f) : 0.0f);
					float rotY = ( gbSettingsScript.randomRotY ? Random.Range (0.0f, 360.0f) : 0.0f);
					float rotZ = ( gbSettingsScript.randomRotZ ? Random.Range (0.0f, 360.0f) : 0.0f);
					Quaternion rotation = Quaternion.Euler(rotX, rotY, rotZ);


					
					// perform a secondary raycast to get the exact normal and point of placement.
					if ( Physics.Raycast(startPos, targetDirection, out m_RaycastHit ) ){ //, Mathf.Infinity, mask ) ){
						if ( gbSettingsScript.preventOverlap ){
							for ( int j = 0; j < gbSettingsScript.activeGeometry.Count; j++ ){
								if ( NamesAreEquivalent( targetTile.name, gbSettingsScript.activeGeometry[j].name ) ){
									continue;
								}
							}
						}
						Vector3 placementPos = m_RaycastHit.point;
						placementPos.y += gbSettingsScript.yOffset;
						CreateAndPlaceObject( gbSettingsScript.activeGeometry[randomID], placementPos, m_RaycastHit.normal, scale, rotation, gbSettingsScript, randomID );						
					}
// if				}				
				placedObjects++;
			}
		}		
	}
	
	public static void EraseGeometryBrush( Vector2 mousePos ){
		// cast a sphere out to the hit point, with radius equal to the deletion range. destroy all objects sharing a name with an item in the geometry brush.
		//RaycastHit[] m_RaycastHits;
		
//		LayerMask mask = LayerMaskExtensions.Create ("Default", "SetTile", "Water", "Grid");
		
		// First, find the settings object in the scene. If missing, inform the user and stop the operation.
		GBSettings gbSettingsScript;
		if ( GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		
		// Cast a ray to the mouse position. If it hits, determine what operation to preform from there.
		Ray m_Ray = HandleUtility.GUIPointToWorldRay( mousePos );
		RaycastHit m_RaycastHit;
		if ( Physics.Raycast(m_Ray, out m_RaycastHit ) ){ 
			RaycastHit firstHit = m_RaycastHit;
		
			Vector3 startPos, targetDirection;
		
			// if we are not firing from the camera, fire from directly above the hit point.
			if ( gbSettingsScript.fireFromCamera == false ){
				startPos = firstHit.point;
				startPos.y = m_Ray.origin.y;
				
				targetDirection = Vector3.down;
			} else {
				// if we are firing from the camera, fire from the camera (the initial ray's origin) and toward the hit point.
				startPos = m_Ray.origin;
				
				targetDirection = firstHit.point - startPos;
				targetDirection.Normalize();
			}
			
			/* we do not use this as it needs collider....
			m_RaycastHits = Physics.SphereCastAll( startPos, gbSettingsScript.brushSize, targetDirection );
			foreach( RaycastHit hit in m_RaycastHits ){
				for ( int i = 0; i < gbSettingsScript.activeGeometry.Count; i++ ){
					if ( hit.transform != null && NamesAreEquivalent( hit.transform.name, gbSettingsScript.activeGeometry[i].name ) ){
						DestroyImmediate ( hit.transform.gameObject );
					}	
				}
			}*/
			
			Component[] filters = gbSettingsScript.parentObject.GetComponentsInChildren(typeof(MeshFilter));
			
			for ( int i = 0; i < filters.Length; i++ ){
				Renderer curRenderer  = filters[i].GetComponent<Renderer>();
				float distance = Vector3.Distance(curRenderer.transform.position, firstHit.point);
				if ( distance < gbSettingsScript.brushSize) {
					DestroyImmediate ( curRenderer.transform.gameObject );	
				}
			}
		}
	}
	
	public static bool GetGeometryBrushToolObject( out GameObject geoBrushObj ){
		geoBrushObj = GameObject.Find ("GBSettings");
		if ( geoBrushObj == null ){
			Debug.LogError ("Error, 'GBSettings' game object not found in scene. Please add the GBSettings prefab (Located in Plugins/GeometryBrush/Prefabs/) to the scene.");
			return false;
		}
		
		return true;
	}
	
	public static bool GetGBSettingsScript( out GBSettings gbSettingsScript ){
		GameObject geoBrushObj;
		
		if ( GetGeometryBrushToolObject( out geoBrushObj ) == false ){
			gbSettingsScript = null;
			return false;
		}
			
		gbSettingsScript = geoBrushObj.GetComponent<GBSettings>();
		
		if ( gbSettingsScript == null ){
			Debug.LogError ("Error, 'GBSettings' game object must have the 'GBSettings' script component attached.");
			gbSettingsScript = null;
			return false;
		}
		
		return true;
	}
	
	public static bool NamesAreEquivalent( string a, string b ){
		return ( TrimEndFromString( a, "(Clone)" ) == TrimEndFromString( b, "(Clone)" ) );
	}
	
	public static string TrimEndFromString( string s, string trimString ){
		string result = "";
		if ( s.EndsWith(trimString) ){
			result = s.Substring (0, s.LastIndexOf(trimString) );
		} else {
			result = s;	
		}
		return result;
	}
	
	public static void CreateAndPlaceObject( GameObject obj, Vector3 placementPos, Vector3 normal, Vector3 scale, Quaternion rotation, GBSettings gbSettingsScript, int randomID ){
		GameObject newObject;
		string assetPath = gbSettingsScript.activeGeometryPaths[randomID];
		string[] str = assetPath.Split( new char[]{'/'} );
		if ( str[str.Length-1].EndsWith(".prefab") == true && Application.isEditor ) {
			// inastantiate a prefab to keep link
			newObject = PrefabUtility.InstantiatePrefab( AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) ) as GameObject;
		}
		else {
			newObject = (GameObject)Instantiate(obj);
		}



		newObject.transform.localScale = scale;
		newObject.transform.position = placementPos;
		
		if ( gbSettingsScript.alignToNormal ){
			// newObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
			// always use randomRotation
			newObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal) * 
				Quaternion.Euler(
					Random.Range(-gbSettingsScript.randomRotation.x, gbSettingsScript.randomRotation.x),
					Random.Range(-gbSettingsScript.randomRotation.y, gbSettingsScript.randomRotation.y),
					Random.Range(-gbSettingsScript.randomRotation.z, gbSettingsScript.randomRotation.z)
				);
		} else {
			newObject.transform.rotation = Quaternion.Euler(
					Random.Range(-gbSettingsScript.randomRotation.x, gbSettingsScript.randomRotation.x),
					Random.Range(-gbSettingsScript.randomRotation.y, gbSettingsScript.randomRotation.y),
					Random.Range(-gbSettingsScript.randomRotation.z, gbSettingsScript.randomRotation.z)
				);
		}
		
		if ( gbSettingsScript.parentObject != null ){
			newObject.transform.parent = gbSettingsScript.parentObject.transform;	
		}
	}
	
	public static int RoundToNearestDivisibleNumber( ref float input, float divisibleNumber ){
		if ( input % divisibleNumber > divisibleNumber * 0.5f ){
			int division = (int)(input/divisibleNumber)+1;
			input = division * divisibleNumber;
			return division;
		} else {
			int division = (int)(input/divisibleNumber);
			input = division * divisibleNumber;
			return division;
		}
	}
	
	public static float FloatParseTextField( Rect layoutInfo, string controlName, ref string inputString, string defaultString, float clampMin, float clampMax ){
		string focus = GUI.GetNameOfFocusedControl();
		
		GUI.SetNextControlName( controlName );
		if ( focus == controlName ){
			inputString = GUI.TextField( layoutInfo, inputString );	
		} else {
			inputString = GUI.TextField( layoutInfo, defaultString );	
		}
		
		float result;
		float.TryParse( inputString, out result );
		
		result = Mathf.Clamp (result, clampMin, clampMax);
		return result;
	}
	
	private static Vector3 GetRandomSpherePoint( Vector3 center, float radius){
	   float u = Random.Range(0.0f,1.0f);
	   float v = Random.Range(0.0f,1.0f);
	   float theta = 2.0f * Mathf.PI * u;
	   float phi = Mathf.Acos(2.0f * v - 1.0f);
	   float x = center.x + (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
	   float y = center.y + (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
	   float z = center.z + (radius * Mathf.Cos(phi));
	   return new Vector3(x,y,z);
	}
}
#endif