using System.Collections.Generic;
using BeatSaberMarkupLanguage;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace SliceDetails.UI;

internal class SelectedTileIndicator : MonoBehaviour
{
	private readonly List<ImageView> tileDots = [];
	private static Sprite? newRoundRect;

	public void Initialize(AssetLoader assetLoader) 
	{
		// Create a new sliced sprite using the existing bloq texture so we don't have to make another resource
		if (newRoundRect == null)
		{
			Vector4 spriteBorder = new Vector4(54, 54, 54, 54);
			var spriteTexture = assetLoader.NoteBackground.texture;
			var rect = new Rect(0, 0, spriteTexture.width, spriteTexture.height);
			newRoundRect = Sprite.Create(spriteTexture, rect, new(0.5f, 0.5f), 200, 1, SpriteMeshType.FullRect, spriteBorder);
		}

		var background = new GameObject("Background").AddComponent<ImageView>();
		background.transform.SetParent(transform, false);
		background.rectTransform.localScale = Vector3.one;
		background.rectTransform.localPosition = Vector3.zero;
		background.rectTransform.sizeDelta = new(15.0f, 12.0f);
		background.sprite = newRoundRect;
		background.type = Image.Type.Sliced;
		background.color = new(0.125f, 0.125f, 0.125f, 0.75f);
		background.material = Utilities.ImageResources.NoGlowMat;

		for (int i = 0; i < 12; i++) 
		{
			var tileDot = new GameObject("TileDot").AddComponent<ImageView>();
			tileDot.transform.SetParent(background.transform, false);
			tileDot.rectTransform.localScale = Vector3.one;
			tileDot.rectTransform.localPosition = new((i % 4) * 3.0f - 4.5f, (i / 4f) * 3.0f - 3.0f, 0.0f);
			tileDot.rectTransform.sizeDelta = new(6.0f, 6.0f);
			tileDot.sprite = assetLoader.NoteDot;
			tileDot.type = Image.Type.Simple;
			tileDot.color = Color.white;
			tileDot.material = Utilities.ImageResources.NoGlowMat;
			tileDots.Add(tileDot);
		}
	}

	public void SetSelectedTile(int tileIndex) 
	{
		for (int i = 0; i < tileDots.Count; i++) 
		{
			tileDots[i].color = (tileIndex == i ? Color.white : new(0.5f, 0.5f, 0.5f, 0.7f));
		}
	}
}