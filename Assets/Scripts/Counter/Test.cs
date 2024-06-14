using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Dictionary<string, int> dictionary;
    public EventHandler OnUpdateHpEvent;
    private Text charHpText;
    private float charHpValue;

    private void Awake()
    {
        OnUpdateHpEvent += UpdateHP;
    }

    private void UpdateHP(object sender, EventArgs e)
    {
        charHpText.text = charHpValue.ToString();
    }

    public void TestSwap<T>(ref T test1, ref T test2)
    {
        T timp = test1;
        test1 = test2;
        test2 = timp;
    }

    private SaveData _saveData;
    private void StarGame()
    {
        Load(data =>
        {
            _saveData = data;
        });
    }
    public void Load(Action<SaveData>cabll){}


}

public class SaveData
{
}
