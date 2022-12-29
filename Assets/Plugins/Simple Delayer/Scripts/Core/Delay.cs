using UnityEngine;

namespace STVR.SimpleDelayer
{
    public class Delay
    {
        private bool _started;
        private static bool _initialized;
        private static DelayManager _delayManager;

        public bool Started => _started;

        public Delay()
        {
            Initialize();
            _started = true;
        }

        private static void Initialize()
        {
            if (!_initialized)
            {
                _initialized = true;
                if (_delayManager == null)
                {
                    if (GameObject.Find("Delay Manager") != null)
                    {
                        _delayManager = GameObject.Find("Delay Manager").GetComponent<DelayManager>();
                        return;
                    }

                    _delayManager = new GameObject("Delay Manager").AddComponent<DelayManager>();
                    Object.DontDestroyOnLoad(_delayManager);
                }
            }
        }

        public void ForceEnd(bool overridePrevCallback = false, System.Action callback = null)
        {
            _delayManager.StopCount(this, overridePrevCallback, callback);
        }


        public bool Expired()
        {
            if (!_started) return true;
            return _delayManager.GetCurrentCount(this) == 0f;
        }

        public float CurrentCount()
        {
            return _delayManager.GetCurrentCount(this);
        }

        public static Delay CreateCount(float initCount, float waitForSeconds = 1f)
        {
            Initialize();
            return _delayManager.CreateCount(initCount, waitForSeconds);
        }

        public static Delay CreateCount(float initCount, System.Action doneCallback, float waitForSeconds = 1f)
        {
            Initialize();
            return _delayManager.CreateCount(initCount, doneCallback, waitForSeconds);
        }

        public static Delay CreateCount(float initCount, System.Action waitForSecondCallback, System.Action doneCallback, float waitForSeconds = 1f)
        {
            Initialize();
            return _delayManager.CreateCount(initCount, waitForSecondCallback, doneCallback, waitForSeconds);
        }

        public static void ForceEndAll(System.Action callback = null)
        {
            _delayManager.StopCountAll(callback);
        }
    }
}