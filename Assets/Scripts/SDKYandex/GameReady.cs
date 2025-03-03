using Agava.YandexGames;
using UnityEngine;

namespace SDKYandex
{
    public class GameReady : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
YandexGamesSdk.GameReady();
             Debug.Log("READY!");
             Debug.Log("READY!");
             Debug.Log("READY!");
#endif
        }
    }
}