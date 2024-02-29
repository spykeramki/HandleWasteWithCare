using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclerInventorySystem : InventorySystem
{
    

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

}
