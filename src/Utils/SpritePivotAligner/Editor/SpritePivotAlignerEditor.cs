using UnityEngine;
using UnityEditor;

namespace SpritePivotAlignerUtility {
    [CustomEditor(typeof(SpritePivotAligner))]
    public class SpritePivotAlignerEditor : Editor {
        public override void OnInspectorGUI() {
            GUILayout.Label("1. Расположи группу спрайтов так, как хочешь\n   видеть их при смене в SpriteRenderer");
            GUILayout.Label("2. Перетащи в поле Aligner тот SpriteRenderer,\n   относительно которого собираешься позиционировать");
            GUILayout.Label("3. Выбери остальные и нажми Set Selected Transforms\n   (не забудь закрепить окно инспектора)");
            GUILayout.Label("4. Жми Format pivots - и пусть ничто тебя не остановит!");
            GUILayout.Space(16);
            DrawDefaultInspector();
            if (GUILayout.Button("SetSelectedTransforms")) {
                (target as SpritePivotAligner).SetOverridableRenderers();
            }
            if (GUILayout.Button("Format pivots")) {
                (target as SpritePivotAligner).AlignPivots();
            }
        }
    }
}
