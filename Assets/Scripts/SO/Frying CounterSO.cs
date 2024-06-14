using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
     public KitchenObjectSo input;
     public KitchenObjectSo outInput;
     public int fryingMaxTime;
}
