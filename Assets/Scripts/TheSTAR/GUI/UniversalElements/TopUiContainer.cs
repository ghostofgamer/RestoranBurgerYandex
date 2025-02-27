using System;
using ReputationContent;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TheSTAR.Utility;
using Zenject;

namespace TheSTAR.GUI
{
    public class TopUiContainer : GuiUniversalElement
    {
        [SerializeField] private TextMeshProUGUI softCounter;
        [SerializeField] private Reputation _reputation;
        [SerializeField] private RateFly _profitFly;
        [SerializeField] private RateFly _tipsFly;
        [SerializeField] private RateFly _xpFly;

        private DollarValue _lastProfit = new DollarValue();
        private DollarValue _lastTips = new DollarValue();

        [Header("Buttons")] [SerializeField] private PointerButton shopButton;
        [SerializeField] private PointerButton settingsButton;

        [Space] [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private Image levelFill;

        private GuiController gui;

        public Reputation Reputation => _reputation;

        [Inject]
        private void Consruct(GuiController gui, XpController xp, CurrencyController currency)
        {
            this.gui = gui;
            /*currency.OnTipsChanged += ShowTipsProfit;
            currency.OnCurrencyChanged+=ShowProfit;*/

            currency.OnCurrencyChanged += SetLastProfit;
            currency.OnTipsChanged += SetLastTips;
            
            xp.OnProfitXpValue += ShowXP;
            currency.Subscribe(OnTransactionReact);
            xp.SubscribeOnChangeXp(OnChangeXp);
        }

        private void OnEnable()
        {
            if (_lastProfit.dollars > 0 || _lastProfit.cents > 0)
                ShowProfit(_lastProfit);
            
            if(_lastTips.dollars > 0 || _lastTips.cents > 0)
                ShowTipsProfit(_lastTips);
        }

        public void InitReputation(FastFood fastFood)
        {
            _reputation.Init(fastFood);
        }

        public void InitGameWorldInteractive(GameWorldInteraction gameWorldInteraction)
        {
            _reputation.InitGameWorldInteraction(gameWorldInteraction);
        }

        private void ShowXP(int value)
        {
            _xpFly.Show(value);
        }

        private void SetLastProfit(DollarValue profit)
        {
            _lastProfit = profit;
        }

        private void SetLastTips(DollarValue tips)
        {
            _lastTips = tips;
        }

        private void ShowProfit(DollarValue profit)
        {
            _profitFly.Show(profit, "profit");
            _lastProfit = new DollarValue();
        }

        private void ShowTipsProfit(DollarValue tips)
        {
            _tipsFly.Show(tips, "tips");
            _lastTips = new DollarValue();
        }

        public override void Init()
        {
            base.Init();

            shopButton.Init(() => { gui.Show<ComputerStoreScreen>(); });
            settingsButton.Init(() =>
            {
                var settingsScreen = gui.FindScreen<SettingsScreen>();
                settingsScreen.Init(gui.CurrentScreen, false);
                gui.Show(settingsScreen);
            });
        }

        #region Reacbables

        private void OnTransactionReact(CurrencyType currencyType, DollarValue finalValue)
        {
            if (currencyType != CurrencyType.Soft) return;

            softCounter.text = TextUtility.FormatPrice(finalValue, false);
        }

        private void OnChangeXp(int level, int currentXp, int neededXp)
        {
            levelNumberText.text = $"LEVEL {level + 1}";
            levelFill.fillAmount = (float)currentXp / (float)neededXp;
        }

        #endregion
    }
}