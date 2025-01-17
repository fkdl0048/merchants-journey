using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontColorChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI button;
    [SerializeField] private Color hoverColor;
    private Color originalColor;
    
    void Start()
    {
        originalColor = button.color;
    }
    
    public void ChangeColorToHover()
    {
        button.color = hoverColor;
    }
    
    public void ChangeColorToOriginal()
    {
        button.color = originalColor;
    }
}
