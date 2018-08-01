using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine.Rendering;

//public enum TerrainBendingModes {
//    	VertexColors = 1,
//    	VertexColorsAndUV4 = 2
//}

[ExecuteInEditMode]
[AddComponentMenu("AFS/Setup Advanced Foliage Shader")]
public class SetupAdvancedFoliageShader : MonoBehaviour {
	//	Editor Variables
	#if UNITY_EDITOR
		public bool newEditor = true;
		public bool FoldLighting = false;
		public bool FoldFog = false;
		public bool FoldWind = false;
		public bool FoldRain = false;
		public bool FoldGrass = false;
		public bool FoldVegTerrain = false;
		public bool FoldSpecFade = false;
		public bool FoldBillboard = false;
		public bool FoldCulling = false;
		public bool FoldRender = false;
	#endif
	// Lighting
	public bool isLinear;
    public bool AmbientLightingSH;
	public GameObject TerrianLight0;
    public GameObject TerrianLight1;
    // Sun lighting
    public GameObject DirectionalLightReference;
    	private Vector3 DirectlightDir;
    	private Light Sunlight;
    	private Vector3 SunLightCol;
    	private float SunLuminance;
    public bool GrassApproxTrans = false;
	// Fog Mode
	public int AFSFog_Mode = 2; // default is exp2
	public string AFSShader_Folder = "Assets/Advanced Foliage Shader v4/Shaders/";
	public bool disableFoginShader = false;
	//	Wind parameters which are needed by all shaders
	public Vector4 Wind = new Vector4(0.85f,0.075f,0.4f,0.5f);
	[Range(0.01f, 2.0f)]
	public float WindFrequency = 0.25f;
	[Range(0.1f, 25.0f)]
	public float WaveSizeFoliageShader = 10.0f;
	[Range(0.0f, 10.0f)]
	public float LeafTurbulenceFoliageShader = 4.0f;
	//	Wind parameters only needed by the advanced grass shaders
	[Range(0.01f, 1.0f)]
	public float WindMultiplierForGrassshader = 1.0f;
	[Range(0.00f, 10.0f)]
	public float WaveSizeForGrassshader = 1.0f;
	[Range(0.0f, 1.0f)]
	public float WindJitterFrequencyForGrassshader = 0.25f;
		
	[Range(0.0f, 1.0f)]
	public float WindJitterScaleForGrassshader = 0.15f;
	public bool SyncWindDir = false;
	//	Wind Parameters for Tree Creator Shaders
	[Range(0.0f, 10.0f)]
	public float WindMuliplierForTreeShaderPrimary = 1.0f;
	[Range(0.0f, 10.0f)]
	public float WindMuliplierForTreeShaderSecondary = 1.0f;
	public Vector4 WindMuliplierForTreeShader = new Vector4(1,1,1,1);
		private float temp_WindFrequency = 0.25f;
		private float temp_WindJitterFrequency = 0.25f;
		const float TwoPI = 2*Mathf.PI;
		private float freqSpeed = 0.05f;
		private float domainTime_Wind = 0.0f;
		private float domainTime_2ndBending = 0.0f;
		private float domainTime_Grass = 0.0f;
	//	Rain Settings
	[Range(0.0f, 1.0f)]
	public float RainAmount = 0.0f;
	//	Terrain Detail Vegetation Settings
	[Range(0.0f, 1.0f)]
	public float VertexLitAlphaCutOff = 0.3f;
	public Color VertexLitTranslucencyColor = new Color(0.73f,0.85f,0.4f,1f);
	[Range(0.0f, 1.0f)]
	public float VertexLitTranslucencyViewDependency = 0.7f;
	[Range(0.0f, 1.0f)]
	public float VertexLitShadowStrength = 0.8f;
	public Color VertexLitSpecularReflectivity = new Color(0.2f,0.2f,0.2f,1f);
	[Range(0.0f, 100.0f)]
	public Vector2 AfsSpecFade = new Vector2(30.0f, 10.0f);
	//public TerrainBendingModes TerrainBendingMode = TerrainBendingModes.VertexColors;
	public Texture TerrainFoliageNrmSpecMap;
	//	Grass, Tree and Billboard settings
	public bool AutoSyncToTerrain = false;
	public Terrain SyncedTerrain;
	public bool AutoSyncInPlaymode = false;
	public float DetailDistanceForGrassShader = 80.0f;
	[Range(0.0f, 1000.0f)]
	public float BillboardStart = 50.0f;
	[Range(0.0f, 30.0f)]
	public float BillboardFadeLenght = 5.0f;
	public bool GrassAnimateNormal = false;
	public Color GrassWavingTint = Color.white;
	//	Tree Render settings
	public Color AFSTreeColor = Color.black;
	//

