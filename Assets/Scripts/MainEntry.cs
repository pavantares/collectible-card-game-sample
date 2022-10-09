using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pavantares.CCG.Controllers;
using Pavantares.CCG.Models;
using Pavantares.CCG.Services;
using Pavantares.CCG.Utilities;
using Pavantares.CCG.View;
using UnityEngine;

namespace Pavantares.CCG
{
    public class MainEntry : MonoBehaviour
    {
        [SerializeField]
        private MainView mainView;

        private readonly List<IDisposable> disposables = new();

        private async void Awake()
        {
            var cards = await GetInitialCards();
            var game = new Game(cards);
            var cardsController = new CardsController(game, mainView);
            var controlsController = new ControlsController(game, mainView);

            disposables.Add(cardsController);
            disposables.Add(controlsController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
        }

        private static async Task<List<Card>> GetInitialCards()
        {
            var pictureService = new PictureService();
            var cardsCount = Randomizer.GetCardsCount();
            var cards = new List<Card>(cardsCount);

            for (var i = 0; i < cardsCount; i++)
            {
                var picture = await pictureService.Load();
                var card = new Card(Randomizer.GetId(), picture, Randomizer.GetValue(), Randomizer.GetValue(), Randomizer.GetValue());
                cards.Add(card);
            }

            return cards;
        }
    }
}
