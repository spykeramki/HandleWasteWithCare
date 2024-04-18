using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclerInventorySystem : InventorySystem
{
    public MachineInteractionCtrl machineInteractionCtrl;

    public override void AddItem(InventoryItemData itemData)
    {
        if(itemData.garbageType == GarbageManager.GarbageType.NONE)
        {
            return;
        }
        base.AddItem(itemData);
    }

}
