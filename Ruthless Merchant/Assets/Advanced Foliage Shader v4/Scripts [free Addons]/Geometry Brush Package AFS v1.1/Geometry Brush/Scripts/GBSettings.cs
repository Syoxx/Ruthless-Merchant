using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GBSettings : MonoBehaviour {
	
	public enum RaycastMode{
		coneCast, sphereCast
	}
	
	// -- Window Options -- //
	public float brushSize = 10.0f;
	public bool fireFromCamera = false;
	public float minScale = 1.0f;
	public float maxScale = 1.0f;
	public float spacing = 5.0f;
	public bool preventOverlap = false;
	public float yOffset = 0.0f;
	public bool alignToNormal = true;
	
	public bool randomRotX = false;
	public bool randomRotY = false;
	public bool randomRotZ = false;
	public bool brushActive = true;
	
	public Vector3 randomRotation = new Vector3(0.0f, 360.0f, 0.0f);
	
	public bool delete = false;
	
	public RaycastMode raycastMode = RaycastMode.coneCast;
	
	// -- Basic Options -- //
	public GameObject parentObject;
	
	// -- Advanced Options -- //
	public float minBrushSize = 0.1f;
	public float maxBrushSize = 8.0f;
	
	public float minMinScale = 0.1f;
	public float maxMinScale = 5.0f;
	
	public float minMaxScale = 0.1f;
	public float maxMaxScale = 5.0f;
	
	public float minYOffset = -2.5f;
	public float maxYOffset = 2.5f;
	
	public float minSpacing = 0.1f;
	public float maxSpacing = 50.0f;
	
	// -- Internal Options -- //
	public Vector3 gizmoPos;
	public bool gizmoActive = false;
	public List<GameObject> activeGeometry;
	
	// we need the path if we want to instantiate the prefab
	public List<String> activeGeometryPaths;
	
	
	private void OnDrawGizmos(){
		if ( gizmoActive ){
			if ( raycastMode == RaycastMode.coneCast ){
				if (!delete) {
					Gizmos.color = Color.green;
				}
				else {
					Gizmos.color = Color.red;
				}
				Gizmos.DrawWireSphere( gizmoPos, brushSize );
			} else if ( raycastMode == RaycastMode.sphereCast ) {
				if (!delete) {
					Gizmos.color = Color.cyan;
				}
				else {
					Gizmos.color = Color.red;
				}
				Gizmos.DrawWireSphere( gizmoPos, 0.3f );
			}
		}
	}
}
