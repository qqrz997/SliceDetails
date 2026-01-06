using SiraUtil.Affinity;
using SliceDetails.UI;
using UnityEngine;

namespace SliceDetails.AffinityPatches;

internal class PartyFreePlayFlowCoordinatorPatch : IAffinity
{
	private readonly UICreator uiCreator;

	public PartyFreePlayFlowCoordinatorPatch(UICreator uiCreator)
	{
		this.uiCreator = uiCreator;
	}

	[AffinityPostfix]
	[AffinityPatch(typeof(PartyFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
	internal void Postfix(LevelCompletionResults levelCompletionResults)
	{
		if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.None 
		    && Plugin.Settings.ShowInCompletionScreen)
		{
			uiCreator.CreateFloatingScreen(Plugin.Settings.ResultsUIPosition, Quaternion.Euler(Plugin.Settings.ResultsUIRotation));
		}
	}
}