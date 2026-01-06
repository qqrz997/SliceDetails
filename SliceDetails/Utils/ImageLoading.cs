using System.Reflection;
using UnityEngine;

namespace SliceDetails.Utils;

internal static class ImageLoading
{
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    
    public static byte[] GetResource(string resourcePath)
    {
        using var stream = Assembly.GetManifestResourceStream(resourcePath);
        var data = new byte[stream.Length];
        stream.Read(data, 0, (int)stream.Length);
        return data;
    }

    public static Sprite? ToSprite(this Texture2D? tex, byte[] data, string rename)
    {
        if (tex == null || !tex.LoadImage(data)) return null;
        var sprite = Sprite.Create(tex, new(0, 0, tex.width, tex.height), new(0.5f, 0.5f), 100);
        if (!string.IsNullOrWhiteSpace(rename)) sprite.name = rename;
        return sprite;
    }
}