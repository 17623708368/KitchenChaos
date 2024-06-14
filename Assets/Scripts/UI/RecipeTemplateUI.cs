using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeTemplateUI : MonoBehaviour
{
   [SerializeField] private Transform icontTransform;
   [SerializeField]private TextMeshProUGUI txtRecipeName;
   private String  ICON_PREFABLE = "UI/IconTemplate";
   /// <summary>
   /// 初始化菜单列表
   /// </summary>
   /// <param name="recipeSo">得到列表的种类</param>
   public void Initialize(RecipeSO recipeSo)
   {
      txtRecipeName.text = recipeSo.reciptName;
      //遍历里面的菜，生成图片提示需要那些菜
      foreach (KitchenObjectSo kitchenObject in recipeSo.KitchenObjectSOList)
      {
        IconTemplateUI iconTemplateUI= Instantiate(Resources.Load<GameObject>(ICON_PREFABLE), icontTransform).GetComponent<IconTemplateUI>();
        iconTemplateUI.SetKitchenObjectSo(kitchenObject);
      }
   }
}