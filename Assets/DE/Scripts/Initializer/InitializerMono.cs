using UnityEngine;
using UnityEngine.SceneManagement;

namespace NPP.DE.Init
{

    public class InitializerMono : MonoBehaviour
    {
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            DefaultGameSettings settings = new DefaultGameSettings();

            if (!settings.Get.BypassInitializer)
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Initializer"))
                    SceneManager.LoadScene("Initializer");
        }

    }
}
