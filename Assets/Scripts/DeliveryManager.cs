
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{//菜单列表
   [SerializeField] private RecipeListSO recipeListSo;
   //添加待出餐
   private List<RecipeSO> waitingRecipeSoList;
    private static DeliveryManager instance;
    public static DeliveryManager Instance=>instance;
    //点单间隔时间
    private float spawnRecipeTime=4;
    private float spawnRecipeTimeMax=4;
    //同时最大只能有4个
    private float waitRecipeMax=4;
    
    //当前上菜个数
    private float recipeCount = 0;
    //生成时调用的事件
    public event EventHandler OnRecipeSpawned; 
    //出餐时调用移除事件
    public event EventHandler OnRecipeComplted; 
    public event EventHandler OnRecipeSuccess; 
    public event EventHandler OnRecipeFailed; 
    
    private void Awake()
    {
        instance = this;
        //初始化待餐列表
        waitingRecipeSoList = new List<RecipeSO>();
    }

    public List<RecipeSO> GetWaitingRecipeSoList()
    {
        return waitingRecipeSoList;
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
         spawnRecipeTime -= Time.deltaTime;
        if (spawnRecipeTime<=0)
        { spawnRecipeTime = spawnRecipeTimeMax;
            if (GameMgr.Instance.IsGamePlaying()&&waitingRecipeSoList.Count<waitRecipeMax)
            {    //得到单个菜的种类
                int recipeSoIndex = Random.Range(0, recipeListSo.recipteSoList.Count);
               
                //添加待出餐列表
               SpawnNewWaitingRecipeClientRpc(recipeSoIndex);
            }
          
        }
    }
      [ClientRpc]
    public void SpawnNewWaitingRecipeClientRpc(int  recipeSoIndex)
    {
        RecipeSO recipeSo= recipeListSo.recipteSoList[recipeSoIndex]; 

        waitingRecipeSoList.Add(recipeSo);
        OnRecipeSpawned?.Invoke(this,EventArgs.Empty);

    }
    
/// <summary>
/// 出餐
/// </summary>
/// <param name="plateKitchenObject">得到盘子里的菜</param>
    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        //遍历菜单得到出的是哪一个菜
        for (int i = 0; i < waitingRecipeSoList.Count; i++)
        {
            RecipeSO recipeSO = waitingRecipeSoList[i];
            //如果盘子里的菜和菜单里的菜数量相同说明有概率是同一个菜
            if (plateKitchenObject.GetkitchenObjectSoList().Count==recipeSO.KitchenObjectSOList.Count)
            {
                bool plateContentsMatchesRecipe = true;
                //双重遍历比对菜
                foreach (KitchenObjectSo recipeKitchenObjectSo in recipeSO.KitchenObjectSOList)
                {
                    bool ingreddientFound = false;
                    foreach (KitchenObjectSo pKitchenObjectSo in plateKitchenObject.GetkitchenObjectSoList())
                    {
                        if (recipeKitchenObjectSo==pKitchenObjectSo)
                        {
                            ingreddientFound = true;
                            break;
                         }
                    }

                    if (!ingreddientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                    
                }
            //如果是true说明是对的
                if (plateContentsMatchesRecipe)
                {
                    DeliverCorrectRecipeServerRpc(i);
                    return; 

                }
                
            }
        }

        DeliverIncorrectRecipServerRpc();
    }
[ServerRpc(RequireOwnership = false)]
private void DeliverIncorrectRecipServerRpc()
{
    DeliverIncorrectRecipClientRpc();
}
[ClientRpc]
private void DeliverIncorrectRecipClientRpc()
{
    OnRecipeFailed?.Invoke(this,EventArgs.Empty);

}
[ServerRpc(RequireOwnership = false)]
private void DeliverCorrectRecipeServerRpc(int i)
{
    DeliverCorrectRecipeClientRpc(i);
}

[ClientRpc]
private void DeliverCorrectRecipeClientRpc(int i)
{
    recipeCount++;
    waitingRecipeSoList.RemoveAt(i);
    OnRecipeComplted?.Invoke(this,EventArgs.Empty);
    OnRecipeSuccess?.Invoke(this,EventArgs.Empty);
}

 
public float GetRecipeCount()
{
    return recipeCount;
}

}