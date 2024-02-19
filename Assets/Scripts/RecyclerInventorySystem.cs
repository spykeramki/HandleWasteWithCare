using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclerInventorySystem : InventorySystem
{
    [Serializable]
    public struct TimeToRecycleWastages
    {
        public float plastic;
        public float glass;
        public float oil;
        public float organic;
        public float radioactive;

    }

    [SerializeField] 
    public TimeToRecycleWastages recycleTimes;

    private float _currentRecycleTime = 0;

    private List<InventoryItemData> itemsListInRecycler;

    private void Awake()
    {
        itemsListInRecycler = new List<InventoryItemData>();
    }

    public override void AddItem(InventoryItemData itemData)
    {
        if(itemData.garbageCtrl != null)
        {
            if(itemData.garbageCtrl.GarbageType == GarbageManager.GarbageType.NONE)
            {
                return;
            }
            base.AddItem(itemData);
        }
    }

    private void SetRecyclingProcess()
    {
        SetItemList();
        _currentRecycleTime = GetRecyclingTimeAsPerGarbageType(itemsListInRecycler[0].garbageCtrl.GarbageType);
        StartCoroutine("StartRecycling");
    }

    private void SetItemList()
    {
        foreach (KeyValuePair<string, InventoryItemData> item in itemsData)
        {
            itemsListInRecycler.Add(item.Value);
        }
    }

    private IEnumerator StartRecycling()
    {
        while (_currentRecycleTime>0)
        {
            yield return new WaitForSeconds(1f);
            _currentRecycleTime -= 1f;
        }
        if(_currentRecycleTime <= 0)
        {
            StopCoroutine("StartRecycling");
            ContinueRecycling();
        }
    }

    private void ContinueRecycling()
    {
        RemoveItem(itemsListInRecycler[0]);
        if (itemsData.Count > 0)
        {
            SetRecyclingProcess();
        }
    }

    private float GetRecyclingTimeAsPerGarbageType(GarbageManager.GarbageType garbageType)
    {
        float time = 0f;
        switch (garbageType)
        {
            case GarbageManager.GarbageType.PLASTIC:
                time = recycleTimes.plastic;
                break;
            case GarbageManager.GarbageType.GLASS:
                time = recycleTimes.glass;
                break;
            case GarbageManager.GarbageType.ORGANIC:
                time = recycleTimes.organic;
                break;
            case GarbageManager.GarbageType.OIL:
                time = recycleTimes.oil;
                break;
            case GarbageManager.GarbageType.RADIOACTIVE:
                time = recycleTimes.radioactive;
                break;
            default:
                time = 0f;
                break;
        }
        return time;
    }
}
