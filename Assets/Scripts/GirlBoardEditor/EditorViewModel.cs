using System.Collections.Generic;
using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public delegate void DelegateGirlChanged(GirlInfo girlInfo);
    public interface ISelectedGirl
    {
        void OnSelectedGirl(GirlInfo girlInfo);
        void AddListener(DelegateGirlChanged action);
        void RemoveListener(DelegateGirlChanged action);
    }
    
    public class EditorViewModel : BaseViewModel,ISelectedGirl
    {
        public event DelegateGirlChanged OnSelectGirl; 
        private VisualElement root;

        private GirlDetailViewModel girlDetail;
        private GirlInfoListViewModel girlList;
        
        private List<GirlInfo> _girlInfos = new ();
        public List<GirlInfo> GirlInfos => _girlInfos;
        
        public EditorViewModel(VisualElement root) : base(root)
        {
            girlDetail = new GirlDetailViewModel(root,this);
            girlList = new GirlInfoListViewModel(root,this);
        }


        public void OnSelectedGirl(GirlInfo girlInfo)
        {
            OnSelectGirl?.Invoke(girlInfo);
        }


        public void AddListener(DelegateGirlChanged action)
        {
            OnSelectGirl += action;
        }

        public void RemoveListener(DelegateGirlChanged action)
        {
            OnSelectGirl -= action;
        }
    }
}