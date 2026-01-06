using System.Collections.Generic;
using UnityEngine;

namespace SliceDetails.Data;

internal class Tile
{
	public List<NoteInfo>[] TileNoteInfos { get; } = new List<NoteInfo>[18];
	public float[] AngleAverages { get; private set; } = new float[18];
	public float[] OffsetAverages { get; private set; } = new float[18];
	public Score[] ScoreAverages { get; private set; } = new Score[18];
	public int[] NoteCounts { get; private set; } = new int[18];
	public float ScoreAverage { get; private set; }
	public bool AtLeastOneNote { get; private set; }
	public int NoteCount { get; private set; }

	public void CalculateAverages() 
	{
		AngleAverages = new float[18];
		OffsetAverages = new float[18];
		ScoreAverages = new Score[18];
		NoteCounts = new int[18];
		ScoreAverage = 0.0f;
		AtLeastOneNote = false;

		for (int i = 0; i < 18; i++) 
		{
			ScoreAverages[i] = new(0.0f, 0.0f, 0.0f);
		}

		NoteCount = 0;
		for (int i = 0; i < TileNoteInfos.Length; i++)
		{
			if (TileNoteInfos[i].Count <= 0) continue;
			
			int preSwingCount = 0;
			int postSwingCount = 0;
			Vector2 angleXYAverages = Vector2.zero;
			foreach (var noteInfo in TileNoteInfos[i]) {
				AtLeastOneNote = true;
				angleXYAverages.x += Mathf.Cos(noteInfo.CutAngle * Mathf.PI / 180f);
				angleXYAverages.y += Mathf.Sin(noteInfo.CutAngle * Mathf.PI / 180f);
				OffsetAverages[i] += noteInfo.CutOffset;
				ScoreAverages[i] += noteInfo.Score;
				NoteCounts[i]++;
				ScoreAverage += noteInfo.Score.TotalScore;
				preSwingCount += ScoreAverages[i].CountPreSwing ? 1 : 0;
				postSwingCount += ScoreAverages[i].CountPostSwing ? 1 : 0;
				NoteCount++;
			}
			angleXYAverages.x /= TileNoteInfos[i].Count;
			angleXYAverages.y /= TileNoteInfos[i].Count;
			AngleAverages[i] = Mathf.Atan2(angleXYAverages.y, angleXYAverages.x) * 180f / Mathf.PI;
			OffsetAverages[i] /= TileNoteInfos[i].Count;
			ScoreAverages[i].PreSwing /= preSwingCount;
			ScoreAverages[i].PostSwing /= postSwingCount;
			ScoreAverages[i].Offset /= TileNoteInfos[i].Count;
		}
		ScoreAverage /= NoteCount;
	}
}