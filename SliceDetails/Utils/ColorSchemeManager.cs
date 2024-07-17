using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceDetails.Utils
{
	internal class ColorSchemeManager
	{
		private static ColorScheme _colorScheme;

		public static ColorScheme GetMainColorScheme() {
			if (_colorScheme == null) {
				ColorSchemesSettings colorSchemesSettings = Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault()._playerData.colorSchemesSettings;
				_colorScheme = colorSchemesSettings.GetSelectedColorScheme();
			}
			return _colorScheme;
		}
	}
}
