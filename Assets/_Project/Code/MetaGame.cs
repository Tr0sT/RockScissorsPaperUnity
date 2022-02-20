#nullable enable
using Cysharp.Threading.Tasks;
using RSP.Controllers;
using RSP.GUI;
using UniRx;
using UnityEngine;

namespace RSP.Meta
{
    public class MetaGame
    {
        private readonly RSPGameLauncher rspGameLauncherPrefab;

        private ScoreWindow scoreWindow = null!;
        private RSPGame rspGame = null!;

        private readonly ReactiveProperty<int> playerScore = new();
        private readonly ReactiveProperty<int> enemyScore = new();

        private RSPGameLauncher rspGameLauncher = null!;

        public static async UniTask Launch()
        {
            var rspGamePrefab = (await UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(RSPGameLauncher.Path))!.GetComponent<RSPGameLauncher>();
            
            new MetaGame(rspGamePrefab).LaunchInternal();
        }
        
        private MetaGame(RSPGameLauncher rspGameLauncherPrefab)
        {
            this.rspGameLauncherPrefab = rspGameLauncherPrefab;
        }

        private MetaGame LaunchInternal()
        {
            rspGameLauncher = GameObject.Instantiate(rspGameLauncherPrefab.gameObject).GetComponent<RSPGameLauncher>();

            scoreWindow = ScoreWindow.Create(playerScore, enemyScore);

            rspGame = rspGameLauncher.Launch();
            rspGame.OnWin += OnWin;
            rspGame.OnLose += OnLose;
            rspGame.OnDraw += OnDraw;
            
            return this;
        }

        private void Restart() => rspGame.Restart();

        private void OnDraw() => ShowEndWindow();

        private void OnLose()
        {
            enemyScore.Value++;
            ShowEndWindow();
        }

        private void OnWin()
        {
            playerScore.Value++;
            ShowEndWindow();
        }

        private void ShowEndWindow()
        {
            EndWindow.Create().OnHidden += _ => Restart();
        }

        private void Dispose()
        {
            scoreWindow.Close();
            UnityEngine.AddressableAssets.Addressables.ReleaseInstance(rspGameLauncherPrefab.gameObject);
            playerScore.Dispose();
            enemyScore.Dispose();
        }
    }
}