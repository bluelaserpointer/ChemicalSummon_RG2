using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SBA_Slide : SBA_TracePosition
{
    [SerializeField]
    Transform initialPosition, outPosition;
    //data
    bool slidedOut;
    public bool SlidedOut => slidedOut;
    public void SlideOut()
    {
        slidedOut = true;
        SetTarget(outPosition);
        StartAnimation();
    }
    public void SlideBack()
    {
        slidedOut = false;
        SetTarget(initialPosition);
        StartAnimation();
    }
    public void Switch() {
        if (slidedOut)
            SlideBack();
        else
            SlideOut();
    }
}
