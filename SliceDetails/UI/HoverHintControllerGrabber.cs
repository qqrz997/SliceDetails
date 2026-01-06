using HMUI;
using Zenject;

namespace SliceDetails.UI;

internal class HoverHintControllerGrabber : IInitializable
{
	private readonly HoverHintControllerHandler hoverHintControllerHandler;
	private readonly HoverHintController hoverHintController;

	public HoverHintControllerGrabber(HoverHintControllerHandler hoverHintControllerHandler, HoverHintController hoverHintController) 
	{
		this.hoverHintControllerHandler = hoverHintControllerHandler;
		this.hoverHintController = hoverHintController;
	}

	public void Initialize()
	{
		hoverHintControllerHandler.SetOriginalHoverHintController(hoverHintController);
	}
}