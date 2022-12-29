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

        [Inject]
        private void ConstructGameCounter(GameCounter counter)
        {
            _counter = counter;
        }

        private void Awake()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameLoadedSignal>().AddListener(OnGameLoaded);
        }

        private void OnGameLoaded(GameLoadedParameter obj)
        {
            _counter.StartCount();
            _menuManager = obj.Menu;
            _menuManager.SetGameUi(true);
        }
    }
}
