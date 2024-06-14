using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI txtRecipesDelivered;
   [SerializeField] private Button btnBackScene;

   
   private void Start()
   {
      GameMgr.Instance.OnStateChenged += GameMgr_OnStateChenged;
      btnBackScene.onClick.AddListener(() =>
      {
         NetworkManager.Singleton.Shutdown();
         Loader.Load(Loader.Scene.BeginScene);
      });
      Hide();
   }

   private void GameMgr_OnStateChenged(object sender, EventArgs e)
   {
      if (GameMgr.Instance.IsGameOver())
      {
         Show();
         txtRecipesDelivered.text = DeliveryManager.Instance.GetRecipeCount().ToString();

      }
      else
      {
         Hide();
      }
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }

   private void Show()
   {
      gameObject.SetActive(true);
   }

   private void OnDestroy()
   {
      GameMgr.Instance.OnStateChenged -= GameMgr_OnStateChenged;

   }
}