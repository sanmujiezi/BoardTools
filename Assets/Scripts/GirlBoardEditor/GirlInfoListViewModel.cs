using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public class GirlInfoListViewModel : BaseViewModel
    {
        private BaseViewModel parent;

        private List<GirlInfoModel> m_girlLists = new();
        public List<GirlInfoModel> MGirlLists => m_girlLists;

        private Dictionary<string, Texture2D> m_girlCommonImage = new();
        public Dictionary<string, Texture2D> MGirlCommonImage => m_girlCommonImage;

        public GirlInfoListViewModel(VisualElement mRoot, BaseViewModel parent, BaseModel model) : base(mRoot, model)
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
                    string fileName = Path.GetFileNameWithoutExtension(girlPathItem);

                    if (fileName.Equals(PathDefine.GirlCommonImagePath))
                    {
                        if (m_girlLists.Count > 0)
                        {
                            m_girlLists.Clear();
                        }

                        var commonPath = girlPathItem + "/";
                        foreach (var itemPath in Directory.GetFiles(commonPath))
                        {
                            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(itemPath);
                            if (image && !m_girlCommonImage.ContainsKey(image.name))
                            {
                                m_girlCommonImage.Add(image.name, image);
                            }
                        }

                        continue;
                    }

                    if (!fileName.Contains("Girl"))
                    {
                        continue;
                    }

                    var girlInfo = new GirlInfoModel();
                    girlInfo.id = fileName;
                    girlInfo.path = girlPathItem;
                    girlInfo.describe = "This is " + girlInfo.id;
                    
                    
                    
                    //TODO:得到编号
                    var match = Regex.Match(fileName, @"\d+");
                    int index = 1;
                    try
                    {
                        index = int.Parse(match.Groups[0].Value);
                    }
                    catch (FormatException e)
                    {
                        DebugLogger.Instance.LogError(this, $"GirlID convert fialed \"{fileName}\"");
                        throw;
                    }

                    var halfImageName = "girl_half_" + index + "_1";
                    var headImageName = "girl_icon_" + index;

                    if (m_girlCommonImage.ContainsKey(halfImageName))
                    {
                        girlInfo.halfImage = m_girlCommonImage[halfImageName];
                    }

                    if (m_girlCommonImage.ContainsKey(headImageName))
                    {
                        girlInfo.headImage = m_girlCommonImage[headImageName];
                    }

                    m_girlLists.Add(girlInfo);
                }
            }

            DebugLogger.Instance.Log(this, "Load Grils was Finshed!");
        }

        private void BindingGirlList()
        {
            var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.EditorGirlItemUxml);
            var girlList = m_root.Q<ListView>("content_list");

            if (girlList == null)
            {
                Debug.Log("列表不存在");
                return;
            }

            if (item == null)
            {
                Debug.Log("列表元素为空");
            }

            if (m_girlLists != null)
            {
                m_girlLists.Clear();
            }

            LoadGrils();

            Func<VisualElement> makeItem = () => item.Instantiate();
            Action<VisualElement, int> bindItem = (item, index) => BindingGirlList(item, m_girlLists[index]);

            girlList.fixedItemHeight = 100;

            girlList.makeItem = makeItem;
            girlList.bindItem = bindItem;
            girlList.itemsSource = m_girlLists;
            girlList.selectionType = SelectionType.Single;

            girlList.selectionChanged += (e) =>
            {
                foreach (var VARIABLE in e)
                {
                    if (VARIABLE is GirlInfoModel girlInfo)
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

        private void BindingGirlList(VisualElement item, GirlInfoModel infoModel)
        {
            var image = item.Q<VisualElement>("headImage");
            var girlID = item.Q<Label>("girlID");
            var describe = item.Q<Label>("describe");

            image.style.backgroundImage = new StyleBackground(infoModel.halfImage);
            //image.style.backgroundImage.value.sprite.
            girlID.text = infoModel.id;
            describe.text = infoModel.describe;
        }
    }
}