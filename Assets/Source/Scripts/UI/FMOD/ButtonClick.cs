using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class  ButtonClick : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private EventReference _sound;
    [SerializeField] private EventReference _switch;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        AudioManager.Instance.PlayOneShot(_sound, transform.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(_switch, transform.position);
    }
}
