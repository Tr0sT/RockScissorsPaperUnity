#nullable enable
using NuclearBand;

namespace RSP.GUI
{
    public class EndWindow : Window
    {
        #region static
        private const string Path = "GUI/EndWindow/EndWindow";
        public static EndWindow Create()
        {
            return (EndWindow)WindowsManager.CreateWindow(Path).Window;
        }
        #endregion
    }
}