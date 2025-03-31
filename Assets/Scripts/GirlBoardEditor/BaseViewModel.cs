using GirlBoardEditor.Model;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public abstract class BaseViewModel
    {
        protected VisualElement root;
       
        public BaseViewModel(VisualElement root)
        {
            this.root = root;
        }
    }
}