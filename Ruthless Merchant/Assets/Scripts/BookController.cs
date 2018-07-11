using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    private enum BookState
    {
        InventoryPages,
        QuestPages,
        SettingPages
    }

    public GameObject center;
    private List<GameObject> Pages;
    private GameObject currentPage;
    private GameObject pageParentObject;
    private BookState currentChapter;
    private int PageIndex;

	// Use this for initialization
	void Start ()
    {
        currentChapter = BookState.InventoryPages;
        pageParentObject = transform.GetChild(3).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void TurnPage()
    {
        if (Input.GetKey(KeyCode.Q) && currentPage.transform.localRotation.eulerAngles.z < 180)
        {
            Debug.Log(currentPage.transform.rotation.eulerAngles.z);
            transform.RotateAround(center.transform.position, new Vector3(0, 0, 20), 450 * Time.deltaTime);
            transform.position += new Vector3(0, 0.00055f, 0);
        }

        if (Input.GetKey(KeyCode.E) && transform.localRotation.eulerAngles.z < 180)
        {
            Debug.Log(transform.rotation.eulerAngles.z);
            transform.RotateAround(center.transform.position, new Vector3(0, 0, -20), 70 * Time.deltaTime);
            transform.position += new Vector3(0, -0.00055f, 0);
        }
    }
}
