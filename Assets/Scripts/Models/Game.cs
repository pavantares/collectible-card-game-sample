using System.Collections.Generic;
using Pavantares.CCG.Utilities;

namespace Pavantares.CCG.Models
{
    public class Game
    {
        private readonly List<Card> cards;

        public IReadOnlyList<Card> Cards => cards;

        public Game(List<Card> cards)
        {
            this.cards = cards;
        }

        public void RandomHealth()
        {
            cards.FindAll(x => !x.IsOnBoard).ForEach(x => x.Health = Randomizer.GetTestValue());
        }

        public void RandomMana()
        {
            cards.FindAll(x => !x.IsOnBoard).ForEach(x => x.Mana = Randomizer.GetTestValue());
        }

        public void RandomAttack()
        {
            cards.FindAll(x => !x.IsOnBoard).ForEach(x => x.Attack = Randomizer.GetTestValue());
        }
    }
}
