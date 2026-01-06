using SiraUtil.Affinity;
using SliceDetails.UI;

namespace SliceDetails.AffinityPatches;

internal class ResultsViewControllerPatch : IAffinity
{
	private readonly UICreator uiCreator;

	public ResultsViewControllerPatch(UICreator uiCreator)
	{
		this.uiCreator = uiCreator;
	}

	[AffinityPostfix]
	[AffinityPatch(typeof(ResultsViewController), nameof(ResultsViewController.DidDeactivate))]
	internal void Postfix() 
	{
		if (Plugin.Settings.ShowInCompletionScreen) uiCreator.RemoveFloatingScreen();
	}
}