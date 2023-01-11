using UnityEngine;
using Zenject;
using NPP.DE.Core.Signal;
using NPP.DE.Ui;

namespace NPP.DE.Core.Game
{


    public class CoreManager : MonoBehaviour
    {
        private GameCounter _counter;
        private MenuManager _menuManager;

        [Zenject.Inject]
        private void ConstructGameCounter(GameCounter counter)
        {
            _counter = counter;
        }

        private void OnEnable()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameLoadedSignal>().AddListener(OnGameLoaded);
        }

        private void OnDisable()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameLoadedSignal>().RemoveListener(OnGameLoaded);
        }

        private void OnGameLoaded(GameLoadedParameter obj)
        {
            _counter.StartCount();
            _menuManager = obj.Menu;
            _menuManager.SetGameUi(true);
        }
    }
}
