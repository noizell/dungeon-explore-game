using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STVR.SimpleDelayer
{
    public class DelayManager : MonoBehaviour
    {
        class DelayAttribute
        {
            public float Count;
            public float CountWait;
            public Coroutine Coroutine;
            public System.Action Callback;
            public System.Action WaitForSecondCallback;

            public DelayAttribute(float count, float countWait)
            {
                Count = count;
                CountWait = countWait;
            }
        }

        private Dictionary<Delay, DelayAttribute> _delayList = new Dictionary<Delay, DelayAttribute>();
        private ICollection<DelayAttribute> _delays => _delayList.Values;

        internal Delay CreateCount(float initCount, float waitForSeconds = 1f)
        {
            Delay _delay = new Delay();
            _delayList.Add(_delay, new DelayAttribute(initCount, waitForSeconds));
            _delayList[_delay].Coroutine = StartCoroutine(StartCount(_delay, initCount, waitForSeconds));
            return _delay;
        }

        internal float GetCurrentCount(Delay delay)
        {
            if (_delayList.ContainsKey(delay))
                return _delayList[delay].Count;
            return 0f;
        }

        internal void StopCount(Delay delay, bool overridePrevCallback, System.Action callback = null)
        {
            if (_delayList.ContainsKey(delay))
            {
                if (_delayList[delay].Count > 0f)
                    _delayList[delay].Count = 0f;

                if (overridePrevCallback)
                    _delayList[delay].Callback = callback;
                //callback?.Invoke();
            }
        }

        internal void StopCountAll(System.Action callback = null)
        {
            foreach (var delay in _delays)
            {
                delay.Callback = null;
                delay.Count = 0f;
            }

            callback?.Invoke();
        }

        private IEnumerator StartCount(Delay delay, float initCount, float waitForSeconds)
        {
            float count = initCount;
            while (count > 0f)
            {
                if (_delayList[delay].Count == 0f)
                {
                    break;
                }

                count -= 1f;
                _delayList[delay].Count = count;
                yield return new WaitForSeconds(waitForSeconds);
                _delayList[delay].WaitForSecondCallback?.Invoke();
            }

            OnDoneCount(delay);

            yield break;
        }

        private void OnDoneCount(Delay delay)
        {
            if (_delayList.ContainsKey(delay))
            {
                _delayList[delay].Callback?.Invoke();
                _delayList.Remove(delay);
            }
        }

        internal Delay CreateCount(float initCount, System.Action doneCallback, float waitForSeconds)
        {
            Delay _delay = new Delay();
            _delayList.Add(_delay, new DelayAttribute(initCount, waitForSeconds));
            _delayList[_delay].Callback = doneCallback;
            _delayList[_delay].Coroutine = StartCoroutine(StartCount(_delay, initCount, waitForSeconds));
            return _delay;
        }

        internal Delay CreateCount(float initCount, System.Action waitForSecondCallback, System.Action doneCallback, float waitForSeconds)
        {
            Delay _delay = new Delay();
            _delayList.Add(_delay, new DelayAttribute(initCount, waitForSeconds));
            _delayList[_delay].Callback = doneCallback;
            _delayList[_delay].WaitForSecondCallback = waitForSecondCallback;
            _delayList[_delay].Coroutine = StartCoroutine(StartCount(_delay, initCount, waitForSeconds));
            return _delay;
        }


    }
}