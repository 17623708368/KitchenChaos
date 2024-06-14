using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
   [SerializeField] private KitchenObjectSo plateKitchenObjectSo;
   public EventHandler OnPlateSpwns;
   public EventHandler RemovePlateCount;

   private float spwnPlateTimer;
   private float spwnPlateTimerMax=4f;
   private float platesSpwneAmount;
   private float platesSpwneAmountMax=4f;
   
    private void Update()
    {
        if (!IsServer)
            return;
        spwnPlateTimer += Time.deltaTime;
        if (GameMgr.Instance.IsGamePlaying()&&spwnPlateTimer>spwnPlateTimerMax)
        {
            spwnPlateTimer = 0;
            if (platesSpwneAmount<platesSpwneAmountMax)
            {
                SpwnePlateServerRpc();
            
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpwnePlateServerRpc()
    {
        SpwnePlateClientRpc();

    }

    [ClientRpc]
    private void SpwnePlateClientRpc()
    {
        platesSpwneAmount++;
        OnPlateSpwns?.Invoke(this,EventArgs.Empty);
    }
    public override void Interact(PlayerObject player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpwneAmount>0)
            {
                InteractLogicServerRpc();
                KitchenObject.SpwanKitchenObject(plateKitchenObjectSo,player);
            }   
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
      
        platesSpwneAmount--;
        RemovePlateCount?.Invoke(this,EventArgs.Empty);

    }
}
