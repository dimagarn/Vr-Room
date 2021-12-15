using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Pencil : MonoBehaviour
{
    public UnityEvent SpawnPen;
    public UnityEvent DestroyPen;
    private bool isUIreleased = false;
    private bool isNotpressed = true;
    private void HandAttachedUpdate(Hand hand)
    {
        GrabTypes bestGrab = hand.GetBestGrabbingType(GrabTypes.Grip, true);
        
        if (bestGrab != GrabTypes.None && isNotpressed)
        {
            if (isUIreleased)
                DestroyPen.Invoke();
            else
                SpawnPen.Invoke();
            isUIreleased = !isUIreleased;
            isNotpressed = false;
        }
        else if(bestGrab == GrabTypes.None)
        {
            isNotpressed = true;
        }
    }
}
