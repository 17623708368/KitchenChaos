using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnplayerGrabbedObject;
    }

    private void ContainerCounter_OnplayerGrabbedObject(object sender, EventArgs e)
    {
         animator.SetTrigger(OPEN_CLOSE);
    }
}
