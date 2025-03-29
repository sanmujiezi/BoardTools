using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GirlBoardCreater
{
    public class GirlBoardEditorWindow : EditorWindow
    {
        private GameObject[] target;
        private Vector2 scrollView;
        private float scaleValue;
        private float tolerance;
        private float realScaleValue;
        private float realTolerance;
        private bool isEditor = false;

        private Texture2D backgroundTexture;

        private GameObject currentEditObj;
        private PolygonCollider2D currentCollider;

        private List<List<Vector2>> originalPaths = new List<List<Vector2>>();
        private List<List<Vector2>> optimizedPoints = new List<List<Vector2>>();


        [MenuItem("工具/GirlBoardEditor")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<GirlBoardEditorWindow>();
            window.titleContent = new GUIContent("GirlBoardEditor");
            window.Show();
        }

        private void OnEnable()
        {
            string hexColor = "#4BC32E";
            Color color;
            backgroundTexture = new Texture2D(1, 1);
            if (ColorUtility.TryParseHtmlString(hexColor, out color))
            {
                backgroundTexture.SetPixel(0, 0, color);
                backgroundTexture.Apply();
            }
        }

        private void OnDestroy()
        {
            OnExitPrefabEditor();
        }

        public void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            if (!isEditor)
            {
                GUILayout.Label("点击按钮获取选中对象", new GUIStyle()
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 16,
                    normal = new GUIStyleState()
                    {
                        textColor = Color.white,
                    }
                });
            }
            else
            {
                GUILayout.Label("使用滑动条调整Collider缩放", new GUIStyle()
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 16,
                    normal = new GUIStyleState()
                    {
                        textColor = Color.white,
                    }
                });
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            if (!isEditor)
            {
                GUILayout.BeginHorizontal();
                if (target == null || target.Length <= 0)
                {
                    if (GUILayout.Button("Get prefab list", GUILayout.Height(30)))
                    {
                        GetObjFromSeleted();
                    }
                }
                else
                {
                    if (GUILayout.Button("Clear prefab list", GUILayout.Height(30)))
                    {
                        ClearObjs();
                    }
                }


                GUILayout.EndHorizontal();
            }

            DrawCollierEditor();

            GUILayout.Space(10);
            scrollView = GUILayout.BeginScrollView(scrollView);
            CreateSelectedObj();

            GUILayout.EndScrollView();


            GUILayout.EndVertical();
        }

        private void DrawCollierEditor()
        {
            GUILayout.BeginVertical();
            if (isEditor)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("缩放值：", GUILayout.Width(60));
                var tempStr = GUILayout.TextArea(((int)scaleValue).ToString(), GUILayout.Width(50));
                try
                {
                    realScaleValue = Convert.ToInt32(tempStr);
                    scaleValue = Convert.ToInt32(tempStr);
                }
                catch (FormatException e)
                {
                    Debug.LogError("缩放值中，输入的类型必须为整数" + e);
                }

                scaleValue = GUILayout.HorizontalSlider(scaleValue, -250, 250);
                realScaleValue = (int)scaleValue;


                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("简化值：", GUILayout.Width(60));
                var tempTolerance = GUILayout.TextArea($"{tolerance:F2}", GUILayout.Width(50));
                try
                {
                    realTolerance = (float)Convert.ToDouble(tempTolerance);
                    tolerance = (float)Convert.ToDouble(tempTolerance);
                }
                catch (FormatException e)
                {
                    Debug.LogError("缩放值中，输入的类型必须为整数" + e);
                }

                tolerance = GUILayout.HorizontalSlider(tolerance, 0, 1);
                realTolerance = tolerance;

                if (GUI.changed)
                {
                    Debug.Log("change");
                    PolygonColliderEditLogic();
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save and Exit", GUILayout.Height(30)))
                {
                    PrefabUtility.ApplyPrefabInstance(currentEditObj, InteractionMode.AutomatedAction);
                    OnExitPrefabEditor();
                }

                if (GUILayout.Button("Exit", GUILayout.Height(30)))
                {
                    OnExitPrefabEditor();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }


        private void OnExitPrefabEditor()
        {
            DestroyImmediate(currentEditObj);
            currentCollider = null;
            currentEditObj = null;
            scaleValue = 0;
            realScaleValue = 0;
            tolerance = 0;
            realTolerance = 0;
            originalPaths.Clear();
            isEditor = false;
            GC.Collect();
        }

        private void OnEnterPrefabEditor(GameObject gameObject)
        {
            scaleValue = 0;
            realScaleValue = 0;
            tolerance = 0;
            realTolerance = 0;
            currentEditObj = gameObject;
            currentCollider = gameObject.GetComponent<PolygonCollider2D>();
            isEditor = true;
        }

        private void ScaleColliderLogic()
        {
            for (int index = 0; index < optimizedPoints.Count; index++)
            {
                var orialPoints = optimizedPoints[index];
                int length = orialPoints.Count;
                //Debug.Log($"长度 {length}");
                Vector2[] _tempVector2s = new Vector2[length];
                for (int i = 0; i < length; i++)
                {
                    //拿到第一个向量
                    Vector2 prePoint = i == 0 ? orialPoints[length - 1] : orialPoints[i - 1];
                    //拿到第二个向量
                    Vector2 nextPoint = i == length - 1 ? orialPoints[0] : orialPoints[i + 1];

                    _tempVector2s[i] =
                        PolygonOffsetUtility.GetOffsetPoint(orialPoints[i], prePoint, nextPoint, realScaleValue / 1000);
                }

                currentCollider.SetPath(index, _tempVector2s);
            }
        }

        private void PolygonColliderEditLogic()
        {
            //Reset the original paths
            if (currentEditObj == null || currentCollider == null || originalPaths.Count <= 0 ||
                originalPaths == null)
            {
                Debug.Log("无碰撞体原始数据");
                return;
            }

            optimizedPoints.Clear();

            if (tolerance <= 0)
            {
                for (int i = 0; i < originalPaths.Count; i++)
                {
                    List<Vector2> path = originalPaths[i];
                    optimizedPoints.Add(path);
                }
            }
            else
            {
                for (int i = 0; i < originalPaths.Count; i++)
                {
                    List<Vector2> path = originalPaths[i];
                    path = ShapeOptimizationHelper.DouglasPeuckerReduction(path, realTolerance);
                    optimizedPoints.Add(path);
                }
            }

            ScaleColliderLogic();
        }


        private void ClearObjs()
        {
            DestroyImmediate(currentEditObj);
            target = null;
            originalPaths.Clear();
        }

        private void GetObjFromSeleted()
        {
            Object[] seleteds = Selection.objects;

            int index = 0;
            target = new GameObject[seleteds.Length];
            foreach (var VARIABLE in seleteds)
            {
                if (VARIABLE is GameObject)
                {
                    target[index++] = VARIABLE as GameObject;
                }
            }
        }

        private void CreateSelectedObj()
        {
            if (target == null)
            {
                return;
            }

            if (target.Length == 0 || target == null)
            {
                return;
            }

            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] != null)
                {
                    CreateSelectedItem(target[i], i, IsInstanceOfPrefab(currentEditObj, target[i]));
                }
            }
        }

        public bool IsInstanceOfPrefab(GameObject instance, GameObject prefab)
        {
            // 获取实例对应的预制体
            if (instance == null || prefab == null)
            {
                return false;
            }

            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance) as GameObject;
            // 比较预制体引用
            return sourcePrefab == prefab;
        }

        private void CreateSelectedItem(GameObject _target, int index, bool isSeleted = false)
        {
            if (isSeleted)
            {
                GUILayoutUtility.GetRect(0, position.x, 5, 5);
            }

            GUILayout.BeginHorizontal(GUILayout.Height(30));

            EditorGUILayout.ObjectField(_target.gameObject.name, _target, typeof(GameObject), true,
                GUILayout.Height(30));
            if (GUILayout.Button("Edit", GUILayout.Height(30)))
            {
                Debug.Log(_target.gameObject.name);
                if (currentCollider || currentEditObj)
                {
                    OnExitPrefabEditor();
                }

                isSeleted = true;
                FocusPreab(_target);
            }

            if (!isEditor)
            {
                if (GUILayout.Button("Delete", GUILayout.Height(30)))
                {
                    target[index] = null;
                }
            }

            GUILayout.EndHorizontal();
            if (isSeleted)
            {
                Rect rightPanelRect1 = GUILayoutUtility.GetRect(0, position.x, 2, 2);
                GUI.DrawTexture(rightPanelRect1, backgroundTexture);
                GUILayoutUtility.GetRect(0, position.x, 3, 3);
            }
        }

        private void FocusPreab(GameObject gameObject)
        {
            GameObject obj = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;
            Selection.activeObject = obj;
            OnEnterPrefabEditor(obj);

            InitCurrentDate(obj);
        }

        private void InitCurrentDate(GameObject target)
        {
            if (currentCollider == null)
            {
                Debug.Log("没有获取到碰撞体");
                return;
            }

            for (int i = 0; i < currentCollider.pathCount; i++)
            {
                List<Vector2> path = new List<Vector2>(currentCollider.GetPath(i));
                originalPaths.Add(path);
            }

            if (target == null)
            {
                Debug.Log("未检测到碰撞体");
                return;
            }
        }
    }
}