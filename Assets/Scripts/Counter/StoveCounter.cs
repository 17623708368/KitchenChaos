using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
   public enum State
    {
        Idle,
        Frying,
        Fride,
        Burned,
    }
     
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSoArray;
  
    //进度条
    private int fryingProgress;
    private  NetworkVariable< State> state=new NetworkVariable<State>(State.Idle);
   //  private float fryingTimer;
   
     public EventHandler< OnProgressChangedArgs> OnProgressChanged;
     //声明网络变量用与通用
     private NetworkVariable<float> fryingTimer = new NetworkVariable<float>();
     private NetworkVariable<float> burningTimer = new NetworkVariable<float>();
     private FryingRecipeSO fryingRecipeSo;
     private BurningRecipeSO burningRecipeSO;
     public NetworkVariable< bool> isValibProgress=new NetworkVariable<bool>(false);
  
     public class OnProgressChangedArgs:EventArgs
     {
         public float progressNomel;
     }

     public EventHandler<OnStateChangedEventAgr> OnStateChanged;
     public class  OnStateChangedEventAgr:EventArgs
     {
         public State state;
     }
   
 
    /// <summary>
    /// 添加网络函数事件监听，当物体生成时第一时间调用
    /// </summary>
    public override void OnNetworkSpawn()
    {  
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged; 
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }


    /// <summary>
    /// 当网络变量发生变化就调用该值
    /// </summary>
    /// <param name="previousvalue"></param>
    /// <param name="newvalue"></param>
    private void FryingTimer_OnValueChanged(float previousvalue, float newvalue)
    {
        float fryingMaxTime=fryingRecipeSo!=null ? fryingRecipeSo.fryingMaxTime : 1;
        OnProgressChanged?.Invoke(this, new OnProgressChangedArgs()
        {
            progressNomel = fryingTimer.Value/ fryingMaxTime
        });
    }
    private void BurningTimer_OnValueChanged(float previousvalue, float newvalue)
    {
        float burningMaxTime=burningRecipeSO!=null ? burningRecipeSO.burningMaxTime : 1;
        OnProgressChanged?.Invoke(this, new OnProgressChangedArgs()
        {
            progressNomel = burningTimer.Value/ burningMaxTime
        });
    }

    /// <summary>
    /// 检测网络状态变量
    /// </summary>
    /// <param name="previousvalue"></param>
    /// <param name="newvalue"></param>
    private void State_OnValueChanged(State previousvalue, State newvalue)
    {    
        OnStateChanged?.Invoke(this,new OnStateChangedEventAgr()
        {
            state =state.Value
            
        });
        if (state.Value==State.Idle||state.Value==State.Burned)
        {
            OnProgressChanged?.Invoke(this,new OnProgressChangedArgs()
            {
                progressNomel = 0f
            });
        }
    }

    private void  Update() 
    {
        if (!IsServer)
            return;
        if (!isValibProgress.Value)
        {
            SetProgressSliderServerRpc();
        }
            switch (state.Value)  {
            case State.Frying:
                fryingTimer.Value += Time.deltaTime;
                if (fryingTimer.Value>this.fryingRecipeSo.fryingMaxTime)
                {
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpwanKitchenObject(fryingRecipeSo.outInput, this);
                    state.Value = State.Fride;
                    isValibProgress.Value = true;
                    burningTimer.Value = 0;
                    SetBuringinRecipeSOClientRpc(KitchenGameMultiPlayer.Instance.GetKitchenSoIndex(GetKitchenObject().GetKitchenObjectSo()));
                }
             break;
            case State.Fride:
                burningTimer.Value  += Time.deltaTime;
                if (burningTimer.Value >this.burningRecipeSO.burningMaxTime)
                {
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpwanKitchenObject(burningRecipeSO.outInput, this);
                    state.Value = State.Burned;
                }
                break;
            case State.Burned:
                //停止播放音效
                break;
        }
    }
