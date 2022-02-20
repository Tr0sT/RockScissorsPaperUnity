#nullable enable
using NuclearBand;
using UnityEngine;

namespace RSP.Models
{
    public class RSPSettings : DataNode
    {
        public static string Path = "RSPSettings";

        [SerializeField] private AIStrategySettings aiStrategySettings = new RandomAIStrategySettings();
        public AIStrategySettings AIStrategySettings => aiStrategySettings;

        [SerializeField] private RSPRules rspRules = null!;
        public RSPRules RSPRules => rspRules;
    }
}