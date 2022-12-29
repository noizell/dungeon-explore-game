using UnityEngine;
using Zenject;
using NPP.DE.Core.Game;

namespace NPP.DE.Installer
{
    [CreateAssetMenu(fileName = "Game Installer", menuName = "NPP/DE/Create new Game Installer")]
    public class GameInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameCounter>().AsSingle();
        }

    }
}