using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

   [SerializeField] private Button btnResume;
  [SerializeField]private Button btnMainMenu;
  [SerializeField] private Button btnOption;
    private void Start()
    {
        GameMgr.Instance.OnLocalPauseGameUI += GameMgrOnLocalPauseGameUI;
        GameMgr.Instance.OnUnLocalPauseGameUI += GameMgrOnUnLocalPauseGameUI;
        
        btnResume.onClick.AddListener(()=>
        {
            GameMgr.Instance.PuseGame();
        });
        btnMainMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.BeginScene);
        });
        btnOption.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show(() =>
            {
                Show();
            });
            Hide();
        });
        Hide(); 
    }

    private void GameMgrOnUnLocalPauseGameUI(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameMgrOnLocalPauseGameUI(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        btnResume.Select();
    } private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMgr.Instance.OnLocalPauseGameUI -= GameMgrOnLocalPauseGameUI;
        GameMgr.Instance.OnUnLocalPauseGameUI -= GameMgrOnUnLocalPauseGameUI;
    }
}

