using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using SliceDetails.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace SliceDetails.UI;

[HotReload(RelativePathToLayout = @"Views\gridView.bsml")]
[ViewDefinition("SliceDetails.UI.Views.gridView.bsml")]
internal class GridViewController : BSMLAutomaticViewController
{
	[Inject] private readonly AssetLoader assetLoader = null!;
	[Inject] private readonly PlayerDataModel playerDataModel = null!;
	[Inject] private readonly HoverHintControllerHandler hoverHintControllerHandler = null!;
	[Inject] private readonly SliceProcessor sliceProcessor = null!;
	[Inject] private readonly DiContainer diContainer = null!;

	[UIObject("tile-grid")] private readonly GameObject tileGrid = null!;
	[UIObject("tile-row")] private readonly GameObject tileRow = null!;
	[UIComponent("tile")] private readonly ClickableImage tile = null!;

	[UIComponent("note-modal")] private readonly ModalView noteModal = null!;
	[UIObject("note-horizontal")] private readonly GameObject noteHorizontal = null!;
	[UIObject("note-grid")] private readonly GameObject noteGrid = null!;
	[UIObject("note-row")] private readonly GameObject noteRow = null!;

	[UIComponent("note")] private readonly ImageView note = null!;
	[UIComponent("note-dir-arrow")] private readonly ImageView noteDirArrow = null!;
	[UIComponent("note-cut-arrow")] private readonly ImageView noteCutArrow = null!;
	[UIComponent("note-cut-distance")] private readonly ImageView noteCutDistance = null!;
	[UIComponent("sd-version")] private readonly TextMeshProUGUI sdVersionText = null!;
	[UIComponent("reset-button")] private readonly RectTransform resetButtonTransform = null!;

	private readonly List<ClickableImage> tiles = [];
	private readonly List<NoteUI> notes = [];
	private SelectedTileIndicator? selectedTileIndicator;
	private BasicUIAudioManager? basicUIAudioManager;

	[UIAction("#post-parse")]
	public void PostParse() 
	{
		noteDirArrow.gameObject.name = "NoteDirArrow";
		noteCutArrow.gameObject.name = "NoteCutArrow";
		noteCutDistance.gameObject.name = "NoteCutDistance";

		note.sprite = assetLoader.NoteBackground;
		tile.sprite = assetLoader.GradientBackground;
		noteDirArrow.sprite = assetLoader.NoteArrow;
		noteCutArrow.sprite = assetLoader.CutArrow;

		sdVersionText.text = $"SliceDetails v{ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3) }";
		sdVersionText.InvokeMethod<object, TextMeshProUGUI>("Awake"); // For some reason this is necessary
		sdVersionText.rectTransform.sizeDelta = new(40.0f, 10.0f);
		sdVersionText.transform.localPosition = new(0.0f, -17.0f, 0.0f);

		if (SceneManager.GetActiveScene().name == "MainMenu") 
		{
			Destroy(resetButtonTransform.gameObject);
		} 
		else 
		{ 
			resetButtonTransform.sizeDelta = new(8.0f, 4.0f);
			resetButtonTransform.localPosition = new(-15.0f, -17.0f, 0.0f);
			resetButtonTransform.GetComponentInChildren<CurvedTextMeshPro>().fontStyle = FontStyles.Normal;
			foreach (var imageView in resetButtonTransform.GetComponentsInChildren<ImageView>()) 
			{
				imageView.SetField("_skew", 0.0f);
				imageView.transform.localPosition = Vector3.zero;
			}
		}

		// Create first row of tiles
		for (int i = 0; i < 4; i++) 
		{
			ClickableImage tileInstance = Instantiate(tile.gameObject, tileRow.transform).GetComponent<ClickableImage>();
			tiles.Add(tileInstance);
		}

		// Create other 2 rows of tiles
		for (int i = 0; i < 2; i++) 
		{
			GameObject tileRowInstance = Instantiate(tileRow, tileGrid.transform);
			tileRowInstance.transform.SetAsFirstSibling();
			tiles.AddRange(tileRowInstance.GetComponentsInChildren<ClickableImage>());
		}

		// Set tile click events and data
		for (int i = 0; i < tiles.Count; i++) 
		{
			tiles[i].OnClickEvent += PresentNotesModal;
			tiles[i].DefaultColor = tile.DefaultColor;
			tiles[i].HighlightColor = tile.HighlightColor;
		}

		var noteParent = noteRow.transform;
		var rowParent = noteGrid.transform;
		var currentHoverHintController = hoverHintControllerHandler.HoverHintController;
		for (int i = 0; i < 18; i++)
		{
			if (i % 9 == 0) 
			{
				rowParent = Instantiate(noteGrid, noteHorizontal.transform).transform;
			}
			if (i % 3 == 0)
			{
				noteParent = Instantiate(noteRow, rowParent).transform;
			}

			var cutDirection = (OrderedNoteCutDirection)(i % 9);
			var colorScheme = playerDataModel.playerData.colorSchemesSettings.GetSelectedColorScheme();
			var color = i >= 9 ? colorScheme.saberBColor : colorScheme.saberAColor;
			var uiNote = Instantiate(note.gameObject, noteParent).AddComponent<NoteUI>();
			uiNote.Initialize(cutDirection, color, assetLoader);
			uiNote.SetHoverHintController(currentHoverHintController);

			notes.Add(uiNote);
		}

		basicUIAudioManager = Resources.FindObjectsOfTypeAll<BasicUIAudioManager>()
			.First(x => x.isActiveAndEnabled);
			
		selectedTileIndicator = new GameObject("SelectedTileIndicator").AddComponent<SelectedTileIndicator>();
		selectedTileIndicator.Initialize(assetLoader);
		selectedTileIndicator.transform.SetParent(noteModal.transform, false);
		selectedTileIndicator.transform.localPosition = new(0f, 30f, 0f);

		DestroyImmediate(note.gameObject);
		DestroyImmediate(noteRow);
		DestroyImmediate(noteGrid);
		DestroyImmediate(tile.gameObject);
	}

