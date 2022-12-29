using UnityEngine;

namespace STVR.SimpleDelayer.Sample
{
    public class CountTest : MonoBehaviour
    {
        Delay t;
        Delay w;
        // Start is called before the first frame update
        void Start()
        {

            w = Delay.CreateCount(4f);
        }

        private void Update()
        {
            if (t.NotRunning())
            {
                t = Delay.CreateCount(6f);
                Debug.Log("done.");
                t = Delay.CreateCount(9f, OnDoneCounting);
            }

            if (w.Expired())
                Debug.Log("start woy");

            Debug.Log(t.CurrentCount());
        }

        private void OnDoneCounting()
        {
            Debug.Log("whew");
        }
    }
}