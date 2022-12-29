using Zenject;
using NPP.DE.Misc;
using NPP.DE.Ui;
using NPP.DE.Core.Services;
using NPP.DE.Animations;

namespace NPP.DE.Init
{
    public class Initialization
    {
        private SceneLoader _sceneLoader;
        private DiContainer _container;

        public Initialization(SceneLoader loader, DiContainer container)
        {
            _sceneLoader = loader;
            _container = container;
            LoadMenu();
        }

        private void LoadMenu()
        {
            _sceneLoader.LoadScene("Menu", () =>
            {
                _container.Unbind<Initialization>();
                _sceneLoader.UnloadScene("Initializer");
            },
            UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        #region Injection
        [Inject]
        private void InstallTransitionManager(IPersistent transitionManager)
            => PersistentServices.Current.Register(transitionManager as TransitionManager);
        [Inject]
        private void InstallSceneLoader(SceneLoader loader)
            => PersistentServices.Current.Register(loader);
        [Inject]
        private void InstallJSONLoader(JSONSerializer json)
            => PersistentServices.Current.Register(json);
        [Inject]
        private void InstallAssetLoader(AssetLoader json)
            => PersistentServices.Current.Register(json);
        #endregion
    }
}
