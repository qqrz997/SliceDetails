namespace SliceDetails.Data;

internal class NoteInfo
{
	public NoteData NoteData { get; }
	public float CutAngle { get; }
	public float CutOffset { get; }
	public int NoteIndex { get; }
	public Score Score { get; set; } = Score.Zero;

	public NoteInfo(NoteData noteData, float cutAngle, float cutOffset, int noteIndex)
	{
		NoteData = noteData;
		CutAngle = cutAngle;
		CutOffset = cutOffset;
		NoteIndex = noteIndex;
	}
}