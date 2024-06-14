using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    enum Mode
    {
     LookAt,
     LookAtInverted,
     CameraForward,
     CameraForwardInverted
    }

   [SerializeField] private Mode mode;
    private void LateUpdate()
    {
            switch (mode)
            {
               case  Mode.LookAt:
                   this.transform.LookAt(Camera.main.transform);
                   break;
               case  Mode.LookAtInverted:
                   Vector3 dir = transform.forward - Camera.main.transform.forward;
                   transform.LookAt(transform.position+dir);
                   break;
               case  Mode.CameraForward:
                   this.transform.forward = Camera.main.transform.forward;
                   break;
               case  Mode.CameraForwardInverted :
                   this.transform.forward =- Camera.main.transform.forward;
break;
        }
    }
}
