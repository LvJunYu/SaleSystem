using UnityEditor;

public class TextureImportSetting : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.StartsWith("Assets/Sprites"))
        {
            TextureImporter importer = (TextureImporter) assetImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 128;
        }
    }
}