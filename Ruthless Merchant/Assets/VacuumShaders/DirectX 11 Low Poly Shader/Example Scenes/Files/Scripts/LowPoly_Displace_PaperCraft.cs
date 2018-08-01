

//This is an exact copy of 'LowPoly_Displace' script but with time modification


using UnityEngine;
using System.Collections;


namespace VacuumShaders
{
    namespace DirectX11LowPolyShader
    {
        public class LowPoly_Displace_PaperCraft : MonoBehaviour
        {
            //Variables //////////////////////////////////////////////////////////////////
            [Range(0f, 360f)]
            public float direction = 0;
            public float speed = 1;
            public float amplitude = 1f;
            public float frequency = 0.5f;
            public float noise = 1.0f;

            public Material targetMaterial;

            Vector3 originalPosition;

            public float stepLength = 0.5f;
            float deltaTime = 0;
            float deltaStep = 0;


            //Functions ////////////////////////////////////////////////////////////
            void Start()
            {
                originalPosition = transform.position;
            }

            void Update()
            {
                deltaTime += Time.deltaTime;
                deltaStep += Time.deltaTime;

                if (deltaStep > stepLength)
                {

                    #region Shader time sync
                    if (targetMaterial != null)
                    {
                        //Synchronize shader and system time
                        targetMaterial.SetFloat("_V_LP_Time", deltaTime);

                        targetMaterial.SetFloat("_DisplaceDirection", direction);
                        targetMaterial.SetFloat("_DisplaceSpeed", speed);
                        targetMaterial.SetFloat("_DisplaceAmplitude", amplitude);
                        targetMaterial.SetFloat("_DisplaceFrequency", frequency);
                        targetMaterial.SetFloat("_DisplaceNoiseCoef", noise);


                        //Animate Water shader surface
                        Vector4 ST = new Vector4(1, 1, Random.value, 0);
                        targetMaterial.SetVector("_PixelTex_ST", ST);
                        targetMaterial.SetVector("_BumpTex_ST", ST);
                    }
                    #endregion



                    #region Update transform position
                    //Current world position
                    Vector3 worldPos = originalPosition;

                    //Direction
                    Vector2 dir = Rotate(new Vector2(worldPos.x, worldPos.z), direction - 45);

                    //Noise
                    float n = Noise(new Vector3(worldPos.x, worldPos.z, worldPos.z)) * noise;

                    //Calculate diplace
                    float displace = (amplitude * (n + 1)) * Mathf.Sin(Vector2.Dot(dir, Vector2.one * frequency) + deltaTime * speed + n);

                    //Update position
                    Vector3 newPos = worldPos;
                    newPos.y += displace;

                    transform.position = Vector3.Lerp(originalPosition, newPos, Mathf.SmoothStep(0f, 1f, deltaTime));
                    #endregion


                    //Reset deltaStep 
                    deltaStep = 0;
                }
            }

            float Noise(Vector3 e)
            {
                float r = Mathf.Sin(Vector3.Dot(e, new Vector3(12.9898f, 78.233f, 63.9137f)));

                return r - Mathf.Floor(r);
            }

            public Vector2 Rotate(Vector2 v, float degrees)
            {
                float radians = degrees * Mathf.Deg2Rad;
                float sin = Mathf.Sin(radians);
                float cos = Mathf.Cos(radians);

                float tx = v.x;
                float ty = v.y;

                return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
            }
        }
    }
}