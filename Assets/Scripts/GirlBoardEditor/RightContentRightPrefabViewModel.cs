using System;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class RightContentRightPrefabViewModel : BaseViewModel
    {
        private float _zoomValue;
        private float _simplifyValue;
        private float _offsetX;
        private float _offsetY;
        private string _detailState;
        private GameObject _editorPrefab;

        private float m_zoomValue
        {
            get { return _zoomValue; }
            set
            {
                if (value > maxZoomValue)
                {
                    value = maxZoomValue;
                }
                else if (value < minZoomValue)
                {
                    value = minZoomValue;
                }

                _zoomValue = value;
                zoomSlider.value = _zoomValue;
                zoomValue.value = _zoomValue;
                DebugLogger.Instance.Log(this, $"Changed Zoom Value is {value}");
            }
        }

        private float m_simplifyValue
        {
            get { return m_simplifyValue; }
            set
            {
                if (value > maxSimplifyValue)
                {
                    value = maxSimplifyValue;
                }
                else if (value < minSimplifyValue)
                {
                    value = minSimplifyValue;
                }

                _simplifyValue = value;
                simplifySlider.value = _simplifyValue;
                simplifyValue.value = _simplifyValue;
                DebugLogger.Instance.Log(this, $"Changed simplify Value is {value}");
            }
        }


        private float m_offsetX
        {
            get { return _offsetX; }
            set
            {
                _offsetX = value;
                offsetValue.value = new Vector2(_offsetX, _offsetY);
                DebugLogger.Instance.Log(this, $"Changed offsetX Value is {value}");
            }
        }

        private float m_offsetY
        {
            get { return _offsetY; }
            set
            {
                _offsetY = value;
                offsetValue.value = new Vector2(_offsetX, _offsetY);
                DebugLogger.Instance.Log(this, $"Changed offsetY Value is {value}");
            }
        }

        public string detailState
        {
            get { return _detailState; }
            set
            {
                _detailState = value;
                RefreshView();
                Debug.Log(value);
            }
        }
        
        public GameObject editPrefab
        {
            get { return _editorPrefab; }
            set
            {
                _editorPrefab = value; 
            }
        }

        private float maxZoomValue = 1f;
        private float minZoomValue = -1f;

        private float maxSimplifyValue = 1f;
        private float minSimplifyValue = -1f;

        private FloatField zoomValue;
        private Slider zoomSlider;
        private FloatField simplifyValue;
        private Slider simplifySlider;
        private Vector2Field offsetValue;

        private Button saveBtn;
        private Button exitBtn;
        private Button editBtn;
        private Button createPrefabBtn;
        private VisualElement boardEditorGroup;
        private VisualElement saveButtonGroup;
        private VisualElement editButtonGroup;


        public RightContentRightPrefabViewModel(VisualElement mRoot, BaseModel model) : base(mRoot, model)
        {
            BindingCompontent();
        }

        public void BindingCompontent()
        {
            var root = m_root.Q("RightContentRightPrefab");
            zoomValue = root.Q<FloatField>("zoomValue");
            zoomSlider = root.Q<Slider>("zoomSlider");
            simplifyValue = root.Q<FloatField>("simplifyValue");
            simplifySlider = root.Q<Slider>("simplifySlider");
            offsetValue = root.Q<Vector2Field>("offsetValue");

            saveBtn = root.Q<Button>("saveBtn");
            exitBtn = root.Q<Button>("exitBtn");
            editBtn = root.Q<Button>("editBtn");
            createPrefabBtn = root.Q<Button>("createPrefab");
            editButtonGroup = root.Q<VisualElement>("editButtonGroup");
            saveButtonGroup = root.Q<VisualElement>("saveButtonGroup");
            boardEditorGroup = root.Q<VisualElement>("boardEditorGroup");

            saveBtn.clicked += OnClickSaveBtn;
            exitBtn.clicked += OnClickExitBtn;
            editBtn.clicked += OnClickEditBtn;

            zoomValue.RegisterValueChangedCallback(OnChangeZoomValue);
            zoomSlider.RegisterValueChangedCallback(OnChangeZoomValue);
            simplifyValue.RegisterValueChangedCallback(OnChangeSimplifyValue);
            simplifySlider.RegisterValueChangedCallback(OnChangeSimplifyValue);
            offsetValue.RegisterValueChangedCallback(OnChangeOffsetValue);


            zoomSlider.highValue = maxZoomValue;
            zoomSlider.lowValue = minZoomValue;
            simplifySlider.highValue = maxSimplifyValue;
            simplifySlider.lowValue = minSimplifyValue;
        }

        private void RefreshView()
        {
            switch (Enum.Parse(typeof(DetailState), detailState))
            {
                case DetailState.EditBoard:
                    if (editPrefab)
                    {
                        SetBoardEditorGroupActive(true);
                        SetCreatePrefabButtonActive(false);
                        SetEditButtonGroupActive(false);
                        SetCreatePrefabButtonActive(false);
                    }
                    else if (m_Model is GirlDataModel girlDataModel
                             && girlDataModel.boardPrefab.Count <= 0)
                    {
                        SetBoardEditorGroupActive(false);
                        SetCreatePrefabButtonActive(true);
                    }

                    break;
                case DetailState.ChatImage:
                case DetailState.CompairBoard:
                case DetailState.GirlImage:
                    SetBoardEditorGroupActive(false);
                    SetCreatePrefabButtonActive(false);
                    break;
            }

            InitValue();
        }

        private void InitValue()
        {
            zoomValue.value = 0;
            zoomSlider.value = 0;
            simplifyValue.value = 0;
            simplifySlider.value = 0;
            offsetValue.value = Vector2.zero;
            m_zoomValue = 0;
            m_simplifyValue = 0;
            m_offsetX = 0;
            m_offsetY = 0;
        }

        private void SetSaveButtonGroupActive(bool isActive)
        {
            DisplayStyle displayStyle = DisplayStyle.Flex;
            if (!isActive)
            {
                displayStyle = DisplayStyle.None;
            }

            saveButtonGroup.style.display = new StyleEnum<DisplayStyle>(displayStyle);
        }

        private void SetEditButtonGroupActive(bool isActive)
        {
            DisplayStyle displayStyle = DisplayStyle.Flex;
            if (!isActive)
            {
                displayStyle = DisplayStyle.None;
            }

            editButtonGroup.style.display = new StyleEnum<DisplayStyle>(displayStyle);
        }

        private void SetBoardEditorGroupActive(bool isActive)
        {
            DisplayStyle displayStyle = DisplayStyle.Flex;
            if (!isActive)
            {
                displayStyle = DisplayStyle.None;
            }

            boardEditorGroup.style.display = new StyleEnum<DisplayStyle>(displayStyle);
        }

        private void SetCreatePrefabButtonActive(bool isActive)
        {
            DisplayStyle displayStyle = DisplayStyle.Flex;
            if (!isActive)
            {
                displayStyle = DisplayStyle.None;
            }

            createPrefabBtn.style.display = new StyleEnum<DisplayStyle>(displayStyle);
        }

        private void OnClickEditBtn()
        {
            SetSaveButtonGroupActive(true);
            SetEditButtonGroupActive(false);
        }

        private void OnClickExitBtn()
        {
            SetSaveButtonGroupActive(false);
            SetEditButtonGroupActive(true);
        }

        private void OnClickSaveBtn()
        {
            DebugLogger.Instance.Log(this, $"Saved prefab {editPrefab}");
        }

        private void OnChangeOffsetValue(ChangeEvent<Vector2> evt)
        {
            m_offsetX = evt.newValue.x;
            m_offsetY = evt.newValue.y;
        }

        private void OnChangeZoomValue(ChangeEvent<float> evt)
        {
            m_zoomValue = evt.newValue;
        }

        private void OnChangeSimplifyValue(ChangeEvent<float> evt)
        {
            m_simplifyValue = evt.newValue;
        }

        public float GetZoomValue()
        {
            return m_zoomValue;
        }

        public float GetSimplifyValue()
        {
            return m_simplifyValue;
        }

        public void RefreshSkinView()
        {
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}