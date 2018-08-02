#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

// Make the script execute in edit mode.
[ExecuteInEditMode]
[AddComponentMenu("AFS/Trees/AFS Xtra Tree Bending")]
public class TreeXtrabending : MonoBehaviour {

	[Range(0.0f, 10.0f)]
	public float AfsXtraLeafBending = 0.0f;
	
	// Update is called once per frame
	void Update () {
		//Material[] materials = GetComponent<Renderer>().sharedMaterials;
		for (int i = 0; i < GetComponent<Renderer>().sharedMaterials.Length; i++) {
			if (GetComponent<Renderer>().sharedMaterials[i].HasProperty("_AfsXtraBending")) {
				GetComponent<Renderer>().sharedMaterials[i].SetFloat("_AfsXtraBending", AfsXtraLeafBending);
			}
		}
	}
}
#endif