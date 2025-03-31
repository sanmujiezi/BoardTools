using System;
using System.Collections.Generic;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public enum DetailState
    {
        CompairBoard,
        EdiorBoard,
        ChatImage,
        GirlImage
    }

    public class GirlDetailViewModel : BaseViewModel
    {
        private BaseViewModel parent;

        private VisualElement detailView;
        private Label girlTitle;
        private ObjectField boardImage;
        private ObjectField boardPrefab;
        private ObjectField chatImage;
        private ObjectField girlImage;
        private DropdownField selectedState;
        private ListView list;

        public GirlDetailViewModel(VisualElement root, BaseViewModel parent) : base(root)
        {
            this.parent = parent;
            BindingData();
            InitCompontent();
            RefreshView();
        }

        private void BindingData()
        {
            detailView = root.Q<VisualElement>("right_content_right");
            girlTitle = root.Q<Label>("right_girlId");
            boardImage = root.Q<ObjectField>("right_boardImage_field");
            boardPrefab = root.Q<ObjectField>("right_boardPrefab_field");
            chatImage = root.Q<ObjectField>("right_girlImage_field");
            girlImage = root.Q<ObjectField>("right_chatImage_field");
            selectedState = root.Q<DropdownField>("right_content_left_detailState");
            list = root.Q<ListView>("right_content_left_list");
        }

        private void InitCompontent()
        {
            var choises = new List<string>()
            {
                DetailState.ChatImage.ToString(),
                DetailState.GirlImage.ToString(),
                DetailState.EdiorBoard.ToString(),
                DetailState.CompairBoard.ToString()
            };
            selectedState.choices = choises;
            selectedState.value = choises[0];

            selectedState.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                DebugLogger.Instance.Log(this, $"Enum change {evt.newValue}");
                selectedState.value = evt.newValue;
            });

            var _parent = parent as ISelectedGirl;
            if (_parent != null)
            {
                _parent.AddListener(RefreshView);
            }
        }

        public void RefreshView(GirlInfo girlInfo = null)
        {
            if (girlInfo ==null)
            {
                return;
            }
            DebugLogger.Instance.Log(this, $"RefreshView {girlInfo.id}");
        }
    }
}