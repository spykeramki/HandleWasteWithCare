using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//controls image and count of items in inventory ui throughout the game
public class InventoryItemUiCtrl : MonoBehaviour
{
    //class data
    public struct UiData{
        public Sprite itemImage;
        public int count;
    }

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemCount;

    //data setter
    public void SetDataInUi(UiData uiData)
    {
        itemImage.sprite = uiData.itemImage;
        itemCount.text = uiData.count.ToString();
    }
}
