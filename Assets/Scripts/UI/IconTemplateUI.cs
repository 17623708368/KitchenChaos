using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplateUI : MonoBehaviour
{

    [SerializeField]private Image image;
    public void SetKitchenObjectSo(KitchenObjectSo kitchenObjectSo)
    {
        image.sprite = kitchenObjectSo.sprite;
    }
}
