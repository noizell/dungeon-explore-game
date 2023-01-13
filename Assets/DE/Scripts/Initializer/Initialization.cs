using Zenject;
using NPP.DE.Misc;
using NPP.DE.Ui;
using NPP.DE.Core.Services;
using NPP.DE.Core.Signal;
using NPP.DE.Core.State;


namespace NPP.DE.Init
{
    public class Initialization
    {
        private SceneLoader _sceneLoader;
        private DiContainer _container;
        private bool _experiment;

        public Initialization(SceneLoader loader, DiContainer container, bool experiment)
        {
            _sceneLoader = loader;
            _container = container;
            _experiment = experiment;
            LoadMenu();
        }

        private void LoadMenu()
        {
            GameState.Initialize();

            GlobalServices.GameStateTransition();

            _sceneLoader.LoadScene(!_experiment ? "Menu" : "_Experiment", () =>
             {
                 _container.Unbind<Initialization>();
                 _sceneLoader.UnloadScene("Initializer");
             },
            UnityEngine.SceneManagement.LoadSceneMode.Additive);

        }

        #region Injection
        [Zenject.Inject]
        private void InstallTransitionManager(IPersistent transitionManager)
            => PersistentServices.Current.Register(transitionManager as TransitionManager);
        [Zenject.Inject]
        private void InstallSceneLoader(SceneLoader loader)
            => PersistentServices.Current.Register(loader);
        [Zenject.Inject]
        private void InstallJSONLoader(JSONSerializer json)
            => PersistentServices.Current.Register(json);
        [Zenject.Inject]
        private void InstallAssetLoader(AssetLoader json)
            => PersistentServices.Current.Register(json);

        #endregion
    }
}
