using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;
    private string IS_WALKING="IsWalking";
    [SerializeField] private PlayerObject player;
    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    
    private void Update()
    {
        if (!IsOwner)
      return;
        animator.SetBool(IS_WALKING,player.IsWalking()); 
    }
}
