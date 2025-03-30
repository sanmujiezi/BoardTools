using System.Collections.Generic;
using GirlBoardEditor.Config;
using GirlBoardEditor.Tools;
using UnityEngine;

namespace GirlBoardEditor.Tools
{
    public class BoardPrefabCreator
    {
        private static BoardPrefabCreator _instance = new BoardPrefabCreator();
        public static BoardPrefabCreator Instance => _instance;

        private List<GameObject> _prefabs = new();
        public List<GameObject> prefabs => _prefabs;
        
        private ToolConfig _toolConfig;
        public ToolConfig toolConfig => _toolConfig;
        
        private PrefabLoader _prefabLoader;
        private ConfigLoader _configLoader;
        private ImageLoader _imageLoader;
        public PrefabLoader prefabLoader => _prefabLoader;
        public ConfigLoader configLoader => _configLoader;
        public ImageLoader imageLoader => _imageLoader;

        private void InitlizeDependence()
        {
            _toolConfig = Resources.Load<ToolConfig>(PathDefine.ConfigPath);
            _prefabLoader = new PrefabLoader(_toolConfig.prefabPath);
            _configLoader = new ConfigLoader(_toolConfig.configPath);
        }

        public void CreatePrefab()
        {
            
            //_prefabLoader = new PrefabLoader();
        }

        public void CreatePrefab(string path)
        {
        }
        
        public void DeletePrefab(GameObject gameObject)
        {
        }

        public void DeleteAllPrefab() => _prefabs.Clear();
    }
}