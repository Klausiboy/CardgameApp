using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardgameApp
{
    class Deck
    {
        //Constructor sets up the standard deck and shuffles it
        public Deck()
        {
            NumbOfSuits = 4;
            NumbOfValues = 8;
            Colors = new List<string>();
            Cards = new List<Card>();

            FillColors(Colors);
            GenerateDeck();
        }

        public int NumbOfSuits { get; set; }
        public int NumbOfValues { get; set; }
        public List<Card> Cards { get; set; }
        public List<string> Colors { get; set; }
        public int NumbOfCards()
        {
            return NumbOfSuits*NumbOfValues;     
        }

        private void FillColors(List<string> colors)
        {
            //Manually define colors for the suits of the cards. May need something smarter or more dynamic
            colors.Add("Red");
            colors.Add("Blue");
            colors.Add("Green");
            colors.Add("Yellow");
        }

        public void GenerateDeck()
        {
            Cards.Clear();
            //Loop through the amount of suits and values a card can have and generate a card for the deck for each combination.
            for (int i = 0; i < NumbOfSuits; i++)
            {
                for (int j = 0; j < NumbOfValues; j++)
                {
                    Cards.Add(new Card(j+1,i+1,Colors[i]));
                }
            }
            ShuffleDeck();
        }
        
        public void ShuffleDeck()
        {
            Random rng = new Random();
            Cards = Cards.OrderBy(x => rng.Next()).ToList();
        }

        internal void DealCard(Player player)
        {
            player.Hand.Add(Cards[0]);
            Cards.RemoveAt(0);
        }
    }
}