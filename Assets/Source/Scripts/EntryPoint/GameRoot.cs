using Assets.Scripts.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private SwitchableElement _loseScreen;

    [Header("Bars")]
    [SerializeField] private ResourceView _playerHealthBar;
    [SerializeField] private ResourceView _playerStaminaBar;
    [SerializeField] private ResourceView _playerMoneyBar;

    [Header("Player stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStamina;
    [SerializeField] private int _maxMoney;

    [Header("Loot")]
    [SerializeField] private List<Pickapable> _loot;

    private Resource _playerHealth;
    private Resource _money;

    private void Start()
    {
        _playerHealth = new Resource(_maxHealth);
        Resource stamina = new Resource(_maxStamina);
        _money = new Resource(0, _maxMoney);
        InitPlayerResourceViews(stamina);
        Wallet wallet = new Wallet(_loot, _money);

        _exitButton.onClick.AddListener(OnPlayButtonClick);
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _playerHealth.ResourceOver += OnPlayerDying;
        SceneChangerSingleton.Instance.FadeOut();
    }

    private void OnDestroy()
    {
        _playerHealth.ResourceOver -= OnPlayerDying;
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        _exitButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void OnRestartButtonClick()
    {
        SceneChangerSingleton.Instance.LoadScene(Scenes.Game.ToString());
    }

    private void OnPlayerDying()
    {
        _loseScreen.Enable();
    }

    private void InitPlayerResourceViews(Resource stamina)
    {
        _playerHealthBar.Init(_playerHealth);
        _playerStaminaBar.Init(stamina);
        _playerMoneyBar.Init(_money);
    }

    private void OnPlayButtonClick()
    {
        SceneChangerSingleton.Instance.LoadScene(Scenes.Menu.ToString());
    }
}
