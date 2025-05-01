using Assets.Scripts.General;
using UnityEngine;
using UnityEngine.UI;

public class MenuRoot : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void Start()
    {
        SceneChangerSingleton.Instance.FadeOut();

        _playButton.onClick.AddListener(OnPlayButtonClick);
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
