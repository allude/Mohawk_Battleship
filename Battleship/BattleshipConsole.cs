﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Battleship
{
    /**
     * <summary>Provides an interactive console for the user.</summary>
     */
    public class BattleshipConsole
    {
        private BattleshipConfig config; //The main configuration object
        private string[] fsStrings = { "first", "second" }; //Strings used to denote "first" and "second"

        /**
         * <summary>Class constructor. Initializes the configuration object.</summary>
         */
        public BattleshipConsole()
        {
            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
        }

        /**
         * <summary>The base class for console states</summary>
         */
        abstract class ConsoleState
        {
            protected BattleshipConsole main; //Reference to the 'parent'
            protected string extraMenu = ""; //Extra key options available to the user.
            protected const string headerEnds = "====================="; //Styling used for header ends

            public ConsoleState(BattleshipConsole main)
            {
                this.main = main;
            }

            /**
             * <summary>Writes a specific character for a number of times.</summary>
             * <param name="c">The character to print to the console.</param>
             * <param name="n">The number of times to print the character</param>
             */
            protected void WriteChars(char c, int n)
            {
                for (int i = 0; i < n; i++)
                    Console.Write(c);
            }

            /**
             * <summary>Called in the loop, used for input interactivity.</summary>
             * <param name="input">The string entered by the user into the console</param>
             */
            public ConsoleState GetInput(string input)
            {
                switch (input)
                {
                    case "E": //Exit key
                        return null;
                    case "C": //Quit key
                        return this;
                }
                return Response(input);
            }

            /**
             * <summary>Prints the specified text, with the specified ends, centered, to the console.</summary>
             * <param name="text">The text to print centered.</param>
             * <param name="ends">The ends used for styling.</param>
             */
            public void WriteCenteredText(string text, string ends)
            {
                int maxChars = Console.WindowWidth - ends.Length * 2;
                int count = 0;
                while (count < text.Length)
                {
                    int printLen = text.Length - count > maxChars ? maxChars : text.Length - count;
                    int cen = maxChars / 2 - printLen / 2;
                    //Program.PrintDebugMessage("" + cen);

                    Console.Write(ends);
                    WriteChars(' ', cen);

                    Console.Write(text.Substring(count, printLen));

                    WriteChars(' ', Console.WindowWidth - (ends.Length * 2 + cen + printLen));
                    Console.Write(ends);
                    count += printLen;
                }
            }

            /**
             * <summary>Called in the loop, this provides a standard way for user interactivity and display.</summary>
             */
            public void Display()
            {
                Console.Clear();
                WriteCenteredText("BATTLESHIP COMPETITION FRAMEWORK", headerEnds);
                StateDisplay();
                WriteCenteredText("[E]xit, [C]onfigure" + (extraMenu == "" ? "" : ", ")+extraMenu, "===");
                Console.Write("Input : ");
            }

            /**
             * <summary>Overridden to provide specific printing related to the subclass.</summary>
             */
            protected abstract void StateDisplay();

            /**
             * <summary>Called when input is not handled by the base class.</summary>
             * <param name="input">The string entered by the user</param>
             * <returns>The implementing class, null, or a new ConsoleState object. null will end the program.</returns>
             */
            protected abstract ConsoleState Response(string input);
        }

        /**
         * <summary>Allows the user to select a battleship bot</summary>
         */
        class BotChoiceState : ConsoleState
        {

            private int opponentChoiceCount = 0; //The current battleship bot the user is selecting.
            private IBattleshipOpponent[] opponents = new IBattleshipOpponent[2]; //The battleship bots being selected.
            private InputError error = InputError.none; //The error, based on user input.

            public BotChoiceState(BattleshipConsole main)
                : base(main)
            {
            }

            private enum InputError
            {
                none,
                noExist,
                badNumber
            }

            private void ListBots()
            {
                Console.WriteLine("Here is a list of bots currently available:\n");

                int count = 1;
                foreach (string botName in main.config.BotNames)
                    Console.WriteLine((count++) + ": " + botName);
                Console.WriteLine();
            }

            protected override void StateDisplay()
            {
                WriteCenteredText("Bot choice menu", headerEnds);
                Console.WriteLine();
                if (main.config.BotNames.Count == 0)
                {
                    Console.WriteLine("There are no bots that can be listed.");
                    return;
                }

                ListBots();
                Console.WriteLine("\n\n");
                switch (error)
                {
                    case InputError.noExist:
                        Console.WriteLine("> That selection doesn't exist!");
                        break;
                    case InputError.badNumber:
                        Console.WriteLine("> Invalid input.");
                        break;
                }
                WriteCenteredText("Enter a number for the " + main.fsStrings[opponentChoiceCount] + " opponent", "===");
            }

            protected override ConsoleState Response(string input)
            {
                error = InputError.none;
                int choice = -1;

                try
                {
                    choice = int.Parse(input) - 1;
                }
                catch
                {
                    error = InputError.badNumber;
                    return this;
                }

                if (choice < 0 || choice >= main.config.BotNames.Count)
                {
                    error = InputError.noExist;
                    return this;
                }
                opponents[opponentChoiceCount++] = main.config.GetRobot(main.config.BotNames.ElementAt(choice));
                if (opponentChoiceCount > 1)
                    return new CompetitionState(main, opponents);
                return this;
            }
        }

        class CompetitionState : ConsoleState
        {
            private IBattleshipOpponent[] bots;
            private Dictionary<IBattleshipOpponent, int> scores;

            public CompetitionState(BattleshipConsole main, IBattleshipOpponent[] opp) : base(main)
            {
                extraMenu = "[S]election";
                bots = opp;
            }

            protected override void StateDisplay()
            {
                WriteCenteredText("Bot competition mode", headerEnds);
                Console.WriteLine("Running the competition...\n\n");
                BattleshipCompetition bc = new BattleshipCompetition(bots, main.config);
                scores = bc.RunCompetition();
                Console.WriteLine("Done! Press any key to view the final results...");
            }

            protected override ConsoleState Response(string input)
            {
                switch (input)
                {
                    case "S":
                        return new BotChoiceState(main);
                }
                return new ResultState(main, scores);
            }
        }

        class ResultState : ConsoleState
        {

            Dictionary<IBattleshipOpponent, int> results;

            public ResultState(BattleshipConsole main, Dictionary<IBattleshipOpponent, int> results)
                : base(main)
            {
                this.results = results;
                extraMenu = "[R]ematch, [S]election";
            }

            protected override void StateDisplay()
            {
                WriteCenteredText("Battleship competition results", headerEnds);
                foreach (var key in results.Keys.OrderByDescending(k => results[k]))
                    Console.WriteLine(key.Name + " has scored " + results[key]);
            }

            protected override ConsoleState Response(string input)
            {
                switch (input)
                {
                    case "R":
                        return new CompetitionState(main, results.Keys.ToArray());
                    case "S":
                        return new BotChoiceState(main);
                }
                return this;
            }
        }

        public void Start()
        {
            Console.Title = "BATTLESHIP COMPETITION FRAMEWORK";
            Console.WriteLine("Welcome to the Battleship Competition!\n\n");

            ConsoleState state = new BotChoiceState(this);

            while (true)
            {
                state.Display();
                string input = Console.ReadLine();
                state = state.GetInput(input);
                if (state == null)
                    break;
            }

            config.SaveConfigFile();
        }
    }
}
