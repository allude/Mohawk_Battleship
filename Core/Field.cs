using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MBC.Core
{

    
    /// <summary>The Field class represents a battleship field. It contains battleship field information and
    /// two ControllerInfo objects that contain information on the two controllers in the Field.
    /// 
    /// Accessing the individual ControllerInfo objects is made simple by using a value of 0 or 1. The order
    /// of the ControllerInfo objects does not change, so constants can be used to access them.</summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// The size of the battlefield
        /// </summary>
        public Size GameSize;

        /// <summary>
        /// A Random object
        /// </summary>
        public Random FixedRandom;

        /// <summary>
        /// A list of all the ships available on the battlefield
        /// </summary>
        public List<int> ShipSizes;

        /// <summary>
        /// The time limit for this field
        /// </summary>
        public TimeSpan TimeoutLimit;
        private ControllerInfo[] controllerInfos;  //A 2-element array that contains information for each controller.

        
        /// <summary>Constructs a Battlefield object initialized with two opponents</summary>
        public Field(IBattleshipController[] battleshipControllers)
        {
            controllerInfos = new ControllerInfo[2];
            controllerInfos[0] = new ControllerInfo(battleshipControllers[0].Name, battleshipControllers[0].Version);
            controllerInfos[1] = new ControllerInfo(battleshipControllers[1].Name, battleshipControllers[1].Version);
        }

        
        /// <summary>Copy constructor</summary>
        public Field(Field copy)
        {
            controllerInfos = (ControllerInfo[])copy.controllerInfos.Clone();
            GameSize = copy.GameSize;
            FixedRandom = copy.FixedRandom;
            ShipSizes = copy.ShipSizes;
            TimeoutLimit = copy.TimeoutLimit;
        }

        
        /// <summary>Gets the controllers in this field.</summary>
        public ControllerInfo[] Controllers
        {
            get { return controllerInfos; }
        }

        
        /// <summary>It is possible to treat this Field object as a sort of array, by providing the
        /// index value in square-brackets on the object itself.</summary>
        /// <returns>The ConrollerInfo object at the specified index.</returns>
        /// I recommend removing this method of accessing ControllInfo.
        /// This is confusing because I don't know the type of oject that is being returned when using this.
        /// Also we get the same information from the Controllers property.
        [Obsolete]
        public ControllerInfo this[int i]
        {
            get { return controllerInfos[i]; }
        }

        
        /// <summary>Contains information related to the state of the battlefield for
        /// each controller.</summary>
        [Serializable]
        public class ControllerInfo
        {
            /// <summary>
            /// The shots made by this controller.
            /// </summary>
            public List<Point> ShotsMade;

            /// <summary>
            /// The ship placement of this controller.
            /// </summary>
            public List<Ship> Ships;

            /// <summary>
            /// The score for this controller.
            /// </summary>
            public int Score = 0;

            /// <summary>
            /// The name of this controller.
            /// </summary>
            public string Name;

            /// <summary>
            /// The version of this controller.
            /// </summary>
            public Version Version;

            /// <summary>
            /// Contructs a new ControllerInfo that contains information about a controller.
            /// </summary>
            /// <param name="name">The name of the controller.</param>
            /// <param name="version">The version of the controller.</param>
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
