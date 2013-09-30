﻿using MBC.Core.Matches;
using MBC.Shared;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : Event
    {
        public MatchBeginEvent(IDNumber matchID)
        {
            MatchID = matchID;
        }

        private MatchBeginEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public IDNumber MatchID
        {
            get;
            private set;
        }

        public virtual Type EventType
        {
            get
            {
                return Type.MatchBegin;
            }
        }
    }
}