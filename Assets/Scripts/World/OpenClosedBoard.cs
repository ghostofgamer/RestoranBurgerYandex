using System;
using UnityEngine;
using Zenject;

public class OpenClosedBoard : MonoBehaviour
{
    [SerializeField] private TouchInteractive touchInteractive;
    [SerializeField] private GameObject[] closedObjects;
    [SerializeField] private GameObject[] openObjects;
    [SerializeField] private TutorInWorldFocus tutorFocus;

    private bool open;
    public bool Open => open;
    public TutorInWorldFocus TutorFocus => tutorFocus;

    public event Action<bool> OnChangeStatusEvent;

    private void Start()
    {
        Init();
        UpdateVisual();
    }

    private void Init()
    {
        touchInteractive.OnClickEvent += ChangeStatus;
    }

    private void UpdateVisual()
    {
        foreach (var element in closedObjects) element.SetActive(!open);
        foreach (var element in openObjects) element.SetActive(open);
    }

    private void ChangeStatus()
    {
        open = !open;
        UpdateVisual();

        OnChangeStatusEvent?.Invoke(open);
    }
}