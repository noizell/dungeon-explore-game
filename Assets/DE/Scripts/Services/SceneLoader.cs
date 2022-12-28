using NPP.DE.Core.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NPP.DE.Misc
{
    public class SceneLoader : IPersistent
    {
        private string _currentLoadedSceneName;
        private bool _allowActive;
        private System.Action _onComplete;
        private System.Action _onUnloadComplete;

        public void LoadScene(string name, System.Action onCompleteCallback = null, LoadSceneMode loadMode = LoadSceneMode.Single, bool setLoadedActive = true)
        {
            _allowActive = setLoadedActive;
            _currentLoadedSceneName = name;
            _onComplete = onCompleteCallback;
            SceneManager.LoadSceneAsync(name, loadMode).completed += OnLoadComplete;
        }

        private void OnLoadComplete(AsyncOperation op)
        {
            if (_allowActive)
                SetSceneActive(_currentLoadedSceneName);

            _onComplete?.Invoke();
        }

        private void OnUnloadComplete(AsyncOperation obj)
        {
            _onUnloadComplete?.Invoke();
        }


        public bool IsSceneLoaded(string scene)
        {
            return SceneManager.GetSceneByName(scene).isLoaded;
        }

        public void UnloadScene(string scene, System.Action onCompleteCallback = null)
        {
            _onUnloadComplete = onCompleteCallback;
            SceneManager.UnloadSceneAsync(scene).completed += OnUnloadComplete;
        }

        public void SetSceneActive(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
        }
    }
}