using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Ads
{
    public abstract  class RewardedAd : AD
    {
        [SerializeField] private Button _button;

        protected Action<bool> RewardCallback;
        
        public override void Show(Action<bool> rewardCallback)
        {
            RewardCallback = rewardCallback;
            
            if (YandexGamesSdk.IsInitialized)
                VideoAd.Show(OnOpen, OnReward, OnClose);
        }

        protected abstract void OnReward();

        protected override void OnClose()
        {
            base.OnClose();

            if (_button != null)
                _button.enabled = true;
        }
    }
}