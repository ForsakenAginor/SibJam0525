using Assets.Scripts.General;
using UnityEngine;
using UnityEngine.UI;

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void Start()
    {
        Time.timeScale = 0f;
        _playButton.onClick.AddListener(OnPlayButtonClick);

        Time.timeScale = 1f;
        SceneChangerSingleton.Instance.FadeOut();
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        SceneChangerSingleton.Instance.LoadScene(Scenes.Game.ToString());
    }
}
