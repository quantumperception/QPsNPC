using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QPNPC
{
    public class QuestManager : MonoBehaviour
    {
		public bool AssignedQuest { get; set; }
		[SerializeField]
		public GameObject quests;
		public HashSet<Quest> m_activeQuests = new HashSet<Quest>();
		public HashSet<Quest> m_completedQuests = new HashSet<Quest>();
	}
}
