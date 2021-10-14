using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal {
    public string ItemID { get; set; }
    string playerName = Player.m_localPlayer.m_name;
    Inventory localInventory = Player.m_localPlayer.GetInventory();
    int oldItemsNum;
    int itemsNum = Player.m_localPlayer.GetInventory().NrOfItems();

    public CollectionGoal(Quest quest, string itemID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.ItemID = itemID;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        localInventory.m_onChanged += CheckInventory;
    }

    void CheckInventory()
    {
        this.CurrentAmount = localInventory.CountItems(this.ItemID);
        if (this.CurrentAmount == 0) { }
        else if (this.CurrentAmount < this.RequiredAmount) { }
        else if (this.CurrentAmount >= this.RequiredAmount) { Evaluate(); }
        else { Evaluate(); }
    }

}
