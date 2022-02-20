#nullable enable

namespace RSP.Models
{
    public abstract class AIStrategySettings
    {
    }

    public class RandomAIStrategySettings : AIStrategySettings
    {
    }

    public class HalfRandomAIStrategySettings : AIStrategySettings
    {
        public float P = 0.5f;
    }
}