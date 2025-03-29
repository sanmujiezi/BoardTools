using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class UIToolkitTest : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;
    private TemplateContainer labelFromUXML;
    private List<GirlInfo> girlListItem = new();

    private float screenWidth;
    private float screenHeight;

    private VisualElement root;

    [MenuItem("工具/UI Toolkit/UIToolkitTest", false, 0)]
    public static void ShowExample()
    {
        UIToolkitTest wnd = GetWindow<UIToolkitTest>();
        wnd.titleContent = new GUIContent("UIToolkitTest");
    }

    public void CreateGUI()
    {
        var _rootVisualTreeAsset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/UIToolkitTest/UI/UIToolkitTest.uxml");
        rootVisualElement.styleSheets.Add(
            AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Scripts/UIToolkitTest/Uss/UIToolkitTest.uss"));
        root = _rootVisualTreeAsset.Instantiate();
        rootVisualElement.Add(root);
        root.style.flexGrow = 1;

        CreateGirlList();
    }

    private void CreateGirlList()
    {
        var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/Scripts/UIToolkitTest/UI/LeftContentListItem.uxml");
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
            girlListItem.Add(resourceItem) ;
        }

        Func<VisualElement> makeItem = () => item.Instantiate();
        Action<VisualElement, int> bindItem = (item, index) => BindingGirlList(item, girlListItem[index]);
        
        girlList.fixedItemHeight = 100;
        
        girlList.makeItem = makeItem;
        girlList.bindItem = bindItem;
        girlList.itemsSource = girlListItem;
        girlList.selectionType = SelectionType.Single;

        girlList.selectionChanged += (e) =>
        {
            var list = e as List<Object>;
            foreach (var VARIABLE in list)
            {
                if (VARIABLE is GirlInfo)
                {
                    Debug.Log((VARIABLE as GirlInfo).id);        
                }
                
            }
            
        };
        
        girlList.itemsChosen += (e) =>
        {
            var list = e as List<Object>;
            foreach (var VARIABLE in list)
            {
                if (VARIABLE is GirlInfo)
                {
                    Debug.Log((VARIABLE as GirlInfo).id);        
                }
                
            }
            
        };

    }



    private void BindingGirlList(VisualElement item,GirlInfo info)
    {
        var image = item.Q<VisualElement>("headImage");
        var girlID = item.Q<Label>("girlID");
        var describe = item.Q<Label>("describe");
        
        image.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Girl/Girl001/ChatImage/girl_chat_1.png"));
        girlID.text = info.id;
        describe.text = info.describe;
    }
    
}



public class GirlInfo
{
    public string id;
    public string describe;
}