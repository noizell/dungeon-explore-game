using NPP.DE.Core.Factory;
using System.Collections.Generic;
using UnityEngine;

namespace NPP.DE.Animations
{
    public class AnimationCallback : IFactoryMember
    {
        private List<System.Action> _onEndCallbackList = new List<System.Action>();
        private List<System.Action> _onStartCallbackList = new List<System.Action>();
        private Animator _anim;

        public void RegisterOnEndCallback(Animator anim, System.Action callback)
        {
            _anim = anim;
            _onEndCallbackList.Add(callback);
        }

        public void RegisterOnStartCallback(Animator anim, System.Action callback)
        {
            _anim = anim;
            _onStartCallbackList.Add(callback);
        }

        public void TriggerOnEndCallback(Animator anim)
        {
            ProcessCallback(anim, _onEndCallbackList);
        }

        public void TriggerOnStartCallback(Animator anim)
        {
            ProcessCallback(anim, _onStartCallbackList);
        }

        private void ProcessCallback(Animator anim, List<System.Action> callback)
        {
            if (_anim == anim)
            {
                for (int i = 0; i < callback.Count; i++)
                {
                    callback[i]?.Invoke();
                }

                for (int i = 0; i < callback.Count; i++)
                {
                    callback.Remove(callback[i]);
                }
            }
        }
    }
}