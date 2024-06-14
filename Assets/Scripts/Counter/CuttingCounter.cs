using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter :  BaseCounter
{

   [SerializeField] private CuttingCounterObjectSO[] cuttingRecipeSOArray;
   private int cuttingProgress;
   public static event EventHandler OnCuttingPlaySound;
   public EventHandler Oncut;
   public event EventHandler<OnProgressChangedArgs> OnProgressChanged;

   public static void ResetCallback()
   {
       OnCuttingPlaySound = null;
   }
    public class OnProgressChangedArgs:EventArgs
   {
       public float progressNomel;
   }
    public override void Interact(PlayerObject player)
    {//当前橱柜没有厨房用品时
        if (!HasKitchenObject() )
        {//玩家身上有食品时
            if (player.HasKitchenObject())
            {//玩家身上的食材可以进行切片才能放上去
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()) )
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlatceObjectOnConterServerRpc();
                }

            }
            else
            {
                
            }
        }
        else
        {//玩家身上没有但是橱柜上有就获取到食材设置他的父物体
            if (!player.HasKitchenObject())
            {
                if (cuttingProgress==0)
              GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject() .TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (  plateKitchenObject.TryAddIngredient( GetKitchenObject().GetKitchenObjectSo()))
                    {
                        KitchenObject.DestroyKitchenObject(  GetKitchenObject());
                    }
                }
                else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if ( plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                    {
                        KitchenObject.DestroyKitchenObject( player.GetKitchenObject());
                       
                    }
                }
            }
        }
    }

 
/// <summary>
/// 交互
/// </summary>
/// <param name="playerObject"></param>
    public override void InteractAltemate(PlayerObject playerObject)
    {//如果橱柜上有对象就进行切菜
        if (HasKitchenObject()&&HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            CutObjectServerRpc();
            TestCuttingProgressDoneServerRpc();
        }
    }
/// <summary>
/// 初始化时同步更新进度
/// </summary>
[ServerRpc(RequireOwnership = false)]
private void InteractLogicPlatceObjectOnConterServerRpc()
{
    InteractLogicPlatceObjectOnConterClientRpc();
}
[ClientRpc]
private void InteractLogicPlatceObjectOnConterClientRpc()
{
     cuttingProgress = 0;
    OnProgressChanged?.Invoke(this,new OnProgressChangedArgs()
    {
        progressNomel = 0f
    });
}

[ServerRpc(RequireOwnership = false)]
private void CutObjectServerRpc()
{
    CutObjectClientRpc();
}
[ClientRpc]
private void CutObjectClientRpc()
{
    cuttingProgress++;  
    CuttingCounterObjectSO   outputKitchenObjectSo = GetCuttingCounterObjectSO(GetKitchenObject().GetKitchenObjectSo());
    //调用进度条更新事件
           
    OnProgressChanged?.Invoke(this,new OnProgressChangedArgs()
    {
        progressNomel = (float)cuttingProgress/outputKitchenObjectSo.maxCount
    });
    Oncut?.Invoke(this,EventArgs.Empty);
    OnCuttingPlaySound?.Invoke(this,EventArgs.Empty);
    //当切到一定段数进行切换
    
}
[ServerRpc(RequireOwnership = false)]
private void TestCuttingProgressDoneServerRpc()
{
    CuttingCounterObjectSO   outputKitchenObjectSo = GetCuttingCounterObjectSO(GetKitchenObject().GetKitchenObjectSo());
    if (outputKitchenObjectSo.maxCount<=cuttingProgress)
    {
        TestCuttingProgressDoneClientRpc();
        KitchenObjectSo   outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
        KitchenObject.DestroyKitchenObject( GetKitchenObject());
        KitchenObject.SpwanKitchenObject(outputKitchenObject, this);
    }
}
[ClientRpc]
private void TestCuttingProgressDoneClientRpc()
{
    cuttingProgress = 0;
}
/// <summary>
/// 是否携带这配方
/// </summary>
/// <param name="kitchenObjectSo"></param>
/// <returns></returns>
    private bool HasRecipeWithInput(KitchenObjectSo kitchenObjectSo)
    {
        foreach (CuttingCounterObjectSO slices in cuttingRecipeSOArray)
        {
            if (slices.input==kitchenObjectSo)
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 获取切片后的素材
    /// </summary>
    /// <param name="inputkiKitchenObjectSo"></param>
    /// <returns></returns>
    public KitchenObjectSo GetOutputForInput(KitchenObjectSo inputkiKitchenObjectSo)
    {//获取到菜刀橱柜当前橱柜上的食材，切完片之后的物体
        foreach (CuttingCounterObjectSO slices in cuttingRecipeSOArray)
        {
            if (slices.input==inputkiKitchenObjectSo)
            {
                return slices.outInput ;
            }
        }
        return null;
    }  
    public CuttingCounterObjectSO GetCuttingCounterObjectSO(KitchenObjectSo inputkiKitchenObjectSo)
    {//获取到菜刀橱柜当前橱柜上的食材，切完片之后的物体
        foreach (CuttingCounterObjectSO slices in cuttingRecipeSOArray)
        {
            if (slices.input==inputkiKitchenObjectSo)
            {
                return slices ;
            }
        }
        return null;
    }
    public int GetCount()
    {
        return cuttingProgress;
    }
}
