using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayerUI : MonoBehaviour
{
    
    private void Start()
    {
        GameMgr.Instance.OnLocalPlayerReadyChanged += GameMgr_OnLocalPlayerReadyChanged;
        GameMgr.Instance.OnStateChenged += GameMgr_OnStateChenged;
        GameMgr.Instance.OnCloseReadyUI += GameMgr_OnCloseReadyUI;
        Hide();
    }

    private void GameMgr_OnStateChenged(object sender, EventArgs e)
    {
        if (GameMgr.Instance.IsCountdowToStart())
        {
            Hide();
        }
    }

    private void GameMgr_OnCloseReadyUI()
    {
        Hide();
    }

    private void GameMgr_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (GameMgr.Instance.IsLocalPlayerReadey())
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