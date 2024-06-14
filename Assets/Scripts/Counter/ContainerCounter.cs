using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter 
{
     //食材信息
    [SerializeField]private KitchenObjectSo kitchenObjSo;
    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(PlayerObject player)
    {
        if (player.HasKitchenObject())
        return;
        KitchenObject.SpwanKitchenObject(kitchenObjSo, player);
        InteractLogicServerRpc();
    }
[ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
[ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);

    }

}