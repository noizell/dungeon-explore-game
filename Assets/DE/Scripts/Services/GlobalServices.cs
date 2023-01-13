using NPP.DE.Core.Signal;
using Zenject;

namespace NPP.DE.Core.Services
{
    public class GlobalServices
    {
        public static void GameStateTransition()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameStateTransitionSignal>().Dispatch();
        }

        public static void InstallSceneContext(SceneContext context)
        {
            if (!context.HasInstalled)
                context.Install();

            if (context.HasInstalled && !context.HasResolved)
                context.Resolve();
        }
    }
}
