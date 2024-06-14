using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class BaseCounter : NetworkBehaviour,IkitchenObjectParent
{   //得到食材
   private  KitchenObject kitchenObj;
   public static EventHandler OnAnyObjectPlacedHere;
   public static void ResetCallback()
   {
      OnAnyObjectPlacedHere = null;
   }
   public  abstract void Interact(PlayerObject player);

   public virtual void  InteractAltemate(PlayerObject playerObject)
   {
   }
   //食材的放置位置
   [SerializeField] private Transform kitchenPoint;
 

   public Transform GetKitchenObjectFollowTransform()
   {
      return kitchenPoint;
   }
   public void SetKitchenObject(KitchenObject kitchenObject)
   {
      this.kitchenObj = kitchenObject;
      if (kitchenObj!=null)
      {
         OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty);
      }
   }

   public KitchenObject GetKitchenObject()
   {
      return kitchenObj;

   }
   public void ClearKitchenObject()
   {
      kitchenObj = null;
   }

   public bool HasKitchenObject()
   {
      return kitchenObj != null;
   }
   public NetworkObject GetNetworkObject()
   {
      return  NetworkObject;
   }
}
