using System;
using System.Collections.Generic;
using System.Text;

namespace CardgameApp
{
    public enum GameMode
    {
        QuickRound, TurnByTurn
    }

    class GameManager
    {
        public GameManager(int numbOfRounds, int numbOfPlayers)
        {
            deck = new Deck();
            Players = new List<Player>();
            Rounds = numbOfRounds;
            SetupPlayers(numbOfPlayers);
        }
        
        private string rules = "Cards: Each card has a value and a suit. The suit is a color and acts as a multiplier for the value. Red cards multiply the value by 1, blue by 2, green by 3 and yellow by 4.\nGamemode 1: Quick\nEach player is dealt a hand, one card for each round specified (5 by default), and the player with the highest hand wins.\nGamemode 2: Turn by turn\nEach player is dealt 1 card and then take turns drawing more cards. After drawing a card a player can chose to call for ending the game early. If he does, the remaining players all get to draw one more card before the game ends. Otherwise the game continues for the specified amount of rounds (5 by default) and everyone reveal and compare hands at the end like normally, and the player with the highest hand wins.";
        private int rounds;
        
        public string Rules
        {
            get { return rules; }
        }
        public int Rounds
        {
            get { return rounds; }
            set 
            {
                //Only sets the number of rounds if there are enough cards in the deck for each player
                if (Players.Count * value <= deck.NumbOfCards())
                {
                    rounds = value;
                    Console.WriteLine("Number of rounds set succesfully to " + value);
                }
                //Handle if selected number of rounds is invalid
                else 
                {
                    Console.WriteLine("Rounds could not be set to entered value: " + value);
                    rounds = deck.NumbOfCards()/Players.Count;
                    Console.WriteLine("Rounds instead set to the maximum number of rounds available for the current deck and amount of players: " + rounds);
                }
            }
        }
        public int RoundTracker { get; set; }
        public GameMode Mode { get; set; }
        public Deck deck { get; set; }
        public List<Player> Players { get; set; }
        //Used to avoid changing game settings if a turn by turn game is in progress.
        public bool IsInProgress { get; set; }
        //Used to keep track of the currently active player
        public int PlayerTracker { get; set; }

        public int PlayerSlotsAvailable()
        {
            //Since each player gets dealt a card each round, the maximum amount of players is the amount of cards in deck divided with rounds.
            if (rounds != 0)
            {
                return deck.NumbOfCards() / rounds;
            }
            return 0;
        }
        public void SetupPlayers(int numbOfPlayers)
        {
            if (numbOfPlayers <= PlayerSlotsAvailable())
            Players.Clear();
            for (int i = 0; i < numbOfPlayers; i++)
            {
                Players.Add(new Player());
            }
        }

        private string WriteGameInfo()
        {
            return ("Game beginning with " + Players.Count + " players and for " + rounds + " rounds");
        }

        public string QuickGame()
        {
            string message = BeginGame();
            message += DealHands() + "\n";
            message += EndGame();
            return message;
        }
        public string BeginTurnByTurnGame()
        {
            IsInProgress = true;
            PlayerTracker = 0;
            RoundTracker = 0;
            string message = BeginGame();
            message += DealOneRound() + "\n";
            return message;
        }

        private string BeginGame()
        {
            string message = "";
            message += WriteGameInfo() + "\n";
            //Make sure a player's hand is empty
            foreach (Player player in Players)
            {
                player.Hand.Clear();
            }
            //Reset deck
            deck.GenerateDeck();
            return message;
        }
        
        public void NextPlayer()
        {
            if (PlayerTracker < Players.Count-1)
            {
                PlayerTracker++;
            }
            else
            {
                PlayerTracker = 0;
                RoundTracker++;
            }
        }
        public string EndGame()
        {
            string message = "";
            foreach (Player player in Players)
            {
                message += player.ShowHand();
                message += "Total hand value is " + player.GetHandValue() + "\n\n";
            }
            message += AnnounceWinner();
            IsInProgress = false;
            return message;
        }

        private string DealHands()
        {
            for (int i = 0; i < Rounds; i++)
            {
                for (int j = 0; j < Players.Count; j++)
                {
                    deck.DealCard(Players[j]);
                }
            }
            return("Dealing cards to each player");
        }
        public string DealOneRound()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                deck.DealCard(Players[i]);
            }
            return ("Each player dealt one card");
        }
        public string AnnounceWinner()
        {
            Player leadingPlayer = new Player();
            foreach (Player player in Players)
            {
                if (player.GetHandValue() > leadingPlayer.GetHandValue())
                {
                    leadingPlayer = player;
                }
            }
            return ("Winning player is " + leadingPlayer.Name + " with " + leadingPlayer.GetHandValue() + " points");
        }
    }
}
