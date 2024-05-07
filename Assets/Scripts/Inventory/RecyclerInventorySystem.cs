using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Machine inventory class inherited from parent inventory class
public class RecyclerInventorySystem : InventorySystem
{
    public MachineInteractionCtrl machineInteractionCtrl;

    //extension method to add item to machine inventory
    public override void AddItem(InventoryItemData itemData)
    {
        if(itemData.garbageType == GarbageManager.GarbageType.NONE)
        {
            return;
        }
        base.AddItem(itemData);
    }

}
