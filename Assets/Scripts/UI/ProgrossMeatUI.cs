using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgrossMeatUI : MonoBehaviour
{
     [SerializeField] StoveCounter stoveCounter;
    [SerializeField] private Image barImage;
    private Animator animator;
    private void Start()
    {
        stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;
        barImage.fillAmount = 0;
        animator = GetComponent<Animator>();
        Hide();
    }

 
    void  stoveCounter_OnProgressChanged(object sender,StoveCounter.OnProgressChangedArgs e)
    {
        
        barImage.fillAmount = e.progressNomel;
        if (barImage.fillAmount==0||barImage.fillAmount==1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
         gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
