using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Collections.ObjectModel;

namespace MBC.Core
{
    
    /// <summary>The Controller class wraps around an IBattleshipController object and provides
    /// various utilities that are used by a Competition object.
    /// 
    /// This class is almost exclusively used by the Competition class.
    /// 
    /// There are three constants available that should be used when determining the side the Controller
    /// is on: None, Red, Blue. Use None as an indicator for no controller.
    /// </summary>
    public class Controller
    {

        /// <summary>
        /// In a battleship competition the controller is associated with an agent
        /// This Enum store all the possible agents a controller can be associated with
        ///  None represents no player
        ///  Red represents the first player
        ///  Blue represents the second player
        /// </summary>
        public enum Players : int { None, Red, Blue }

        /// <summary>
        /// Number used in Point objects to signify a loss (timed out).
        /// </summary>
        public const int MagicNumberLose = -50;

        /// <summary>
        /// Integer representing no controller.
        /// </summary>
        public const int None = -1;

        /// <summary>
        /// Integer representing the first controller.
        /// </summary>
        public const int Red = 0;

        /// <summary>
        /// Integer representing the second controller.
        /// </summary>
        public const int Blue = 1;

        /// <summary>
        /// The class implementing the IBattleshipOpponent interface playing the game.
        /// </summary>
        public IBattleshipController BattleshipController;
        private int fieldIndex;
        private Stopwatch stopwatch; //Used to time each call to the iOpponent
        private Field field;
        private Field.ControllerInfo controllerInformation;

        
        /// <summary>Sets up this Controller</summary>
        /// <param name="battleshipController">The object that implements the IBattleshipController interface to play the game.</param>
        /// <param name="f">The Field object this Controller will make changes to.</param>
        /// <param name="index">The index number in the ControllerInfo array in the Field that this Controller represents.</param>
        /// <seealso cref="Field"/>
        public Controller(IBattleshipController battleshipController, Field f, int index)
        {
            BattleshipController = battleshipController;
            field = f;

            controllerInformation = f.Controllers[index];

            fieldIndex = index;

            controllerInformation.Score = 0;
            controllerInformation.ShotsMade = new List<Point>();
            stopwatch = new Stopwatch();
        }

        
        /// <summary>Gets the ControllerInfo index this object represents in the Field.</summary>
        /// <seealso cref="Field"/>
        public int FieldIndex
        {
            get { return fieldIndex; }
        }

        
        /// <returns>The information related to this controller on the battlefield</returns>
        /// <seealso cref="Field.ControllerInfo"/>
        public Field.ControllerInfo ControllerInformation { get { return controllerInformation; } }
        [Obsolete]
        public Field.ControllerInfo GetFieldInfo()
        {
            return controllerInformation;
        }

        
        /// <returns>The time the last action took for this controller to perform.</returns>
        public long TimeTaken { get { return stopwatch.ElapsedMilliseconds; } }
        [Obsolete]
        public long GetTimeTaken()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        
        /// <summary>Checks whether all of the ships have been placed in valid locations</summary>
        /// <returns>True if all ships are placed, not conflicting each other, and in valid locations.
        /// False if otherwise</returns>
        public bool ShipsReady()
        {
            foreach (Ship s1 in controllerInformation.Ships)
            {

                if (!s1.IsPlaced || !s1.IsValid(field.GameSize))
                    return false;

                foreach (Ship s2 in controllerInformation.Ships)
                {
                    if (s2 == s1) continue;
                    if (s2.ConflictsWith(s1)) return false;
                }
            }
            return true;
        }

        
        /// <summary>Notifys the controller that they are matched up with a new opponent.</summary>
        /// <param name="opponent">The name of the controller as a string</param>
        public void NewMatch(string opponent)
        {
            BattleshipController.NewMatch(opponent);
        }

        
        /// <summary>Determines if the stopwatch time passed the maximum time allowed.</summary>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool RanOutOfTime()
        {
            if (stopwatch.Elapsed > field.TimeoutLimit)
                return true;
            return false;
        }

        
        /// <summary>Resets certain data for a new game and notifys the controller of the new game being commenced</summary>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool NewGame()
        {
            stopwatch.Reset();
            controllerInformation.ShotsMade.Clear();
            stopwatch.Start();

            BattleshipController.NewGame(field.GameSize, field.TimeoutLimit, field.FixedRandom);

            stopwatch.Stop();
            return RanOutOfTime();
        }

        
        /// <summary>Notifys the controller to make ship placements.</summary>
        /// <param name="newShips">A list of ships to place</param>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool PlaceShips(List<Ship> newShips)
        {
            controllerInformation.Ships = newShips;
            stopwatch.Start();
            BattleshipController.PlaceShips(controllerInformation.Ships.AsReadOnly());
            stopwatch.Stop();
            return RanOutOfTime();
        }

        
        /// <returns>The ship at the passed in point. Null if there is no ship.</returns>
        public Ship GetShipAtPoint(Point point)
        {
            foreach (Ship s in controllerInformation.Ships)
                if (s.IsAt(point))
                    return s;
            return null;
        }

        
        /// <returns>True if the controller is still in the match, false if the controller has lost</returns>
        public bool IsAlive(List<Point> shots)
        {
            foreach (Ship s in controllerInformation.Ships)
                if (!s.IsSunk(shots))
                    return true;
            return false;
        }

        
        /// <summary>Asks for the controller's shot. ShootAt will repeatedly request the shot
        /// until it hasn't made the same shot twice.</summary>
        /// <param name="opponent">The opposing Controller to "shoot at"</param>
        /// <returns>The point the controller has shot at. If the controller ran out of time,
        /// a point at (LOSE_MAGIC_NUMBER, LOSE_MAGIC_NUMBER) will be returned instead.</returns>
        public Point ShootAt(Controller opponent)
        {
            stopwatch.Start();
            Point shot = BattleshipController.GetShot();
            stopwatch.Stop();

            if (shot.X < 0)
                shot.X = 0;
            if (shot.Y < 0)
                shot.Y = 0;

            if (RanOutOfTime())
                return new Point(MagicNumberLose, MagicNumberLose);

            if (controllerInformation.ShotsMade.Where(s => s.X == shot.X && s.Y == shot.Y).Any())
                return ShootAt(opponent);

            controllerInformation.ShotsMade.Add(shot);
            return shot;
        }

