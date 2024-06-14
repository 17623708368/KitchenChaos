using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingConterVisual : MonoBehaviour
{
    private Animator animator;
    private string CUT="Cut";
    [SerializeField] private CuttingCounter cuttingCounter;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        cuttingCounter.Oncut += CuttingCounter_OnCut;

    }

    private void CuttingCounter_OnCut(object obj, EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
    
 
}
