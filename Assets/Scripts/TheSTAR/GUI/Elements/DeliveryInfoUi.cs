using TheSTAR.GUI;
using TMPro;
using UnityEngine;
using TheSTAR.Utility;
using System;
using LocalizationContent;

public class DeliveryInfoUi : MonoBehaviour
{
    private const string English = "en";
    private const string Russian = "ru";
    private const string Turkish = "tr";
    
    [SerializeField] private TextMeshProUGUI topText;
    [SerializeField] private PointerButton skipForAdBtn;
    [SerializeField] private PointerButton skipForFreeBtn;
    [SerializeField] private GameObject _PressGText;

    private string nearestDeliveryText = "DELIVERY VIA:";

    private Action onClickSkipForAd;
    private Action onClickSkipForFree;

    private void Start()
    {
        _PressGText.SetActive(!Application.isMobilePlatform);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (skipForAdBtn.gameObject.activeSelf)
            {
                onClickSkipForAd?.Invoke();
            }
            else if(skipForFreeBtn.gameObject.activeSelf)
            {
                onClickSkipForFree?.Invoke();
            }
        }
    }

    public void Init(Action onClickSkipForAd, Action onClickSkipForFree)
    {
        skipForAdBtn.Init(onClickSkipForAd);
        skipForFreeBtn.Init(onClickSkipForFree);

        this.onClickSkipForAd = onClickSkipForAd;
        this.onClickSkipForFree = onClickSkipForFree;
    }

    public void SetInfo(GameTimeSpan time)
    {
      var localization =   Localization.Instance.GetCurrentLanguage();

      nearestDeliveryText = localization switch
      {
          English => "DELIVERY VIA:",
          Turkish => "TESLİMAT ŞEKLİ:",
          Russian => "ДОСТАВКА ЧЕРЕЗ:",
          _ => "DELIVERY VIA:"
      };

      topText.text = $"{nearestDeliveryText} {TextUtility.TimeToText(time)}";
    }

    public void SetForFree(bool forFree)
    {
        skipForAdBtn.gameObject.SetActive(!forFree);
        skipForFreeBtn.gameObject.SetActive(forFree);
    }
}