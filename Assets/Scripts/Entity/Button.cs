using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, ISelectable
{
    //[SerializeField] private Material defaultBtnMaterial, hoverBtnMaterial;
    //[SerializeField] MeshRenderer btnRenderer;

    public UnityEvent onPush;
    public UnityEvent onHoverEnter, onHoverExit;

    public void OnSelect()
    {
        onPush.Invoke();
    }
    
    public void OnHoverEnter()
    {
        //btnRenderer.material = hoverBtnMaterial;
        onHoverEnter?.Invoke();
    }

    public void OnHoverExit()
    {
        //btnRenderer.material = defaultBtnMaterial;
        onHoverExit?.Invoke();
    }
}