        /// <summary>Notifys the controller that a shot has been made by the other opponent at
        /// a certain Point</summary>
        /// <param name="shot">The point where the other controller shot</param>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool OpponentShot(Point shot)
        {
            stopwatch.Start();
            BattleshipController.OpponentShot(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /// <summary>Notifys the controller that their shot hit a ship at the specified point</summary>
        /// <param name="shot">The point at which the controllers shot hit a ship</param>
        /// <param name="sunk">If the shot sunk a ship</param>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool ShotHit(Point shot, bool sunk)
        {
            stopwatch.Start();
            BattleshipController.ShotHit(shot, sunk);
            stopwatch.Stop();
            return RanOutOfTime();
        }
        
        /// <summary>Notifys the controller that their shot missed at the specified point</summary>
        /// <param name="shot">The point the controller shot at but missed</param>
        /// <returns>True if the controller ran out of time. False if they didn't</returns>
        public bool ShotMiss(Point shot)
        {
            stopwatch.Start();
            BattleshipController.ShotMiss(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /// <summary>Notifys the controller that a game has been won, and increments this controller's score.</summary>
        public void GameWon()
        {
            controllerInformation.Score++;
            BattleshipController.GameWon();
        }

        /// <summary>Notifys the controller that a game has been lost</summary>
        public void GameLost()
        {
            BattleshipController.GameLost();
        }

        
        /// <summary>Notifys the controller that a matchup is over</summary>
        public void MatchOver()
        {
            BattleshipController.MatchOver();
        }

        
        /// <summary>Generates a string containing the name and version of the encapsulated IBattleshipController</summary>
        public override string ToString()
        {
            return Utility.ControllerToString(BattleshipController);
        }

    }
}
