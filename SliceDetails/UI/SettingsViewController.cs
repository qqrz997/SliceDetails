using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using System;
using Zenject;

namespace SliceDetails.UI
{
    internal class SettingsViewController : IInitializable, IDisposable
	{
		private readonly string resourceName = $"SliceDetails.UI.Views.settingsView.bsml";

        public void Initialize() {
            BSMLSettings.Instance.AddSettingsMenu("SliceDetails", resourceName, this);
        }

		public void Dispose() {
			BSMLSettings.Instance.RemoveSettingsMenu(this);
		}

		[UIValue("show-pause")]
		public bool ShowInPauseMenu
        {
            get => Plugin.Settings.ShowInPauseMenu;
            set => Plugin.Settings.ShowInPauseMenu = value;
        }

        [UIValue("show-completion")]
		public bool ShowInCompletionScreen
        {
            get => Plugin.Settings.ShowInCompletionScreen;
            set => Plugin.Settings.ShowInCompletionScreen = value;
        }

        [UIValue("show-handles")]
		public bool ShowHandle
		{
			get => Plugin.Settings.ShowHandle;
			set => Plugin.Settings.ShowHandle = value;
		}

		[UIValue("show-counts")]
		public bool ShowSliceCounts
		{
			get =>	Plugin.Settings.ShowSliceCounts;
			set => Plugin.Settings.ShowSliceCounts = value;
		}

		[UIValue("true-offsets")]
		public bool TrueCutOffsets
		{
			get => Plugin.Settings.TrueCutOffsets;
			set	=> Plugin.Settings.TrueCutOffsets = value;
		}

		[UIValue("count-arcs")]
		public bool CountArcs
		{
			get => Plugin.Settings.CountArcs;
			set => Plugin.Settings.CountArcs = value;
		}

		[UIValue("count-chains")]
		public bool CountChains
		{
			get => Plugin.Settings.CountChains;
			set => Plugin.Settings.CountChains = value;
		}
	}
}
