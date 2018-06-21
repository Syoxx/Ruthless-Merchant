using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kevin Allgeyer
/// used to resize the terrain, attach to terrain and enter values
/// </summary>
public class TerrainSizer : MonoBehaviour {

    [SerializeField]
	private float sizeX, sizeY, sizeZ;
	private Vector3 terrainSize;
	private Terrain terrain;

	void Start () {
		terrain = GetComponent<Terrain>();
		terrainSize = new Vector3(sizeX, sizeY, sizeZ);
		terrain.terrainData.size = terrainSize;
		
	}
	

}
