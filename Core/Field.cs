using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MBC.Core
{

    /**
     * <summary>The Field class represents a battleship field. It contains battleship field information and
     * two ControllerInfo objects that contain information on the two controllers in the Field.
     * 
     * Accessing the individual ControllerInfo objects is made simple by using a value of 0 or 1. The order
     * of the ControllerInfo objects does not change, so constants can be used to access them.</summary>
     */
    [Serializable]
    public class Field
    {
        public Size GameSize;           //The size of the battlefield
        public Random FixedRandom;      //A Random object
        public List<int> ShipSizes;     //A list of all the ships available on the battlefield
        public TimeSpan TimeoutLimit;   //The time limit for this field
        private ControllerInfo[] controllers;  //A 2-element array that contains information for each controller.

        /**
         * <summary>Constructs a Battlefield object initialized with two opponents</summary>
         */
        public Field(IBattleshipController[] battleshipController)
        {
            controllers = new ControllerInfo[2];
            controllers[0] = new ControllerInfo(battleshipController[0].Name, battleshipController[0].Version);
            controllers[1] = new ControllerInfo(battleshipController[1].Name, battleshipController[1].Version);
        }

        /**
         * <summary>Copy constructor</summary>
         */
        public Field(Field copy)
        {
            controllers = (ControllerInfo[])copy.controllers.Clone();
            GameSize = copy.GameSize;
            FixedRandom = copy.FixedRandom;
            ShipSizes = copy.ShipSizes;
            TimeoutLimit = copy.TimeoutLimit;
        }

        /**
         * <summary>Gets the controllers in this field.</summary>
         */
        public ControllerInfo[] Controllers
        {
            get { return controllers; }
        }

        /**
         * <returns>The ConrollerInfo object at the specified index.</returns>
         * I recommend removing this. The same data can be accessed through the Controllers property.
         * It is more difficult to tell what the type of object returned is.
         */
        [Obsolete]
        public ControllerInfo this[int i]
        {
            get { return controllers[i]; }
        }

        /**
         * <summary>Contains information related to the state of the battlefield for
         * each opponent.</summary>
         */
        [Serializable]
        public class ControllerInfo
        {
            public List<Point> ShotsMade;
            public List<Ship> Ships;
            public int Score = 0;

            public string Name;
            public Version Version;

            public ControllerInfo(string name, Version version)
            {
                ShotsMade = new List<Point>();
                Ships = new List<Ship>();
                Name = name;
                Version = version;
            }
        }
    }
}
