using UnityEngine;

namespace Pavantares.CCG.View
{
    public class MainView : MonoBehaviour
    {
        [SerializeField]
        private CardsHolderView cardsHolderView;

        [SerializeField]
        private BoardView boardView;

        [SerializeField]
        private ControlsView controlsView;

        public CardsHolderView CardsHolderView => cardsHolderView;
        public BoardView BoardView => boardView;
        public ControlsView ControlsView => controlsView;
    }
}
