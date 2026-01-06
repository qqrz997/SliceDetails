using SiraUtil.Affinity;
using SliceDetails.UI;

namespace SliceDetails.AffinityPatches;

internal class PauseMenuManagerPatches : IAffinity
{
	private readonly PauseUIController pauseUIController;

	public PauseMenuManagerPatches(PauseUIController pauseUIController)
	{
		this.pauseUIController = pauseUIController;
	}

	// ReSharper disable once InconsistentNaming required by harmony
	[AffinityPostfix]
	[AffinityPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.ShowMenu))]
	internal void ShowMenuPostfix(PauseMenuManager __instance) 
	{ 
		if (Plugin.Settings.ShowInPauseMenu) pauseUIController.PauseMenuOpened(__instance);
	}

	[AffinityPostfix]
	[AffinityPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.StartResumeAnimation))]
	internal void StartResumeAnimationPostfix() 
	{
		if (Plugin.Settings.ShowInPauseMenu) pauseUIController.PauseMenuClosed();
	}
}