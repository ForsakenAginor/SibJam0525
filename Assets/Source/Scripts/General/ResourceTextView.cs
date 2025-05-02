using TMPro;
using UnityEngine;

public class ResourceTextView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private IResource _resource;

    private void OnDestroy()
    {
        _resource.ResourcesAmountChanged -= OnResourceChanged;
    }

    public void Init(IResource resource)
    {
        _resource = resource;
        OnResourceChanged();

        _resource.ResourcesAmountChanged += OnResourceChanged;
    }

    private void OnResourceChanged()
    {
        _text.text = $"{_resource.Amount}/{_resource.Maximum}";
    }
}
