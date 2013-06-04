using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class ResultDisplay : TerminalModule
    {
        Competition competition;
        VerticalLayout buttonLayout;

        public ResultDisplay(Competition comp)
        {
            competition = comp;

            buttonLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            buttonLayout.Add(new ButtonControl("Back to Main Menu", ButtonClick));

            AddControlLayout(buttonLayout);
        }

        private bool ButtonClick(string txt)
        {
            BattleshipConsole.AddModule(new MainMenu());
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.UpdateDisplay();
            return false;
        }

        protected override void Display()
        {
            WriteCenteredText("=====COMPETITION RESULTS=====");
            NewLine(2);
            WriteCenteredText("The result...");
            NewLine();

            Field.ControllerInfo redInfo = competition.GetBattlefield()[Controller.Red];
            Field.ControllerInfo blueInfo = competition.GetBattlefield()[Controller.Blue];
            if (redInfo.Score > blueInfo.Score)
            {
                WriteCenteredText(redInfo.Name + " won!");
            }
            else if (blueInfo.Score > redInfo.Score)
            {
                WriteCenteredText(blueInfo.Name + " won!");
            }
            else
            {
                WriteCenteredText("DRAW");
            }
            NewLine(2);
            buttonLayout.Display();
        }
    }
}
