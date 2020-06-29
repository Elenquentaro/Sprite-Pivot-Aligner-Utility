# if UNITY_EDITOR
using UnityEngine;

namespace SpritePivotAlignerUtility {
    internal class SpriteInfoReader_Multiply : SpriteMetadataReader {
        protected const string kName = "name: ";
        protected const string kSpriteID = "spriteID";
        protected const string kWidth = "width: ";
        protected const string kHeight = "height: ";
        protected override string kPivot => kMultiplyModePivot;
        protected override string kStartPoint => kName + spriteName;
        protected override string kEndPoint => kSpriteID;

        private string spriteName;

        public SpriteInfoReader_Multiply(string spriteName) {
            this.spriteName = spriteName;
        }

        public override Vector2 GetSize() {
            var widthString = spriteInfo.Substring(kWidth, kNewLine);

            var heightString = spriteInfo.Substring(kHeight, kNewLine);

            float.TryParse(widthString, out float width);
            float.TryParse(heightString, out float height);
            // Debug.Log($"'{widthString}', '{heightString}'");
            return new Vector2(width, height);
        }
    }
}
#endif
