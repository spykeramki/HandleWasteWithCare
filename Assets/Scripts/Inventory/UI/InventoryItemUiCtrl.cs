using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUiCtrl : MonoBehaviour
{
    public struct UiData{
        public Sprite itemImage;
        public int count;
    }

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemCount;

    public void SetDataInUi(UiData uiData)
    {
        itemImage.sprite = uiData.itemImage;
        itemCount.text = uiData.count.ToString();
    }
}
