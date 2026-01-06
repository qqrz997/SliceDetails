using SiraUtil.Affinity;
using SliceDetails.UI;

namespace SliceDetails.AffinityPatches;

internal class MenuTransitionsHelperPatch : IAffinity
{
	private readonly PauseUIController pauseUIController;

	public MenuTransitionsHelperPatch(PauseUIController pauseUIController)
	{
		this.pauseUIController = pauseUIController;
	}

	[AffinityPostfix]
	[AffinityPatch(typeof(MenuTransitionsHelper), nameof(MenuTransitionsHelper.HandleMainGameSceneDidFinish))]
	internal void Postfix() 
	{
		if (Plugin.Settings.ShowInPauseMenu) pauseUIController.CleanUp();
	}
}