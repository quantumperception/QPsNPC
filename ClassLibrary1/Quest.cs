using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace QPNPC
{
    public class Quest : MonoBehaviour
    {
        private int level;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        public List<Goal> m_goals { get; set; }
        public string m_questName { get; set; }
        public string m_description { get; set; }
        public int m_expReward { get; set; }
        public GameObject m_itemReward { get; set; }
        public int m_itemRewardAmount { get; set; }
        public bool m_completed { get; set; }

        public void CheckGoals()
        {
            m_completed = m_goals.All(g => g.m_completed);
        }

        public void GiveReward()
        {
            if (m_itemReward != null)
                Player.m_localPlayer.GetInventory().AddItem(m_itemReward, m_itemRewardAmount);
        }
    }
}
