using System.Collections.Generic;
using System.Linq;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class DetailListItemPrefabViewModel : BaseViewModel
    {
        private List<GameObject> m_prefabs;

        public DetailListItemPrefabViewModel(VisualElement mRoot, BaseModel model, List<GameObject> mPrefabs) : base(
            mRoot, model)
        {
            m_prefabs = mPrefabs;
        }

        public VisualElement MakeItem()
        {
            var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.DetailListItemPrefabViewUxml);
            return item.Instantiate();
        }

        public void BindItem(VisualElement vsRoot,int index)
        {
            var prefabImage = vsRoot.Q<VisualElement>("prefabImage");
            var prefabName = vsRoot.Q<Label>("prefabName");
            
            prefabImage.style.backgroundImage = new StyleBackground(m_prefabs[index].transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
            prefabName.text = m_prefabs[index].name;
            
        }

        public List<GameObject> ItemsResource()
        {
            return m_prefabs;
        }

        public void SelectedChanged(IEnumerable<object> obj)
        {
            foreach (var VARIABLE in obj)
            {
                var obj1 = VARIABLE as GameObject;
                if (obj1!=null)
                {
                    DebugLogger.Instance.Log(this,$"Selectec gameobject is {obj1.name}");
                }
            }
        }
    }
}