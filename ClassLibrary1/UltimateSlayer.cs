using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jotunn;
using Jotunn.Managers;

namespace QPNPC
{
    public class FirstSteps : Quest
    {
        void Start()
        {
            Debug.Log("First steps assigned.");
            m_questName = "First Steps";
            m_description = "Kill a bunch of stuff.";
            m_itemReward = PrefabManager.Instance.GetPrefab("SwordIron");
            m_itemRewardAmount = 1;
            m_goals = new List<Goal>
        {
            new CollectionGoal(this, "Wood", "Consigue 10 de Madera", 10),
            new CollectionGoal(this, "Stone", "Consigue 5 de Piedra", 5)
        };

            m_goals.ForEach(g => g.Init());
        }
    }
}
