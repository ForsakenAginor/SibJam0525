using TMPro;
using UnityEngine;

public class KimchiOverlay : SwitchableElement
{
    [SerializeField] private TMP_Text _priceField;

    public void SetText(int value)
    {
        _priceField.text = $"- {value} $";
    }
}