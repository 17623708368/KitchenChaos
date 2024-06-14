using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(PlayerObject player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                KitchenObject. DestroyKitchenObject( player.GetKitchenObject());
                
            }
            else
            {
                return;
            }
        }
    }
}
