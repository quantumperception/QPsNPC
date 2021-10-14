using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPNPC
{
    public class Goal
    {
        public Quest m_quest { get; set; }
        public string m_description { get; set; }
        public bool m_completed { get; set; }
        public int m_currentAmount { get; set; }
        public int m_requiredAmount { get; set; }
        public GameObject m_itemReward { get; set; }
        public int m_itemRewardAmount { get; set; }
        public bool m_goalHasReward { get; set; }
        public bool m_waitForComeback { get; set; }

        public virtual void Init()
        {
            // default init stuff
        }

        public void Evaluate()
        {
            if (m_currentAmount >= m_requiredAmount)
            {
                Complete();
            }
        }

        public void Complete()
        {
            if (m_goalHasReward && !m_waitForComeback) GiveGoalReward();
            this.m_quest.CheckGoals();
            m_completed = true;
            Debug.Log("Goal marked as completed.");
        }
        public void GiveGoalReward()
        {
            if (m_itemReward != null)
                Player.m_localPlayer.GetInventory().AddItem(m_itemReward, m_itemRewardAmount);
            Debug.Log("Goal completed! Reward: " + m_itemReward.GetComponent<ItemDrop>().m_itemData.m_shared.m_name);
        }
    }
}
