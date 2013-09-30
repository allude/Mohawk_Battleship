﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Shared;

namespace MBC.Core.Players
{
    public class Player : IPlayer
    {
        public Register Register { get; set; }

        public FieldInfo Field { get; set; }

        public MatchConfig Match { get; set; }

        public Team Team { get; set; }
    }
}
