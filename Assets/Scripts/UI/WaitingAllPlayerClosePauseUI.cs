using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingAllPlayerClosePauseUI : MonoBehaviour
{
    private void Start()
    {
        GameMgr.Instance.OnWaitingAllPlayer += GameMgr_OnWaitingAllPlayer;
        GameMgr.Instance.OnUnWaitingAllPlayer += GameMgr_OnUnWaitingAllPlayer;
        Hide();
    }

    private void GameMgr_OnUnWaitingAllPlayer()
    {
        Hide();
    }

    private void GameMgr_OnWaitingAllPlayer()
    {
        Show();
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
