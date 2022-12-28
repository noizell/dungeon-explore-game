using NPP.DE.Animations;
using NPP.DE.Core.Services;
using NPP.DE.Misc;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPP.DE.Ui
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;
        private TransitionManager _transitionManager;

        private void Start()
        {
            _transitionManager = PersistentServices.Current.Get<TransitionManager>();
            _startButton.onClick.AddListener(() => StartGame());
        }

        private void StartGame()
        {
            _transitionManager.PlayTransition("Left", () =>
            {
                _transitionManager.PlayTransition("Loading Simple", () =>
                {
                    _transitionManager.DoneTransition("Left");
                });
            });
        }
    }
}