using System;
using System.Collections.Generic;
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
            
        private List<GirlInfo> _girlListItem = new ();
        public List<GirlInfo> girlListItem => _girlListItem;
        public GirlInfoListViewModel(VisualElement root,BaseViewModel parent) : base(root)
        {
            this.parent = parent;   
            LoadGrils();
            BindingGirlList();
        }
        
        public void LoadGrils()
        {
            DebugLogger.Instance.Log(this,"in Loading Grils ...");
            
            DebugLogger.Instance.Log(this,"Load Grils was Finshed!");
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

            for (int i = 0; i < 10; i++)
            {
                var resourceItem = new GirlInfo()
                {
                    id = "Girl00" + i,
                    describe = "This is girl00" + i
                };
                _girlListItem.Add(resourceItem);
            }

            Func<VisualElement> makeItem = () => item.Instantiate();
            Action<VisualElement, int> bindItem = (item, index) => BindingGirlList(item, _girlListItem[index]);

            girlList.fixedItemHeight = 100;

            girlList.makeItem = makeItem;
            girlList.bindItem = bindItem;
            girlList.itemsSource = _girlListItem;
            girlList.selectionType = SelectionType.Single;

            girlList.selectionChanged += (e) =>
            {
                foreach (var VARIABLE in e)
                {
                    if (VARIABLE is GirlInfo girlInfo)
                    {
                        Debug.Log(girlInfo.id);
                        var _parent = parent as ISelectedGirl;
                        if (_parent!=null)
                        {
                            _parent.OnSelectedGirl(girlInfo);
                        }
                    }
                }
            };
        }

        private void BindingGirlList(VisualElement item, GirlInfo info)
        {
            var image = item.Q<VisualElement>("headImage");
            var girlID = item.Q<Label>("girlID");
            var describe = item.Q<Label>("describe");

            image.style.backgroundImage =
                new StyleBackground(
                    AssetDatabase.LoadAssetAtPath<Texture2D>(
                        "Assets/Resources/Girl/Girl001/ChatImage/girl_chat_1.png"));
            girlID.text = info.id;
            describe.text = info.describe;
        }

    }
}