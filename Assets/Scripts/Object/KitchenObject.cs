using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
  [SerializeField]  private KitchenObjectSo kitchenObjectSo;
  private IkitchenObjectParent kitchenObjectParent;
  private FollowTransform followTransform;


  protected virtual  void Awake()
  {
    followTransform = GetComponent<FollowTransform>();
  }

  public KitchenObjectSo GetKitchenObjectSo()
  {
    return kitchenObjectSo;
  }

  /// <summary>
  /// 设置橱柜
  /// </summary>
  /// <param name="kitchenObjectParent">橱柜对象</param>
  public void SetKitchenObjectParent(IkitchenObjectParent kitchenObjectParent)
  {
    SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
  }
[ServerRpc(RequireOwnership = false)]
  private void SetKitchenObjectParentServerRpc( NetworkObjectReference parent)
  {
    SetKitchenObjectParentClientRpc(parent);
  }
  [ClientRpc]
  private void SetKitchenObjectParentClientRpc(NetworkObjectReference parent)
  {
    
    parent.TryGet(out NetworkObject kitchenParentNetworkObject);
    IkitchenObjectParent kitchenObjectParent = kitchenParentNetworkObject.GetComponent<IkitchenObjectParent>();

    //如果当前橱柜不为空的话说明已经生成了一个食材
    if (this.kitchenObjectParent!=null)
    {
      //清除现有的食材
      this.kitchenObjectParent.ClearKitchenObject();
    }
    //把旧的橱柜设置新的橱柜
    this.kitchenObjectParent = kitchenObjectParent;
    //如果发现新的橱柜里面本来就有一个就报错
    if (kitchenObjectParent.HasKitchenObject())
    {  
      Debug.LogError("已经有一个了");
          
    }
    // 设置橱柜里面的食材把自己传过去
     kitchenObjectParent.SetKitchenObject(this);
    //从当前的橱柜得到坐标设置父对象
    followTransform.SetTargetTranform(this.kitchenObjectParent.GetKitchenObjectFollowTransform());
    //并初始化坐标

  }
  public IkitchenObjectParent  GetClearCouter()
  {
    return kitchenObjectParent;
  }
  
  public void DestroySelf()
  {
    Destroy(gameObject);
  }

  public void DestroyParents()
  {
    kitchenObjectParent.ClearKitchenObject();
  }
  public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
  {
    if (this is PlateKitchenObject)
    {
      plateKitchenObject=this as PlateKitchenObject;
      return true;
    }
    else
    {
      plateKitchenObject = null;
      return false;
    }
    
  }

  public static void SpwanKitchenObject(KitchenObjectSo kitchenObjectSo, IkitchenObjectParent parent)
  {
            KitchenGameMultiPlayer.Instance.SpawnKitchenObject(kitchenObjectSo,parent);
  }

  public static void DestroyKitchenObject(KitchenObject kitchenObject)
  {
    KitchenGameMultiPlayer.Instance.DestroyKitchenObject(kitchenObject);
  }
 
}