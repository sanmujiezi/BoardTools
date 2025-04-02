using System.Collections.Generic;
using System.Linq;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class DetailListItemCompairViewModel : BaseViewModel
    {
        private List<Texture2D> m_images;
        private List<Texture2D[]> m_imagePair;

        public DetailListItemCompairViewModel(VisualElement mRoot, BaseModel model, List<Texture2D> images) : base(
            mRoot, model)
        {
            m_images = images;
        }

        public VisualElement MakeItem()
        {
            var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.DetailListItemCompairViewUxml);
            return item.Instantiate();
        }

        public void BindItem(VisualElement vsRoot, int index)
        {
            var mainImage = vsRoot.Q<VisualElement>("mainBoard");
            var fgImage = vsRoot.Q<VisualElement>("fgBoard");
            var mainImageText = vsRoot.Q<Label>("mainBoardText");
            var fgImageText = vsRoot.Q<Label>("fgBoardText");

            mainImage.style.backgroundImage = new StyleBackground(m_imagePair[index][0]);
            fgImage.style.backgroundImage = new StyleBackground(m_imagePair[index][1]);
            mainImageText.text = m_imagePair[index][0].name;
            fgImageText.text = m_imagePair[index][1].name;
        }

        public List<Texture2D[]> ItemsResource()
        {
            var itemResource = new List<Texture2D[]>();
            int index = m_images.Count / 2;
            for (int i = 0; i < index; i++)
            {
                if (i + index >= m_images.Count)
                {
                    itemResource.Add(new Texture2D[] { m_images[i], m_images[m_images.Count - 1] });
                }
                else
                {
                    itemResource.Add(new Texture2D[] { m_images[i], m_images[i + index] });
                }
            }

            m_imagePair = itemResource;

            return itemResource;
        }

        public void SelectedChanged(IEnumerable<object> evt)
        {
            var info = evt as Texture2D[];
            if (info != null)
            {
                DebugLogger.Instance.Log(this,$"Main Board Image is {info[1]},FG Board Image is {info[0]}");
            }
            else
            {
                DebugLogger.Instance.LogError(this,$"Can not found the selected item");
            }
        }
    }
}