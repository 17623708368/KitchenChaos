using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static EventHandler OnAnyObjectTranshed;

   new  public static void ResetCallback()
    {
        OnAnyObjectTranshed = null;
    }

    public override void Interact(PlayerObject player)
    {
        if (player.HasKitchenObject())
        {
              KitchenObject. DestroyKitchenObject( player.GetKitchenObject());
              InteractLogicServerRpc();
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
        OnAnyObjectTranshed?.Invoke(this,EventArgs.Empty);

    }
}
