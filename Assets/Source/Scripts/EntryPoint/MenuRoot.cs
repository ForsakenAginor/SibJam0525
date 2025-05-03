using Assets.Scripts.General;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _authorsButton;
    [SerializeField] private Button _closeAuthorsButton;
    [SerializeField] private SwitchableElement _buttonsPanel;
    [SerializeField] private SwitchableElement _authorsPanel;

    private void Start()
    {
        Time.timeScale = 0f;
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _authorsButton.onClick.AddListener(OnAuthorsOpen);
        _closeAuthorsButton.onClick.AddListener(OnCloseAuthorsButton);

        Time.timeScale = 1f;
        SceneChangerSingleton.Instance.FadeOut();
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
        _authorsButton.onClick.RemoveListener(OnAuthorsOpen);
        _closeAuthorsButton.onClick.RemoveListener(OnCloseAuthorsButton);
    }

    private void OnAuthorsOpen()
    {
        _authorsPanel.Enable();
        _buttonsPanel.Disable();
    }

    private void OnCloseAuthorsButton()
    {
        _authorsPanel.Disable();
        _buttonsPanel.Enable();
    }

    private void OnPlayButtonClick()
    {
        SceneChangerSingleton.Instance.LoadScene(Scenes.Game.ToString());
    }
}
