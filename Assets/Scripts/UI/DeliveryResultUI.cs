using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField]private Image imgBackGround;
    [SerializeField]private Image imgIconitem;
    [SerializeField]private TextMeshProUGUI txtMessg;
    [SerializeField] private Sprite sprSuccess;
    [SerializeField] private Sprite sprFaile;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    private const string messgSuccessStr = "恭喜你\n上菜成功";
    private const string messgFaileStr = "上菜失败";
    private Animator animator;
    private const string POP = "Pop"; 

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);


    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POP);
        imgBackGround .color= successColor;
        imgIconitem.sprite = sprSuccess;
        txtMessg.text = messgSuccessStr;
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POP);
        imgBackGround .color= failedColor;
        imgIconitem.sprite = sprFaile;
        txtMessg.text = messgFaileStr;
    }
}
