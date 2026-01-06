using System;
using HMUI;
using Object = UnityEngine.Object;

namespace SliceDetails.UI;

internal class HoverHintControllerHandler
{
	public HoverHintController HoverHintController =>
		hoverHintControllerCopy != null ? hoverHintControllerCopy 
		: hoverHintControllerOriginal != null ?  hoverHintControllerOriginal
		: throw new NullReferenceException();

	private HoverHintController? hoverHintControllerOriginal;
	private HoverHintController? hoverHintControllerCopy;

	internal void SetOriginalHoverHintController(HoverHintController original) 
	{
		hoverHintControllerOriginal = original;
	}

	internal void CloneHoverHintController() 
	{
		hoverHintControllerCopy = Object.Instantiate(hoverHintControllerOriginal, null, true);
	}
}