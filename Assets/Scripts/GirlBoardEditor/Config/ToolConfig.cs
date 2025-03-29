using System;
using System.IO;
using GirlBoardEditor.Tools;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GirlBoardEditor.Config
{
    [CreateAssetMenu(fileName = "ToolConfig", menuName = "GirlBoard/ToolConfig")]
    public class ToolConfig : ScriptableObject
    {
        public event UnityAction OnPrefabPathChanged;
        public event UnityAction OnConfigPathChanged;
        //[HideInInspector]
        public string prefabPath;

        //[HideInInspector]
        public string configPath;
    }

    [CustomEditor(typeof(ToolConfig))]
    public class ToolConfigInspector : Editor
    {
        private ToolConfig toolConfig;
        private GameObject prefab;
        private ScriptableObject config;
        private bool isFirstEnter;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            toolConfig = (ToolConfig)target;

            prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
            config = EditorGUILayout.ObjectField("Config", config, typeof(ScriptableObject), false) as ScriptableObject;
            if (!isFirstEnter)
            {
                UpdateFromPath();
                isFirstEnter = true;
            }

            if (GUI.changed)
            {
                UpdateFromObject();
            }
        }


        private void UpdateFromObject()
        {
            if (prefab)
            {
                toolConfig.prefabPath = AssetDatabase.GetAssetPath(prefab);
            }
            else
            {
                toolConfig.prefabPath = null;
                DebugLogger.Instance.Log(this, "Prefab is null");
            }


            if (config)
            {
                toolConfig.configPath = AssetDatabase.GetAssetPath(config);
            }
            else
            {
                toolConfig.configPath = null;
                DebugLogger.Instance.Log(this, "toolConfig is null");
            }
        }

        private void UpdateFromPath()
        {
            if (!string.IsNullOrEmpty(toolConfig.prefabPath))
            {
                if (File.Exists(toolConfig.prefabPath))
                {
                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(toolConfig.prefabPath);
                }
                else
                {
                    DebugLogger.Instance.Log(this, "Prefab Path is not exist");
                }
            }

            if (!string.IsNullOrEmpty(toolConfig.configPath))
            {
                if (File.Exists(toolConfig.configPath))
                {
                    config = AssetDatabase.LoadAssetAtPath<ScriptableObject>(toolConfig.configPath);
                }
                else
                {
                    DebugLogger.Instance.Log(this, "config Path is not exist");
                }
            }
        }

        public string GetConfigPath()
        {
            return toolConfig.configPath;
        }

        public string GetPrefabPath()
        {
            return toolConfig.prefabPath;
        }
    }
}