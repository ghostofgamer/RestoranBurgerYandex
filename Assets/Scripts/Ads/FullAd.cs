using System;
using Agava.YandexGames;
using UnityEngine;

namespace Ads
{
    public class FullAd : AD
    {
        [SerializeField] private ADSInterTimeOutCounter _adsInterTimeOutCounter;
        
        public override void Show()
        {
            Debug.Log("SHOWSHOWSHOW");
            if (YandexGamesSdk.IsInitialized&&_adsInterTimeOutCounter.IsGranted)
            {
                Debug.Log("SHOW@@@");
                InterstitialAd.Show(OnOpen, OnClose);
                _adsInterTimeOutCounter.StartTimer();
            }
        }

        public override void Show(Action<bool> rewardCallback = null)
        {
            
        }

        public bool GetAdCompleted()
        {
            return IsAdCompleted;
        }
    }
}