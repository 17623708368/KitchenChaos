
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField]private Transform container;
    private string PREFABLE_RECIPE = "UI/RecipeTemplate";
    private GameObject recipeTemplate;

    private void Awake()
    {
        
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeComplted += DeliveryManager_OnRecipeComplted;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
    }

    private void DeliveryManager_OnRecipeComplted(object obj, EventArgs e)
    {
        UpdateVisual();
    }
    private void DeliveryManager_OnRecipeSpawned(object obj, EventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach (Transform child  in container)
        {
                Destroy(child.gameObject);
        }
        foreach (RecipeSO recipeSo in DeliveryManager.Instance.GetWaitingRecipeSoList())
        {
            RecipeTemplateUI  recipeTransform = Instantiate(Resources.Load<GameObject>(PREFABLE_RECIPE), container).GetComponent<RecipeTemplateUI>();
             recipeTransform.Initialize(recipeSo);
        }
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeComplted -= DeliveryManager_OnRecipeComplted;
        DeliveryManager.Instance.OnRecipeSpawned -= DeliveryManager_OnRecipeSpawned;
    }
}