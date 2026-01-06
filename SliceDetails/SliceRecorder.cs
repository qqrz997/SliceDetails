using System;
using System.Collections.Generic;
using SliceDetails.Data;
using UnityEngine;
using Zenject;

namespace SliceDetails;

internal class SliceRecorder : UnityEngine.Object, IInitializable, IDisposable
{
	private readonly BeatmapObjectManager beatmapObjectManager;
	private readonly SliceProcessor sliceProcessor;
	private readonly ScoreController scoreController;

	private readonly Dictionary<NoteData, NoteInfo> noteSwingInfos = [];
	private readonly List<NoteInfo> noteInfos = [];

	public SliceRecorder(
		BeatmapObjectManager beatmapObjectManager,
		ScoreController scoreController,
		SliceProcessor sliceProcessor) 
	{
		this.beatmapObjectManager = beatmapObjectManager;
		this.scoreController = scoreController;
		this.sliceProcessor = sliceProcessor;
	}

	public void Initialize() 
	{
		beatmapObjectManager.noteWasCutEvent += OnNoteWasCut;
		scoreController.scoringForNoteFinishedEvent += ScoringForNoteFinishedHandler;
		sliceProcessor.ResetProcessor();
	}

	public void Dispose() 
	{
		beatmapObjectManager.noteWasCutEvent -= OnNoteWasCut;
		scoreController.scoringForNoteFinishedEvent -= ScoringForNoteFinishedHandler;
		// Process slices once the map ends
		ProcessSlices();
	}

	public void ClearSlices() 
	{
		noteInfos.Clear();
		ProcessSlices();
	}

	public void ProcessSlices() 
	{
		sliceProcessor.ProcessSlices(noteInfos);
	}

	private void OnNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
	{
		if (noteController.noteData.colorType != ColorType.None && noteCutInfo.allIsOK)
			ProcessNote(noteController, noteCutInfo);
	}

	private void ProcessNote(NoteController noteController, NoteCutInfo noteCutInfo) 
	{
		if (noteController == null) return;
			
		Vector2 noteGridPosition;
		noteGridPosition.y = (int)noteController.noteData.noteLineLayer;
		noteGridPosition.x = noteController.noteData.lineIndex;
		int noteIndex = (int)(noteGridPosition.y * 4 + noteGridPosition.x);

		// No ME notes allowed >:(
		if (noteGridPosition.x >= 4 || noteGridPosition.y >= 3 || noteGridPosition.x < 0 || noteGridPosition.y < 0) return;

		Vector2 cutDirection = new Vector3(-noteCutInfo.cutNormal.y, noteCutInfo.cutNormal.x);
		float cutAngle = Mathf.Atan2(cutDirection.y, cutDirection.x) * Mathf.Rad2Deg + 180f;

		float cutOffset = noteCutInfo.cutDistanceToCenter;
		Vector3 noteCenter = noteController.noteTransform.position;
		if (Vector3.Dot(noteCutInfo.cutNormal, noteCutInfo.cutPoint - noteCenter) > 0f)
		{
			cutOffset = -cutOffset;
		}

		NoteInfo noteInfo = new NoteInfo(noteController.noteData, cutAngle, cutOffset, noteIndex);

		if (!noteSwingInfos.ContainsKey(noteController.noteData)) 
		{
			noteSwingInfos.Add(noteController.noteData, noteInfo);
		}
	}

	private void ScoringForNoteFinishedHandler(ScoringElement scoringElement)
	{
		if (!noteSwingInfos.TryGetValue(scoringElement.noteData, out var noteSwingInfo)) 
		{
			// Bad cut, do nothing
			return;
		}

		var goodCutScoringElement = (GoodCutScoringElement)scoringElement;
		var cutScoreBuffer = goodCutScoringElement.cutScoreBuffer;

		int preSwing = cutScoreBuffer.beforeCutScore;
		int postSwing = cutScoreBuffer.afterCutScore;
		int offset = cutScoreBuffer.centerDistanceCutScore;

		bool isArcHead = goodCutScoringElement.noteData.scoringType is NoteData.ScoringType.ArcHead;
		bool isArcTail = goodCutScoringElement.noteData.scoringType is NoteData.ScoringType.ArcTail;

		Score? score = goodCutScoringElement.noteData.gameplayType switch 
		{
			NoteData.GameplayType.Normal when isArcHead && Plugin.Settings.CountArcs =>
				new(preSwing, null, offset),
			NoteData.GameplayType.Normal when isArcTail && Plugin.Settings.CountArcs =>
				new(null, postSwing, offset),
			NoteData.GameplayType.Normal =>
				new(preSwing, postSwing, offset),
			NoteData.GameplayType.BurstSliderHead when Plugin.Settings.CountChains =>
				new(preSwing, null, offset),
			_ => null
		};

		if (score != null) 
		{
			noteSwingInfo.Score = score;
			noteInfos.Add(noteSwingInfo);
		}

		noteSwingInfos.Remove(goodCutScoringElement.noteData);
	}
}