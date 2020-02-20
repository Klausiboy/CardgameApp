using System;

namespace CardgameApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup initial game settings
            GameManager game = new GameManager(5, 3);
            string command = "";
            Console.Clear();

            //Loops through commands inputs allowing the user to give the system commands until writing "exit"
            while (command != "exit")
            {
                Console.Write("Command: ");
                command = Console.ReadLine();
                Console.Clear();
                ExeceuteCommand(command, ref game);
            }
        }

        //Executes different actions based on the input.
        private static void ExeceuteCommand(string command, ref GameManager game)
        {
            Console.WriteLine("Entered the command: "+command);
            switch (command)
            {
                case "begin game":
                    NamePlayers(game);
                    if (game.Mode == GameMode.QuickRound)
                        Console.WriteLine(game.QuickGame());
                    else if (game.Mode == GameMode.TurnByTurn)
                    {
                        Console.WriteLine(game.BeginTurnByTurnGame());
                        RunTurnByTurnGame(game);
                    }
                    break;
                case "set players":
                    SetPlayersCommand(game);
                    break;
                case "set deck":
                    Console.WriteLine("Sorry, but this feature relies on changing the decks suite, which is not implemented yet. Use \"set card numbers\" to modify the cards in the deck");
                    break;
                case "set card numbers":
                    SetCardNumbersCommand(game);
                    break;
                case "set card suit":
                    Console.WriteLine("Sorry, but this feature is not implemented yet");
                    break;
                case "set game rounds":
                    SetGameRoundsCommand(game);
                    break;
                case "set game mode":
                    Console.WriteLine("You can set the game mode here. Enter \"turnbased\" for a turnbased gamemode. Any other input will set the gamemode to \"quick\". \nFor information about game modes, you can use the \"rules\" command at the start screen.");
                    if (Console.ReadLine() == "turnbased")
                        game.Mode = GameMode.TurnByTurn;
                    else
                        game.Mode = GameMode.QuickRound;
                    break;
                case "show hand":
                    ShowHandCommand(game);
                    break;
                case "end game":
                    Console.WriteLine(game.AnnounceWinner());
                    break;
                case "help":
                    PrintHelpCommand();
                    break;
                case "rules":
                    Console.WriteLine(game.Rules);
                    break;
                default:
                    Console.WriteLine("Command not recognised. Please enter a valid command or type \"help\" to view commands or \"exit\" to exit the game");
                    break;
            }
        }

        private static void RunTurnByTurnGame(GameManager game)
        {
            string command = "";
            while (game.IsInProgress)
            {
                Console.Clear();
                command = "";
                Console.WriteLine("It is " + game.Players[game.PlayerTracker].Name + "'s turn");
                game.deck.DealCard(game.Players[game.PlayerTracker]);
                Console.WriteLine("You've been dealt a card. Type \"show hand\" to see your hand, \"end early\" to call for an early end or \"pass\" to pass the turn");
                while (command != "pass")
                {
                    command = Console.ReadLine();
                    switch (command)
                    {
                        case "show hand":
                            Console.WriteLine(game.Players[game.PlayerTracker].ShowHand());
                            break;
                        case "end early":
                            Console.WriteLine("You've called to end the game early. All player's will be allowed to draw one more card and the game will end.");
                            game.DealOneRound();
                            Console.WriteLine(game.EndGame());
                            command = "pass";
                            break;
                        case "pass":
                            game.NextPlayer();
                            break;
                        default:
                            Console.WriteLine("Command not recognized");
                            break;
                    }
                }
                //Terminate the game if final round is reached
                if (!(game.RoundTracker < game.Rounds-2))
                {
                    game.DealOneRound();
                    game.IsInProgress = false;
                    Console.WriteLine(game.EndGame());
                }
            }
        }

        private static void PrintHelpCommand()
        {
            Console.WriteLine("Available commands and what they do are displayed below:\n");
            Console.WriteLine("  begin game: Starts the game. If players haven't been named, the user will be asked to do so. Can also be used to reset the game. If gamemode is \"quick\"(Which it is by default) then a hand is dealt to each player, each players hand is shown, and the winner is determined. In \"Turn by turn\" gamemode only one card is initially dealt, the hands aren't shown, and the game proceeds through further user inputs");
            Console.WriteLine("  set players: Change the amount of players playing the game and sets their names. The amount of players is 3 by default");
            Console.WriteLine("  set card numbers: Sets the upper limit for the number on the cards in the deck. It is 8 by default.");
            Console.WriteLine("  set game rounds: Sets the number of rounds the game is played. In quick gamemode this is directly equivalent to the amount of cards dealt to each player. The amount of rounds are limited by the amount of cards in the deck and the number of players, and cannot be set to a value that would cause the deck to run out of cards. It is 5 by default");
            Console.WriteLine("  set game mode: Changes the game mode between \"quick\" and \"turn by turn\". It is \"quick\" by default");
            Console.WriteLine("  show hand: Shows the hand of a player. If game mode is set to \"quick\", the system will ask the user which player should show their hand. Otherwise the system will show the hand of the player that is currently active");
            Console.WriteLine("  end game: Prompts the system to end the game. Only available in game mode \"turn by turn\"");
            Console.WriteLine("  help: Shows this message");
            Console.WriteLine("  rules: Displays the rules of the two games");
            Console.WriteLine("  exit: Exits the program");
        }
        private static void ShowHandCommand(GameManager game)
        {
            Console.Write("please enter the name of the player, who's hand you want to show: ");
            string playerName = Console.ReadLine();
            Player player = game.Players.Find(player => player.Name == playerName);
            if (player != null)
                Console.WriteLine(player.ShowHand());
            else
                Console.WriteLine("Player doesn't exist");
        }
        private static void SetPlayersCommand(GameManager game)
        {
            if (!game.IsInProgress)
            {
                Console.Write("Please enter number of players: ");
                int numbOfPlayers = ValidateInputAsNumber(game.PlayerSlotsAvailable());
                game.SetupPlayers(numbOfPlayers);
                NamePlayers(game);
            }
            else
            {
                Console.WriteLine("Game is in progress and players can't be changed");
            }
        }
        private static void SetCardNumbersCommand(GameManager game)
        {
            if (!game.IsInProgress)
            {
                Console.Write("Please enter how high you want the number on the cards to go: ");
                game.deck.NumbOfValues = ValidateInputAsNumber();
                game.deck.GenerateDeck();
            }
            else
            {
                Console.WriteLine("Game is in progress and the deck can't be changed");
            }
        }
        private static void SetGameRoundsCommand(GameManager game)
        {
            if (!game.IsInProgress)
            {
                Console.Write("Please enter how many rounds you want the game to last: ");
                //Validate input. Value is also validated in the GameManager class to make sure.
                game.Rounds = ValidateInputAsNumber(game.deck.NumbOfCards() / game.Players.Count);
            }
            else
            {
                Console.WriteLine("Game is in progress and the amount of rounds can't be changed");
            }
        }

        private static void NamePlayers(GameManager game)
        {
            foreach (Player player in game.Players)
            {
                if (player.Name == null)
                {
                    Console.WriteLine("Please enter player's name");
                    player.Name = Console.ReadLine();
                }
            }
        }

        private static int ValidateInputAsNumber()
        {
            bool validInput = false;
            int input = 0;
            while (!validInput)
            {
                Console.Write("Input: ");
                validInput = int.TryParse(Console.ReadLine(), out input);
                //Check if input was a number
                if (!validInput)
                {
                    Console.Write("The input was not a number, please try again ");
                }
            }
            return input;
        }
        private static int ValidateInputAsNumber(int upperLimit)
        {
            bool validInput = false;
            int input = 0;
            while (!validInput)
            {
                validInput = int.TryParse(Console.ReadLine(), out input);
                //Check if input was a number
                if (!validInput)
                {
                    Console.Write("The input was not a number, please try again ");
                }
                //Check if input was too high
                if (input > upperLimit)
                {
                    Console.Write("The input was to high, there are not enough cards in the deck for the selected amount of players and rounds, please try again ");
                    validInput = false;
                }
            }
            return input;
        }
    }
}