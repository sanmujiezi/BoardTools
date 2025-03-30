using System.Collections.Generic;
using System.IO;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;


namespace GirlBoardEditor.Tools
{
    public class ImageLoader : BaseLoader
    {
        public List<Texture2D> images ;
        public ImageLoader(string path) : base(path)
        {
        }

        public override void LoadResourceFromPath(string path)
        {
            images = new List<Texture2D>();
            if (!Directory.Exists(path))
            {
                DebugLogger.Instance.Log(this,$"Directory \"{path}\" not exist");
                return;
            }
            
            var files = Directory.GetFiles(path);

            foreach (var VARIABLE in files)
            {
                var textrue = AssetDatabase.LoadAssetAtPath<Texture2D>(VARIABLE);
                if (textrue)
                {
                    images.Add(textrue);
                }
            }
            
        }
    }
}