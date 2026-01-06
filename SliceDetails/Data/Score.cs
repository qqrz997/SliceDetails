namespace SliceDetails.Data;

internal class Score
{
	public static Score Zero { get; } = new(0, 0, 0);
		
	public float PreSwing { get; set; }
	public float PostSwing { get; set; }
	public float Offset { get; set; }
	public float TotalScore 
	{ 
		get
		{
			if (PreSwing < 0)
				return PostSwing + Offset;
			if (PostSwing < 0)
				return PreSwing + Offset;
			return PreSwing + PostSwing + Offset;
		}
	}

	public bool CountPreSwing { get; } = true;
	public bool CountPostSwing { get; } = true;

	public Score(float? preSwing, float? postSwing, float offset) 
	{
		if (preSwing != null)
			PreSwing = (float)preSwing;
		else
			CountPreSwing = false;
		if (postSwing != null)
			PostSwing = (float)postSwing;
		else
			CountPostSwing = false;
		Offset = offset;
	}

	public static Score operator +(Score a, Score b) => 
		new(a.PreSwing + b.PreSwing, a.PostSwing + b.PostSwing, a.Offset + b.Offset);
	
	public static Score operator /(Score a, float b) => 
		new(a.PreSwing / b, a.PostSwing / b, a.Offset / b);
	
	public static Score operator /(Score a, Score b) =>
		new(a.PreSwing / b.PreSwing, a.PostSwing / b.PostSwing, a.Offset / b.Offset);
}