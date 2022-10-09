using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pavantares.CCG.Models;
using Pavantares.CCG.View;

namespace Pavantares.CCG.Controllers
{
    public class CardsController : IDisposable
    {
        private readonly CardsHolderView cardsHolderView;
        private readonly BoardView boardView;
        private readonly CancellationTokenSource disposeTokenSource = new();

        public CardsController(Game game, MainView mainView)
        {
            cardsHolderView = mainView.CardsHolderView;
            boardView = mainView.BoardView;

            for (var i = 0; i < game.Cards.Count; i++)
            {
                cardsHolderView.AddCardView(game.Cards[i]);
            }

            cardsHolderView.OnReleaseCardView.Subscribe(HandleReleaseCardView).AddTo(disposeTokenSource.Token);

            cardsHolderView.SetActive(true);
        }

        public void Dispose()
        {
            disposeTokenSource.Dispose();
        }

        private void HandleReleaseCardView(CardView cardView)
        {
            var container = boardView.GetNextContainer();
            cardView.CompleteRelease(container);

            if (container != null)
            {
                cardsHolderView.RefreshLayout().Forget();
            }
        }
    }
}
