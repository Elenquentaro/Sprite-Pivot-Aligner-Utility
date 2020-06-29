# if UNITY_EDITOR
using UnityEngine;

namespace SpritePivotAlignerUtility {
    public abstract class PivotEditor {
        public enum EditMode {
            Metadata,
            Importer
        }

        public static PivotEditor GetEditor(EditMode mode) {
            switch (mode) {
                case EditMode.Importer: return new ImportPivotEditor();
                case EditMode.Metadata: return new MetadataPivotEditor();
                default: throw new System.ArgumentException();
            }
        }

        public abstract void InitOverridables(params SpriteRenderer[] renderers);

        public abstract void ChangePivot(SpriteRenderer targetRenderer, SpriteRenderer aligner);

        public abstract void ApplyEditions();

        public Vector2 GetNewPivot(Vector2 positionOffset, Vector2 oldPivot, Vector2 size, float pixelsPerUnit) {
            var factor = size / pixelsPerUnit;
            var newPivot = oldPivot - positionOffset / factor;
            Debug.Log($"Offset = {positionOffset.ToString("N3")}, ppu = {pixelsPerUnit}, size = {size.ToString("N3")}, factor = {factor.ToString("N3")}, oldPivot = {oldPivot.ToString("N3")}");
            return newPivot;
        }
    }
}

#endif
