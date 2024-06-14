
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private float nowTimer=0;
    private Animator animator;
    private int previousCountdownNumber;
    private const string ANIMATOR_NUMNAME = "Numbarpop";

    private void Start()
    {
        GameMgr.Instance.OnStateChenged += GameMgr_OnStateChenged;
        animator = GetComponentInChildren<Animator>();
        Hide();
    }

    private void GameMgr_OnStateChenged(object sender, EventArgs e)
    {
        if (GameMgr.Instance.IsCountdowToStart())
        {
            Show();
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

    private void Update()
    {
        UpdateTextTimer(GameMgr.Instance.CountdowToStartTimer());
    }

    public void UpdateTextTimer(float timer)
    {
       // textMeshProUGUI .text=((int) timer).ToString();
       int numCount = ((int)Math.Ceiling(timer));
        textMeshProUGUI .text=numCount .ToString();
        if (numCount!=previousCountdownNumber)
        {
            previousCountdownNumber = numCount;
            SoundManager.Instance.PlayCountdownSound();
            animator.SetTrigger(ANIMATOR_NUMNAME);
        }
    }

    private void OnDestroy()
    {
        GameMgr.Instance.OnStateChenged -= GameMgr_OnStateChenged;

    }
}   
