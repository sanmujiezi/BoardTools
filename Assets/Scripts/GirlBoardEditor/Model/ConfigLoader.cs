using System.IO;
using GirlBoardCreater;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using GirlBoardUtility = GirlBoardEditor.Tools.GirlBoardUtility;
using PathDefine = GirlBoardEditor.Tools.PathDefine;

namespace GirlBoardEditor.Model
{
    public class ConfigLoader : BaseLoader
    {
        public ScriptableObject config;
        public ConfigLoader(string path) : base(path)
        {
        }

        public override void LoadResourceFromPath(string path)
        {
            if (!File.Exists(path))
            {
                DebugLogger.Instance.Log(this,$"Path \"{path}\" is not exist");
                return;
            }
            
            config = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
        }
        
    }
}