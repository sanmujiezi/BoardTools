using UnityEngine.UIElements;

namespace GirlBoardEditor.UICompontent
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