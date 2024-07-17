using System.IO;
using System.Reflection;
using UnityEngine;

namespace SliceDetails.UI
{
	internal class AssetLoader
	{
		public Sprite CutArrow { get; }
		public Sprite GradientBackground { get; }
		public Sprite NoteBackground { get; }
		public Sprite NoteArrow { get; }
		public Sprite NoteDot { get; }

		public AssetLoader() {
			NoteArrow = LoadSpriteFromResource("SliceDetails.Resources.arrow.png");
			CutArrow = LoadSpriteFromResource("SliceDetails.Resources.cut_arrow.png");
			NoteDot = LoadSpriteFromResource("SliceDetails.Resources.dot.png");
			GradientBackground = LoadSpriteFromResource("SliceDetails.Resources.bloq_gradient.png");
			NoteBackground = LoadSpriteFromResource("SliceDetails.Resources.bloq.png");
		}

		public static Sprite LoadSpriteFromResource(string path) {
			Assembly assembly = Assembly.GetCallingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(path)) {
				if (stream != null) {
					byte[] data = new byte[stream.Length];
					stream.Read(data, 0, (int)stream.Length);
					Texture2D tex = new Texture2D(2, 2);
					if (tex.LoadImage(data)) {
						Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
						return sprite;
					}
				}
			}
			return null;
		}
	}
}
