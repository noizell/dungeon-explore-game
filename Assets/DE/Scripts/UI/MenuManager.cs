using NPP.DE.Core.Services;
using NPP.DE.Core.Signal;
using NPP.DE.Core.State;
using NPP.DE.Misc;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPP.DE.Ui
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneContext _context;

        [SerializeField]
        private GameObject _mainMenuUi;
        [SerializeField]
        private GameObject _gameUi;
        [SerializeField]
        private GameObject _gameMenuUi;

        [Header("Main Menu")]
        [SerializeField]
        private Button _startButton;

        [Header("Game Ui")]
        [SerializeField]
        private Button _mainMenuButton;

        private TransitionManager _transitionManager;
        private SceneLoader _sceneLoader;

        private void OnEnable()
        {
            GameState.States.OnGame += OnGameState;
            GameState.States.OnMenu += OnMenuState;
        }

        private void OnDisable()
        {
            GameState.States.OnGame -= OnGameState;
            GameState.States.OnMenu -= OnMenuState;
        }

        private void Start()
        {
            _transitionManager = PersistentServices.Current.Get<TransitionManager>();
            _sceneLoader = PersistentServices.Current.Get<SceneLoader>();
            _startButton.onClick.AddListener(() => StartGame());
            _mainMenuButton.onClick.AddListener(() => ReturnMainMenu());

            GlobalServices.InstallSceneContext(_context);
        }

        private void OnGameState()
        {
            _mainMenuUi.SetActive(false);
            _gameMenuUi.SetActive(false);
            _gameUi.SetActive(true);
        }

        private void OnMenuState()
        {
            _mainMenuUi.SetActive(true);
            _gameMenuUi.SetActive(false);
            _gameUi.SetActive(false);
        }

        public void SetGameUi(bool active)
        {
            _gameUi.SetActive(active);
        }

        public void SetGameMenuUi(bool active)
        {
            _gameMenuUi.SetActive(active);
        }

        private void StartGame()
        {
            _transitionManager.PlayTransition("Left", () =>
            {
                _transitionManager.PlayTransition("Loading Simple", () =>
                {
                    _transitionManager.DoneTransition("Left");
                    _sceneLoader.LoadScene("Game", () =>
                    {
                        GlobalServices.GameStateTransition();
                        Signals.Hub.Get<SignalCollection.AppState.GameLoadedSignal>().Dispatch(new GameLoadedParameter(this));
                        _transitionManager.DoneTransition("Loading Simple", true);
                    }, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                });
            });
        }

        private void ReturnMainMenu()
        {
            _transitionManager.PlayTransition("Left", () =>
            {
                _transitionManager.PlayTransition("Loading Simple", () =>
                {
                    _transitionManager.DoneTransition("Left");
                    _sceneLoader.LoadScene("Menu", () =>
                    {
                        GlobalServices.GameStateTransition();
                        _transitionManager.DoneTransition("Loading Simple", true);
                    }, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                });
            });
        }
    }
}