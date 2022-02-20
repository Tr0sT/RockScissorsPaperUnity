#nullable enable
using System;
using System.Linq;
using LeopotamGroup.Globals;
using RSP.Models;

namespace RSP.Controllers
{
    public interface IAIStrategy
    {
        public ShapeType ChooseShape();
    }
    public abstract class AIStrategy<TSettings> : IAIStrategy where TSettings: AIStrategySettings
    {
        protected TSettings settings { get; }

        protected readonly RSPRules rspRules;

        protected AIStrategy(TSettings settings, RSPRules rspRules)
        {
            this.settings = settings;
            this.rspRules = rspRules;
        }

        public abstract ShapeType ChooseShape();
    }

    public class RandomStrategy : AIStrategy<RandomAIStrategySettings>
    {
        public RandomStrategy(RandomAIStrategySettings settings, RSPRules rspRules) : base(settings, rspRules)
        {
        }

        public override ShapeType ChooseShape() => rspRules.ShapeRules[UnityEngine.Random.Range(0, rspRules.ShapeRules.Count)].Shape;
    }

    public class HalfRandomStrategy : AIStrategy<HalfRandomAIStrategySettings>
    {
        private readonly RSPGame rspGame;
        private ShapeType playerShape;
        public HalfRandomStrategy(HalfRandomAIStrategySettings settings, RSPRules rspRules) : base(settings, rspRules)
        {
            // Создавать контекст для передачи в стратегии бесполезно,
            // потому что АИ могут потребоваться абсолютно любые данные из игры.
            // Так что АИ и туториалов сервис локаторы...
            
            rspGame = Service<RSPGame>.Get();
            rspGame.OnPlayerChooseShape += OnPlayerChooseShape;
        }

        private void OnPlayerChooseShape(ShapeType playerShape)
        {
            this.playerShape = playerShape;
        }

        public override ShapeType ChooseShape()
        {
            var winShapes = rspRules.ShapeRules
                .Where(r => r.Victims.Contains(playerShape))
                .Select(r => r.Shape)
                .ToList();
            var loseAndDrawShapes = rspRules.ShapeRules
                .Where(r => !r.Victims.Contains(playerShape))
                .Select(r => r.Shape)
                .ToList();
            var aiWin = UnityEngine.Random.Range(0.0f, 1.0f) < settings.P;
            return aiWin ? 
                winShapes[UnityEngine.Random.Range(0, winShapes.Count)] : 
                loseAndDrawShapes[UnityEngine.Random.Range(0, loseAndDrawShapes.Count)];
        }
    }

    public static class AIStrategyFabric
    {
        public static IAIStrategy CreateAIStrategy<TSettings>(TSettings strategySettings, RSPRules rspRules) where TSettings : AIStrategySettings =>
            strategySettings switch
            {
                RandomAIStrategySettings randomSettings => new RandomStrategy(randomSettings, rspRules),
                HalfRandomAIStrategySettings halfRandomSettings => new HalfRandomStrategy(halfRandomSettings, rspRules),
                _ => throw new InvalidOperationException()
            };
    }
}