using NPP.DE.Animations;
using NPP.DE.Core.Services;
using STVR.SimpleDelayer;
using UnityEngine;

namespace NPP.DE.Ui
{
    public class LoadingUiActivator : TransitionUiActivator
    {
        [SerializeField]
        private AnimationClip _endLoadingClip;

        [Tooltip("At what time should animation start looping. set lower than 0 to disable loop.")]
        [SerializeField]
        [Range(-1, 1)] private float _normalizedTimeStartLoop = .1f;

        [SerializeField]
        private bool _callbackTriggerOnce = true;
        private bool _callbackCalled = false;

        public override void PlayTransition(System.Action callback = null)
        {
            _animCallback = PersistentServices.Current.Get<TransitionManager>().AnimationCallbackFactory.Create();
            _animancer.Animator.GetBehaviour<AnimationEvent>().SetOnEnd(_animancer.Animator, _animCallback,
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

        public override void DoneTransition(System.Action unload = null)
        {
            _animancer.Stop(_clip);
            _animancer.Play(_endLoadingClip).Events.OnEnd = () =>
            {
                Delay wait = Delay.CreateCount(.2f, () =>
                {
                    unload?.Invoke();
                }, .1f);
            };
        }
    }
}