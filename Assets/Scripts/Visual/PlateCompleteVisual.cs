using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
   public struct KitchenObjectSO_GameObject
   {
       public KitchenObjectSo kitchenObjectSo;
       public GameObject kitchen;
   }
    [SerializeField]private PlateKitchenObject plateKitchenObject;
  [SerializeField] private  List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;

    private void Awake()
    {
            
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += plateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
             kitchenObjectSoGameObject.kitchen.SetActive(false);
            
        }
    }

    private void plateKitchenObject_OnIngredientAdded(object obj, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSo==e.kitchenObject)
            {
                kitchenObjectSoGameObject.kitchen.SetActive(true);
            }
        }
    }
}