	public void SetTileScores() 
	{
		for (int i = 0; i < tiles.Count; i++) 
		{
			var formattableTexts = tiles[i].transform.GetComponentsInChildren<FormattableText>(true);

			if(Plugin.Settings.ShowSliceCounts) 
			{
				formattableTexts[0].transform.localPosition = new(0.0f, 0.75f, 0.0f);
				formattableTexts[1].transform.localPosition = new(0.0f, -1.5f, 0.0f);
				formattableTexts[1].gameObject.SetActive(true);
			} 
			else 
			{
				formattableTexts[1].gameObject.SetActive(false);
			}

			if (sliceProcessor.Tiles[i].AtLeastOneNote) 
			{ 
				formattableTexts[0].text = $"{sliceProcessor.Tiles[i].ScoreAverage:0.00}";
				formattableTexts[1].text = sliceProcessor.Tiles[i].NoteCount.ToString();
			}
			else 
			{ 
				formattableTexts[0].text = "";
				formattableTexts[1].text = "";
			}
		}
	}

	[UIAction("#presentNotesModal")]
	public void PresentNotesModal(PointerEventData eventData) 
	{
		basicUIAudioManager?.HandleButtonClickEvent();

		int tileIndex = tiles.IndexOf(eventData.pointerPress.GetComponent<ClickableImage>());
		selectedTileIndicator?.SetSelectedTile(tileIndex);
		var tile = sliceProcessor.Tiles[tileIndex];
		for (int i = 0; i < notes.Count; i++) 
		{
			notes[i].SetNoteData(tile.AngleAverages[i], tile.OffsetAverages[i], tile.ScoreAverages[i], tile.NoteCounts[i]);
		}

		noteModal.Show(false);
	}

	public void CloseModal(bool animated) 
	{
		noteModal.Hide(animated);
	}

	public void UpdateUINotesHoverHintController()
	{
		var currentHoverHintController = hoverHintControllerHandler.HoverHintController;
		for (int i = 0; i < notes.Count; i++)
		{
			notes[i].SetHoverHintController(currentHoverHintController);
		}
	}

	[UIAction("resetRecorder")]
	public void ResetRecorder() 
	{
		var sliceRecorder = diContainer.TryResolve<SliceRecorder>();
		sliceRecorder?.ClearSlices();
		SetTileScores();
	}
}