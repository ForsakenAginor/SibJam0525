using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _openSettingsButton;
    [SerializeField] private Button _closeSettingsButton;
    [SerializeField] private SwitchableElement _settingsPanel;

    private Vector3 _defaultPosition;

    private void Start()
    {
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
        _settingsPanel.transform.localScale = Vector2.zero;
        throw new NotImplementedException();
    }

    private void OnOpen()
    {
        throw new NotImplementedException();
    }
}
