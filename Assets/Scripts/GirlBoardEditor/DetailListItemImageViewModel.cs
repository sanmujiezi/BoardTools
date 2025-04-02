using System.Collections.Generic;
using System.Linq;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class DetailListItemImageViewModel : BaseViewModel
    {
        private List<Texture2D> m_images;

        public DetailListItemImageViewModel(VisualElement mRoot, BaseModel model, List<Texture2D> images) : base(
            mRoot, model)
        {
            m_images = images;
        }

        public VisualElement MakeItem()
        {
            var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.DetailListItemImageViewUxml);
            return item.Instantiate();
        }

        public void BindItem(VisualElement vsRoot, int index)
        {
            if (index >= m_images.Count)
            {
                return;
            }

            var image = vsRoot.Q<VisualElement>("image");
            var mainImageText = vsRoot.Q<Label>("name");

            image.style.backgroundImage = new StyleBackground(m_images[index]);
            mainImageText.text = m_images[index].name;
        }

        public List<Texture2D> ItemsResource()
        {
            return m_images;
        }

        public void SelectedChanged(IEnumerable<object> obj)
        {
            foreach (var VARIABLE in obj)
            {
                var texture2D = VARIABLE as Texture2D;
                if (texture2D!=null)
                {
                    DebugLogger.Instance.Log(this,$"Seleted Texture is {texture2D.name}");
                }
            }
        }
    }
}