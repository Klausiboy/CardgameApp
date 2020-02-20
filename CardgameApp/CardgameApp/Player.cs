using System;
using System.Collections.Generic;
using System.Text;

namespace CardgameApp
{
    class Player
    {
        public string Name { get; set; }
        public List<Card> Hand = new List<Card>();

        public int GetHandValue()
        {
            int sum = 0;
            foreach (Card card in Hand)
            {
                sum += card.CardValue;
            }
            return sum;
        }

        public string ShowHand()
        {
            string hand = "Showing hand of player " + Name + ":\n";
            foreach (Card card in Hand)
            {
                hand += card + "\n";
            }
            return hand;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}