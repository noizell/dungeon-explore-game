using NPP.DE.Animations;
using NPP.DE.Core.Services;
using NPP.DE.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace NPP.DE.Ui
{

    public class TransitionManager : IPersistent
    {
        private Dictionary<string, TransitionUiActivator> _transitions;
        private Canvas _canvas = null;

        private AnimationCallbackFactory _animFactory;
        private SceneLoader _sceneLoader;

        public AnimationCallbackFactory AnimationCallbackFactory
        {
            get
            {
                if (_animFactory == null)
                {
                    _animFactory = PersistentServices.Current.Get<AssetLoader>().Load<AnimationCallbackFactory>("Animation Callback Factory", "Factory");
                }

                return _animFactory;
            }
        }

        private TransitionManager(params TransitionUiActivatorMember[] transitions)
        {
            _transitions = new Dictionary<string, TransitionUiActivator>();
            for (int i = 0; i < transitions.Length; i++)
            {
                _transitions.Add(transitions[i].TransitionName, transitions[i].TransitionActivator);
            }
        }

        internal GameObject GetLoadingActivatorPrefab()
        {
            return _transitions["Loading Simple"].gameObject;
        }

        public void PlayTransition(string name, System.Action callback = null)
        {
            if (_transitions.ContainsKey(name))
            {
                if (_sceneLoader == null)
                {
                    _sceneLoader = PersistentServices.Current.Get<SceneLoader>();
                }

                if (!_sceneLoader.IsSceneLoaded("Loading"))
                {
                    _sceneLoader.LoadScene("Loading", () =>
                    {
                        _canvas = GameObject.FindGameObjectWithTag("Transition Canvas").GetComponent<Canvas>();
                        Transform _spawned = _canvas.transform.Find($"Transition {name}");
                        TransitionUiActivator _activator = null;

                        if (_spawned != null)
                        {
                            _activator = _spawned.GetComponentInChildren<TransitionUiActivator>();
                        }

                        if (_activator == null)
                        {
                            var g = GameObject.Instantiate(_transitions[name], _canvas.transform);
                            g.gameObject.name = $"Transition {name}";

                            _activator = g;
                        }

                        _activator.PlayTransition(callback);
                    }, UnityEngine.SceneManagement.LoadSceneMode.Additive, false);
                }
                else
                {
                    _canvas = GameObject.FindGameObjectWithTag("Transition Canvas").GetComponent<Canvas>();
                    Transform _spawned = _canvas.transform.Find($"Transition {name}");
                    TransitionUiActivator _activator = null;

                    if (_spawned != null)
                    {
                        _activator = _spawned.GetComponentInChildren<TransitionUiActivator>();
                    }

                    if (_activator == null)
                    {
                        var g = GameObject.Instantiate(_transitions[name], _canvas.transform);
                        g.gameObject.name = $"Transition {name}";

                        _activator = g;
                    }

                    _activator.PlayTransition(callback);
                }
            }
        }

        public void DoneTransition(string name, bool unloadScene = false)
        {
            _canvas = GameObject.FindGameObjectWithTag("Transition Canvas").GetComponent<Canvas>();
            Transform _spawned = _canvas.transform.Find($"Transition {name}");
            TransitionUiActivator _activator = null;

            if (_spawned != null)
            {
                _activator = _spawned.GetComponentInChildren<TransitionUiActivator>();
                if (unloadScene)
                    _activator.DoneTransition(UnloadScene);
                else
                    _activator.DoneTransition();
            }
        }

        public void UnloadScene()
        {
            if (_sceneLoader.IsSceneLoaded("Loading"))
            {
                _sceneLoader.UnloadScene("Loading");
            }

        }
    }
}