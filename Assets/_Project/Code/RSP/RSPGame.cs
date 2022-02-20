#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using LeopotamGroup.Globals;
using RSP.Models;
using RSP.Views;

namespace RSP.Controllers
{
    public class RSPGame
    {
        public event Action? OnWin;
        public event Action? OnLose;
        public event Action? OnDraw;

        public event Action<ShapeType>? OnPlayerChooseShape;

        private readonly RSPGameLauncher gameLauncher;
        private readonly RSPSettings rspSettings;

        private readonly IAIStrategy aiStrategy;

        private readonly Dictionary<ShapeType, RSPShapeView> activeShapeViews = new();

        public RSPGame()
        {
            gameLauncher = null!;
            rspSettings = null!;
            aiStrategy = null!;
        }
        
        public RSPGame(RSPGameLauncher gameLauncher, RSPSettings rspSettings)
        {
            Service<RSPGame>.Set(this);

            this.gameLauncher = gameLauncher;
            this.rspSettings = rspSettings;

            aiStrategy = AIStrategyFabric.CreateAIStrategy(rspSettings.AIStrategySettings, rspSettings.RSPRules);
        }

        public RSPGame Launch() => Restart();
        
        public RSPGame Restart()
        {
            DeInit();

            InitPlayerShapes();

            gameLauncher.RoundSequenceAnimation.Init(activeShapeViews.Values.ToList());
            
            return this;
        }


        private void InitPlayerShapes()
        {
            foreach (var rule in rspSettings.RSPRules.ShapeRules)
            {
                var shapeView = gameLauncher.ShapeViews.Find(v => v.ShapeType == rule.Shape);
                shapeView.Init(ChooseShape);
                activeShapeViews.Add(rule.Shape, shapeView);
            }
        }

        private void ChooseShape(ShapeType playerShape)
        {
            OnPlayerChooseShape?.Invoke(playerShape);
            var enemyShape = aiStrategy.ChooseShape();
            var gameResult = CalcGameResult(playerShape, enemyShape);
            activeShapeViews.Values.ToList().ForEach(v => v.Disable());

            gameLauncher.RoundSequenceAnimation.Launch(playerShape, enemyShape, gameResult, OnChooseAnimationEnd);

            void OnChooseAnimationEnd()
            {
                switch (gameResult)
                {
                    case RSPGameResult.Win:
                        OnWin?.Invoke();
                        break;
                    case RSPGameResult.Lose:
                        OnLose?.Invoke();
                        break;
                    case RSPGameResult.Draw:
                        OnDraw?.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private RSPGameResult CalcGameResult(ShapeType playerShape, ShapeType enemyShape)
        {
            var shapeRules = rspSettings.RSPRules.ShapeRules;

            var playerShapeRule = shapeRules.Find(r => r.Shape == playerShape);
            if (playerShapeRule.Victims.Contains(enemyShape))
                return RSPGameResult.Win;

            var enemyShapeRule = shapeRules.Find(r => r.Shape == enemyShape);
            if (enemyShapeRule.Victims.Contains(playerShape))
                return RSPGameResult.Lose;

            return RSPGameResult.Draw;
        }

        private void DeInit()
        {
            foreach (var activeShapeView in activeShapeViews.Values)
                activeShapeView.DeInit();

            activeShapeViews.Clear();

            gameLauncher.RoundSequenceAnimation.Stop();
        }
    }
}