using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCallBack : MonoBehaviour
{
    private void Awake()
    {
         BaseCounter.ResetCallback();
         TrashCounter.ResetCallback();
         CuttingCounter.ResetCallback();
         PlayerObject.ResetCallback();
    }
}