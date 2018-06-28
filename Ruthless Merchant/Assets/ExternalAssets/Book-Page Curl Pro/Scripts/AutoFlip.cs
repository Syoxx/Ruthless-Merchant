using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BookPro))]
public class AutoFlip : MonoBehaviour
{
    public BookPro ControledBook;
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float DelayBeforeStart;
    public float TimeBetweenPages=5;
    public bool AutoStartFlip=true;
    bool flippingStarted = false;
    bool isPageFlipping = false;
    float elapsedTime = 0;
    float nextPageCountDown = 0;
    // Use this for initialization
    void Start () {
        if (!ControledBook)
            ControledBook = GetComponent<BookPro>();
        ControledBook.interactable = false;
        if (AutoStartFlip)
            StartFlipping();
    }
    public void FlipRightPage()
    {
        if (isPageFlipping) return;
        if (ControledBook.CurrentPaper >= ControledBook.papers.Length) return;
        isPageFlipping = true;
        PageFlipper.FlipPage(ControledBook, PageFlipTime, FlipMode.RightToLeft, ()=> { isPageFlipping = false; });
    }
    public void FlipLeftPage()
    {
        if (isPageFlipping) return;
        if (ControledBook.CurrentPaper <= 0) return;
        isPageFlipping = true;
        PageFlipper.FlipPage(ControledBook, PageFlipTime, FlipMode.LeftToRight, () => { isPageFlipping = false; });
    }
    public void StartFlipping()
    {
        flippingStarted = true;
        elapsedTime = 0;
        nextPageCountDown = 0;
    }
    void Update()
    {
        if (flippingStarted)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > DelayBeforeStart)
            {
                if (nextPageCountDown < 0)
                {
                    if ((ControledBook.CurrentPaper <= ControledBook.EndFlippingPaper &&
                        Mode == FlipMode.RightToLeft) ||
                        (ControledBook.CurrentPaper > ControledBook.StartFlippingPaper &&
                        Mode == FlipMode.LeftToRight))
                    {
                        isPageFlipping = true;
                        PageFlipper.FlipPage(ControledBook, PageFlipTime, Mode, ()=> { isPageFlipping = false; });
                    }
                    else
                    {
                        flippingStarted = false;
                        this.enabled = false;
                    }

                    nextPageCountDown = PageFlipTime + TimeBetweenPages+ Time.deltaTime;
                }
                nextPageCountDown -= Time.deltaTime;
            }
        }
    }
}
