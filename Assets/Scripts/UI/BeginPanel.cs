using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : MonoBehaviour
{
    [SerializeField]private Button btnQuitGame;
   [SerializeField] private Button btnStartGame;

   private void Start()
   {
        btnQuitGame.onClick.AddListener(()=>
        {
            Application.Quit();
        });
        btnStartGame.onClick.AddListener(() =>
        {
            PlayClic();
        });
        Time.timeScale = 1;
   }

   private void PlayClic()
   {
       Loader.Load(Loader.Scene.GameScene);
   }
}