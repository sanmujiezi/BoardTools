using System.IO;
using UnityEditor;
using UnityEngine;

namespace GirlBoardEditor.Tools
{
    public class PrefabLoader : BaseLoader
    {
        public GameObject templatePrefab;
        public PrefabLoader(string path) : base(path)
        {
        }

        public override void LoadResourceFromPath(string path)
        {
            if (!File.Exists(path))
            {
                DebugLogger.Instance.Log(this, $"File \"{path}\" not exist");
                return;
            }
            templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);   
        }
    }
}