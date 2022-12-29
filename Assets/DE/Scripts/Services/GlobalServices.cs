using NPP.DE.Core.Signal;

namespace NPP.DE.Core.Services
{
    public class GlobalServices
    {
        public static void GameStateTransition()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameStateTransitionSignal>().Dispatch();
        }
    }
}
