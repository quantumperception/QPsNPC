using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPNPC
{
	public class QuestNPC : MonoBehaviour, Hoverable, Interactable
	{
		[Serializable]
		public class TradeItem
		{
			public ItemDrop m_prefab;

			public int m_stack = 1;

			public int m_price = 100;
		}

		public string m_name = "Herrero";

		public float m_standRange = 15f;

		public float m_greetRange = 5f;

		public float m_byeRange = 5f;

		public List<TradeItem> m_items = new List<TradeItem>();

		[Header("Dialog")]
		public float m_hideDialogDelay = 5f;

		public float m_randomTalkInterval = 30f;

		public List<string> m_randomTalk = new List<string>();

		public List<string> m_randomGreets = new List<string>();

		public List<string> m_randomGoodbye = new List<string>();

		public List<string> m_randomStartTrade = new List<string>();

		public List<string> m_randomBuy = new List<string>();

		public List<string> m_randomSell = new List<string>();

		public EffectList m_randomTalkFX = new EffectList();

		public EffectList m_randomGreetFX = new EffectList();

		public EffectList m_randomGoodbyeFX = new EffectList();

		public EffectList m_randomStartTradeFX = new EffectList();

		public EffectList m_randomBuyFX = new EffectList();

		public EffectList m_randomSellFX = new EffectList();

		private bool m_didGreet;

		private bool m_didGoodbye;

		private Animator m_animator;

		private LookAt m_lookAt;

		public QuestManager questManager = Player.m_localPlayer.GetComponent<QuestManager>();

		[SerializeField]
		public static string questType;

		Quest thisQuest { get; set; }

		Quest thisQuestInPlayer { get; set; }

		private void Start()
		{
			m_animator = GetComponentInChildren<Animator>();
			m_lookAt = GetComponentInChildren<LookAt>();
			InvokeRepeating("RandomTalk", m_randomTalkInterval, m_randomTalkInterval);
		}

		private void Update()
		{
			Player closestPlayer = Player.GetClosestPlayer(base.transform.position, m_standRange);
			if ((bool)closestPlayer)
			{
				m_animator.SetBool("Stand", value: true);
				m_lookAt.SetLoockAtTarget(closestPlayer.GetHeadPoint());
				float num = Vector3.Distance(closestPlayer.transform.position, base.transform.position);
				if (!m_didGreet && num < m_greetRange)
				{
					m_didGreet = true;
					Say("Wena Conchetumare", "Greet");
					m_randomGreetFX.Create(base.transform.position, Quaternion.identity);
				}
				if (m_didGreet && !m_didGoodbye && num > m_byeRange)
				{
					m_didGoodbye = true;
					Say("Chao Conchetumare", "Greet");
					m_randomGoodbyeFX.Create(base.transform.position, Quaternion.identity);
				}
			}
			else
			{
				m_animator.SetBool("Stand", value: true);
				m_lookAt.ResetTarget();
			}
		}

		void AssignQuest()
		{
			questManager.AssignedQuest = true;
			questManager.m_activeQuests.Add((Quest)questManager.quests.AddComponent(Type.GetType(questType)));
		}

		void CheckQuest()
		{
			if (!Player.m_localPlayer) thisQuestInPlayer = (Quest)questManager.quests.GetComponent(questType);
			if (thisQuestInPlayer != null && questManager.m_activeQuests.Contains(thisQuestInPlayer) && thisQuestInPlayer.m_completed)
			{
				questManager.m_activeQuests.Remove(thisQuestInPlayer);
				questManager.m_completedQuests.Add(thisQuestInPlayer);
				thisQuestInPlayer.GiveReward();
				Say("Gracias por la wea! Toma tu recompensa.", "Stand");
				m_randomSellFX.Create(base.transform.position, Quaternion.identity);
			}
			else
			{
				Say("Aún no terminas la quest.", "Stand");
			}
		}
		private void RandomTalk()
		{
			if (m_animator.GetBool("Stand") && !StoreGui.IsVisible() && Player.IsPlayerInRange(base.transform.position, m_greetRange))
			{
				Say(m_randomTalk, "Talk");
				m_randomTalkFX.Create(base.transform.position, Quaternion.identity);
			}
		}

		public string GetHoverText()
		{
			return Localization.instance.Localize(m_name + "\n[<color=yellow><b>$KEY_Use</b></color>] $raven_interact");
		}

		public string GetHoverName()
		{
			return Localization.instance.Localize(m_name);
		}

		public bool Interact(Humanoid character, bool hold, bool alt)
		{
			if (!Player.m_localPlayer) thisQuestInPlayer = (Quest)questManager.quests.GetComponent(questType);
			if (hold)
			{
				return false;
			}
			if (!questManager.m_activeQuests.Contains(thisQuestInPlayer) && !questManager.m_completedQuests.Contains(thisQuestInPlayer))
			{
				Say("Wena culiao, traeme 10 de madera y 5 de piedra", "Stand");
				AssignQuest();
			}
			else if (questManager.m_activeQuests.Contains(thisQuestInPlayer) && !thisQuestInPlayer.m_completed)
			{
				CheckQuest();
			}
			else
			{
				Say("Vale por la wea choro", "Stand");
			}
			return false;
		}

		private void DiscoverItems(Player player)
		{
			foreach (TradeItem item in m_items)
			{
				player.AddKnownItem(item.m_prefab.m_itemData);
			}
		}

		private void Say(List<string> texts, string trigger)
		{
			Say(texts[UnityEngine.Random.Range(0, texts.Count)], trigger);
		}

		private void Say(string text, string trigger)
		{
			Chat.instance.SetNpcText(base.gameObject, Vector3.up * 1.8f, 20f, m_hideDialogDelay, "", text, large: false);
			if (trigger.Length > 0)
			{
				m_animator.SetTrigger(trigger);
			}
		}

		public bool UseItem(Humanoid user, ItemDrop.ItemData item)
		{
			return false;
		}

		public void OnBought(TradeItem item)
		{
			Say(m_randomBuy, "Buy");
			m_randomBuyFX.Create(base.transform.position, Quaternion.identity);
		}

		public void OnSold()
		{
			Say(m_randomSell, "Sell");
			m_randomSellFX.Create(base.transform.position, Quaternion.identity);
		}
	}
}