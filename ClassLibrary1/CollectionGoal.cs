using System.Collections;
using System.Collections.Generic;
using Jotunn;
using Jotunn.Managers;
using UnityEngine;

namespace QPNPC
{
    public class CollectionGoal : Goal
    {
        public string item { get; set; }
        string playerName = Player.m_localPlayer.m_name;
        Inventory localInventory = Player.m_localPlayer.GetInventory();

        public CollectionGoal(Quest quest, string itemName, string description, int requiredAmount, bool goalHasReward = false, string itemReward = "Wood", int itemRewardAmount = 1, bool waitForComeback = true)
        {
            this.m_quest = quest;
            this.item = itemName;
            this.m_description = description;
            this.m_completed = false;
            this.m_currentAmount = localInventory.CountItems(this.item);
            this.m_requiredAmount = requiredAmount;
            this.m_goalHasReward = goalHasReward;
            this.m_itemReward = PrefabManager.Instance.GetPrefab(itemReward);
            this.m_itemRewardAmount = itemRewardAmount;
            this.m_waitForComeback = waitForComeback;
        }

        public override void Init()
        {
            //base.Init();
            localInventory.m_onChanged += CheckInventory;
        }

        void CheckInventory()
        {
            this.m_currentAmount = localInventory.CountItems(this.item);
            if (this.m_currentAmount == 0) { }
            else if (this.m_currentAmount < this.m_requiredAmount) { }
            else if (this.m_currentAmount >= this.m_requiredAmount) { Evaluate(); }
            else { Evaluate(); }
        }

    }
}
