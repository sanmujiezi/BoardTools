using System.Collections.Generic;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace GirlBoardEditor.UICompontent
{
    public class EditorViewModel : BaseViewModel
    {
        private VisualElement root;

        private GirlDetailViewModel girlDetail;
        private GirlInfoListViewModel girlList;
        
        private List<GirlInfo> _girlInfos = new ();
        public List<GirlInfo> GirlInfos => _girlInfos;

        public EditorViewModel(VisualElement root) : base(root)
        {
            girlDetail = new GirlDetailViewModel(root);
            girlList = new GirlInfoListViewModel(root);
        }

        
    }
}