using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Collider col;

    [SerializeField]private Dragger currentDragger;
    public Dragger CurrentDragger => currentDragger;
    public Collider Col => col;

    public event Action OnStartDragEvent;
    public event Action OnEndDragEvent;
    public event Action<Vector3> OnThrowEvent;

    public void SetDragger(Dragger dragger)
    {
        currentDragger = dragger;
    }
    
    public virtual void OnStartDrag(Dragger dragger, bool autoChangeInteractable)
    {
        currentDragger = dragger;

        if (col)
        {
            if (autoChangeInteractable) col.enabled = false;
            else col.isTrigger = true;
        }

        OnStartDragEvent?.Invoke();
    }

    public virtual void OnEndDrag(bool autoChangeInteractable)
    {
        currentDragger = null;

        if (col)
        {
            if (autoChangeInteractable) col.enabled = true;
            else col.isTrigger = false;
        }

        OnEndDragEvent?.Invoke();
    }

    public virtual void Throw(Vector3 force)
    {
        OnThrowEvent?.Invoke(force);
    }

    public void ForceActivateCol()
    {
        col.enabled = true;
    }

    public void ForceDeactivateCol()
    {
        col.enabled = false;
    }
}