using UnityEngine;
using Zenject;

namespace SliceDetails.UI;

internal class PauseUIController : IInitializable
{
	private readonly SliceRecorder sliceRecorder;
	private readonly UICreator uiCreator;
	private readonly HoverHintControllerHandler hoverHintControllerHandler;

	private readonly GridViewController gridViewController;
	private GameObject? gridViewControllerParent;

	public PauseUIController(
		SliceRecorder sliceRecorder,
		UICreator uiCreator,
		GridViewController gridViewController,
		HoverHintControllerHandler hoverHintControllerHandler) 
	{
		this.sliceRecorder = sliceRecorder;
		this.uiCreator = uiCreator;
		this.gridViewController = gridViewController;
		this.hoverHintControllerHandler = hoverHintControllerHandler;
	}

	public void Initialize() 
	{
		if (Plugin.Settings.ShowInPauseMenu) 
		{
			hoverHintControllerHandler.CloneHoverHintController();
			uiCreator.CreateFloatingScreen(Plugin.Settings.PauseUIPosition, Quaternion.Euler(Plugin.Settings.PauseUIRotation));
			gridViewControllerParent = gridViewController.transform.parent.gameObject;
			gridViewControllerParent?.SetActive(false);
		}
	}

	public void PauseMenuOpened(PauseMenuManager pauseMenuManager) 
	{
		gridViewControllerParent?.SetActive(true);
		uiCreator.ParentFloatingScreen(pauseMenuManager.transform);
		sliceRecorder.ProcessSlices();
		gridViewController.SetTileScores();
	}

	public void PauseMenuClosed() 
	{
		gridViewController.CloseModal(false);
		gridViewControllerParent?.SetActive(false);
	}

	public void CleanUp()
	{
		gridViewController.CloseModal(false);
		uiCreator.RemoveFloatingScreen();
	}
}