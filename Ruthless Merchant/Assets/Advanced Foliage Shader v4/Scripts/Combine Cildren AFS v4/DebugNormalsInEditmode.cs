using UnityEngine;
using System.Collections;

// Make the script execute in edit mode.
[ExecuteInEditMode]
public class DebugNormalsInEditmode : MonoBehaviour {
	
	private CombineChildrenAFS cc;
	
	// Update is called once per frame
	void Update () {
		if (!Application.isPlaying) {
			cc = GetComponent<CombineChildrenAFS>();
			//if(cc.debugNormals) {
				Component[] filters = GetComponentsInChildren(typeof(MeshFilter));
				for (int i=0;i<filters.Length;i++) {
					Renderer curRenderer  = filters[i].GetComponent<Renderer>();
					///// sample ground normal
					Vector3 objectPos = curRenderer.transform.position;
					RaycastHit hit1;
					if (cc.GroundMaxDistance < 0f) {
						cc.GroundMaxDistance = 0.01f;
					}
					if (Physics.Raycast(objectPos + (Vector3.up * cc.GroundMaxDistance * 0.5f), Vector3.down, out hit1, cc.GroundMaxDistance)) {
						Debug.DrawLine(objectPos + (Vector3.up * cc.GroundMaxDistance * 0.5f), objectPos - (cc.GroundMaxDistance * Vector3.up * 0.5f), Color.green, 0.1f, false);
						Debug.DrawLine(objectPos, objectPos + (1.0f * hit1.normal), Color.red, 0.1f, false);
						
						// is it terrain? that makes a big difference!
						if (cc.UnderlayingTerrain) {
							Vector3 terrainPos = (hit1.point - cc.UnderlayingTerrain.transform.position) / cc.UnderlayingTerrain.terrainData.size.x;
							if (hit1.transform.gameObject.name == cc.UnderlayingTerrain.name ){
								if(cc.debugNormals) {
									Debug.DrawLine(objectPos, objectPos + (cc.UnderlayingTerrain.terrainData.GetInterpolatedNormal(terrainPos.x, terrainPos.z)), Color.blue, 5.0f, false);
								}
								hit1.normal = cc.UnderlayingTerrain.terrainData.GetInterpolatedNormal(terrainPos.x, terrainPos.z);
							}
						}
					}
					else {
						hit1.normal = new Vector3 (0,1,0);
						Debug.DrawLine(objectPos, objectPos + (1.0f * hit1.normal), Color.yellow, 0.1f, false);
					}
					
				}
			//}

		}	
	}
}
