using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GirlBoardEditor
{
    public class EditorMainWindow : EditorWindow
    {
        public event UnityAction OnClickCreate;
        public event UnityAction OnClickAdjust;
        public event UnityAction OnClickConfig;
        private Rect boxRect = new Rect(10, 10, 200, 100); // 定义Box的位置和大小

        [MenuItem("工具/Girl Board Editor", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(EditorMainWindow));
            window.titleContent = new GUIContent("Girl Board Editor");
        }

        private void OnEnable()
        {
            OnClickCreate += OnCreatePrefab;
            OnClickAdjust += OnAdjustCollider;
            OnClickConfig += OnConfigAdjust;
        }

        private void OnDestroy()
        {
            OnClickCreate -= OnCreatePrefab;
            OnClickAdjust -= OnAdjustCollider;
            OnClickConfig -= OnConfigAdjust;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("GirlBoardEditor", new GUIStyle()
            {
                fontSize = 20,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                }
            }, GUILayout.Width(200), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            CreateTopButtom(@"创建预制", OnClickCreate);
            CreateTopButtom("调整碰撞", OnClickAdjust);
            CreateTopButtom("配置调整", OnClickConfig);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void OnCreatePrefab()
        {
            Debug.Log("OnCreatePrefab");
        }

        private void OnAdjustCollider()
        {
            Debug.Log("OnAdjustCollider");
        }

        private void OnConfigAdjust()
        {
            Debug.Log("OnConfigAdjust");
        }

        private void CreateTopButtom(string name, UnityAction action, float width = 80, float height = 30)
        {
            if (GUILayout.Button(name, GUILayout.Width(width), GUILayout.Height(height)))
            {
                action?.Invoke();
            }
        }
    }
}