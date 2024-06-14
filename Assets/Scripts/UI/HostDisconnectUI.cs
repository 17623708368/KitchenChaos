using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button btnBack;

    private void Start()
    {
        btnBack.onClick.AddListener(()=>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.BeginScene);
        });
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId==NetworkManager.ServerClientId)
        {
            Show();
        }
        
    }

 
    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
