using NPP.DE.Core.Factory;
using UnityEngine;

namespace NPP.DE.Animations
{

    [CreateAssetMenu(fileName = "Animation Callback Factory", menuName = "NPP/DE/Factories/Create Animation Callback Factory")]
    public class AnimationCallbackFactory : BaseFactory<AnimationCallback>
    {
        public override AnimationCallback Create()
        {
            return new AnimationCallback();
        }
    }
}