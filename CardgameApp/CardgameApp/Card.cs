using System;
using System.Collections.Generic;
using System.Text;

namespace CardgameApp
{
    class Card
    {
        public Card(int number, int multiplier, string suit)
        {
            Number = number;
            Multiplier = multiplier;
            Suit = suit;
        }

        public int Number { get; set; }
        public int Multiplier { get; set; }
        public string Suit { get; set; }

        //The value resulting of multiplying the number with the suit 
        public int CardValue
        {
            get { return Multiplier*Number; }
        }

        public override string ToString()
        {
            return (Number + " of " + Suit+ " (value: " + CardValue + ")");
        }
    }
}