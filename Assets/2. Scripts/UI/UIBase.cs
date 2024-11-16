using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected RectTransform rectTransform;
    
    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
