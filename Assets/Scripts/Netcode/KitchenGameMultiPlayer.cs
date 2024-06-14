using System;
using Unity.Netcode;
using UnityEngine;


public class KitchenGameMultiPlayer : NetworkBehaviour
{
  private static KitchenGameMultiPlayer instance;
  public static KitchenGameMultiPlayer Instance => instance;
  [SerializeField]private KitchenObjectListSo kitchenObjectListSo;

  private void Awake()
  {
      instance = this;
  }
/// <summary>
/// 外部调用的生成方法负责转类型传递给Rpc去
/// </summary>
/// <param name="kitchenObjectSo"></param>
/// <param name="parent"></param>
  public void SpawnKitchenObject(KitchenObjectSo  kitchenObjectSo,IkitchenObjectParent parent )
  {
      SpwanKitchenObjectServerRpc( GetKitchenSoIndex(kitchenObjectSo), parent.GetNetworkObject());
  
  }
  [ServerRpc(RequireOwnership = false)] 
  void SpwanKitchenObjectServerRpc(int kitchenObjectSoIndex,NetworkObjectReference parent )
  {
      KitchenObjectSo kitchenObjectSo = kitchenObjectListSo.kitchenObjectSos[kitchenObjectSoIndex];
      //就重新生成一个
      Transform pre=  Instantiate(kitchenObjectSo.prefab );
      //设置食材的橱柜
      NetworkObject kitchenObjectNetworkObject = pre.GetComponent<NetworkObject>();
      
      kitchenObjectNetworkObject.Spawn(true);
      
      parent.TryGet(out NetworkObject kitchenParentNetworkObject);
      
      KitchenObject kitchenObject = pre.GetComponent<KitchenObject>();
      
      IkitchenObjectParent kitchenObjectParent=    kitchenParentNetworkObject.GetComponent<IkitchenObjectParent>();
      kitchenObject.  SetKitchenObjectParent(kitchenObjectParent);
  }
 
 
  public int  GetKitchenSoIndex(KitchenObjectSo kitchenObjectSo)
  {
      return kitchenObjectListSo.kitchenObjectSos.IndexOf(kitchenObjectSo);
  }

  public KitchenObjectSo GetKitchenSo(int kitchenSOIndex)
  {
      return kitchenObjectListSo.kitchenObjectSos[kitchenSOIndex];
  }

  public void  DestroyKitchenObject(KitchenObject kitchenObject)
  {
      DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
  }

  [ServerRpc(RequireOwnership = false)]
  public void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
  {
      kitchenObjectReference.TryGet(out Unity.Netcode.NetworkObject kitchenNetworkObjectet);
     KitchenObject kitchenObject=  kitchenNetworkObjectet.GetComponent<KitchenObject>();
     DestroyKitchenObjectClientRpc(kitchenObjectReference);
     kitchenObject.DestroySelf();
  }

  [ClientRpc]
  private void DestroyKitchenObjectClientRpc(NetworkObjectReference kitchenObjectReference)
  {
      kitchenObjectReference.TryGet(out  NetworkObject kitchenNetworkObjectet);
      KitchenObject kitchenObject=  kitchenNetworkObjectet.GetComponent<KitchenObject>();
      kitchenObject.DestroyParents();
  }
}