using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GirlBoardCreater
{
    public partial class GirlBoardCreater : EditorWindow
    {
        private float _leftPanelWidth = 300f; // 左侧栏的初始宽度
        private bool _isResizing; // 是否正在调整宽度
        private float _resizeHandleWidth = 5f; // 调整宽度的手柄宽度
        private Texture2D _backgroundTexture;
        private float _minMaxWidth = 300f;

        private string _grilId;
        private string _mainTransformName = "Main";
        private string _hightlightTransformName = "HightLight";
        private string _shadowTransformName = "Shadow";
        private List<Texture2D> _texturesList;
        private List<Texture2D> _texturesList_Fg;

        private Object _createPrefabObj;
        private Object _lastPrefabObj;
        private Object _resourcesObj;
        private string _resourcesPath;
        private string _createPrefabPath;

        GUIStyle _headLabelStyle;

        [MenuItem("工具/CreateGirlBoard")]
        private new static void Show()
        {
            EditorWindow window = GetWindow<GirlBoardCreater>("DirlBoardCreater");
            window.minSize = new Vector2(600, 600);
            window.Show();
        }

        private void OnDestroy()
        {
            _texturesList.Clear();
            _texturesList_Fg.Clear();
            OnGirlResourceChanged -= LoadTextrueByPath;
        }

        private void OnEnable()
        {
            Init();

            // 创建纯黑色的背景纹理
            string hexColor = "#262626";
            Color color;
            _backgroundTexture = new Texture2D(1, 1);
            if (ColorUtility.TryParseHtmlString(hexColor, out color))
            {
                _backgroundTexture.SetPixel(0, 0, color);
                _backgroundTexture.Apply();
            }
        }

        private void Init()
        {
            _mainTransformName = "Main";
            _hightlightTransformName = "Highlight";
            _shadowTransformName = "Shadow";

            _headLabelStyle = new GUIStyle();
            _headLabelStyle.fontStyle = FontStyle.Bold;
            _headLabelStyle.fontSize = 16;
            _headLabelStyle.normal.textColor = Color.white;
            _headLabelStyle.alignment = TextAnchor.MiddleCenter;

            _texturesList = new List<Texture2D>();
            _texturesList_Fg = new List<Texture2D>();

            SetBoardTemplate();

            OnGirlResourceChanged += LoadTextrueByPath;
        }

        private void OnGUI()
        {
            // 开始水平布局
            EditorGUILayout.BeginHorizontal();

            // 左侧栏
            DrawLeftPanel();

            // 绘制调整宽度的手柄
            DrawResizeHandle();

            // 右侧栏
            DrawRightPanel();

            // 结束水平布局
            EditorGUILayout.EndHorizontal();
        }


        private void DrawResizeHandle()
        {
            // 定义调整宽度的手柄区域
            Rect resizeHandleRect = new Rect(_leftPanelWidth, 0, _resizeHandleWidth, position.height);
            GUI.Box(resizeHandleRect, ""); // 绘制手柄
            EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal); // 设置光标样式

            // 处理鼠标事件
            HandleResize(resizeHandleRect);
        }

        private void HandleResize(Rect resizeHandleRect)
        {
            // 鼠标按下时开始调整宽度
            if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
            {
                _isResizing = true;
            }

            // 如果正在调整宽度
            if (_isResizing)
            {
                // 根据鼠标位置更新左侧栏宽度
                _leftPanelWidth = Event.current.mousePosition.x;
                _leftPanelWidth = Mathf.Clamp(_leftPanelWidth, _minMaxWidth, position.width - _minMaxWidth); // 限制最小和最大宽度
                Repaint(); // 刷新界面
            }

            // 鼠标松开时停止调整宽度
            if (Event.current.type == EventType.MouseUp)
            {
                _isResizing = false;
            }
        }
    }
}