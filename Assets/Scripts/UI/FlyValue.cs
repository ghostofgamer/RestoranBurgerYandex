using TMPro;
using UnityEngine;

namespace UI
{
    public class FlyValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _flyDuration = 1.0f;
        [SerializeField] private float _flyHeight = 3.0f;
        [SerializeField] private Transform startPosition;
        
        private Vector3 _startPosition;
        private Coroutine _flyCoroutine;
        private Color _color;
        
        public void Show()
        {
            
        }
    }
}