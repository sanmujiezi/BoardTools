using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Directory = UnityEngine.Windows.Directory;
using File = UnityEngine.Windows.File;
using Object = UnityEngine.Object;

namespace GirlBoardCreater
{
    public partial class GirlBoardCreater
    {
        public event UnityAction OnGirlResourceChanged;

        public GameObject prefabTemplate;


        private void DrawLeftPanel()
        {
            // 设置左侧栏的宽度
            EditorGUILayout.BeginVertical(GUILayout.Width(_leftPanelWidth));

            //__________________________________
            EditorGUILayout.BeginVertical();


            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.Label("操作区", _headLabelStyle, GUILayout.Height(50));
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            Rect leftLineRect = GUILayoutUtility.GetRect(0, position.width - _leftPanelWidth - _resizeHandleWidth,
                2, 2);
            GUI.DrawTexture(leftLineRect, _backgroundTexture);
            EditorGUILayout.EndVertical();
            //__________________________________


            GUIOpertionAreaCom();

            //__________________________________
            GUILayout.BeginVertical();

            Rect rightPanelRect1 = GUILayoutUtility.GetRect(0, position.width - _leftPanelWidth - _resizeHandleWidth,
                2, 2);
            GUI.DrawTexture(rightPanelRect1, _backgroundTexture);

            GUILayout.Label("测试数据", _headLabelStyle, GUILayout.Height(50));

            GUITestAreaCom();

            EditorGUILayout.EndVertical();

            // 在这里添加左侧栏的内容
            EditorGUILayout.EndVertical();
        }

