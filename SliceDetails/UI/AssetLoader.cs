using System;
using System.IO;
using SliceDetails.Utils;
using UnityEngine;

namespace SliceDetails.UI;

internal class AssetLoader
{
	private const string ResourcesPath = "SliceDetails.Resources.";
	public Sprite CutArrow { get; }
	public Sprite GradientBackground { get; }
	public Sprite NoteBackground { get; }
	public Sprite NoteArrow { get; }
	public Sprite NoteDot { get; }

	public AssetLoader() 
	{
		NoteArrow = LoadSpriteResource("arrow.png");
		CutArrow = LoadSpriteResource("cut_arrow.png");
		NoteDot = LoadSpriteResource("dot.png");
		GradientBackground = LoadSpriteResource("bloq_gradient.png");
		NoteBackground = LoadSpriteResource("bloq.png");
	}

	private static Sprite LoadSpriteResource(string resourceName)
	{
		var imageData = ImageLoading.GetResource(ResourcesPath + resourceName);
		return new Texture2D(2, 2).ToSprite(imageData, Path.GetFileNameWithoutExtension(resourceName))
		       ?? throw new InvalidOperationException("Failed to create a sprite from an internal image");
	}
}