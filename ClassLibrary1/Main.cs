using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jotunn.Utils;
using Jotunn.Managers;
using System.Reflection;
using UnityEngine;
using WorldofValheimZones;
using ValEx;

namespace QPNPC
{
	[BepInPlugin("QPNPC", "QP's NPCs", "0.0.1")]
	[BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("MrRageous.ValEx", BepInDependency.DependencyFlags.HardDependency)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
	public partial class QPNPC : BaseUnityPlugin
	{
		AssetBundle npcBundle = AssetUtils.LoadAssetBundleFromResources("npcbundle", typeof(QPNPC).Assembly);
		private static readonly Harmony harm = new Harmony("harmony");
		private void Awake()
		{
			GameObject npc = npcBundle.LoadAsset<GameObject>("Herrero");
			Destroy(npc.GetComponent<Trader>());
			QuestNPC customTrader = npc.AddComponent<QuestNPC>();
			customTrader.m_name = "Herrero";
			PrefabManager.Instance.AddPrefab(npc);
			harm.PatchAll();
		}
		static class OnSpawned_Patch
		{
			[HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
			static void Postfix(Player __instance)
            {
				if (Player.m_localPlayer) { Player.m_localPlayer.transform.gameObject.AddComponent<QuestManager>(); }
            }
		}
	}
}