/// <summary>
/// 交互
/// </summary>
/// <param name="player"></param>
    public override void Interact(PlayerObject player)
    {
        //当前橱柜没有厨房用品时
        if (!HasKitchenObject() )
        {//玩家身上有食品时
            if (player.HasKitchenObject())
            {//玩家身上的食材可以进行切片才能放上去
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()) )
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc(KitchenGameMultiPlayer.Instance.GetKitchenSoIndex(kitchenObject.GetKitchenObjectSo()));
                }
            }
        }
        else
        {//玩家身上没有但是橱柜上有就获取到食材设置他的父物体
            if (!player.HasKitchenObject())
            {
                SetStateIdleServerRpc();
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                 if (player.GetKitchenObject() .TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if ( plateKitchenObject.TryAddIngredient( GetKitchenObject().GetKitchenObjectSo()))
                    {
                         SetStateIdleServerRpc();
                      KitchenObject.DestroyKitchenObject( GetKitchenObject());
                    
                    }
                   
                }
            }
        }
    }
[ServerRpc(RequireOwnership = false)]
private void SetStateIdleServerRpc()
{
    state.Value = State.Idle;
    isValibProgress.Value = false;
}
 
[ServerRpc(RequireOwnership = false)]
private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectIndex)
{
    fryingTimer.Value = 0;
    this.state.Value = State.Frying;
    isValibProgress.Value = true;
    SetFringRecipeSOClientRpc(kitchenObjectIndex);
}
[ClientRpc]
private void SetFringRecipeSOClientRpc(int kitchenObjectIndex)
{   
        
    KitchenObjectSo kitchenSo   =  KitchenGameMultiPlayer.Instance.GetKitchenSo(kitchenObjectIndex);
    fryingRecipeSo= GetFryingRecipeSOWithInput(kitchenSo);
}
[ClientRpc]
private void SetBuringinRecipeSOClientRpc(int kitchenObjectIndex)
{   
        
    KitchenObjectSo kitchenSo   =  KitchenGameMultiPlayer.Instance.GetKitchenSo(kitchenObjectIndex);
    burningRecipeSO= GetBurningRecipeSOWithInput(kitchenSo);
}
[ServerRpc(RequireOwnership = false)]
private void SetProgressSliderServerRpc()
{
    SetProgressSliderClientRpc();
}
[ClientRpc]
private void SetProgressSliderClientRpc()
{ 
    OnProgressChanged?.Invoke(this,new OnProgressChangedArgs()
    {
        progressNomel = 0f
    });
    
}
    private bool HasRecipeWithInput(KitchenObjectSo inputfryingObjectSo)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputfryingObjectSo);

        return fryingRecipeSO!=null;
    }
    public KitchenObjectSo GetOutputForInput(KitchenObjectSo inputfryingObjectSo)
    {//获取到菜刀橱柜当前橱柜上的食材，切完片之后的物体
        foreach (FryingRecipeSO fryingRecipe in fryingRecipeSOArray)
        {
            if (fryingRecipe.input==inputfryingObjectSo)
            {
                return fryingRecipe.outInput ;
            }
        }
        return null;
    }

    public FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSo inputfryingObjectSo)
      {
     foreach (FryingRecipeSO fryingRecipe in fryingRecipeSOArray)
     {
         if (fryingRecipe.input==inputfryingObjectSo)
         {
             return fryingRecipe ;
         }
     }
     return null;
    }
    public BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSo inputBurningObjectSo)
    {
        foreach (BurningRecipeSO burningRecipeSo in burningRecipeSoArray)
        {
            if (burningRecipeSo.input==inputBurningObjectSo)
            {
                return burningRecipeSo ;
            }
        }
        return null;
    }
   public int GetCount()
    {
        return fryingProgress;
    }

   public bool IsFried()
   {
     
       return this.state.Value == State.Fride;
   }
}
