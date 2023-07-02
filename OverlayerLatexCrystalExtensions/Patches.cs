using ADOFAI;
using AdofaiTweaks.Core.Attributes;
using AdofaiTweaks.Tweaks.RestrictJudgments;
using HarmonyLib;
using Overlayer.Core;
using Overlayer.Core.Tags;
using Overlayer.Patches;
using Overlayer.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// TODO: Rename this namespace to your mod's name.
namespace OverlayerLatexCrystalExtensions
{
    /// <summary>
    /// Add all of your <see cref="HarmonyPatch"/> classes here. If you find
    /// this file getting too large, you may want to consider separating the
    /// patches into several different classes.
    /// </summary>
    internal static class Patches
    {
		[HarmonyPatch(typeof(Replacer), "Replace")]
		public static class OverlayerAdditionalTags
		{
			[HarmonyPrepare]
			public static bool IsPrepared() => true;

			[HarmonyPostfix]
			public static void PostFix(ref string __result) => LCExtensionTags.ReplaceLCTags(ref __result);
		}

		//[HarmonyPatch(typeof(scrController), "OnDamage")]
		//public static class CaptureFail
		//{


		//	[HarmonyPostfix]
		//	public static void PostFix()
		//	{
		//		if (LCExtensionTags.LCKeyStackQueue.Count <= 0) { return; }
				
		//		var tuple = LCExtensionTags.LCKeyStackQueue.Dequeue();
		//		LCExtensionTags.LCKeyStackQueue.Enqueue(
		//			new Tuple<KeyCode, int, HitMargin>(tuple.Item1, tuple.Item2, HitMargin.FailMiss));
		//	}
		//}


		[HarmonyPatch(typeof(scrMisc), "IsValidHit")]
		private static class CaptureHit
		{
			//public static Dictionary<KeyCode, bool> Triggered = new Dictionary<KeyCode, bool>();
			public static DateTime PrevPress = DateTime.Now;

			[HarmonyPostfix]
			public static void Postfix(ref bool __result, HitMargin margin)
			{
				foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
				{
					if (Input.GetKeyDown(kcode))
					{
						//if (!Triggered.ContainsKey(kcode))
						//{
						//	Triggered.Add(kcode, true);
						//}
						if (LCExtensionTags.LCKeyStackQueue.Count > LCExtensionTags.LCKeyStackQueueCount)
						{
							LCExtensionTags.LCKeyStackQueue.Dequeue();
						}
						LCExtensionTags.LCKeyStackQueue.Enqueue(new Tuple<KeyCode, int, HitMargin>(
								kcode, Math.Min(10000, (int)(DateTime.Now - PrevPress).TotalMilliseconds), margin));
						PrevPress = DateTime.Now;
						//if (Triggered[kcode])
						//{

						//	Triggered[kcode] = false;
						//}
						//else
						//{
						//	Triggered[kcode] = true;
						//}
					}
				}
			}
		}
	}
}
