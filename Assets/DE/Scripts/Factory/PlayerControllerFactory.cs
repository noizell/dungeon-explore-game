using NPP.DE.Core.Factory;
using UnityEngine;

namespace NPP.DE.Core.Character
{
    [CreateAssetMenu(fileName = "Player Controller Factory", menuName = "NPP/DE/Factories/Create Player Controller Factory")]
    public class PlayerControllerFactory : BaseFactory<PlayerController>
    {
        public override PlayerController Create()
        {
            return new PlayerController();
        }
    }
}