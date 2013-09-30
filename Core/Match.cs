﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.Core.Matches
{
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", 2, 3, 3, 4, 5)]
    [Configuration("mbc_game_mode", GameMode.Classic, null)]
    [Configuration("mbc_match_playeradd_init_only", true)]
    [Configuration("mbc_match_teams", 1)]
    [Configuration("mbc_match_rounds_mode", RoundMode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    public class Match
    {
        private List<Event> events;
        private MatchConfig info;
        private List<IPlayer> players;

        private Dictionary<IDNumber, IPlayer> playersByID;

        private List<Round> rounds;
        private bool started;

        public Match(Configuration conf)
        {
            Init();
            EnsureGameModeCompatibility();
            SetConfiguration(conf);
        }

        private Match()
        {
            Init();
        }

        public event MBCEventHandler EventCreated;

        /// <summary>
        /// Gets the <see cref="Configuration"/> used to determine game behaviour.
        /// </summary>
        public Configuration Config
        {
            get;
            private set;
        }

        public MatchConfig Info
        {
            get
            {
                return info;
            }
        }

        public IList<IPlayer> Players
        {
            get
            {
                return players.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the <see cref="BooleanThreader"/> that handle multi-threading and automatic progression.
        /// </summary>
        [XmlIgnore]
        public BooleanThreader Thread
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Event"/>s that have been generated.
        /// </summary>
        private IList<Event> Events
        {
            get
            {
                return events.AsReadOnly();
            }
        }

        private IList<Round> Rounds
        {
            get
            {
                return rounds.AsReadOnly();
            }
        }

        public void AddController(ControlledPlayer plr)
        {
            if (started && Config.GetValue<bool>("mbc_match_playeradd_init_only"))
            {
                throw new InvalidOperationException("Cannot add players after the match has started (mbc_match_playeradd_init_only set to true)");
            }
            for (int i = 0; i < playersByID.Count; i++)
            {
                if (!playersByID.ContainsKey(i))
                {
                    playersByID[i] = plr;
                    break;
                }
            }
            plr.NewMatch(Info, players.Count);
            players.Add(plr);
            AttachEvent(new MatchAddPlayerEvent(plr.Register));
        }

        public void End()
        {
            Thread.Stop();
            Thread.Join();
            AttachEvent(new MatchEndEvent());
        }

        public IPlayer GetPlayerByID(IDNumber id)
        {
            return playersByID[id];
        }

        public bool PlayRound()
        {
            if (!started)
            {
                AttachEvent(new MatchBeginEvent());
            }
            if (MatchConditionsMet())
            {
                return true;
            }
            Round newRound = CreateNewRound();


            return MatchConditionsMet();
        }

        public void SaveToFile(File fLocation)
        {
        }

        internal virtual void AttachEvent(Event ev)
        {
            events.Add(ev);
            if (EventCreated != null)
            {
                EventCreated(ev);
            }
        }

        private Round CreateNewRound()
        {
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                switch (mode)
                {
                    case GameMode.Classic:
                        return new ClassicRound(this);
                }
            }
            throw new InvalidOperationException("An unsupported game mode was configured for this match!");
        }

        private void EnsureGameModeCompatibility()
        {
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                if (mode == GameMode.Teams)
                {
                    throw new NotImplementedException("The " + mode.ToString() + " game mode is not supported.");
                }
            }
        }

        private void Init()
        {
            events = new List<Event>();
            rounds = new List<Round>();
            players = new List<IPlayer>();
            playersByID = new Dictionary<IDNumber, IPlayer>();
            Thread = new BooleanThreader(PlayRound);
            started = false;
        }

        private bool MatchConditionsMet()
        {
            switch (Info.RoundMode)
            {
                case RoundMode.AllRounds:
                    return rounds.Count >= Info.NumberOfRounds;
                case RoundMode.FirstTo:
                    foreach (var player in players)
                    {
                        if (player.Stats.Score >= Info.NumberOfRounds)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        private void SetConfiguration(Configuration config)
        {
            Config = config;
            info = new MatchConfig();
            Info.FieldSize = new Coordinates(Config.GetValue<int>("mbc_field_width"), Config.GetValue<int>("mbc_field_height"));
            Info.NumberOfRounds = Config.GetValue<int>("mbc_match_rounds");

            Info.Registers = new List<Register>();
            foreach (var player in players)
            {
                Info.Registers.Add(player.Register);
            }

            Info.StartingShips = new ShipList();
            foreach (var length in Config.GetList<int>("mbc_ship_sizes"))
            {
                Info.StartingShips.Add(new Ship(length));
            }

            Info.TimeLimit = Config.GetValue<int>("mbc_player_thread_timeout");

            Info.GameMode = 0;
            foreach (var mode in Config.GetList<GameMode>("mbc_game_mode"))
            {
                Info.GameMode |= mode;
            }
        }
    }
}