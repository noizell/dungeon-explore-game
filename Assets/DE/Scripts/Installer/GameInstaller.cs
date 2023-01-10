using UnityEngine;
using Zenject;
using NPP.DE.Core.Game;
using NPP.DE.Core.Dungeon.Generator;
using NPP.DE.Core.Character;

namespace NPP.DE.Installer
{
    [CreateAssetMenu(fileName = "Game Installer", menuName = "NPP/DE/Create new Game Installer")]
    public class GameInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameCounter>().AsSingle();
            Container.Bind<MazeRecursiveDFS>().AsSingle();
            Container.Bind<PlayerController>().FromIFactory(x => x.To<PlayerControllerFactory>().
            FromScriptableObjectResource("Factory/Player Controller Factory").AsSingle()).AsSingle();
        }

    }
}