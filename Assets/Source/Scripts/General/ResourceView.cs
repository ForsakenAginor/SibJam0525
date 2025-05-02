using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private Image _image; 
    private IResource _resource;

    private void OnDestroy()
    {
        if (_resource != null)
        {
            _resource.ResourcesAmountChanged -= OnResourceChanged;
        }
    }

    public void Init(IResource resource)
    {
        _resource = resource;
        UpdateImageFill(); 

        _resource.ResourcesAmountChanged += OnResourceChanged;
    }

    private void OnResourceChanged()
    {
        UpdateImageFill(); 
    }

    private void UpdateImageFill()
    {
        
        float fillAmount = _resource.Amount / _resource.Maximum;
        _image.fillAmount = fillAmount; 
    }
}

