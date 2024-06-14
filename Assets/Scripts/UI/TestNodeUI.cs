using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNodeUI : MonoBehaviour
{
  [SerializeField]  private Button btnHost;
  [SerializeField]  private Button btnClien;

  private void Start()
  {
     btnHost.onClick.AddListener(() =>
     {
         Debug.Log("Host");
         NetworkManager.Singleton.StartHost();
         Hide();
     }); 
     btnClien.onClick.AddListener(() =>
     {
         Debug.Log("Client");
         NetworkManager.Singleton.StartClient();
    Hide();
     });
     
  }

  void Hide()
  {
      gameObject.SetActive(false);
  }
}