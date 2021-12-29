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
    public GameObject pencil;
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

    public void InitPencil()
    {
        pencil.GetComponent<Pencil>().pencil = gameObject;
    }

    public void ChangeTool(GameObject tool)
    {

        var oldTool = pencil.transform.GetChild(1).gameObject;
        Destroy(oldTool, .1f);
        var newTool = Instantiate(tool);
        newTool.transform.parent = pencil.transform;
        newTool.transform.localPosition =  new Vector3(0.05259991f, 0, 0.22f);
        newTool.transform.localScale = new Vector3(1, 1, 1);        
    }
}
