using UnityEngine;

namespace Pavantares.CCG.Models
{
    public class Card
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public Texture Picture { get; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public bool IsOnBoard { get; set; }

        public Card(string id, Texture picture, int attack, int health, int mana)
        {
            Id = id;
            Title = "Lorem ipsum";
            Description = "Lorem ipsum";
            Picture = picture;
            Attack = attack;
            Health = health;
            Mana = mana;
        }
    }
}
