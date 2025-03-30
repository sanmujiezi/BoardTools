using System.Diagnostics;
using System.IO;
using GirlBoardEditor.Tools;
using UnityEngine;
using Object = System.Object;

namespace GirlBoardEditor.Tools
{
    public abstract class BaseLoader : Object, ICanLoadResourceFromPath
    {
        private string loadPath;

        public BaseLoader(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                DebugLogger.Instance.Log(this, $"SetLoadPath \"{path}\" is null or empty");
                return;
            }
            
            SetLoadPath(path);
            LoadResourceFromPath(loadPath);
        }

        private void SetLoadPath(string path)
        {
            if (!string.IsNullOrEmpty(loadPath))
            {
                DebugLogger.Instance.Log(this, $"Path {loadPath} was set the path {path}");
            }

            loadPath = path;
        }

        public abstract void LoadResourceFromPath(string path);
    }

    public interface ICanLoadResourceFromPath
    {
        public void LoadResourceFromPath(string path);
    }
}