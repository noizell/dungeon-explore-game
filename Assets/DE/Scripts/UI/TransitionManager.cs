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

        public AnimationCallbackFactory AnimationCallbackFactory
        {
            get
            {
                if (_animFactory == null)
                {
                    _animFactory = PersistentServices.Current.Get<AssetLoader>().Load<AnimationCallbackFactory>("Animation Callback Factory","Factory");
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
                _canvas = GameObject.FindObjectOfType<Canvas>();
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

        public void DoneTransition(string name)
        {
            _canvas = GameObject.FindObjectOfType<Canvas>();
            Transform _spawned = _canvas.transform.Find($"Transition {name}");
            TransitionUiActivator _activator = null;

            if (_spawned != null)
            {
                _activator = _spawned.GetComponentInChildren<TransitionUiActivator>();
                _activator.DoneTransition();
            }
        }
    }
}