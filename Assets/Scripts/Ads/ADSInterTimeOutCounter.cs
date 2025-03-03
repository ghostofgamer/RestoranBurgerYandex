using System.Collections;
using UnityEngine;

namespace Ads
{
    public class ADSInterTimeOutCounter : MonoBehaviour
    {
        private float _elapsedTime;
        private float _delay = 1f;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds;

        public bool IsGranted;
        
        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(_delay);
            IsGranted = true;
        }

        public void StartTimer()
        {
            if(_coroutine!=null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Timer());
        }
        
        private IEnumerator Timer()
        {
            IsGranted = false;
            yield return _waitForSeconds;
            IsGranted = true;
        }
    }
}