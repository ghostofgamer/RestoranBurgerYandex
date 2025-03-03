using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDKYandex
{
    public class SDKInitialization : MonoBehaviour
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(OnInitialized);
        }

        private void OnInitialized()
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!");
            SceneManager.LoadScene(1);
        }
    }
}