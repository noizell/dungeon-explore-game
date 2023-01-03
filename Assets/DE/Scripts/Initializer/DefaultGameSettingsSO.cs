using UnityEngine;

namespace NPP.DE.Init
{
    public class DefaultGameSettings
    {
        private DefaultGameSettingsSO _settings;

        public DefaultGameSettingsSO Get
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<DefaultGameSettingsSO>("Dungeon Explorer Default Settings");
                }
                return _settings;
            }
        }

        public DefaultGameSettings()
        {

        }
    }

    [CreateAssetMenu(fileName = "Dungeon Explorer Default Settings", menuName = "NPP/DE/Create Default Settings")]
    public class DefaultGameSettingsSO : ScriptableObject
    {
        [SerializeField]
        private bool _bypassInitializer;

        public bool BypassInitializer => _bypassInitializer;
    }
}
