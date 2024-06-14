using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform platePoint;
    [SerializeField]private Transform platePrefab;
    [SerializeField]private PlatesCounter platesCounter;
    private List<Transform> plateList;
    private void Start()
    {
        plateList = new List<Transform>();
        platesCounter.OnPlateSpwns += platesCounter_OnPlateInstance;
        platesCounter.RemovePlateCount += PlatesCounter_RemovePlateCount;
    }

    private void platesCounter_OnPlateInstance(object obj,EventArgs e)
    {
        Transform platTransform = Instantiate(platePrefab,platePoint);
        float plateOffestY = .1f;
        platTransform.localPosition = new Vector3(0, plateOffestY*plateList.Count, 0);
        plateList.Add(platTransform);
    }

    private void PlatesCounter_RemovePlateCount(object obj,EventArgs e)
    {
           Transform plateTransform=  plateList[plateList.Count-1];
           plateList.Remove(plateTransform);
           GameObject.Destroy(plateTransform.gameObject);
    }
}
