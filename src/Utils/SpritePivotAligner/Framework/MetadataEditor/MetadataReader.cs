# if UNITY_EDITOR
using System.Globalization;
using UnityEngine;
using UnityEditor;

namespace SpritePivotAlignerUtility {
    internal abstract class SpriteMetadataReader {
        #region const keywords
        protected const string kNewLine = "\n";
        protected const string kAlignment = "alignment: ";
        protected const string kSpriteMode = "spriteMode: ";
        protected const string kMultiplyModePivot = "pivot: ";
        protected const string kSingleModePivot = "spritePivot: ";
        protected const string kPixelsPerUnit = "spritePixelsToUnits: ";
        #endregion

        public static readonly CultureInfo cultureInfo = new CultureInfo("en-us");

        protected string importer_meta;
        protected string spriteInfo;

        // По сути выступает в качестве идентификатора для дальнейшей записи в MetadataStorage
        private string path;

        protected abstract string kStartPoint { get; }
        protected abstract string kEndPoint { get; }
        protected abstract string kPivot { get; }

        public virtual void Init(string importer_meta) {
            this.importer_meta = importer_meta;
            spriteInfo = GetSpriteInfo();
        }

        protected string GetSpriteInfo() {
            return importer_meta.Substring(kStartPoint, kEndPoint, false);
        }

        protected string GetPivotString() {
            return spriteInfo.Substring(kPivot, kNewLine);
        }

        public abstract Vector2 GetSize();

        public Vector2 GetPivotFromMeta() {
            var pivotString = GetPivotString();
            var xString = pivotString.Substring("x: ", ",");
            var yString = pivotString.Substring("y: ", "}");
            Debug.Log($"'{xString}', '{yString}'");

            float.TryParse(xString, NumberStyles.AllowDecimalPoint, cultureInfo, out float x);
            float.TryParse(yString, NumberStyles.AllowDecimalPoint, cultureInfo, out float y);
            return new Vector2(x, y);
        }

        public float GetPixelsPerUnit() {
            string token = importer_meta.Substring(kPixelsPerUnit, kNewLine);
            float.TryParse(token, out float result);
            return result;
        }

        protected void SetCustomAlignment(ref string segment) {
            var includiveSubstring = segment.Substring(kAlignment, kNewLine, false);
            var replacing = segment.Substring(kAlignment, kNewLine);
            var alignString = includiveSubstring.Replace(replacing, ((int)SpriteAlignment.Custom).ToString());
            segment = segment.Replace(includiveSubstring, alignString);
            Debug.Log($"{segment}\n\n : {includiveSubstring}\n\n : {segment}");
        }

        private int GetImporterAlignment() {
            var token = importer_meta.Substring(kAlignment, kNewLine);
            int.TryParse(token, out int result);
            Debug.Log($"Parsing importer alignment from '{token}' : {result}");
            return result;
        }

        public void SetNewPivot(Vector2 value) {
            var oldPivotToken = GetPivotString();
            var newSpriteInfo = spriteInfo.Replace(oldPivotToken, Vector2ToMeta(value));
            SetCustomAlignment(ref newSpriteInfo);
            if (GetImporterAlignment() != (int)SpriteAlignment.Custom) {
                SetCustomAlignment(ref importer_meta);
            }
            Debug.Log($"old info: {spriteInfo}");
            Debug.Log($"new info: {newSpriteInfo}");
            importer_meta = importer_meta.Replace(spriteInfo, newSpriteInfo);
            spriteInfo = newSpriteInfo;
        }

        public void WriteChanges(MetadataStorage storage) {
            storage.WriteNewMetadata(path, importer_meta);
        }

        public static SpriteMetadataReader GetReader(Sprite sprite, MetadataStorage storage) {
            if (!sprite) throw new System.ArgumentException();

            string metadata = storage.GetMetadata(sprite);

            int.TryParse(metadata.Substring(kSpriteMode, kNewLine), out int mode);
            SpriteImportMode importMode = (SpriteImportMode)mode;

            SpriteMetadataReader reader;
            switch (importMode) {
                case SpriteImportMode.Multiple:
                    reader = new SpriteInfoReader_Multiply(sprite.name);
                    break;
                case SpriteImportMode.Single:
                    reader = new SpriteInfoReader_Single(sprite.textureRect.size);
                    break;
                default: throw new System.Exception($"{importMode} is unsupportable sprite import mode!");
            }
            reader.path = MetadataStorage.GetAssetMetaFilePath(sprite);
            reader.Init(metadata);
            return reader;
        }

        public static string Vector2ToMeta(Vector2 vector) {
            return "{" + $"x: {vector.x.ToString(cultureInfo)}, y: {vector.y.ToString(cultureInfo)}" + "}";
        }
    }
}
#endif
