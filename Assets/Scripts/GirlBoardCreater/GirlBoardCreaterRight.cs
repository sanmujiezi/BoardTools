using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GirlBoardCreater
{
    public partial class GirlBoardCreater
    {
        private Texture2D selectedItem; // 当前选中的项
        private Vector2 scrollPosition;

        private void DrawRightPanel()
        {
            // 右侧栏
            EditorGUILayout.BeginVertical();

            // 绘制黑色背景（在最底层）
            Rect rightPanelRect = EditorGUILayout.BeginVertical();
            GUI.DrawTexture(rightPanelRect, _backgroundTexture);

            // 绘制其他 GUI 元素
            DrawRightPanelContent();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private void DrawRightPanelContent()
        {
            //实时读取列表如果列表不为空则刷新页面，不显示按钮，显示获取到的所有物体的名称
            if (_texturesList.Count > 0 || _texturesList == null || _texturesList_Fg.Count > 0 || _texturesList_Fg == null)
            {
                RefreshListView();
            }
            else
            {
                //载入选中的文件夹中所有的Sprite
                GUILayout.FlexibleSpace(); // 上方弹性空间

                // 水平居中
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace(); // 左侧弹性空间
                if (GUILayout.Button("载入资源", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    //实时读取列表如果列表不为空则刷新页面，不显示按钮，显示获取到的所有物体的名称
                    //LoadSpriteBySeleted();
                    LoadTextrueByPath();
                }

                GUILayout.FlexibleSpace(); // 右侧弹性空间
                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();
            }
        }

        private void LoadTextrueByPath()
        {
            _texturesList.Clear();
            _texturesList_Fg.Clear();
            
            if (_createPrefabObj == null && this._createPrefabPath == null)
            {
                var selectedObjects = Selection.objects;

                if (selectedObjects.Length <= 0)
                {
                    Debug.LogWarning("您没有选择物品");
                    return;
                }

                _createPrefabObj = selectedObjects[0];
            }

            var _createPrefabPath = AssetDatabase.GetAssetPath(_createPrefabObj) + "/" + PathDefine.GirlImagePath;
            LoadTexture2DFromFolder(_createPrefabPath);

            _lastPrefabObj = _createPrefabObj;
        }

        private void LoadTexture2DFromFolder(string folderPath)
        {
            // 获取文件夹中的所有资源
            string[] assetPaths = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

            foreach (var assetPath in assetPaths)
            {
                // 加载 Sprite

                string fullPath = AssetDatabase.GUIDToAssetPath(assetPath);
                Texture2D textrue = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);

                if (textrue != null)
                {
                    
                    if (textrue.name.Contains("_fg_"))
                    {
                        _texturesList_Fg.Add(textrue);
                    }
                    else
                    {
                        _texturesList.Add(textrue);
                    }
                }
            }

            Debug.Log($"Loaded {_texturesList.Count } { _texturesList_Fg.Count} textrue from folder: {folderPath}");
        }

        private void RefreshListView()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            foreach (var VARIABLE in _texturesList)
            {
                CreateListItem(VARIABLE);
            }

            foreach (var VARIABLE in _texturesList_Fg)
            {
                CreateListItem(VARIABLE);
            }

            GUILayout.EndScrollView();
        }

        private void CreateListItem(Texture2D variable)
        {
            // 开始水平布局
            EditorGUILayout.BeginHorizontal(GUILayout.Height(30));

            // 绘制 Sprite 图标
            Texture2D icon = variable;
            GUILayout.Label(new GUIContent(icon), GUILayout.Width(30), GUILayout.Height(30));

            // 绘制 Sprite 名称
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft // 水平左对齐，垂直居中
            };

            // 点击项时选中
            if (GUILayout.Button(variable.name, labelStyle, GUILayout.Height(30)))
            {
                selectedItem = variable;
                Debug.Log("Selected: " + variable.name);
            }

            // 结束水平布局
            EditorGUILayout.EndHorizontal();

            // 绘制黑色分割线
            DrawDivider();
        }

        private void DrawDivider()
        {
            // 创建一个高度为 3 的黑色分割线
            Rect rect = EditorGUILayout.GetControlRect(false, 3);
            EditorGUI.DrawRect(rect, Color.black);
        }
    }
}