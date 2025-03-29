using UnityEditor;
using UnityEngine;

namespace GirlBoardEditor.Model
{
    public class PrefabLoader : BaseLoader
    {
        public GameObject templatePrefab;
        public PrefabLoader(string path) : base(path)
        {
        }

        public override void LoadResourceFromPath(string path)
        {
            templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);   
        }
    }
}