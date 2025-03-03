using Ads;
using UnityEngine;
using Zenject;
using TheSTAR.Data;
using TheSTAR.Sound;

/// <summary>
/// Здесь биндим в контексте всего проекта
/// </summary>
public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameLoader gameControllerPrefab;
    [SerializeField] private SoundController soundControllerPrefab;
    [SerializeField] private FocusScreen _focusScreen;
    [SerializeField] private RewardDelivery _rewardDelivery;
    [SerializeField] private FullAd _fullAd;

    public override void InstallBindings()
    {
        Container.Bind<DataController>().AsSingle();
        Container.Bind<DailyBonusService>().AsSingle();
        Container.Bind<NotificationController>().AsSingle();
        Container.Bind<CurrencyController>().AsSingle();
        Container.Bind<XpController>().AsSingle();
        Container.Bind<AnalyticsManager>().AsSingle();
        Container.Bind<AdsManager>().AsSingle();
        Container.Bind<VibrationController>().AsSingle();
        Container.Bind<AllPrices>().AsSingle();
        Container.Bind<OrdersManager>().AsSingle();

        var soundController = Container.InstantiatePrefabForComponent<SoundController>(soundControllerPrefab,
            soundControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<SoundController>().FromInstance(soundController).AsSingle();

        var gameLoader = Container.InstantiatePrefabForComponent<GameLoader>(gameControllerPrefab,
            gameControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GameLoader>().FromInstance(gameLoader).AsSingle();

        var focusScreen = Container.InstantiatePrefabForComponent<FocusScreen>(_focusScreen,
            _focusScreen.transform.position, Quaternion.identity, null);
        Container.Bind<FocusScreen>().FromInstance(focusScreen).AsSingle();

        var rewardDelivery = Container.InstantiatePrefabForComponent<RewardDelivery>(_rewardDelivery,
            _rewardDelivery.transform.position, Quaternion.identity, null);
        Container.Bind<RewardDelivery>().FromInstance(rewardDelivery).AsSingle();

        var fullAd = Container.InstantiatePrefabForComponent<FullAd>(_fullAd, _fullAd.transform.position, 
            Quaternion.identity, null);
        Container.Bind<FullAd>().FromInstance(fullAd).AsSingle();
    }
}