using System;
using Agava.YandexGames;

namespace Ads
{
    public class FullAd : AD
    {
        public override void Show()
        {
            if (YandexGamesSdk.IsInitialized)
                InterstitialAd.Show(OnOpen, OnClose);
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