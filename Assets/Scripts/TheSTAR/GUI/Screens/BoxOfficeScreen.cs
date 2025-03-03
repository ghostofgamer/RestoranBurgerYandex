using System;
using System.Collections.Generic;
using Ads;
using UnityEngine;
using Zenject;

namespace TheSTAR.GUI
{
    public class BoxOfficeScreen : GuiScreen
    {   
        [Space]
        [SerializeField] private UnityDictionary<int, IndexCallbackButton> dollarButtons;

        [Space]
        [SerializeField] private UnityDictionary<int, IndexCallbackButton> centButtons;

        [Space]
        [SerializeField] private PointerButton closeBtn;
        [SerializeField] private PointerButton acceptBtn;
        [SerializeField] private PointerButton resetBtn;
        [SerializeField] private PointerButton deleteLastBtn;

        private DollarValue neededValue;
        private Action successAction;

        private DollarValue currentValue;

        private Stack<DollarValue> lastValues = new(); // последнее что вводил игок

        public event Action<DollarValue> OnUpdateGivingEvent;

        private GuiController gui;
        private FullAd _fullAd;

        [Inject]
        private void Construct(GuiController gui,FullAd fullAd)
        {
            this.gui = gui;
            _fullAd = fullAd;
        }

        public override void Init()
        {
            base.Init();

            foreach (var dollarInfo in dollarButtons.KeyValues)
            {
                dollarInfo.Value.Init(dollarInfo.Key, OnDollarClick);
            }

            foreach (var centInfo in centButtons.KeyValues)
            {
                centInfo.Value.Init(centInfo.Key, OnCentsClick);
            }

            closeBtn.Init(gui.ShowMainScreen);
            resetBtn.Init(ResetChangeValue);
            deleteLastBtn.Init(DeleteLast);
            acceptBtn.Init(() =>
            {
                if (currentValue.dollars != neededValue.dollars || currentValue.cents != neededValue.cents) return;
                
#if UNITY_WEBGL && !UNITY_EDITOR
            _fullAd.Show();
#else
                Debug.Log("Full ad is not shown because this is not a web build.");
#endif
                
                successAction?.Invoke();
            });
        }

        public void Init(DollarValue neededValue, Action successAction)
        {
            this.neededValue = neededValue;
            this.successAction = successAction;
        }

        protected override void OnShow()
        {
            base.OnShow();
            ResetChangeValue();
        }

        protected override void OnHide()
        {
            base.OnHide();
            ResetChangeValue();
        }

        private void ResetChangeValue()
        {
            currentValue = new();
            lastValues = new();
            OnUpdateGivingEvent?.Invoke(currentValue);
        }

        private void DeleteLast()
        {
            if (lastValues == null || lastValues.Count == 0) return;

            currentValue -= lastValues.Pop();
            OnUpdateGivingEvent?.Invoke(currentValue);
        }

        private void OnDollarClick(int value)
        {
            var newValue = new DollarValue(value, 0);
            lastValues.Push(newValue);
            currentValue += newValue;
            OnUpdateGivingEvent?.Invoke(currentValue);
        }

        private void OnCentsClick(int value)
        {
            var newValue = new DollarValue(0, value);
            lastValues.Push(newValue);
            currentValue += newValue;
            OnUpdateGivingEvent?.Invoke(currentValue);
        }
    }
}