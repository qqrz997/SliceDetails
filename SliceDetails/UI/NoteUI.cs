using HMUI;
using IPA.Utilities;
using SliceDetails.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceDetails.UI;

internal class NoteUI : MonoBehaviour
{
	// Set in initialize
	private ImageView directionArrowImage = null!;
	private Transform cutGroup = null!;
	private ImageView cutArrowImage = null!;
	private ImageView cutDistanceImage = null!;
	private ImageView backgroundImage = null!;

	private HoverHint noteHoverHint = null!;
	private HoverHintController hoverHintController = null!;
	private TextMeshProUGUI hoverPanelTmpro = null!;
	private Color noteColor;

	private float noteRotation;

	public void Initialize(OrderedNoteCutDirection cutDirection, Color color, AssetLoader assetLoader) {
		transform.localScale = Vector3.one * 0.9f;

		backgroundImage = GetComponent<ImageView>();
		directionArrowImage = transform.Find("NoteDirArrow").GetComponent<ImageView>();
		cutArrowImage = transform.Find("NoteCutArrow").GetComponent<ImageView>();
		cutDistanceImage = transform.Find("NoteCutDistance").GetComponent<ImageView>();

		cutGroup = new GameObject("NoteCutGroup").transform;
		cutGroup.SetParent(backgroundImage.transform);
		cutGroup.localPosition = Vector3.zero;
		cutGroup.localScale = Vector3.one;
		cutGroup.localRotation = Quaternion.identity;
		cutArrowImage.transform.SetParent(cutGroup);
		cutDistanceImage.transform.SetParent(cutGroup);

		noteColor = color;

		backgroundImage.color = noteColor;
		cutDistanceImage.color = new(0.0f, 1.0f, 0.0f, 0.75f);

		var square = new Texture2D(2, 2);
		square.filterMode = FilterMode.Point;
		square.Apply();
		cutDistanceImage.sprite = Sprite.Create(square, new(0, 0, square.width, square.height), new(0, 0), 100);

		noteHoverHint = backgroundImage.gameObject.AddComponent<HoverHint>();
		noteHoverHint.text = "";
		
		if (cutDirection is OrderedNoteCutDirection.Any)
		{
			directionArrowImage.sprite = assetLoader.NoteDot;
		}

		noteRotation = cutDirection switch
		{
			OrderedNoteCutDirection.Down => 0.0f,
			OrderedNoteCutDirection.Up => 180.0f,
			OrderedNoteCutDirection.Left => 270.0f,
			OrderedNoteCutDirection.Right => 90.0f,
			OrderedNoteCutDirection.DownLeft => 315.0f,
			OrderedNoteCutDirection.DownRight => 45.0f,
			OrderedNoteCutDirection.UpLeft => 225.0f,
			OrderedNoteCutDirection.UpRight => 135.0f,
			OrderedNoteCutDirection.Any => 0.0f,
			_ => noteRotation
		};

		transform.localRotation = Quaternion.Euler(new(0f, 0f, noteRotation));
	}

	public void SetHoverHintController(HoverHintController hoverHintController) 
	{
		this.hoverHintController = hoverHintController;
		var hoverHintPanel = this.hoverHintController.GetField<HoverHintPanel, HoverHintController>("_hoverHintPanel");
		// Skew cringe skew cringe
		hoverHintPanel.GetComponent<ImageView>().SetField("_skew", 0.0f);
		hoverPanelTmpro = hoverHintPanel.GetComponentInChildren<TextMeshProUGUI>();
		hoverPanelTmpro.fontStyle = FontStyles.Normal;
		hoverPanelTmpro.alignment = TextAlignmentOptions.Left;
		hoverPanelTmpro.overflowMode = TextOverflowModes.Overflow;
		hoverPanelTmpro.textWrappingMode = TextWrappingModes.NoWrap;
		var contentSizeFitter = hoverPanelTmpro.gameObject.AddComponent<ContentSizeFitter>();
		contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
	}

	public void SetNoteData(float angle, float offset, Score score, int count) 
	{
		noteHoverHint.SetField("_hoverHintController", hoverHintController);

		if (angle == 0f && offset == 0f) 
		{
			backgroundImage.color = Color.gray;
			cutArrowImage.gameObject.SetActive(false);
			cutDistanceImage.gameObject.SetActive(false);
			directionArrowImage.color = new(0.8f, 0.8f, 0.8f);
			noteHoverHint.text = "";
		} 
		else 
		{
			backgroundImage.color = noteColor;
			cutArrowImage.gameObject.SetActive(true);
			cutDistanceImage.gameObject.SetActive(true);
			cutGroup.transform.localRotation = Quaternion.Euler(new(0f, 0f, angle - noteRotation - 90f));
			if (Plugin.Settings.TrueCutOffsets) 
			{
				cutArrowImage.transform.localPosition = new(offset * 20.0f, 0f, 0f);
				cutDistanceImage.transform.localScale = new Vector2(-offset * 1.33f, 1.0f);
			}
			else 
			{
				cutArrowImage.transform.localPosition = new(offset * (30.0f + score.Offset), 0f, 0f);
				cutDistanceImage.transform.localScale = new Vector2(-offset * (1.995f + score.Offset*0.0665f), 1.0f);
			}
			directionArrowImage.color = Color.white;
			string noteNotes = count == 1 ? "note" : "notes";
			noteHoverHint.text = "Average score (" + count + " " + noteNotes + ")\n" +
			                     $"<color=#ff0000>{score.TotalScore:0.00}</color>\n" +
			                     $"<color=#666666><size=3><line-height=115%>Pre-swing - {score.PreSwing:0.00}\n" +
			                     $"Post-swing - {score.PostSwing:0.00}\n" +
			                     $"Accuracy - {score.Offset:0.00}</line-height></size></color>";
		}
	}
}