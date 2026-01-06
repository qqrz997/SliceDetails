using SiraUtil.Affinity;
using SliceDetails.UI;
using UnityEngine;

namespace SliceDetails.AffinityPatches;

internal class SoloFreePlayFlowCoordinatorPatch : IAffinity
{
	private readonly UICreator uiCreator;

	public SoloFreePlayFlowCoordinatorPatch(UICreator uiCreator) 
	{
		this.uiCreator = uiCreator;
	}

	[AffinityPostfix]
	[AffinityPatch(typeof(SoloFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
	internal void Postfix(LevelCompletionResults levelCompletionResults)
	{
		if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.None 
		    && Plugin.Settings.ShowInCompletionScreen)
		{
			uiCreator.CreateFloatingScreen(Plugin.Settings.ResultsUIPosition, Quaternion.Euler(Plugin.Settings.ResultsUIRotation));
		}
	}
}