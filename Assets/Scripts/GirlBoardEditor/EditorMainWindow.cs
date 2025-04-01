using System;
using System.Collections.Generic;
using GirlBoardEditor;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using GirlBoardEditor.UICompontent;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class EditorMainWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;
    private TemplateContainer labelFromUXML;
    private List<GirlInfoModel> girlListItem = new();

    private VisualElement root;
    private EditorViewModel editorViewModel;

    [MenuItem("工具/UI Toolkit/UIToolkitTest", false, 0)]
    public static void ShowExample()
    {
        EditorMainWindow wnd = GetWindow<EditorMainWindow>();
        wnd.titleContent = new GUIContent("UIToolkitTest");
    }

    public void CreateGUI()
    {
        var _rootVisualTreeAsset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathDefine.EditorMainUxml);
        rootVisualElement.styleSheets.Add(
            AssetDatabase.LoadAssetAtPath<StyleSheet>(PathDefine.EditorMainUss));
        root = _rootVisualTreeAsset.Instantiate();
        rootVisualElement.Add(root);
        root.style.flexGrow = 1;

        editorViewModel = new EditorViewModel(root,null);
    }

    public void OnEnable()
    {
        GlobalEventer.Instance.OnEnable();
    }

    public void OnDisable()
    {
        GlobalEventer.Instance.OnDisable();
    }
    
}