using System;
using System.Collections.Generic;
using System.IO;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using GirlBoardEditor.UICompontent;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class GirlInfoListViewModel : BaseViewModel
    {
        private BaseViewModel parent;

        private List<GirlInfo> _girlLists = new();
        public List<GirlInfo> GirlLists => _girlLists;

        public GirlInfoListViewModel(VisualElement root, BaseViewModel parent) : base(root)
        {
            this.parent = parent;
            LoadGrils();
            BindingGirlList();
            
        }

        public void LoadGrils()
        {
            DebugLogger.Instance.Log(this, "in Loading Grils ...");

            var girlPath = PathDefine.GirlPath + "/";
            if (Directory.Exists(girlPath))
            {
                var girlPaths = Directory.GetDirectories(girlPath);
                foreach (var girlPathItem in girlPaths)
                {
                    var girlInfo = new GirlInfo();

                    girlInfo.id = Path.GetFileNameWithoutExtension(girlPathItem);
                    girlInfo.path = girlPathItem;
                    girlInfo.describe = "This is " + girlInfo.id;

                    var halfImagePath = girlPathItem + "/" + PathDefine.GirlChatImagePath + "/";

                    //TODO: 这里要修改头像的加载路径
                    var headImagePath = girlPathItem + "/" + PathDefine.GirlChatImagePath + "/";
                    if (Directory.Exists(halfImagePath))
                    {
                        halfImagePath = Directory.GetFiles(halfImagePath)[0];
                    }

                    if (Directory.Exists(headImagePath))
                    {
                        headImagePath = Directory.GetFiles(headImagePath)[0];
                    }

                    girlInfo.halfImage =
                        AssetDatabase.LoadAssetAtPath<Texture2D>(halfImagePath);
                    girlInfo.headImage =
                        AssetDatabase.LoadAssetAtPath<Texture2D>(headImagePath);
                    _girlLists.Add(girlInfo);
                }
            }

            DebugLogger.Instance.Log(this, "Load Grils was Finshed!");
        }

        private void BindingGirlList()
        {
            var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.EditorGirlItemUxml);
            var girlList = root.Q<ListView>("content_list");

            if (girlList == null)
            {
                Debug.Log("列表不存在");
                return;
            }

            if (item == null)
            {
                Debug.Log("列表元素为空");
            }

            // for (int i = 0; i < 10; i++)
            // {
            //     var resourceItem = new GirlInfo()
            //     {
            //         id = "Girl00" + i,
            //         describe = "This is girl00" + i
            //     };
            //     _girlListItem.Add(resourceItem);
            // }
            if (_girlLists != null)
            {
                _girlLists.Clear();
            }

            LoadGrils();

            Func<VisualElement> makeItem = () => item.Instantiate();
            Action<VisualElement, int> bindItem = (item, index) => BindingGirlList(item, _girlLists[index]);

            girlList.fixedItemHeight = 100;

            girlList.makeItem = makeItem;
            girlList.bindItem = bindItem;
            girlList.itemsSource = _girlLists;
            girlList.selectionType = SelectionType.Single;

            girlList.selectionChanged += (e) =>
            {
                foreach (var VARIABLE in e)
                {
                    if (VARIABLE is GirlInfo girlInfo)
                    {
                        //Debug.Log(girlInfo.id);
                        var _parent = parent as ISelectedGirl;
                        if (_parent != null)
                        {
                            _parent.OnSelectedGirl(girlInfo);
                        }
                    }
                }
            };
            
            girlList.SetSelection(0);
            
        }

        private void BindingGirlList(VisualElement item, GirlInfo info)
        {
            var image = item.Q<VisualElement>("headImage");
            var girlID = item.Q<Label>("girlID");
            var describe = item.Q<Label>("describe");

            image.style.backgroundImage = new StyleBackground(info.headImage);
            girlID.text = info.id;
            describe.text = info.describe;
        }
    }
}