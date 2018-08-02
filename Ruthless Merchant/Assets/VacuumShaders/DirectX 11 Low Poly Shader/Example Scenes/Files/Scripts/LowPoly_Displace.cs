using UnityEngine;
using System.Collections;


namespace VacuumShaders
{
    namespace DirectX11LowPolyShader
    {
        public class LowPoly_Displace : MonoBehaviour
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
            //Unity Functions ////////////////////////////////////////////////////////////
            void Start()
            {
                originalPosition = transform.position;
            }

            void Update()
            {

                #region Shader time sync
                if (targetMaterial != null)
                {
                    //Synchronize shader and system time
                    targetMaterial.SetFloat("_V_LP_Time", Time.time);

                    targetMaterial.SetFloat("_DisplaceDirection", direction);
                    targetMaterial.SetFloat("_DisplaceSpeed", speed);
                    targetMaterial.SetFloat("_DisplaceAmplitude", amplitude);
                    targetMaterial.SetFloat("_DisplaceFrequency", frequency);
                    targetMaterial.SetFloat("_DisplaceNoiseCoef", noise);
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
                float displace = (amplitude * (n + 1)) * Mathf.Sin(Vector2.Dot(dir, Vector2.one * frequency) + Time.time * speed + n);

                //Update position
                Vector3 newPos = worldPos;
                newPos.y += displace;

                transform.position = Vector3.Lerp(originalPosition, newPos, Mathf.SmoothStep(0f, 1f, Time.time));
                #endregion
            }

            //Custom Functions ///////////////////////////////////////////////////////////
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