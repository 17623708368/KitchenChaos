using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
   [SerializeField] private GameObject stoveOnGameObject;
   [SerializeField] private GameObject particlesGameObject;
   [SerializeField] private StoveCounter stoveCounter;

   private void Start()
   {
       stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
   }

   public void StoveCounter_OnStateChanged(object obj, StoveCounter.OnStateChangedEventAgr e)
   {
       bool showVisual = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fride);
       stoveOnGameObject.SetActive(showVisual);
       particlesGameObject.SetActive(showVisual);
   }
}
