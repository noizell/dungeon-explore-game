using NPP.DE.Animations;
using NPP.DE.Core.Services;
using UnityEngine;

namespace NPP.DE.Ui
{
    public class LoadingUiActivator : TransitionUiActivator
    {
        [Tooltip("At what time should animation start looping. set lower than 0 to disable loop.")]
        [SerializeField]
        [Range(-1, 1)] private float _normalizedTimeStartLoop = .1f;

        [SerializeField]
        private bool _callbackTriggerOnce = true;
        private bool _callbackCalled = false;

        public override void PlayTransition(System.Action callback = null)
        {
            _animCallback = PersistentServices.Current.Get<TransitionManager>().AnimationCallbackFactory.Create();
            _animancer.Animator.GetBehaviour<CallbackEvent>().SetOnEnd(_animancer.Animator, _animCallback,
                () =>
                {
                    if (!_callbackCalled)
                    {
                        _callbackCalled = true;
                        callback?.Invoke();
                        if (!_callbackTriggerOnce)
                            _callbackCalled = false;
                    }

                    if (_normalizedTimeStartLoop >= 0)
                    {
                        _animancer.Animator.Play(_clip.name, 0, _normalizedTimeStartLoop);
                        PlayTransition(callback);
                    }
                });
        }
    }
}