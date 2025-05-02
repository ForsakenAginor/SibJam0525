using UnityEngine;
using UnityEngine.UI;

public class ResourceSliderView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private IResource _resource;

    private void OnDestroy()
    {
        _resource.ResourcesAmountChanged -= OnResourceChanged;
    }

    public void Init(IResource resource)
    {
        _resource = resource;
        _slider.maxValue = _resource.Maximum;
        _slider.value = _resource.Amount;

        _resource.ResourcesAmountChanged += OnResourceChanged;
    }

    private void OnResourceChanged()
    {
        _slider.value = _resource.Amount;
    }
}