	private Camera MainCam;

	public bool TreeBillboardShadows = false;
	[Range(10.0f, 100.0f)]
	public float BillboardFadeOutLength = 60.0f;
	public bool BillboardAdjustToCamera = true;
	[Range(10.0f, 90.0f)]
	public float BillboardAngleLimit = 30.0f;
		
	//	"Shaded" Billboard settings
	public Color BillboardShadowColor;
	[Range(0.0f, 4.0f)]
	public float BillboardAmbientLightFactor = 1.0f;
	[Range(0.0f, 2.0f)]
	public float BillboardAmbientLightDesaturationFactor = 0.7f;
	public bool AutosyncShadowColor = false;

	//	Camera Layer Culling Settings
	public bool EnableCameraLayerCulling = false;
	[Range(10, 300)]
	public int SmallDetailsDistance = 70;
	[Range(10, 300)]
	public int MediumDetailsDistance = 90;

	//	Special Render Settings
	public bool AllGrassObjectsCombined = false;

	// Init vars used by the scripts
	private Vector4 TempWind;
	private float GrassWindSpeed;
	private float SinusWave;
	private Vector4 TriangleWaves;
	private float Oscillation;
	private Vector3 CameraForward = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 ShadowCameraForward = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 CameraForwardVec;
	private float rollingX;
	private float rollingXShadow;
	private Vector3 lightDir;
	private Vector3 templightDir;
	private float CameraAngle;
	private Terrain[] allTerrains;
	
	private Vector3[] fLight = new Vector3[9];
	private Vector4[] vCoeff = new Vector4[3];

	#if UNITY_EDITOR
		private Color myCol = new Color(.5f, .8f, .0f, 1f); // Light Green
	#endif

//	////////////////////////////////

	void Awake () {
		//
		afsSyncFrequencies();
		//
		afsCheckColorSpace();
		afsLightingSettings();
		afsUpdateWind();
		afsUpdateRain();
		afsSetupTerrainEngine();
		afsAutoSyncToTerrain();
		afsUpdateGrassTreesBillboards();
		afsSetupCameraLayerCulling();
		//
		afsFogSettings();
		afsSetupGrassShader();
	}

	public void Update () {
		afsLightingSettings();
		//afsUpdateWind();
		afsUpdateRain();
		afsAutoSyncToTerrain();
		afsUpdateGrassTreesBillboards();
		#if UNITY_EDITOR
			afsCheckColorSpace();
			afsFogSettings();
			afsSetupTerrainEngine();
			afsSetupGrassShader();
		#endif
	}

	public void FixedUpdate() {	
		afsUpdateWind();
	}

//	////////////////////////////////
	#if UNITY_EDITOR
	void OnDrawGizmosSelected()
    {
    	float hsize = HandleUtility.GetHandleSize(transform.position);
    	Handles.color = myCol;
    	if(SyncWindDir) {
    		Handles.ArrowCap(0, transform.position, transform.rotation, hsize);
    	}
    	else {
    		Quaternion rotation = Quaternion.LookRotation(new Vector3(Wind.x, Wind.y, Wind.z));
    		Handles.ArrowCap(0, transform.position, rotation, hsize);
    	}
    }
    #endif

//	////////////////////////////////
//	Main Functions
//	////////////////////////////////

	void afsSyncFrequencies() {
		temp_WindFrequency = WindFrequency;
		temp_WindJitterFrequency = WindJitterFrequencyForGrassshader;
		domainTime_Wind = 0.0f;
		domainTime_2ndBending = 0.0f;
		domainTime_Grass = 0.0f;
	}

