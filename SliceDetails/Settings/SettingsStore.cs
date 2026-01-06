using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace SliceDetails.Settings;

internal class SettingsStore
{
	public Vector3 ResultsUIPosition = new(-3.25f, 3.25f, 1.75f);
	public Vector3 ResultsUIRotation = new(340.0f, 292.0f, 0.0f);
	public Vector3 PauseUIPosition = new(-3.0f, 1.5f, 0.0f);
	public Vector3 PauseUIRotation = new(0.0f, 270.0f, 0.0f);
	public bool ShowInPauseMenu = true;
	public bool ShowInCompletionScreen = true;
	public bool ShowHandle = false;
	public bool TrueCutOffsets = true;
	public bool CountArcs = true;
	public bool CountChains = true;
	public bool ShowSliceCounts = true;
}