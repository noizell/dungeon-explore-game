using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;
using NPP.DE.Ui;
using NPP.DE.Core.Services;
using System;

namespace NPP.DE.Init
{
    public class InitializerMono : MonoBehaviour
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Initializer"))
                SceneManager.LoadScene("Initializer");
        }

    }
}
