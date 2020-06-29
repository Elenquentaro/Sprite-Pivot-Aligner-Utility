# if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpritePivotAlignerUtility {
    public class ImportPivotEditor : PivotEditor {
        private Dictionary<Texture, TextureImporter> textureImportersDict = null;

        public override void InitOverridables(params SpriteRenderer[] renderers) {
            InitTextureImportersDict(renderers);
        }

        private void InitTextureImportersDict(params SpriteRenderer[] overridables) {
            textureImportersDict = new Dictionary<Texture, TextureImporter>();
            for (int i = 0; i < overridables.Length; i++) {
                var texture = overridables[i].sprite?.texture;
                if (texture || !textureImportersDict.ContainsKey(texture)) {
                    string path = AssetDatabase.GetAssetPath(texture);
                    textureImportersDict[texture] = AssetImporter.GetAtPath(path) as TextureImporter;
                    textureImportersDict[texture].isReadable = true;
                }
            }
        }

        public override void ChangePivot(SpriteRenderer targetRenderer, SpriteRenderer aligner) {
            var texture = targetRenderer.sprite?.texture;
            if (!texture) return;

            TextureImporter textureImporter = textureImportersDict[texture];

            int metaDataIndex = GetMetaDataIndex(textureImporter, targetRenderer.sprite.name);

            var spritesheet = textureImporter.spritesheet;

            int targetAlignment = (int)SpriteAlignment.Custom;

            Vector2 size;
            Vector2 oldPivot;
            Debug.Log(aligner.sprite.rect.size);
            Debug.Log(targetRenderer.sprite.rect.size);
            if (textureImporter.spriteImportMode == SpriteImportMode.Multiple) {
                size = spritesheet[metaDataIndex].rect.size;
                oldPivot = spritesheet[metaDataIndex].pivot;
            } else {
                size = targetRenderer.sprite.rect.size;
                oldPivot = textureImporter.spritePivot;
            }
            Vector2 newPivot = GetNewPivot(targetRenderer.transform.position - aligner.transform.position,
                oldPivot, size, textureImporter.spritePixelsPerUnit);
            targetRenderer.transform.position = aligner.transform.position;

            spritesheet[metaDataIndex].alignment = targetAlignment;
            spritesheet[metaDataIndex].pivot = newPivot;
            textureImporter.spritesheet = spritesheet;
            Debug.Log("New pivot = " + newPivot.ToString("N3") + "; "
                + textureImporter.spritesheet[metaDataIndex].pivot.ToString("N3") + "; " + spritesheet[metaDataIndex].pivot.ToString("N3"));
        }

        private int GetMetaDataIndex(TextureImporter textureImporter, string spriteName) {
            for (int i = 0; i < textureImporter.spritesheet.Length; i++) {
                if (textureImporter.spritesheet[i].name == spriteName) return i;
            }
            throw new System.ArgumentException($"Sprite {spriteName} isn't exist in texture importer {textureImporter.name}!");
        }

        public override void ApplyEditions() {
            foreach (var pair in textureImportersDict) {
                var path = AssetDatabase.GetAssetPath(pair.Key);
                Debug.Log("reimporting asset at " + path);
                pair.Value.isReadable = false;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
    }
}

#endif
