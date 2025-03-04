using System;
using UnityEngine;
using TMPro;
using Zenject;

namespace TheSTAR.GUI
{
    public class EnterNameScreen : GuiScreen
    {
        [SerializeField] private PointerButton closeButton;
        [SerializeField] private PointerButton acceptButton;
        [SerializeField] private TMP_InputField inputField;
        
        private Action<string> onAcceptAction;

        private GuiController gui;

        [Inject]
        private void Construct(GuiController gui)
        {
            this.gui = gui;
        }

        public override void Init()
        {
            Debug.Log("+++");
            base.Init();

            closeButton.Init(() =>
            {
                gui.ShowMainScreen();
                
                if (!Application.isMobilePlatform)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            });

            acceptButton.Init(() =>
            {
                onAcceptAction?.Invoke(inputField.text);
                gui.ShowMainScreen();
                
                if (!Application.isMobilePlatform)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            });
        }

        public void Init(Action<string> onAcceptAction)
        {
            if (!Application.isMobilePlatform)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;  
            }
            
            Debug.Log("!!!");
            this.onAcceptAction = onAcceptAction;
        }
    }
}