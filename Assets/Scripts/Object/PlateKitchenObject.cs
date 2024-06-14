using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    
    [SerializeField]  private List<KitchenObjectSo> validKitchenObjectList;
    private List<KitchenObjectSo> kitchenObjectSoList;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs:EventArgs
    {
        public KitchenObjectSo kitchenObject;

    }
    protected override void Awake()
    {
        kitchenObjectSoList = new List<KitchenObjectSo>();
        base.Awake();
    }

    public List<KitchenObjectSo> GetkitchenObjectSoList()
    {
        return kitchenObjectSoList;
    }
    public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
    {
        if (!validKitchenObjectList.Contains(kitchenObjectSo))
            return false;
        if (kitchenObjectSoList.Contains(kitchenObjectSo))
        {
            return false;
        }
        else
        {
            AddIngredientServerRpc( KitchenGameMultiPlayer.Instance.GetKitchenSoIndex(kitchenObjectSo));
            return true;
            
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
[ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
    KitchenObjectSo    kitchenObjectSo=  KitchenGameMultiPlayer.Instance.GetKitchenSo(kitchenObjectSOIndex);
        kitchenObjectSoList.Add(kitchenObjectSo);
        OnIngredientAdded?.Invoke(this,new OnIngredientAddedEventArgs()
        {
            kitchenObject = kitchenObjectSo
        });
    }
}
