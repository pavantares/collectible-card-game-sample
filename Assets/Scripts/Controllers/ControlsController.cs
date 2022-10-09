using System;
using System.Reactive;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pavantares.CCG.Models;
using Pavantares.CCG.View;
using UnityEngine.SceneManagement;

namespace Pavantares.CCG.Controllers
{
    public class ControlsController : IDisposable
    {
        private readonly Game game;
        private readonly CardsHolderView cardsHolderView;
        private readonly ControlsView controlsView;
        private readonly CancellationTokenSource disposeTokenSource = new();

        public ControlsController(Game game, MainView mainView)
        {
            this.game = game;
            controlsView = mainView.ControlsView;
            cardsHolderView = mainView.CardsHolderView;

            controlsView.SetActive(true);

            controlsView.OnRandomHealth.Subscribe(HandleRandomHealth).AddTo(disposeTokenSource.Token);
            controlsView.OnRandomMana.Subscribe(HandleRandomMana).AddTo(disposeTokenSource.Token);
            controlsView.OnRandomAttack.Subscribe(HandleRandomAttack).AddTo(disposeTokenSource.Token);
            controlsView.OnRestart.Subscribe(HandleRestart).AddTo(disposeTokenSource.Token);
        }

        public void Dispose()
        {
            disposeTokenSource.Dispose();
        }

        private async void HandleRandomHealth(Unit unit)
        {
            game.RandomHealth();

            await cardsHolderView.RefreshValues();

            for (var i = 0; i < game.Cards.Count; i++)
            {
                var card = game.Cards[i];

                if (card.Health > 0)
                {
                    continue;
                }

                await cardsHolderView.RemoveFromHandCardView(card.Id);
            }

            cardsHolderView.RefreshLayout().Forget();
        }

        private void HandleRandomMana(Unit unit)
        {
            game.RandomMana();
            cardsHolderView.RefreshValues().Forget();
        }

        private void HandleRandomAttack(Unit unit)
        {
            game.RandomAttack();
            cardsHolderView.RefreshValues().Forget();
        }

        private void HandleRestart(Unit unit)
        {
            SceneManager.LoadScene(0);
        }
    }
}
