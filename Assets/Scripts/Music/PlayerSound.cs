using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerObject playerObject;
    private float footstepTimer;
    private float footstepTimerMax=.1f;
    private void Awake()
    {
        playerObject = GetComponent<PlayerObject>();
        
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer<=0)
        {
            
            footstepTimer = footstepTimerMax;

            if (playerObject.IsWalking())
            {
                float volume=1;
                SoundManager.Instance.PlayFootstepSound(playerObject.transform.position,volume );
            }
            
        }
    }
}
