using System;
using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;


public class ClearCounter : BaseCounter 
{
    
    //食材信息
    [SerializeField]private KitchenObjectSo kitchenObjSo;
  
/// <summary>
/// 交互
/// </summary>
    public override void Interact(PlayerObject player)
    {
        //如果柜子上没有食材
        if (!HasKitchenObject() )
        {
            //玩家身上有食材
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);

            }
            //玩家身上没有
            else
            {
                
            }
        }
        //柜子上有
        else
        {//玩家身上没有,就把柜子上的物体拿给玩家
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            //玩家身上有
            else
            {
                if (player.GetKitchenObject() .TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (  plateKitchenObject.TryAddIngredient( GetKitchenObject().GetKitchenObjectSo()))
                    {
                        KitchenObject.DestroyKitchenObject(   GetKitchenObject());
                 
                    }
                }
                else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if ( plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                    {
                        KitchenObject. DestroyKitchenObject( player.GetKitchenObject());
          
                    }
                }
            }
        }
       
    }

}