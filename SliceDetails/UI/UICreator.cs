using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SliceDetails.UI;

internal class UICreator
{
	private readonly GridViewController gridViewController;
	private readonly SliceProcessor sliceProcessor;

	private FloatingScreen? floatingScreen;
	
	public UICreator(GridViewController gridViewController, SliceProcessor sliceProcesssor)
	{
		this.gridViewController = gridViewController;
		sliceProcessor = sliceProcesssor;
	}

	public void CreateFloatingScreen(Vector3 position, Quaternion rotation)
	{
		gridViewController.UpdateUINotesHoverHintController();
		
		floatingScreen = FloatingScreen.CreateFloatingScreen(new(150f, 120f), true, position, rotation);
		floatingScreen.SetRootViewController(gridViewController, ViewController.AnimationType.None);
		floatingScreen.ShowHandle = Plugin.Settings.ShowHandle;
		floatingScreen.HandleSide = FloatingScreen.Side.Bottom;
		floatingScreen.HighlightHandle = true;
		floatingScreen.Handle.transform.localScale = Vector3.one * 5.0f;
		floatingScreen.Handle.transform.localPosition = new(0.0f, -25.0f, 0.0f);
		floatingScreen.HandleReleased += OnHandleReleased;
		floatingScreen.gameObject.name = "SliceDetailsScreen";
		floatingScreen.transform.localScale = Vector3.one * 0.03f;

		gridViewController.SetTileScores();
		gridViewController.transform.localScale = Vector3.one;
		gridViewController.transform.localEulerAngles = Vector3.zero;
		gridViewController.gameObject.SetActive(true);
	}

	public void RemoveFloatingScreen() 
	{
		// Destroying the hover hint panel breaks everything so move it out of the screen before destroying
		if (floatingScreen != null)
		{
			var hoverHintPanel = floatingScreen.transform.GetComponentInChildren<HoverHintPanel>(true);
			if (hoverHintPanel != null) 
			{
				hoverHintPanel.transform.SetParent(null);
			}
			gridViewController.transform.SetParent(null);
			gridViewController.gameObject.SetActive(false);
			Object.Destroy(floatingScreen.gameObject);
		}
		sliceProcessor.ResetProcessor();
	}

	public void ParentFloatingScreen(Transform parent) 
	{
		floatingScreen?.transform.SetParent(parent);
	}

	private void OnHandleReleased(object sender, FloatingScreenHandleEventArgs args)
	{
		if (floatingScreen == null) return;
		
		if (SceneManager.GetActiveScene().name == "MainMenu") 
		{
			Plugin.Settings.ResultsUIPosition = floatingScreen.transform.position;
			Plugin.Settings.ResultsUIRotation = floatingScreen.transform.eulerAngles;
		} 
		else if (SceneManager.GetActiveScene().name == "GameCore") 
		{
			Plugin.Settings.PauseUIPosition = floatingScreen.transform.position;
			Plugin.Settings.PauseUIRotation = floatingScreen.transform.eulerAngles;
		}
	}
}