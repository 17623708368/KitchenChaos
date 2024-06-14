using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
     [SerializeField]private StoveCounter stoveCounter;
     private bool show;
     private void Start()
     {
         stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;
         stoveCounter.OnStateChanged += stoveCounter_OnStateChanged;
         Hide();
     }

     private void stoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventAgr e)
     {
         Hide();
     }

     private void stoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedArgs e)
     {
         float burnShowProgressAount = 0.5f;
            show = stoveCounter.IsFried()&&e.progressNomel >= burnShowProgressAount;
         if (show)
         {
             Show();
         }
        
             
     }
 
     private void Show()
     {
         gameObject.SetActive(true);
     }

     private void Hide()
     {
         gameObject.SetActive(false);

     }
     
}
