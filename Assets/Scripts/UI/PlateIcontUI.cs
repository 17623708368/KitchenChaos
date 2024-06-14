using UnityEngine;

public class PlateIcontUI : MonoBehaviour
{
    private string ICON_NAME="UI/IconTemplate";
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += plateKitchenObject_OnIngredientAdded;
    }
    private void plateKitchenObject_OnIngredientAdded(object obj, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
         Transform  iconTransform= Instantiate(Resources.Load<GameObject>(ICON_NAME)).transform;
         iconTransform.SetParent(this.transform);
         (iconTransform as RectTransform).localPosition=Vector3.zero;
          iconTransform.GetComponent<IconTemplateUI>().SetKitchenObjectSo(e.kitchenObject);
          
    }
    private void OnDestroy()
    {
        plateKitchenObject.OnIngredientAdded -= plateKitchenObject_OnIngredientAdded;

    }
}
