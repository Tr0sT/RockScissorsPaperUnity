#nullable enable
using Cysharp.Threading.Tasks;
using NuclearBand;
using UnityEngine;
using RSP.Meta;

namespace RSP.Core
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameObject loadingGO = null!;
        private bool initialized;
        private async UniTaskVoid Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            // я на самом деле не фанат static классов, просто исторически так апи сложилось,
            // а переписать времени нет
            WindowsManager.Init(new WindowsManagerSettings
            {
                InputBlockPath = "GUI/InputBlocker",
                RootPath = "GUI/Canvas"
            });

            await SODatabase.InitAsync(null, null);
            await SODatabase.LoadAsync();

            // контекст верхнего уровня, метаигра - набор очков.
            // ну а RSPGame - просто движок одного раунда
            await MetaGame.Launch();
            
            Destroy(loadingGO);
            initialized = true;
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!initialized)
                return;
        
            if (pauseStatus)
                SODatabase.Save();
        }

        private void OnApplicationQuit()
        {
            if (!initialized)
                return;
            SODatabase.Save();
        }
    }
}
