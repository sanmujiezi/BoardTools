using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GirlBoardEditor.Model
{
    public abstract class BaseModel{}
    public class GirlInfoModel : BaseModel
    {
        public string id;
        public string describe;
        public string path;
        public Texture2D headImage;
        public Texture2D halfImage;
    }

    public class GirlDataModel : BaseModel 
    {
        public string id;
        public List<Texture2D> chatImage;
        public List<Texture2D> girlImage;
        public List<Texture2D> boardImage;
        public List<GameObject>boardPrefab;

        public void ClearData()
        {
            chatImage.Clear();
            girlImage.Clear();
            boardImage.Clear();
            boardPrefab.Clear();
        }
    }
}