using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        EditBoard,
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
        private ListView listView;

        new private GirlDataModel m_Model;
        private RightContentRightPrefabViewModel prefabViewModel;

        public GirlDetailViewModel(VisualElement mRoot, BaseViewModel parent,BaseModel model) : base(mRoot,model)
        {
            this.parent = parent;
            prefabViewModel = new RightContentRightPrefabViewModel(m_root,m_Model);
            BindingData();
            InitCompontent();
            RefreshView();
        }

        private void BindingData()
        {
            detailView = m_root.Q<VisualElement>("right_content_right");
            girlTitle = m_root.Q<Label>("right_girlId");
            boardImage = m_root.Q<ObjectField>("right_boardImage_field");
            boardPrefab = m_root.Q<ObjectField>("right_boardPrefab_field");
            chatImage = m_root.Q<ObjectField>("right_girlImage_field");
            girlImage = m_root.Q<ObjectField>("right_chatImage_field");
            selectedState = m_root.Q<DropdownField>("right_content_left_detailState");
            listView = m_root.Q<ListView>("right_content_left_list");
        }

        private void InitCompontent()
        {
            
            var choises = new List<string>()
            {
                DetailState.ChatImage.ToString(),
                DetailState.GirlImage.ToString(),
                DetailState.EditBoard.ToString(),
                DetailState.CompairBoard.ToString()
            };
            selectedState.choices = choises;
            selectedState.value = choises[0];

            selectedState.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                DebugLogger.Instance.Log(this, $"Enum change {evt.newValue}");
                selectedState.value = evt.newValue;
                
                prefabViewModel.detailState = evt.newValue;
                
                RefreshChildView();
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

        public void RefreshView(GirlInfoModel girlInfoModel = null)
        {
            if (girlInfoModel == null)
            {
                return;
            }

            DebugLogger.Instance.Log(this, $"RefreshView {girlInfoModel.id}");

            if (m_Model != null)
            {
                m_Model.ClearData();
            }

            m_Model = LoadGirlDataByInfo(girlInfoModel.path);
            m_Model.id = girlInfoModel.id;
            girlTitle.text = girlInfoModel.id;
            
            RefreshChildView();
        }

        public void RefreshChildView()
        {
            listView.Clear();
            listView.makeItem = null;
            listView.bindItem = null;
            listView.itemsSource = null;
            listView.selectionType = SelectionType.Single;
            listView.fixedItemHeight = 100;

            switch (Enum.Parse(typeof(DetailState), selectedState.value) )
            {
                case DetailState.CompairBoard:
                    var _item1 = new DetailListItemCompairViewModel(m_root, null,m_Model.boardImage);
                    listView.makeItem = _item1.MakeItem;
                    listView.bindItem = _item1.BindItem;
                    listView.itemsSource = _item1.ItemsResource();
                    listView.selectionChanged += _item1.SelectedChanged;

                    break;
                case DetailState.EditBoard:
                    var _item2 = new DetailListItemPrefabViewModel(m_root, null,m_Model.boardPrefab);
                    listView.makeItem= _item2.MakeItem;
                    listView.bindItem= _item2.BindItem;
                    listView.itemsSource = _item2.ItemsResource();
                    listView.selectionChanged += _item2.SelectedChanged;
                    listView.selectionChanged += (e) =>
                    {
                        foreach (var VARIABLE in e)
                        {
                            if (VARIABLE is GameObject obj)
                            {
                                prefabViewModel.editPrefab = obj;
                            }
                        }
                        
                    };
                    
                    break;
                case DetailState.ChatImage:
                    var _item3 = new DetailListItemImageViewModel(m_root, null,m_Model.chatImage);
                    listView.makeItem= _item3.MakeItem;
                    listView.bindItem= _item3.BindItem;
                    listView.itemsSource = _item3.ItemsResource();
                    listView.selectionChanged += _item3.SelectedChanged;
                    
                    break;
                case DetailState.GirlImage:
                    var _item4 = new DetailListItemImageViewModel(m_root, null,m_Model.girlImage);
                    listView.makeItem= _item4.MakeItem;
                    listView.bindItem= _item4.BindItem;
                    listView.itemsSource = _item4.ItemsResource();
                    listView.selectionChanged += _item4.SelectedChanged;
                    
                    break;
            }
            listView.RefreshItems();
            listView.SetSelection(0);



        }

        private GirlDataModel LoadGirlDataByInfo(string path)
        {
            GirlDataModel girlDataModel = new GirlDataModel();

            var girlImagesPath = path + "/" + PathDefine.GirlImagePath;
            var boardImagesPath = path + "/" + PathDefine.GirlBoardImagePath;
            var chatImagsPath = path + "/" + PathDefine.GirlChatImagePath;
            var boardPrefabsPath = path + "/" + PathDefine.GirlBoardPrefabPath;

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
            
            girlDataModel.girlImage = girlImages.images;
            girlDataModel.boardImage = boardImages.images;
            girlDataModel.chatImage = chatImages.images;
            girlDataModel.boardPrefab = boardPrefabs.prefabs;

            return girlDataModel;
        }
        
        
    }
}