	void afsCheckColorSpace() {
		#if UNITY_EDITOR
			// LINEAR
			if (PlayerSettings.colorSpace == ColorSpace.Linear) {
				if(!isLinear) {
					Debug.Log("Colorspace changed to linear.");
				}
				isLinear = true;
			}
			// GAMMA
			else {
				if(isLinear) {
					Debug.Log("Colorspace changed to gamma.");
				}
				isLinear = false;
			}
		#endif
	}

//	Lighting Settings
	void afsLightingSettings() {
		// Udpate main Directional Light Values (Sun)
		if (DirectionalLightReference != null) {
			DirectlightDir = DirectionalLightReference.transform.forward;
			if (Sunlight == null) {
				Sunlight = DirectionalLightReference.GetComponent<Light>();
			}
			if (!Sunlight.enabled) {
				Shader.SetGlobalVector("_AfsDirectSunDir", new Vector4(0,0,0,0) );
			}
			else {
				SunLightCol = new Vector3 (Sunlight.color.r, Sunlight.color.g, Sunlight.color.b) * Sunlight.intensity;
				SunLuminance = Vector3.Dot(SunLightCol, new Vector3(0.22f, 0.707f, 0.071f) );
	     		Shader.SetGlobalVector("_AfsDirectSunDir", new Vector4(DirectlightDir.x, DirectlightDir.y, DirectlightDir.z, SunLuminance) );
	     		Shader.SetGlobalVector("_AfsDirectSunCol", SunLightCol);
			}
		}
		else {
			Shader.SetGlobalVector("_AfsDirectSunDir", new Vector4(0,0,0,0) );
			Shader.SetGlobalVector("_AfsDirectSunCol", new Vector3(0,0,0) );	
		}
		// Set ambient lighting for Tree Rendertex shaders
		UpdateLightingForClassicBillboards();
		// Set specular lighting for foliage shader
		Shader.SetGlobalVector("_AfsSpecFade", new Vector4(AfsSpecFade.x, AfsSpecFade.y, 1, 1 ));
		// Enable/disable approximated translucency in grass shaders
		if(GrassApproxTrans) {
			Shader.EnableKeyword ("_AFS_GRASS_APPROXTRANS");
		}
		else {
			Shader.DisableKeyword ("_AFS_GRASS_APPROXTRANS");
		}
	}

//	Fog Settings
	void afsFogSettings() {
		if(disableFoginShader) {
			Shader.EnableKeyword ("_AFS_DEFERRED");
		}
		else {
			Shader.DisableKeyword ("_AFS_DEFERRED");
		}
	}

//	Special Render Settings
	void afsSetupGrassShader() {
		//	Tell the "advancedGrassShader" how to lit the objects
		if (Application.isPlaying || AllGrassObjectsCombined) {
			// Lighting based on baked normals
			Shader.DisableKeyword("IN_EDITMODE");
			Shader.EnableKeyword("IN_PLAYMODE");
		}
		else {
			// Lighting according to rotation
			Shader.DisableKeyword("IN_PLAYMODE");
			Shader.EnableKeyword("IN_EDITMODE");
		}
	}

//	Terrain engine settings
	void afsSetupTerrainEngine() {
		Shader.SetGlobalFloat("_AfsAlphaCutOff", VertexLitAlphaCutOff);
		Shader.SetGlobalFloat("_AfsTranslucencyViewDependency", VertexLitTranslucencyViewDependency);
		Shader.SetGlobalFloat("_AfsShadowStrength", VertexLitShadowStrength);
		Shader.SetGlobalColor("_AfsTranslucencyColor", VertexLitTranslucencyColor);
		if(isLinear){
			Shader.SetGlobalColor("_AfsSpecularReflectivity", VertexLitSpecularReflectivity.linear);	
		}
		else {
			Shader.SetGlobalColor("_AfsSpecularReflectivity", VertexLitSpecularReflectivity);	
		}
		if(TerrainFoliageNrmSpecMap != null) {
			Shader.SetGlobalTexture("_TerrianBumpTransSpecMap", TerrainFoliageNrmSpecMap);
		}
		else {
			Shader.SetGlobalTexture("_TerrianBumpTransSpecMap", null);	
		}
	}

//	Update Wind Settings / Simple wind animation for the foliage and grass shaders
	void afsUpdateWind() {
	//	Sync Wind Dir to rotation
		if (SyncWindDir) {
			Wind = new Vector4(transform.forward.x, transform.forward.y, transform.forward.z, Wind.w);
		}
	//	Set Main Wind
		TempWind = Wind;
		TempWind.w *= 2.0f;
		Shader.SetGlobalVector("_Wind", TempWind);
		// Currently not used in the shaders, see below
		Shader.SetGlobalFloat("_WindFrequency", WindFrequency);
		// We must not use Unity.Time.time and WindFrequency/WindJitterFrequencyForGrassshader directly, so:
		// http://answers.unity3d.com/questions/274098/how-to-smooth-between-values.html
		domainTime_Wind = (domainTime_Wind + temp_WindFrequency * Time.deltaTime * 2.0f); // % TwoPI; / % TwoPI causes hiccups
		domainTime_2ndBending = domainTime_2ndBending + Time.deltaTime;
		// x: time * frequency, y: time, zw: turbulence for 2nd bending
		Shader.SetGlobalVector("_AfsTimeFrequency", new Vector4(domainTime_Wind, domainTime_2ndBending, 0.375f * (1.0f + temp_WindFrequency * LeafTurbulenceFoliageShader), 0.193f * (1.0f + temp_WindFrequency * LeafTurbulenceFoliageShader)));
		//Shader.SetGlobalFloat("_AfsTurbulence", 0.375f * (1.0f + temp_WindFrequency * LeafTurbulenceFoliageShader));
		temp_WindFrequency = Mathf.MoveTowards(temp_WindFrequency, WindFrequency, freqSpeed);
	//	Calculate Wind for Grass shaders
		// SinusWave = Mathf.Sin(UnityEngine.Time.time * 2.0f * WindFrequency);
		SinusWave = Mathf.Sin(domainTime_Wind);
		TriangleWaves = SmoothTriangleWave( new Vector4(SinusWave, SinusWave * 0.8f, 0.0f, 0.0f));
		Oscillation = TriangleWaves.x + (TriangleWaves.y * TriangleWaves.y);
		Oscillation = (Oscillation + 8.0f) * 0.125f * TempWind.w;
		TempWind.x *= Oscillation;
		TempWind.z *= Oscillation;
		Shader.SetGlobalFloat("_AfsWaveSize", (0.5f / WaveSizeFoliageShader) );
		Shader.SetGlobalFloat("_AfsWindJitterScale", WindJitterScaleForGrassshader * 10.0f); // 4.0f	
		Shader.SetGlobalVector("_AfsGrassWind", TempWind * WindMultiplierForGrassshader);
		// We must not use Unity.Time.time and WindJitterFrequencyForGrassshader directly, so:
		domainTime_Grass = (domainTime_Grass + temp_WindJitterFrequency * Time.deltaTime); // % TwoPI; / % TwoPI causes hiccups
		temp_WindJitterFrequency = Mathf.MoveTowards(temp_WindJitterFrequency, WindJitterFrequencyForGrassshader, freqSpeed);
		// GrassWindSpeed = UnityEngine.Time.time * WindJitterFrequencyForGrassshader * 0.1f;
		GrassWindSpeed = domainTime_Grass * 0.1f;
		// _AfsWaveAndDistance = Wind speed, wave size, wind amount, max pow2 distance
		Shader.SetGlobalVector("_AfsWaveAndDistance", new Vector4( GrassWindSpeed, WaveSizeForGrassshader, TempWind.w, DetailDistanceForGrassShader * DetailDistanceForGrassShader ) );
		Shader.SetGlobalVector("_AFSWindMuliplier", WindMuliplierForTreeShader);
	}

//	Update Rain Settings
	void afsUpdateRain() {
		Shader.SetGlobalFloat("_AfsRainamount", RainAmount);
	}

//	AutoSyncToTerrain
	void afsAutoSyncToTerrain() {
		if(AutoSyncToTerrain && SyncedTerrain != null) {
			DetailDistanceForGrassShader = SyncedTerrain.detailObjectDistance;
			BillboardStart = SyncedTerrain.treeBillboardDistance;
			// We only correct this if the app is playing. Otherwise it collides with the editor.
			if (Application.isPlaying) {
				if(!TreeBillboardShadows) {
				BillboardFadeLenght = SyncedTerrain.treeCrossFadeLength;
				}
			}
			GrassWavingTint = SyncedTerrain.terrainData.wavingGrassTint;
		}
	}
	
//	Grass, Tree and Billboard Settings
	void afsUpdateGrassTreesBillboards() {
		// DetailDistanceForGrassShader has already been passed with: _AfsWaveAndDistance
		Shader.SetGlobalColor("_AfsWavingTint", GrassWavingTint); 
		// Tree Variables
		Shader.SetGlobalColor("_AfsTreeColor", AFSTreeColor);
		// Billboardstart
		Shader.SetGlobalVector("_AfsTerrainTrees", new Vector4(BillboardStart, BillboardFadeLenght, BillboardFadeOutLength, 0 ));
		//	Camera Settings for the Billboard Shader
		if (BillboardAdjustToCamera) {
			if (Camera.main) {
				MainCam = Camera.main;
				
			/*	#if UNITY_EDITOR
					if (UnityEditor.SceneView.currentDrawingSceneView){
						MainCam = UnityEditor.SceneView.currentDrawingSceneView.camera;
					}
					else {
						MainCam = Camera.main;
					}
				#endif */

				CameraForward = MainCam.transform.forward;
				ShadowCameraForward = CameraForward;
				rollingX = MainCam.transform.eulerAngles.x;
				if (rollingX >= 270.0f) {					// looking up
					rollingX = (rollingX - 270.0f);
					rollingX = (90.0f - rollingX);
					rollingXShadow = rollingX;
				}
				else {										// looking down
					rollingXShadow = -rollingX;
					if (rollingX > BillboardAngleLimit) {
						rollingX = Mathf.Lerp(rollingX, 0.0f,  (rollingX / BillboardAngleLimit) - 1.0f );
					}
					rollingX *= -1;
				}
			}
			else {
				Debug.Log("You have to tag your Camera as MainCamera");
			}
		}
		else {
			rollingX = 0.0f;
			rollingXShadow = 0.0f;
		}
		CameraForward *= rollingX / 90.0f;
		ShadowCameraForward *= rollingXShadow / 90.0f;
		Shader.SetGlobalVector("_AfsBillboardCameraForward", new Vector4( CameraForward.x, CameraForward.y, CameraForward.z, 1.0f));
		Shader.SetGlobalVector("_AfsBillboardShadowCameraForward", new Vector4( ShadowCameraForward.x, ShadowCameraForward.y, ShadowCameraForward.z, 1.0f));
		
		// Set lightDir for Billboard Shadows
		/*
		if (TreeBillboardShadows) {
			if (DirectionalLightReference != null) {
				lightDir = DirectionalLightReference.transform.forward;
				templightDir = lightDir;
				lightDir = Vector3.Cross(lightDir, Vector3.up);
				// flip lightDir if camera is aligned with light
				if (Vector3.Dot(templightDir, MainCam.transform.forward) > 0) {
					lightDir = Quaternion.AngleAxis(180, Vector3.up) * lightDir;
				}
				Shader.SetGlobalVector("_AfsSunDirection", new Vector4(lightDir.x, lightDir.y, lightDir.z, 1) );
				CameraForwardVec = MainCam.transform.forward;
				allTerrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
				// We only correct this if the app is playing. Otherwise it collides with the editor.
				if (Application.isPlaying) {
					for (int i = 0; i < allTerrains.Length; i ++) {
						allTerrains[i].treeCrossFadeLength = 0.0f; //0.0001f;
					}
				}
				allTerrains = null;
				CameraAngle = MainCam.fieldOfView;
				CameraAngle = CameraAngle - CameraAngle * BillboardShadowEdgeFadeThreshold;
				CameraAngle = Mathf.Cos( CameraAngle * Mathf.Deg2Rad );
				Shader.SetGlobalVector("_CameraForwardVec", new Vector4(CameraForwardVec.x, CameraForwardVec.y, CameraForwardVec.z, CameraAngle) );
			}
			else {
				Debug.LogWarning("You have to specify a Light Reference!");
			}
		}*/
		/*
		if (AutosyncShadowColor) {
			//	Set desaturated ambient light for shaded billboards
			BillboardShadowColor = RenderSettings.ambientLight;
			BillboardShadowColor = Desaturate(BillboardShadowColor.r * BillboardAmbientLightFactor, BillboardShadowColor.g * BillboardAmbientLightFactor, BillboardShadowColor.b * BillboardAmbientLightFactor);
			if (DirectionalLightReference) {
				BillboardShadowColor += 0.5f * (BillboardShadowColor * (1.0f - DirectionalLightReference.GetComponent<Light>().shadowStrength));
			}
		}
		Shader.SetGlobalColor("_AfsAmbientBillboardLight", BillboardShadowColor );
		*/
	}

//	Camera Layer Culling Settings
	void afsSetupCameraLayerCulling() {
		if(EnableCameraLayerCulling) { 
			for (int i = 0; i < Camera.allCameras.Length; i++) {
				float[] distances = new float[32];
				distances = Camera.allCameras[i].layerCullDistances;
				distances[8] = SmallDetailsDistance;		// small things like DetailDistance of the terrain engine
				distances[9] = MediumDetailsDistance;		// small things like DetailDistance of the terrain engine
				Camera.allCameras[i].layerCullDistances = distances;
				distances = null;
			}
		}
	}

//	////////////////////////////////

//	Helper functions

