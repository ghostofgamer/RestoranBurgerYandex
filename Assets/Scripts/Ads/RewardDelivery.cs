using System;
using Ads;

public class RewardDelivery : RewardedAd
{
    protected override void OnReward()
    {
        if (RewardCallback != null)
        {
            RewardCallback(true);
        }
    }

    public override void Show()
    {
    }

    public override void Show(Action<bool> rewardCallback)
    {
        base.Show(rewardCallback);
    }
}