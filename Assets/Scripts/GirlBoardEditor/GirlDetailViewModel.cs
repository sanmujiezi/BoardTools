using System;
using System.Collections.Generic;
using System.IO;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

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

        private GirlData girlData;

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

        public override void OnDisable()
        {
            base.OnDisable();
            var _parent = parent as ISelectedGirl;
            if (_parent != null)
            {
                _parent.RemoveListener(RefreshView);
            }
        }

        public void RefreshView(GirlInfo girlInfo = null)
        {
            if (girlInfo == null)
            {
                return;
            }

            DebugLogger.Instance.Log(this, $"RefreshView {girlInfo.id}");

            if (girlData != null)
            {
                girlData.ClearData();
            }

            girlData = LoadGirlDataByInfo(girlInfo);
            girlTitle.text = girlInfo.id;
        }

        private GirlData LoadGirlDataByInfo(GirlInfo info)
        {
            GirlData girlData = new GirlData();

            var girlImagesPath = info.path + "/" + PathDefine.GirlImagePath;
            var boardImagesPath = info.path + "/" + PathDefine.GirlBoardImagePath;
            var chatImagsPath = info.path + "/" + PathDefine.GirlChatImagePath;
            var boardPrefabsPath = info.path + "/" + PathDefine.GirlBoardPrefabPath;

            if (Directory.Exists(girlImagesPath + "/"))
            {
                girlImage.value = AssetDatabase.LoadAssetAtPath<Object>(girlImagesPath);
            }
            else
            {
                DebugLogger.Instance.LogError(this, $"Directory \"{girlImagesPath}\" not exist");
            }

            if (Directory.Exists(boardImagesPath + "/"))
            {
                boardImage.value = AssetDatabase.LoadAssetAtPath<Object>(boardImagesPath);
            }
            else
            {
                DebugLogger.Instance.LogError(this, $"Directory \"{boardImagesPath}\" not exist");
            }

            if (Directory.Exists(chatImagsPath + "/"))
            {
                chatImage.value = AssetDatabase.LoadAssetAtPath<Object>(chatImagsPath);
            }
            else
            {
                DebugLogger.Instance.LogError(this, $"Directory \"{chatImagsPath}\" not exist");
            }

            if (Directory.Exists(boardPrefabsPath + "/"))
            {
                boardPrefab.value = AssetDatabase.LoadAssetAtPath<Object>(boardPrefabsPath);
            }
            else
            {
                DebugLogger.Instance.LogError(this, $"Directory \"{boardPrefabsPath}\" not exist");
            }
            
            girlImage.SetEnabled(false);
            boardImage.SetEnabled(false);
            chatImage.SetEnabled(false);
            boardPrefab.SetEnabled(false);
            
            var girlImages = new ImageLoader(girlImagesPath + "/");
            var boardImages = new ImageLoader(boardImagesPath + "/");
            var chatImages = new ImageLoader(chatImagsPath + "/");
            var boardPrefabs = new PrefabsLoader(boardPrefabsPath + "/");

            girlData.id = info.id;
            girlData.girlImage = girlImages.images;
            girlData.boardImage = boardImages.images;
            girlData.chatImage = chatImages.images;
            girlData.boardPrefab = boardPrefabs.prefabs;

            return girlData;
        }
    }
}