        private void GUIOpertionAreaCom()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("预制模板", GUILayout.Height(30));
            prefabTemplate =
                EditorGUILayout.ObjectField(prefabTemplate, typeof(GameObject), true, GUILayout.Height(30)) as
                    GameObject;

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("女孩资源位置", GUILayout.Height(30));
            _createPrefabObj =
                EditorGUILayout.ObjectField(_createPrefabObj, typeof(Object), true, GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("资源列表位置", GUILayout.Height(30));
            _resourcesObj =
                EditorGUILayout.ObjectField(_resourcesObj, typeof(Object), true, GUILayout.Height(30));
            if (GUI.changed && _resourcesObj != null)
            {
                _resourcesPath = AssetDatabase.GetAssetPath(_resourcesObj);
                Debug.Log(_resourcesPath);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("主模型", GUILayout.Height(30));
            GUILayout.Label("高亮", GUILayout.Height(30));
            GUILayout.Label("阴影", GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _mainTransformName = GUILayout.TextField(_mainTransformName, GUILayout.Height(30));
            _hightlightTransformName =
                GUILayout.TextField(_hightlightTransformName, GUILayout.Height(30));
            _shadowTransformName = GUILayout.TextField(_shadowTransformName, GUILayout.Height(30));
            GUILayout.EndHorizontal();


            ResourceChangeListener();


            if (GUILayout.Button("创建预制"))
            {
                CreatePrefab();
            }

            //____________________
            GUILayout.Label("", GUILayout.Height(10));
        }

        private void GUITestAreaCom()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("女孩编号：", GUILayout.Height(30));

            _grilId = GetGilrID();


            GUILayout.Label("Girl" + _grilId, GUILayout.Height(30));

            GUILayout.EndHorizontal();
        }

        private void SetBoardTemplate()
        {
            if (!File.Exists(PathDefine.boardTemplateFolder
                             + PathDefine.boardTemplate
                             + PathDefine.prefabPostfix))
            {
                Debug.LogError($"<color=#F91AFF>{PathDefine.boardTemplateFolder}</color> 文件夹不存在");
                return;
            }

            GameObject temp = AssetDatabase.LoadAssetAtPath<GameObject>(
                PathDefine.boardTemplateFolder + PathDefine.boardTemplate + PathDefine.prefabPostfix);

            if (temp == null)
            {
                Debug.LogError($"<color=#F91AFF>{PathDefine.boardTemplateFolder}</color> 文件夹不存在");
                return;
            }

            prefabTemplate = temp;
        }

        private void CreatePrefab()
        {
            if (_resourcesObj == null)
            {
                Debug.LogError("资源列表为空");
                return;
            }

            //检查资源文件，检查texturesList是否有内容
            if (_texturesList == null || _texturesList.Count == 0)
            {
                Debug.LogError("纹理列表为空");
                return;
            }

            if (_texturesList.Count != _texturesList_Fg.Count)
            {
                Debug.LogError(
                    $"texturesList {_texturesList.Count} : texturesList_Fg {_texturesList_Fg.Count} 纹理列表和前景纹理列表数量不一致");
                return;
            }

            string savePath = AssetDatabase.GetAssetPath(_createPrefabObj) + "/" + PathDefine.GirlPrefabPath;
            //开始遍历texturesList资源文件
            for (int i = 0; i < _texturesList.Count; i++)
            {
                //根据模板prefabTemplate在createPrefabPath的路径中
                string tempPath = AssetDatabase.GetAssetPath(prefabTemplate);
                string prefabName = "Girl" + _grilId + "_board_" + (i + 1);
                // 确保目标文件夹存在
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // 复制Prefab到目标文件夹并重命名
                string newPrefabPath =
                    Path.Combine(savePath, prefabName + PathDefine.prefabPostfix);
                if (!File.Exists(newPrefabPath))
                {
                    AssetDatabase.CopyAsset(tempPath, newPrefabPath);
                }
                else
                {
                    Debug.Log($"{newPrefabPath} 已经存在");
                }

                // 加载新创建的Prefab
                PrefabEditor(newPrefabPath, i, prefabName);

                // 刷新AssetDatabase以显示新文件
            }

            AssetDatabase.Refresh();

            //获取预制的Guid保存
            //获取预制的filID保存


            //修改LevelOS文件夹
        }

        private void PrefabEditor(string newPrefabPath, int prefabIndex, string prefabName)
        {
            GameObject newPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPrefabPath);
            if (newPrefab == null)
            {
                Debug.LogError("Failed to load the new prefab.");
                return;
            }

            // 实例化Prefab以进行修改
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(newPrefab) as GameObject;

            if (String.IsNullOrEmpty(_mainTransformName)
                || String.IsNullOrEmpty(_shadowTransformName) ||
                String.IsNullOrEmpty(_hightlightTransformName))
            {
                Debug.LogError("请输入正确的子物体名称");
                return;
            }

            // 查找子物体main和shadow
            Transform mainChild = prefabInstance.transform.GetChild(0).Find(_mainTransformName);
            Transform shadowChild = prefabInstance.transform.GetChild(0).Find(_shadowTransformName);
            Transform hightlightChild = prefabInstance.transform.GetChild(0).Find(_hightlightTransformName);

            if (mainChild != null && shadowChild != null && hightlightChild != null)
            {
                // 获取SpriteRenderer组件
                SpriteRenderer mainSpriteRenderer = mainChild.GetComponent<SpriteRenderer>();
                SpriteRenderer shadowSpriteRenderer = shadowChild.GetComponent<SpriteRenderer>();
                SpriteRenderer hightlightSpriteRenderer = hightlightChild.GetComponent<SpriteRenderer>();

                if (mainSpriteRenderer != null && shadowSpriteRenderer != null && hightlightSpriteRenderer != null)
                {
                    // 修改Sprite（假设你已经有一个新的Sprite资源）
                    string textruePath = AssetDatabase.GetAssetPath(_texturesList[prefabIndex]);
                    string textruePathFg = AssetDatabase.GetAssetPath(_texturesList_Fg[prefabIndex]);
                    Sprite newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(textruePath);
                    Sprite newSpriteFg = AssetDatabase.LoadAssetAtPath<Sprite>(textruePathFg);

                    if (newSprite != null && newSpriteFg != null)
                    {
                        mainSpriteRenderer.sprite = newSprite;
                        shadowSpriteRenderer.sprite = newSprite;
                        hightlightSpriteRenderer.sprite = newSpriteFg;
                        Debug.Log(newSpriteFg.name);
                    }
                    else
                    {
                        Debug.LogError("New sprite not found.");
                    }

                    // 在main组件上创建PolygonCollider2D
                    PolygonCollider2D mainPolygonCollider = mainChild.gameObject.AddComponent<PolygonCollider2D>();

                    // 复制PolygonCollider2D的数值
                    if (mainPolygonCollider != null)
                    {
                        // 获取PolygonCollider2D的路径点
                        Vector2[] colliderPath = mainPolygonCollider.points;

                        // 检查根节点是否有碰撞体，如果没有则创建
                        PolygonCollider2D rootCollider = prefabInstance.GetComponent<PolygonCollider2D>();
                        if (rootCollider == null)
                        {
                            rootCollider = prefabInstance.AddComponent<PolygonCollider2D>();
                        }

                        rootCollider.points = colliderPath; // 粘贴数值
                        DestroyImmediate(mainPolygonCollider);
                    }
                    else
                    {
                        Debug.LogError($"{newPrefab.gameObject.name} 的 PolygonCollider2D 出错了 .");
                    }
                }
                else
                {
                    Debug.LogError("SpriteRenderer components not found on main or shadow.");
                }
            }
            else
            {
                Debug.LogError("main or shadow child not found.");
                Debug.LogError($"{mainChild} {shadowChild} {hightlightChild}");
            }

            GirlBoardUtility.GetPrefabScript(prefabInstance, prefabName);
            CreatereResourcesMap(prefabName, newPrefab);

            // 保存修改后的Prefab
            PrefabUtility.SaveAsPrefabAsset(prefabInstance, newPrefabPath);
            Debug.Log($"{newPrefab.gameObject.name} 保存成功 .");

            // 销毁实例化的对象
            DestroyImmediate(prefabInstance);

            // 建立引用链接
        }

