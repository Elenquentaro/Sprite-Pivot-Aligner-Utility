# if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SpritePivotAlignerUtility {
    public class SpritePivotAligner : MonoBehaviour {
        [SerializeField] private SelectionMode selectionMode = SelectionMode.TopLevel;

        [Tooltip("Режим изменения пивота:\nMetadata - путём редактирования метафайлов напрямую, требуется Refresh\nImporter - редактирование через взаимодействие с импортером (не работает на некоторых форматах)")]
        [SerializeField] private PivotEditor.EditMode editMode = PivotEditor.EditMode.Metadata;

        [Tooltip("SpriteRenderer со спрайтом, относительно которого всё будет позиционироваться")]
        [SerializeField] private SpriteRenderer aligner = null;

        [Tooltip("SpriteRenderer'ы со спрайтами, пивоты которых будут отпозиционированы относительно расположения Aligner'а")]
        [SerializeField] private SpriteRenderer[] overridables = null;

        private PivotEditor pivotEditor = null;


        public void AlignPivots() {
            if (aligner == null) {
                throw new System.Exception("Aligner sprite renderer must be assigned!");
            }
            if (overridables == null || overridables.Length == 0) {
                throw new System.Exception("You're not assigned any overridable sprite renderer with sprite!");
            }

            pivotEditor = PivotEditor.GetEditor(editMode);
            pivotEditor.InitOverridables(overridables);

            for (int i = 0; i < overridables.Length; i++) {
                pivotEditor.ChangePivot(overridables[i], aligner);
            }

            pivotEditor.ApplyEditions();

            Selection.objects = new Object[0];
            overridables = null;
            aligner = null;
        }

        public void SetOverridableRenderers() {
            overridables = (
                from Object obj
                in Selection.GetFiltered(typeof(SpriteRenderer), SelectionMode.TopLevel)
                select obj as SpriteRenderer)
                .ToArray();
        }
    }
}
#endif
