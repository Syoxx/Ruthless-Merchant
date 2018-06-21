using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSizer : MonoBehaviour {

	public float sizeX, sizeY, sizeZ;
	private Vector3 terrainSize;
	private Terrain terrain;

	void Start () {
		terrain = GetComponent<Terrain>();
		terrainSize = new Vector3(sizeX, sizeY, sizeZ);
		terrain.terrainData.size = terrainSize;
		
	}
	

}
