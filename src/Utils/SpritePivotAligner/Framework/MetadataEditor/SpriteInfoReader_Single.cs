# if UNITY_EDITOR
using UnityEngine;

namespace SpritePivotAlignerUtility {
    internal class SpriteInfoReader_Single : SpriteMetadataReader {
        protected const string kTextureSettings = "textureSettings:";
        protected const string kSpriteGenerateFallbackPhysicsShape = "spriteGenerateFallbackPhysicsShape";
        protected override string kPivot => kSingleModePivot;
        protected override string kStartPoint => kTextureSettings;
        protected override string kEndPoint => kSpriteGenerateFallbackPhysicsShape;

        private Vector2 size;

        public SpriteInfoReader_Single(Vector2 size) {
            this.size = size;
        }

        public override Vector2 GetSize() {
            return size;
        }
    }
}
#endif
