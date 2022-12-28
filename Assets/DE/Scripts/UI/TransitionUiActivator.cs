using Animancer;
using NPP.DE.Animations;
using NPP.DE.Core.Services;
using UnityEngine;
using Zenject;

namespace NPP.DE.Ui
{
    public class TransitionUiActivator : MonoBehaviour
    {

        [SerializeField]
        protected AnimationClip _clip;

        [SerializeField]
        protected AnimancerComponent _animancer;

        protected AnimationCallback _animCallback;

        public virtual void PlayTransition(System.Action callback = null)
        {
            //gameObject.SetActive(true);
            _animCallback = PersistentServices.Current.Get<TransitionManager>().AnimationCallbackFactory.Create();
            _animancer.Play(_clip).Events.OnEnd = callback;
        }

        /// <summary>
        /// this will deactivate current transition.
        /// </summary>
        public virtual void DoneTransition()
        {
            _animancer.Stop(_clip);
            //gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public struct TransitionUiActivatorMember
    {
        public string TransitionName;
        public TransitionUiActivator TransitionActivator;
    }
}