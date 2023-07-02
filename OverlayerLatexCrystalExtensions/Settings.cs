using Overlayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace OverlayerLatexCrystalExtensions
{
	internal class Settings : UnityModManager.ModSettings
	{
		public void Load()
		{
			
		}

		public void Draw()
		{
			AddTextHelper("Latex Crystal为Overlayer添加的Tags：");
			AddTextHelper("[每个Tag只能用1次！]");
			AddTextHelper("");
			AddTextHelper("{LCProgBar:竖条数量,未填充颜色码,填充颜色码} 进度条");
			AddTextHelper("注释：颜色码用十六进制表示，可以是RGB或RGBA形式。");
			AddTextHelper("示例：{LCProgBar:114,333333,2AD8F0}表示一个长度为114竖条、颜色为灰色和浅蓝色的进度条。");
			AddTextHelper("");
			AddTextHelper("{LCKeyStack:总高度,是否以判定作为文字颜色} 自下而上堆叠的按键显示 [创意来自：蔚恒Eternity]");
			AddTextHelper("注释：总高度使用正整数，推荐20~35。是否以判定作为文字颜色，“1”为是，“0”为否。");
			AddTextHelper("示例：{LCKeyStack:35,1}表示一个总高度为35的按键堆，并且文字颜色是以判定决定的。");
			AddTextHelper("");
			AddTextHelper("{LCIntStack} 自下而上堆叠的按键间隔显示");
			AddTextHelper("注释：请配合LCKeyStack使用，并且总高度和LCKeyStack自动一致。");
			AddTextHelper("示例：{LCIntStack}表示一个总高度和LCKeyStack高度相同的按键间隔堆。");
			AddTextHelper("");
			AddTextHelper("{LCDiffStack} 自下而上堆叠的按键间隔的变化量显示");
			AddTextHelper("注释：请配合LCKeyStack使用，并且总高度和LCKeyStack自动一致。");
			AddTextHelper("示例：{LCDiffStack}表示一个总高度和LCIntStack高度相同的按键间隔的变化量堆。");
		}

		private void AddTextHelper(string text)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(text);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
}
