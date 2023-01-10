using NPP.DE.Animations;
using UnityEngine;
using Zenject;

namespace NPP.DE.Installer
{
    [CreateAssetMenu(fileName = "Animation Callback Installer", menuName = "NPP/DE/Create new Animation Callback Installer")]
    public class AnimationCallbackInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AnimationCallback>().FromIFactory(x=> x.To<AnimationCallbackFactory>().
            FromScriptableObjectResource("Factory/Animation Callback Factory").AsSingle()).AsSingle();
        }
    }
}