using Ads;
using UnityEngine;
using TheSTAR.Data;
using Zenject;
using TheSTAR.Sound;

namespace TheSTAR.GUI
{
    public class SettingsScreen : GuiScreen
    {
        [SerializeField] private PointerButton closeButton;
        [SerializeField] private PointerToggle soundsToggle;
        [SerializeField] private PointerToggle musicToggle;
        [SerializeField] private PointerToggle vibrationsToggle;
        [SerializeField] private PointerButton rateUsButton;

        [Header("Cheats")] [SerializeField] private GameObject cheatsContainer;

        [SerializeField] private PointerButton cheatAddCurrencyBtn;
        [SerializeField] private PointerButton cheatAddXpBtn;
        [SerializeField] private PointerButton cheatClearCurrencyBtn;
        [SerializeField] private PointerButton cheatTestTerminalBtn;

        private GameController game;
        private GuiController gui;
        private DataController data;
        private CurrencyController currency;
        private XpController xp;
        private SoundController sounds;
        private FullAd _fullAd;
        private GuiScreen from;

        [Inject]
        private void Construct(GameController game, DataController data, GuiController gui, CurrencyController currency,
            XpController xp, SoundController sounds,FullAd fullAd)
        {
            this.game = game;
            this.data = data;
            this.gui = gui;
            this.currency = currency;
            this.xp = xp;
            this.sounds = sounds;
            _fullAd = fullAd;
        }

        public void Init(GuiScreen from, bool useMainMenuFon)
        {
            this.from = from;
            SetUseMainMenuFon(useMainMenuFon);
        }

        private void ExitFromSettings()
        {
            gui.Show(from);
            
#if UNITY_WEBGL && !UNITY_EDITOR
            _fullAd.Show();
#else
            Debug.Log("Full ad is not shown because this is not a web build.");
#endif
        }

        public override void Init()
        {
            base.Init();

            closeButton.Init(ExitFromSettings);

            soundsToggle.Init(data.gameData.settingsData.isSoundsOn, OnToggleSounds);
            musicToggle.Init(data.gameData.settingsData.isMusicOn, OnToggleMusic);
            vibrationsToggle.Init(data.gameData.settingsData.isVibrationOn, OnToggleVibration);

            cheatAddCurrencyBtn.Init(() => { currency.AddCurrency(new(100, 0)); });

            cheatAddXpBtn.Init(() =>
            {
                xp.AddXp(100); // .AddCurrency(new(100, 0));
            });

            cheatClearCurrencyBtn.Init(() => { currency.ClearCurrency(CurrencyType.Soft); });

            rateUsButton.Init(() =>
            {
                var rateUsScreen = gui.FindScreen<RateUsScreen>();
                rateUsScreen.Init(ExitFromSettings, UseMainMenuFon);
                gui.Show(rateUsScreen);
            });

            cheatTestTerminalBtn.Init(() => { gui.Show<TerminalScreen>(); });
        }

        protected override void OnShow()
        {
            base.OnShow();
            cheatsContainer.SetActive(game.UseCheats);
        }

        protected override void OnHide()
        {
            base.OnHide();
            data.Save(DataSectionType.Settings);
        }

        private void OnToggleSounds(bool value)
        {
            data.gameData.settingsData.isSoundsOn = value;
        }

        private void OnToggleMusic(bool value)
        {
            data.gameData.settingsData.isMusicOn = value;

            if (value) game.PlayRandomMusic();
            else sounds.StopMusic();
        }

        private void OnToggleVibration(bool value)
        {
            data.gameData.settingsData.isVibrationOn = value;
        }
    }
}