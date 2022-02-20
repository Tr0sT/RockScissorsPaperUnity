#nullable enable
using System;
using System.Collections.Generic;
using NuclearBand;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RSP.GUI
{
    public class ScoreWindow : Window
    {
        #region static
        private const string Path = "GUI/ScoreWindow/ScoreWindow";
        public static ScoreWindow Create(ReactiveProperty<int> playerScore,ReactiveProperty<int> enemyScore)
        {
            return (ScoreWindow)WindowsManager.CreateWindow(Path, window =>
            {
                var scoreWindow = (ScoreWindow)window!;
                scoreWindow.playerScore = playerScore;
                scoreWindow.enemyScore = enemyScore;
            }).Window;
        }
        #endregion

        [SerializeField]private Text scoreText = null!;
        
        private ReactiveProperty<int> playerScore = null!;
        private ReactiveProperty<int> enemyScore = null!;

        private readonly List<IDisposable> disposables = new(); 
        
        public override void Init()
        {
            base.Init();
            UpdateScore();
            playerScore.Subscribe(_ => UpdateScore()).AddTo(disposables);
            enemyScore.Subscribe(_ => UpdateScore()).AddTo(disposables);
        }

        private void UpdateScore() => scoreText.text = $"{playerScore}:{enemyScore}";

        protected override void DeInit()
        {
            disposables.ForEach(d => d.Dispose());
            disposables.Clear();
            base.DeInit();
        }
    }
}