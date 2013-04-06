﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolades
{
    public class Domination : AccoladeProcessor
    {
        int diff = 0;
        IBattleshipController last = null;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (last != a.ibc)
            {
                diff = 0;
                last = a.ibc;
            }
            diff++;
            if (diff > Configuration.GetGlobalDefault().GetConfigValue<int>("accolade_dom_diff", 9))
            {
                diff = 0;
                return RoundLog.RoundAccolade.Domination;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}