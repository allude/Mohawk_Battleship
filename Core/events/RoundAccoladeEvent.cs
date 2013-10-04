﻿using MBC.Core.Accolades;
using MBC.Core.Rounds;
using System;
using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Round"/> that has an <see cref="Accolade"/> added to it.
    /// </summary>
    public class RoundAccoladeEvent : RoundEvent
    {
        private Accolade accolade;

        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and stores the <paramref name="accolade"/>.
        /// </summary>
        /// <param name="round">The <see cref="Round"/> that has had the <paramref name="accolade"/> added.</param>
        /// <param name="accolade">The <see cref="Accolade"/> added to the <paramref name="round"/>.</param>
        public RoundAccoladeEvent(IDNumber roundID, Accolade accolade) : base(roundID)
        {
            this.accolade = accolade;
        }

        private RoundAccoladeEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        /// <summary>
        /// Gets an <see cref="Accolade"/> that has been added to the <see cref="RoundEvent.Round"/>.
        /// </summary>
        public Accolade Accolade
        {
            get
            {
                return accolade;
            }
        }
    }
}