using System;
using UnityEngine;
using Zenject;

namespace Ads
{
    public  abstract class AD : MonoBehaviour
    {
        [SerializeField]private FocusScreen _focusScreen;
    
        private int _activeValue = 1;
        private int _inactiveValue = 0;

        protected bool IsAdCompleted = false;
    
        public abstract void Show();
        
        public abstract void Show(Action<bool> rewardCallback = null);
        
        [Inject]
        private void Consruct(
           FocusScreen focusScreen)
        {
            _focusScreen = focusScreen;
        }
        
        public void SetValueAdCompleted(bool isCompleted)
        {
            IsAdCompleted = isCompleted;
        }

        protected void OnOpen()
        {
            _focusScreen.SetValueWork(false);
            SetValue(_inactiveValue);
        }

        protected void OnClose(bool isClosed)
        {
            CloseCallback();
        }

        protected virtual void OnClose()
        {
            CloseCallback();
        }

        private void CloseCallback()
        {
            _focusScreen.SetValueWork(true);
            SetValueAdCompleted(true);
            SetValue(_activeValue); 
        }

        private void SetValue(int value)
        {
            Time.timeScale = value;
            AudioListener.volume = value;
        }
    }
}