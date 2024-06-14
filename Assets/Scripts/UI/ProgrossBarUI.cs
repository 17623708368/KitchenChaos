using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgrossBarUI : MonoBehaviour
{
     [SerializeField] CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;
 
    private void Start()
    {
        cuttingCounter.OnProgressChanged += CuttingCouter_OnProgressChanged;
        barImage.fillAmount = 0;
        Hide();
    }

    private void Update()
    {
        
    }

    void CuttingCouter_OnProgressChanged(object sender,CuttingCounter.OnProgressChangedArgs e)
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

    private void OnDestroy()
    {
        cuttingCounter.OnProgressChanged -= CuttingCouter_OnProgressChanged;

    }
}
