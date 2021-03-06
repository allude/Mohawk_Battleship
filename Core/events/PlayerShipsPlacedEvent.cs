﻿using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    public class PlayerShipsPlacedEvent : PlayerEvent
    {
        private ShipList ships;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the rest of the parameters,
        /// and generates a message based on the state of the given <see cref="ShipList"/>.
        /// </summary>
        /// <param name="register">A <see cref="Register"/>.</param>
        /// <param name="newShips">The <see cref="ShipList"/> associated with the <see cref="Register"/></param>
        public PlayerShipsPlacedEvent(IDNumber plr, ShipList newShips)
            : base(plr)
        {
            ships = newShips;
        }

        public PlayerShipsPlacedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="ShipList"/> of the <see cref="Controller.Register"/>.
        /// </summary>
        public ShipList Ships
        {
            get
            {
                return new ShipList(ships);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}