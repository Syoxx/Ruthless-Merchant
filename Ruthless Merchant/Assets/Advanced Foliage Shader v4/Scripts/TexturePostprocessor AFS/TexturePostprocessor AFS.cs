// AFS TEXTURPOSTPROCESSOR 

// textures have to be marked as readable

#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

internal class AFSTexturePostprocessor : AssetPostprocessor {

	public const string SpecSuffix = "_AfsNTR";
	public const string SpecSuffixNew = "_AfsNTS";

	public void OnPostprocessTexture (Texture2D specMap) {
		if (assetPath.Contains(SpecSuffix) || assetPath.Contains(SpecSuffixNew) ) {
			string filename = Path.GetFileNameWithoutExtension(assetPath);
			Debug.Log("Filtering Texture: " + filename);			
			int width = specMap.width;
			int height = specMap.height;
			//if(normal.width != width || normal.height != height)
			//{
			//	normal.Resize(width, height);
			//}
			int mipmapCount = specMap.mipmapCount;
			
			// Start with mip level 1
			for (int mipLevel = 1; mipLevel < mipmapCount; mipLevel++) {
				ProcessMipLevel(ref specMap, width, height, mipLevel);
			}
			specMap.Apply(false, false);
		}	
	}


	private static void ProcessMipLevel(ref Texture2D specMap, int maxwidth, int maxheight, int mipLevel)
	{
		// Create color array which will hold the processed texels for the given MipLevel
		Color32[] colors = specMap.GetPixels32(mipLevel);

		// Get NormalMap MipLevel 0
		Color32[] BumpMap = specMap.GetPixels32(0);

		// Calculate Width and Height for the given mipLevel
		int width = Mathf.Max(1, specMap.width >> mipLevel);
		int height = Mathf.Max(1, specMap.height >> mipLevel);

		int pointer = 0;
		int texelFootprint = 1 << mipLevel;

		// Declare vars outside the loop!
		Vector3 sampleNormal;
		Color32 normalSample;
		float texelPosX;
		float texelPosY;
		int texelPointerX;
		int texelPointerY;
		int samplePosX;
		int samplePosY;

		for (int row = 0; row < height; row++)
			{
			for (int col = 0; col < width; col++)
				{

					texelPosX = (float)col/width;														// equals U
					texelPosY = (float)row/height;													// equals V
					texelPointerX = Mathf.FloorToInt(texelPosX * maxwidth);								// remap to mipLevel 0
					texelPointerY = Mathf.FloorToInt( (texelPosY) * maxheight);							// remap to mipLevel 0
				
				//	Sample all normal map texels from the base mip level that are within the footprint of the current mipmap texel
					Vector3 avgNormal = Vector3.zero;
					for(int y = 0; y < texelFootprint; y++)
						{
						for(int x = 0; x < texelFootprint; x++)
						{
							samplePosX = texelPointerX + x;
							samplePosY = texelPointerY + y;
							// Read Pixel from Combined Texture out of Array
			             	normalSample = BumpMap[ samplePosY * maxheight + samplePosX];
							// Decode Normal
							sampleNormal = new Vector3(normalSample.a/255.0f*2-1, normalSample.g/255.0f*2-1, 0);
							sampleNormal.z = Mathf.Sqrt(1.0f - sampleNormal.x * sampleNormal.x - sampleNormal.y * sampleNormal.y);
							// If normal was not compressed
							// sampleNormal = new Vector3(normalSample.r/255.0f*2-1, normalSample.g/255.0f*2-1, normalSample.b/255.0f*2-1);
							sampleNormal.Normalize();
							avgNormal += sampleNormal;
						}
					}
					avgNormal /= (float)(texelFootprint * texelFootprint);
		    	
		    	//	Get Roughness (byte to float)
		    		float glossiness = colors[pointer].b / 255.0f;
		    		float N_alpha = ((Vector3) avgNormal).magnitude;
					float variance = Mathf.Clamp01( (1.0f - N_alpha) / N_alpha );
					// Convert Roughness to Specular Power (matches Lux Blinn Phong)
					float specPower = Mathf.Pow(2, glossiness * 10 + 1) - 1.75f;
					// Apply Toksvig factor
					specPower = specPower / (1.0f + variance * specPower);
					// Convert Specular Power to Roughness and store new Roughness value (float to byte)
					// colors[pointer].b = (byte)( (Mathf.Log( (specPower + 1.75f), 2.0f) - 1 ) / 10 * 255);

					// Additionally degrade roughness by miplevel
					//colors[pointer].b = (byte)( (Mathf.Log( (specPower + 1.75f), 2.0f) - 1 ) / 10 * 255 * 1.0f/mipLevel * 1.0f/mipLevel );
					colors[pointer].b = (byte)( (Mathf.Log( (specPower + 1.75f), 2.0f) - 1 ) / 10 * 255);
		        	pointer++;
				}
			}
		// Apply modified mipLevel
		specMap.SetPixels32(colors, mipLevel);
		
		// Clean up
		colors = null;
		BumpMap = null;

	}
}

#endif
