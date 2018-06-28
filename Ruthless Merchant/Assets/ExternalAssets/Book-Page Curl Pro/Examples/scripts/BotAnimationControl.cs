using UnityEngine;
using System.Collections;

public class BotAnimationControl : MonoBehaviour {

    public Animator anim;
	// Use this for initialization
	void Start () {
        if (!anim)
            anim = GetComponent<Animator>();
	}
    public void Close()
    {
        anim.ResetTrigger("Open");
        anim.ResetTrigger("Close");
        anim.SetTrigger("Close");
    }
    public void Open()
    {
        anim.ResetTrigger("Close");
        anim.ResetTrigger("Open");
        anim.SetTrigger("Open");
    }
    // Update is called once per frame
    void Update()
    {

    }
}