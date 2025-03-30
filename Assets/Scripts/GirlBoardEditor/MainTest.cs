using System;
using DefaultNamespace;
using GirlBoardEditor.Config;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GirlBoardEditor
{
    public class MainTest : MonoBehaviour
    {
        public ConfigLoader configLoader;
        public ImageLoader imageLoader;
        public PrefabLoader prefabLoader;
        [SerializeField]
        public ScriptableObject boardConfigOS;
        [SerializeField]
        public Object imageResource;
    }

    [CustomEditor(typeof(MainTest))]
    public class MainTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MainTest mainTest = target as MainTest;
            var toolConfig = AssetDatabase.LoadAssetAtPath<ToolConfig>(PathDefine.ConfigPath); 
            
            if (GUILayout.Button("Load Config"))
            {
                mainTest.configLoader = new ConfigLoader(toolConfig.configPath);
                DebugLogger.Instance.Log(this,$"Load {mainTest.configLoader.config} Config Success!");
                GameObject gameObject = null;
                ((mainTest.configLoader.config) as TestConfigOS).boardDic.TryGetValue("Girl001_board_1", out gameObject);
                DebugLogger.Instance.Log(this,$"Load Resource \"{gameObject.name}\""); 
            }
            if (GUILayout.Button("Load Image"))
            {
                var imagePath = AssetDatabase.GetAssetPath(mainTest.imageResource);
                mainTest.imageLoader = new ImageLoader(imagePath);
                DebugLogger.Instance.Log(this,$"Load {mainTest.imageLoader.images.Count} Image Success!");
            }
            if (GUILayout.Button("Load Prefab"))
            {
                mainTest.prefabLoader = new PrefabLoader(toolConfig.prefabPath);
                DebugLogger.Instance.Log(this,$"Load {mainTest.prefabLoader.templatePrefab} Prefab Success!"); 
            }
            
        }
    }
    
}