using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace OverlayerLatexCrystalExtensions
{
	public class LCExtensionTags
	{
		public static void ReplaceLCTags(ref string result)
		{
			foreach (var pattern in Patterns)
			{
				int tagStart = result.IndexOf($"{{{pattern.Key}");
				
				if (tagStart != -1)
				{
					int tagEnd = result.IndexOf("}", tagStart);
					if (tagEnd != -1)
					{
						string uncompiledTag = result.Substring(tagStart, tagEnd - tagStart + 1);
						result = result.Replace(uncompiledTag, pattern.Value(uncompiledTag));
					}
				}
			}
		}

		private static Dictionary<string, Func<string, string>> Patterns
			=> new Dictionary<string, Func<string, string>>()
			{
				{ "LCProgBar", LCProgBar },
				
				{ "LCKeyStack", LCKeyStack }, 
				{ "LCIntStack", LCIntStack }, 
				{ "LCDiffStack", LCDiffStack },
			};

		
		public static Queue<Tuple<KeyCode, int, HitMargin>> LCKeyStackQueue 
			= new Queue<Tuple<KeyCode, int, HitMargin>>();
		public static int LCKeyStackQueueCount = 10;
		
		private static string LCKeyStack(string matchedPart)
		{			
			string[] parameters = matchedPart.Substring(12, matchedPart.Length - 13).Split(',');
			if (parameters.Length != 2)
			{
				return "参数不正确！";
			}

			if (!int.TryParse(parameters[0], out LCKeyStackQueueCount)
				|| LCKeyStackQueueCount <= 0)
			{
				LCKeyStackQueueCount = 10;
			}


			StringBuilder sb = new StringBuilder();
			if (parameters[1] == "0")
			{
				foreach (var tuple in LCKeyStackQueue)
				{
					sb.AppendLine(LCUtils.GetKeyStr(tuple.Item1));
				}
			}
			else
			{
				foreach (var tuple in LCKeyStackQueue)
				{
					sb.AppendLine($"<color={LCUtils.GetMarginHex(tuple.Item3)}>" +
						$"{LCUtils.GetKeyStr(tuple.Item1)}</color>");
				}
			}

			return sb.ToString();
		}

		private static string LCIntStack(string matchedPart)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var tuple in LCKeyStackQueue)
			{
				sb.AppendLine($"{tuple.Item2}ms");
			}
			return sb.ToString();
		}

		public static int PrevInterval = 0;
		private static string LCDiffStack(string matchedPart)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var tuple in LCKeyStackQueue)
			{
				sb.AppendLine($"{(tuple.Item2 > PrevInterval ? "+" : "")}{tuple.Item2 - PrevInterval}ms");
				PrevInterval = tuple.Item2;
			}
			return sb.ToString();
		}

		private static string LCProgBar(string matchedPart)
		{
			if (!scrLevelMaker.instance)
			{
				return "";
			}
			

			string[] parameters = matchedPart.Substring(11, matchedPart.Length - 12).Split(',');
			if (parameters.Length != 3)
			{
				return "参数不正确！";
			}

			if (!int.TryParse(parameters[0], out int paramBarCount) || paramBarCount <= 0)
			{
				paramBarCount = 10;
			}
			string paramEmptyColour = parameters[1];
			string paramFilledColour = parameters[2];



			int startBarIndex = Overlayer.Tags.Variables.StartTile == 1 ?
				0 : Math.Max((int)(Overlayer.Tags.Variables.StartProg / 100 * paramBarCount), 1);
			int playedBarIndex = (int)(Overlayer.Tags.Misc.Progress() / 100 * paramBarCount);



			return $"[<color=#{paramEmptyColour}>{"".PadRight(startBarIndex, '|')}</color>" +
					$"<color=#{paramFilledColour}>{"".PadRight(playedBarIndex - startBarIndex, '|')}</color>" +
					$"<color=#{paramEmptyColour}>{"".PadRight(paramBarCount - playedBarIndex, '|')}</color>]";
		}	
	}

	public class LCUtils
	{
		public static string GetMarginHex(HitMargin margin)
		{
			switch (margin)
			{
				case HitMargin.TooEarly: 
					return RDConstants.data.hitMarginColoursUI.colourTooEarly.ToHex();
				case HitMargin.VeryEarly:
					return RDConstants.data.hitMarginColoursUI.colourVeryEarly.ToHex();
				case HitMargin.EarlyPerfect:
					return RDConstants.data.hitMarginColoursUI.colourLittleEarly.ToHex();
				case HitMargin.Perfect:
					return RDConstants.data.hitMarginColoursUI.colourPerfect.ToHex();
				case HitMargin.LatePerfect:
					return RDConstants.data.hitMarginColoursUI.colourLittleLate.ToHex();
				case HitMargin.VeryLate:
					return RDConstants.data.hitMarginColoursUI.colourVeryLate.ToHex();
				case HitMargin.TooLate:
					return "#" + Overlayer.Tags.HexCode.FMHex;
				case HitMargin.Multipress:
					return RDConstants.data.hitMarginColoursUI.colourMultipress.ToHex();
				case HitMargin.FailMiss:
					return "#" + Overlayer.Tags.HexCode.FMHex;
				case HitMargin.FailOverload:
					return "#" + Overlayer.Tags.HexCode.FOHex;
				case HitMargin.Auto:
					return RDConstants.data.hitMarginColoursUI.colourPerfect.ToHex();
				default: return RDConstants.data.hitMarginColoursUI.colourPerfect.ToHex();
			}
		}



		private static readonly Dictionary<KeyCode, string> KeyToStr =
			new Dictionary<KeyCode, string>() {
				{ KeyCode.Alpha0, "0" },
				{ KeyCode.Alpha1, "1" },
				{ KeyCode.Alpha2, "2" },
				{ KeyCode.Alpha3, "3" },
				{ KeyCode.Alpha4, "4" },
				{ KeyCode.Alpha5, "5" },
				{ KeyCode.Alpha6, "6" },
				{ KeyCode.Alpha7, "7" },
				{ KeyCode.Alpha8, "8" },
				{ KeyCode.Alpha9, "9" },
				{ KeyCode.Keypad0, "0" },
				{ KeyCode.Keypad1, "1" },
				{ KeyCode.Keypad2, "2" },
				{ KeyCode.Keypad3, "3" },
				{ KeyCode.Keypad4, "4" },
				{ KeyCode.Keypad5, "5" },
				{ KeyCode.Keypad6, "6" },
				{ KeyCode.Keypad7, "7" },
				{ KeyCode.Keypad8, "8" },
				{ KeyCode.Keypad9, "9" },
				{ KeyCode.KeypadPlus, "+" },
				{ KeyCode.KeypadMinus, "-" },
				{ KeyCode.KeypadMultiply, "*" },
				{ KeyCode.KeypadDivide, "/" },
				{ KeyCode.KeypadEnter, "↵" },
				{ KeyCode.KeypadEquals, "=" },
				{ KeyCode.KeypadPeriod, "." },
				{ KeyCode.Return, "↵" },
				{ KeyCode.None, " " },
				{ KeyCode.Tab, "⇥" },
				{ KeyCode.Backslash, "\\" },
				{ KeyCode.Slash, "/" },
				{ KeyCode.Minus, "-" },
				{ KeyCode.Equals, "=" },
				{ KeyCode.LeftBracket, "[" },
				{ KeyCode.RightBracket, "]" },
				{ KeyCode.Semicolon, ";" },
				{ KeyCode.Comma, "," },
				{ KeyCode.Period, "." },
				{ KeyCode.Quote, "'" },
				{ KeyCode.UpArrow, "↑" },
				{ KeyCode.DownArrow, "↓" },
				{ KeyCode.LeftArrow, "←" },
				{ KeyCode.RightArrow, "→" },
				{ KeyCode.Space, "␣" },
				{ KeyCode.BackQuote, "`" },
				{ KeyCode.LeftShift, "L⇧" },
				{ KeyCode.RightShift, "R⇧" },
				{ KeyCode.LeftControl, "LCtrl" },
				{ KeyCode.RightControl, "RCtrl" },
				{ KeyCode.LeftAlt, "LAlt" },
				{ KeyCode.RightAlt, "AAlt" },
				{ KeyCode.Delete, "Del" },
				{ KeyCode.PageDown, "Pg↓" },
				{ KeyCode.PageUp, "Pg↑" },
				{ KeyCode.Insert, "Ins" },
				{ KeyCode.Backspace, "←" },
			};
		public static string GetKeyStr(KeyCode keyCode)
		{
			KeyToStr.TryGetValue(keyCode, out string key);
			return key ?? keyCode.ToString();
		}
	}
}
