using UnityEngine;
using Zenject;
using NPP.DE.Misc;
using NPP.DE.Ui;
using NPP.DE.Init;
using NPP.DE.Core.Game;

namespace NPP.DE.Installer
{

    [CreateAssetMenu(fileName = "Core Installer", menuName = "NPP/DE/Create new Core Installer")]
    public class CoreInstaller : ScriptableObjectInstaller
    {

        [Header("Transition Manager Component")]
        [SerializeField]
        private TransitionUiActivatorMember[] _transitionList;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<AssetLoader>().AsSingle();
            Container.Bind<JSONSerializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<TransitionManager>().AsSingle().WithArguments(_transitionList);

            Container.Bind<Initialization>().AsSingle().NonLazy();
        }
    }
}