# if UNITY_EDITOR
using System.Linq;
using System.Globalization;
using UnityEngine;

namespace SpritePivotAlignerUtility {
    public class MetadataPivotEditor : PivotEditor {

        private MetadataStorage storage = null;

        public override void InitOverridables(params SpriteRenderer[] renderers) {
            InitPathsMetaDict(renderers);
        }

        private void InitPathsMetaDict(params SpriteRenderer[] overridables) {
            Sprite[] sprites = (from SpriteRenderer sr in overridables where sr.sprite select sr.sprite).ToArray();
            storage = new MetadataStorage(sprites);
        }

        public override void ChangePivot(SpriteRenderer targetRenderer, SpriteRenderer aligner) {
            SpriteMetadataReader reader = SpriteMetadataReader.GetReader(targetRenderer.sprite, storage);

            float pixelsPerUnit = reader.GetPixelsPerUnit();
            Vector2 size = reader.GetSize();
            Vector2 oldPivot = reader.GetPivotFromMeta();
            Vector2 newPivot = GetNewPivot(targetRenderer.transform.position - aligner.transform.position, oldPivot, size, pixelsPerUnit);

            reader.SetNewPivot(newPivot);
            targetRenderer.transform.position = aligner.transform.position;
            reader.WriteChanges(storage);
        }

        public override void ApplyEditions() {
            storage?.SaveAll();
        }
    }
}
#endif
