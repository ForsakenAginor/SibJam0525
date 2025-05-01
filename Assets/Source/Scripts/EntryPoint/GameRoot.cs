using Assets.Scripts.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button[] _exitButtons;
    [SerializeField] private Button _restartButton;
    [SerializeField] private SwitchableElement _loseScreen;
    [SerializeField] private SwitchableElement _winScreen;
    [SerializeField] private SwitchableElement _buttonCanvas;
    [SerializeField] private PickapableOverlay _overlay;

    [Header("Bars")]
    [SerializeField] private ResourceView _playerHealthBar;
    [SerializeField] private ResourceView _playerStaminaBar;
    [SerializeField] private ResourceView _playerMoneyBar;

    [Header("Player")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStamina;
    [SerializeField] private int _maxMoney;
    [SerializeField] private PlayerInitializer _playerInitializer;

    [Header("Loot")]
    [SerializeField] private List<Pickapable> _loot;

    [Header("lifeCycle")]
    [SerializeField] private GameLifeCycle _lifeCycle;

    private Resource _playerHealth;
    private Resource _money;

    private void Start()
    {
        Time.timeScale = 0f;

        _playerHealth = new Resource(_maxHealth);
        Resource stamina = new Resource(_maxStamina);
        _money = new Resource(0, _maxMoney);
        InitPlayerResourceViews(stamina);
        Wallet wallet = new Wallet(_loot, _money);
        _lifeCycle.Init(_money);
        _playerInitializer.Init(stamina, _playerHealth);

        foreach (var item in _loot)
            item.Init(_overlay);

        Subscribe();
        Time.timeScale = 1f;
        SceneChangerSingleton.Instance.FadeOut();
    }


    private void OnDestroy()
    {
        _lifeCycle.PlayerWon -= OnPlayerWon;
        _playerHealth.ResourceOver -= OnPlayerDying;
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);

        foreach (var button in _exitButtons)
            button.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void Subscribe()
    {
        _lifeCycle.PlayerWon += OnPlayerWon;
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _playerHealth.ResourceOver += OnPlayerDying;

        foreach (var button in _exitButtons)
            button.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnRestartButtonClick()
    {
        SceneChangerSingleton.Instance.LoadScene(Scenes.Game.ToString());
    }

    private void OnPlayerWon()
    {
        Time.timeScale = 0f;
        _buttonCanvas.Disable();
        _winScreen.Enable();
    }

    private void OnPlayerDying()
    {
        Time.timeScale = 0f;
        _buttonCanvas.Disable();
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
