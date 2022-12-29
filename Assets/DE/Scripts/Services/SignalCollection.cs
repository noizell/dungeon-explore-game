
using deVoid.Utils;

namespace NPP.DE.Core.Signal
{
    public static class Signals
    {
        private static SignalHub _hub;

        public static SignalHub Hub
        {
            get
            {
                if (_hub == null)
                    _hub = new SignalHub();

                return _hub;
            }
        }
    }

    public class SignalCollection
    {
        public class AppState
        {
            public class GameLoadedSignal : ASignal<GameLoadedParameter> { }


        }
    }

    #region Signals Parameter

    public struct GameLoadedParameter
    {
        public Ui.MenuManager Menu;

        public GameLoadedParameter(Ui.MenuManager menu)
        {
            Menu = menu;
        }
    }

    #endregion

}