        private void CreatereResourcesMap(string prefabName, GameObject prefabObj)
        {
            string fileId, guid , type;

            fileId = GirlBoardUtility.GetPrefabFileID(prefabObj);
            guid = GirlBoardUtility.GetPrefabGuid(prefabObj);
            type = "3";
            string newPrefabPath = "{" + $"fileID: {fileId}, guid: {guid}, type: {type}" + "}";

            GirlBoardUtility.AddBoardData(_resourcesPath, prefabName, newPrefabPath);
        }


        private string GetGilrID()
        {
            //对比路径，如果图片的女孩编号与实际输出到文件夹的女孩编号文件不对应则弹出提示，例如：通过路径中的Girl100中的数字编号100来确认

            if (prefabTemplate == null)
            {
                Debug.LogError("没有选择预制模板");
                return "没有选择预制模板";
            }

            if (_createPrefabObj == null)
            {
                return "没有选择创建路径";
            }

            //检查创建路径，获取unity的Object类型的资源createPrefabPath的位置是否存在
            var createPrefabPath = AssetDatabase.GetAssetPath(_createPrefabObj);

            if (string.IsNullOrEmpty(createPrefabPath) || !AssetDatabase.IsValidFolder(createPrefabPath))
            {
                Debug.LogError("创建路径无效或不存在: " + createPrefabPath);
                _createPrefabObj = null;
                return "创建路径无效或不存在: " + createPrefabPath;
            }

            Match girlMatchPath = Regex.Match(createPrefabPath, @".*Girl\d+");
            Match girlMatch = Regex.Match(createPrefabPath, @"Girl(\d+)");
            if (!girlMatchPath.Success)
            {
                Debug.LogError("路径中未找到 Girl 编号（格式示例：Girl100）");
                _createPrefabObj = null;
            }

            createPrefabPath = girlMatchPath.Value;
            string girlNumber = girlMatch.Groups[1].Value;
            _createPrefabObj = AssetDatabase.LoadAssetAtPath<Object>(createPrefabPath);
            createPrefabPath = null;
            return girlNumber;
        }

        private void ResourceChangeListener()
        {
            if (_createPrefabObj != _lastPrefabObj)
            {
                OnGirlResourceChanged?.Invoke();
            }
        }
        
    }
}