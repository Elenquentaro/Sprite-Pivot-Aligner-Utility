# if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpritePivotAlignerUtility {
    internal class MetadataStorage {
        private Dictionary<string, string> pathsAndMetaDict = null;

        public MetadataStorage(params Object[] objs) {
            pathsAndMetaDict = new Dictionary<string, string>();
            foreach (var obj in objs) {
                var path = GetAssetMetaFilePath(obj);
                if (!pathsAndMetaDict.ContainsKey(path)) {
                    pathsAndMetaDict[path] = File.ReadAllText(path);
                    Debug.Log("path: " + path + "\nmetadata: " + pathsAndMetaDict[path]);
                }
            }
        }

        public string GetMetadata(Object obj) {
            return GetMetadata(GetAssetMetaFilePath(obj));
        }

        public string GetMetadata(string path) {
            if (!pathsAndMetaDict.ContainsKey(path)) {
                Debug.LogWarning("Storage doesn't contains that metadata path!");
                pathsAndMetaDict[path] = File.ReadAllText(path);
            }
            return pathsAndMetaDict[path];
        }

        // public void WriteNewMetadata(Object obj, string value) {
        //     var path = GetAssetMetaFilePath(obj);
        //     var oldValue = GetMetadata(path);
        //     pathsAndMetaDict[path] = value;
        //     Debug.Log("Old metadata value:\n" + oldValue);
        //     Debug.Log("Writing new metadata value:\n" + value);
        // }

        public void WriteNewMetadata(string path, string value) {
            pathsAndMetaDict[path] = value;
        }

        public void SaveAll() {
            foreach (var pair in pathsAndMetaDict) {
                Debug.Log("Saving metadata from path " + pair.Key);
                File.WriteAllText(pair.Key, pair.Value);
            }
        }

        public static string GetAssetMetaFilePath(Object obj) {
            return AssetDatabase.GetAssetPath(obj) + ".meta";
        }
    }
}
#endif
