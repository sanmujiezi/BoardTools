using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GirlBoardEditor.Tools
{
    public class PrefabsLoader : BaseLoader
    {
        public List<GameObject> prefabs;
        public PrefabsLoader(string path) : base(path)
        {
        }

        public override void LoadResourceFromPath(string path)
        {
            prefabs = new List<GameObject>();
            if (!Directory.Exists(path))
            {
                DebugLogger.Instance.Log(this, $"Directory \"{path}\" not exist");
                return;
            }
            
            var files = Directory.GetFiles(path);

            foreach (var VARIABLE in files)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(VARIABLE);
                if (prefab)
                {
                    prefabs.Add(prefab);
                }
            }
        }
    }
}