	//	Update Light Settings for Tree Creator Rendertex Shaders
	private void UpdateLightingForClassicBillboards() {
		// Skybox
        if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Skybox) {
            AmbientLightingSH = true;
            // Udpate SH Lighting
			UdpateSHLightingforBillboards();
            Shader.EnableKeyword("AFS_SH_AMBIENT");
            Shader.DisableKeyword("AFS_COLOR_AMBIENT");
            Shader.DisableKeyword("AFS_GRADIENT_AMBIENT");
        }
        // Trilight
        else if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Trilight) {
        	AmbientLightingSH = false;
        	if(isLinear){
	        	Shader.SetGlobalColor("_AfsSkyColor", RenderSettings.ambientSkyColor.linear); 
				Shader.SetGlobalColor("_AfsGroundColor", RenderSettings.ambientGroundColor.linear); 
				Shader.SetGlobalColor("_AfsEquatorColor", RenderSettings.ambientEquatorColor.linear);
			}
			else {
				Shader.SetGlobalColor("_AfsSkyColor", RenderSettings.ambientSkyColor); 
				Shader.SetGlobalColor("_AfsGroundColor", RenderSettings.ambientGroundColor); 
				Shader.SetGlobalColor("_AfsEquatorColor", RenderSettings.ambientEquatorColor);	
			}
			Shader.DisableKeyword("AFS_SH_AMBIENT");
            Shader.DisableKeyword("AFS_COLOR_AMBIENT");
            Shader.EnableKeyword("AFS_GRADIENT_AMBIENT");
        }
        // Flat
        else if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat) {
            AmbientLightingSH = false;
            Shader.SetGlobalVector("_AfsAmbientColor", RenderSettings.ambientLight); 
            Shader.DisableKeyword("AFS_SH_AMBIENT");
            Shader.EnableKeyword("AFS_COLOR_AMBIENT");
            Shader.DisableKeyword("AFS_GRADIENT_AMBIENT");
        }
	}

	private void UdpateSHLightingforBillboards() {
		// http://www.ppsloan.org/publications/StupidSH36.pdf

		SphericalHarmonicsL2 probe = RenderSettings.ambientProbe;

		fLight[0].x = probe[0,0];
		fLight[0].y = probe[1,0];
		fLight[0].z = probe[2,0];

		fLight[1].x = probe[0,1];
		fLight[1].y = probe[1,1];
		fLight[1].z = probe[2,1];

		fLight[2].x = probe[0,2];
		fLight[2].y = probe[1,2];
		fLight[2].z = probe[2,2];

		fLight[3].x = probe[0,3];
		fLight[3].y = probe[1,3];
		fLight[3].z = probe[2,3];

		fLight[4].x = probe[0,4];
		fLight[4].y = probe[1,4];
		fLight[4].z = probe[2,4];

		fLight[5].x = probe[0,5];
		fLight[5].y = probe[1,5];
		fLight[5].z = probe[2,5];

		fLight[6].x = probe[0,6];
		fLight[6].y = probe[1,6];
		fLight[6].z = probe[2,6];

		fLight[7].x = probe[0,7];
		fLight[7].y = probe[1,7];
		fLight[7].z = probe[2,7];

		fLight[8].x = probe[0,8];
		fLight[8].y = probe[1,8];
		fLight[8].z = probe[2,8];

		float s_fSqrtPI = 3.54490770181F;
		float fC0 = 1.0f/(2.0f*s_fSqrtPI);
		float fC1 = (float)Mathf.Sqrt(3.0f)/(3.0f*s_fSqrtPI);
		float fC2 = (float)Mathf.Sqrt(15.0f)/(8.0f*s_fSqrtPI);
		float fC3 = (float)Mathf.Sqrt(5.0f)/(16.0f*s_fSqrtPI);
		float fC4 = 0.5f*fC2;

		float ambientIntensity = RenderSettings.ambientIntensity;
		if (isLinear) {
			ambientIntensity = Mathf.Pow(ambientIntensity * (1.0f / ambientIntensity), 1.0f/2.2f);
		}
		else {
			ambientIntensity = ambientIntensity * (1.0f / ambientIntensity);
		}

		int iC;
		for( iC=0; iC<3; iC++ )
		{
			vCoeff[iC].x = -fC1 * fLight[3][iC];
			vCoeff[iC].y = -fC1 * fLight[1][iC];
			vCoeff[iC].z = fC1 * fLight[2][iC];
			vCoeff[iC].w = fC0 * fLight[0][iC] - fC3*fLight[6][iC];
		}
		Shader.SetGlobalVector("afs_SHAr", vCoeff[0]*ambientIntensity);
		Shader.SetGlobalVector("afs_SHAg", vCoeff[1]*ambientIntensity);
		Shader.SetGlobalVector("afs_SHAb", vCoeff[2]*ambientIntensity);
		for( iC=0; iC<3; iC++ )
		{
		    vCoeff[iC].x =      fC2*fLight[4][iC];
		    vCoeff[iC].y =     -fC2*fLight[5][iC];
		    vCoeff[iC].z = 3.0f*fC3*fLight[6][iC];
		    vCoeff[iC].w =     -fC2*fLight[7][iC];
		}
		Shader.SetGlobalVector("afs_SHBr", vCoeff[0]*ambientIntensity);
		Shader.SetGlobalVector("afs_SHBg", vCoeff[1]*ambientIntensity);
		Shader.SetGlobalVector("afs_SHBb", vCoeff[2]*ambientIntensity);

		vCoeff[0].x = fC4*fLight[8][0];
		vCoeff[0].y = fC4*fLight[8][1];
		vCoeff[0].z = fC4*fLight[8][2];
		vCoeff[0].w = 1.0f;
		Shader.SetGlobalVector("afs_SHC", vCoeff[0]*ambientIntensity);
	}

	private Color Desaturate(float r, float g, float b) {
		float grey = 0.3f * r + 0.59f * g + 0.11f * b;
		r = grey * BillboardAmbientLightDesaturationFactor + r * (1.0f - BillboardAmbientLightDesaturationFactor);
		g = grey * BillboardAmbientLightDesaturationFactor + g * (1.0f - BillboardAmbientLightDesaturationFactor);
		b = grey * BillboardAmbientLightDesaturationFactor + b * (1.0f - BillboardAmbientLightDesaturationFactor);
		return (new Color(r, g, b, 1.0f));
	}
	float CubicSmooth( float x ) {   
		return x * x *( 3.0f - 2.0f * x );
	}
	float TriangleWave( float x ) {   
		//	return abs( frac( x + 0.5f ) * 2.0f - 1.0f ); 
		return Mathf.Abs( ( x + 0.5f ) % 1.0f * 2.0f - 1.0f );
	}
	float SmoothTriangleWave( float x ) {   
		return CubicSmooth( TriangleWave( x ) );
	}

	Vector4 CubicSmooth( Vector4 x ) {   
		//	return x * x *( 3.0 - 2.0 * x );
		x = Vector4.Scale(x,x);
		x = Vector4.Scale(x, new Vector4 (3.0f,3.0f,3.0f,3.0f) - 2.0f * x);
		return x ;
	}
	Vector4 TriangleWave( Vector4 x ) {   
		//	return abs( frac( x + 0.5f ) * 2.0f - 1.0f );
			//x = FracVecFour(x + new Vector4(0.5f,0.5f,0.5f,0.5f) ) * 2.0f - new Vector4(1.0f,1.0f,1.0f,1.0f);
			// no frac as the input uses sin
			x = (x + new Vector4(0.5f,0.5f,0.5f,0.5f)) * 2.0f - new Vector4(1.0f,1.0f,1.0f,1.0f);
		return AbsVecFour(x);
	}
	Vector4 SmoothTriangleWave( Vector4 x ) {   
		return CubicSmooth( TriangleWave( x ) );
	}

	Vector4 FracVecFour (Vector4 a) {
		a.x = a.x % 1.0f; 
		a.y = a.y % 1.0f; 
		a.z = a.z % 1.0f; 
		a.w = a.w % 1.0f;
		return a;
	}
	Vector4 AbsVecFour (Vector4 a) {
		a.x = Mathf.Abs(a.x); 
		a.y = Mathf.Abs(a.y);
		a.z = Mathf.Abs(a.z);
		a.w = Mathf.Abs(a.w); 
		return a;
	}
}