using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VR_HintGlowing : MonoBehaviour
{
    [SerializeField]
    bool isText;

    Renderer renderer;
    TextMeshPro textMesh;
    int fading = -1;
    float normalAlpha;

	void Awake ()
    {
        renderer = GetComponent<Renderer>();

        if (!isText)
            normalAlpha = renderer.material.color.a;
        else
        {
            textMesh = GetComponent<TextMeshPro>();
            normalAlpha = textMesh.color.a;
        }
    }
	
	void Update ()
    {
        if (!isText)
        {
            foreach (Material material in renderer.materials)
            {
                material.color += new Color(0, 0, 0, Time.deltaTime * fading * 0.075f);
            }

            if (renderer.material.color.a >= normalAlpha)
                fading = -1;

            if (renderer.material.color.a <= 0)
                fading = 1;
        }
        else
        {
            textMesh.color += new Color(0, 0, 0, Time.deltaTime * fading * 0.4f);

            if (textMesh.color.a >= normalAlpha)
                fading = -1;

            if (textMesh.color.a <= 0)
                fading = 1;
        }
    }
}
