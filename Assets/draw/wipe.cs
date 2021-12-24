using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class wipe : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "wall")
         Destroy(other.GetComponent(name));
   }
}
