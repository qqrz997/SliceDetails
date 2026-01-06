using System;
using System.Collections.Generic;
using SliceDetails.Data;
using SliceDetails.UI;

namespace SliceDetails;

internal class SliceProcessor
{
	public Tile[] Tiles = new Tile[12];
	public bool Ready { get; private set; } = false;

	public void ResetProcessor() {
		Ready = false;

		Tiles = new Tile[12];

		// Create "tiles", basically allocate information about each position in the 4x3 note grid.
		for (int i = 0; i < 12; i++) {
			Tiles[i] = new();
			for (int j = 0; j < 18; j++) {
				Tiles[i].TileNoteInfos[j] = new();
			}
		}
	}

	public void ProcessSlices(List<NoteInfo> noteInfos) {
		ResetProcessor();

		// Populate the tiles' note infos.  Each List<NoteInfo> in tileNoteInfos cooresponds to each direction/color combination (i.e. DownLeft/ColorA)
		// where elements 0-8 are ColorA notes and elements 9-17 are ColorB notes numbering from NoteCutDirection.Up (0) to NoteCutDirection.Any (8)
		foreach (NoteInfo ni in noteInfos) {
			int noteDirection = (int)Enum.Parse(typeof(OrderedNoteCutDirection), ni.NoteData.cutDirection.ToString());
			int noteColor = (int)ni.NoteData.colorType;
			int tileNoteDataIndex = noteColor * 9 + noteDirection;

			Tiles[ni.NoteIndex].TileNoteInfos[tileNoteDataIndex].Add(ni);
		}

		// Calculate average angles and offsets
		for (int i = 0; i < 12; i++) {
			Tiles[i].CalculateAverages();
		}

		Ready = true;
	}
}