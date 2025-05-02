using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _openSettingsButton;
    [SerializeField] private Button _closeSettingsButton;
    [SerializeField] private SwitchableElement _settingsPanel;
    [SerializeField] private SwitchableElement _buttonCanvas;

    private Vector2 _defaultPosition = new Vector2(0.5f, 0.5f);
    private RectTransform _settingPanelTransform;

    private void Start()
    {
        _settingPanelTransform = _settingsPanel.GetComponent<RectTransform>();
        _closeSettingsButton.interactable = false;
        _openSettingsButton.onClick.AddListener(OnOpen);
        _closeSettingsButton.onClick.AddListener(OnClose);
    }

    private void OnDestroy()
    {
        _openSettingsButton.onClick.RemoveListener(OnOpen);
        _closeSettingsButton.onClick.RemoveListener(OnClose);
    }

    private void OnClose()
    {
        _closeSettingsButton.interactable = false;

        _settingsPanel.transform.localScale = Vector2.one;
        _settingPanelTransform.anchorMax = _defaultPosition;
        _settingPanelTransform.anchorMin = _defaultPosition;
        Vector2 currentPosition = _defaultPosition;

        _settingsPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InCubic);
        DOTween.To(() => currentPosition, x =>
            {
                currentPosition = x;
                _settingPanelTransform.anchorMax = currentPosition;
                _settingPanelTransform.anchorMin = currentPosition;
            },
            Vector2.one,
            0.2f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                _buttonCanvas.Enable();
                _settingsPanel.Disable();
            });
    }

    private void OnOpen()
    {
        _buttonCanvas.Disable();
        _settingsPanel.transform.localScale = Vector2.zero;
        _settingsPanel.Enable();
        _settingPanelTransform.anchorMax = Vector2.one;
        _settingPanelTransform.anchorMin = Vector2.one;
        Vector2 currentPosition = Vector2.one;

        _settingsPanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutQuint);
        DOTween.To(() => currentPosition, x =>
            {
                currentPosition = x;
                _settingPanelTransform.anchorMax = currentPosition;
                _settingPanelTransform.anchorMin = currentPosition;
            },
            _defaultPosition,
            0.4f)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => _closeSettingsButton.interactable = true);
    }
}
