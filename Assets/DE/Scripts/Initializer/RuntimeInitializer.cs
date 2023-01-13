using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
namespace NPP.DE.Init
{

    public class RuntimeInitializer
    {
        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            if (SceneManager.GetActiveScene().name != "Initializer")
            {
                SceneManager.LoadSceneAsync("Initializer").completed += (AsyncOperation op) =>
                {
                    SceneContext _installer = GameObject.FindObjectOfType<SceneContext>();
                    Core.Services.GlobalServices.InstallSceneContext(_installer);
                    _initialized = true;
                };

            }

            if (!_initialized)
            {
                _initialized = true;
                SceneContext _installer = GameObject.FindObjectOfType<SceneContext>();
                Core.Services.GlobalServices.InstallSceneContext(_installer);
            }
        }

    }
}
