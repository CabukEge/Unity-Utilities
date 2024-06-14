using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteExtractor : MonoBehaviour
{
    [MenuItem("Tools/Extract Sprites")]
    public static void ExtractSprites()
    {
       
        string spriteSheetPath = ""; 
      
        string outputDirectory = "";


        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        Texture2D spriteSheet = AssetDatabase.LoadAssetAtPath<Texture2D>(spriteSheetPath);
        if (spriteSheet == null)
        {
            Debug.LogError("Sprite Sheet nicht gefunden!");
            return;
        }

       
        string spriteSheetAssetPath = AssetDatabase.GetAssetPath(spriteSheet);
        TextureImporter textureImporter = AssetImporter.GetAtPath(spriteSheetAssetPath) as TextureImporter;
        if (textureImporter != null && !textureImporter.isReadable)
        {
            textureImporter.isReadable = true;
            textureImporter.SaveAndReimport(); 
        }

        Object[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(spriteSheetAssetPath);
        foreach (Object obj in objects)
        {
            if (obj is Sprite)
            {
                Sprite sprite = (Sprite)obj;
                SaveSpriteToFile(sprite, outputDirectory);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Sprites erfolgreich extrahiert!");
    }

    private static void SaveSpriteToFile(Sprite sprite, string outputDirectory)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(pixels);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string filename = Path.Combine(outputDirectory, sprite.name + ".png");
        File.WriteAllBytes(filename, bytes);
    }
}
