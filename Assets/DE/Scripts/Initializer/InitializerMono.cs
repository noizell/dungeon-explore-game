using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;
using NPP.DE.Ui;
using NPP.DE.Core.Services;

namespace NPP.DE.Init
{
    public class InitializerMono : MonoBehaviour
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {

            //if (!SceneManager.GetSceneByName("Initializer").isLoaded)
            SceneManager.LoadScene("Initializer");
        }
    }
}
