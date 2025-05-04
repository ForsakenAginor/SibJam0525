using Assets.Scripts.General;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private EventReference _sound;
    [SerializeField] private Transform _player;

    [Header("UI")]
    [SerializeField] private Button[] _exitButtons;
    [SerializeField] private Button[] _restartButtons;
    [SerializeField] private EndgameAnimation _loseScreen;
    [SerializeField] private SwitchableElement _winScreen;
    [SerializeField] private SwitchableElement _buttonCanvas;
    [SerializeField] private PickapableOverlay _overlay;

    [Header("Bars")]
    [SerializeField] private ResourceSliderView _playerHealthBar;
    [SerializeField] private ResourceSliderView _playerStaminaBar;
    [SerializeField] private ResourceSliderView _playerMoneyBar;
    [SerializeField] private ResourceTextView _playerHealthText;
    [SerializeField] private ResourceTextView _playerStaminaText;
    [SerializeField] private ResourceTextView _playerMoneyText;

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

        AudioManager.Instance.PlayOneShot(_sound, _player.position);
        Subscribe();
        Time.timeScale = 1f;
        SceneChangerSingleton.Instance.FadeOut();
    }


    private void OnDestroy()
    {
        _lifeCycle.PlayerEscaped -= OnPlayerWon;
        _lifeCycle.PlayerLoose -= OnPlayerDying;
        _playerHealth.ResourceOver -= OnPlayerDying;

        foreach(var button in _restartButtons)
            button.onClick.RemoveListener(OnRestartButtonClick);

        foreach (var button in _exitButtons)
            button.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void Subscribe()
    {
        _lifeCycle.PlayerEscaped += OnPlayerWon;
        _lifeCycle.PlayerLoose += OnPlayerDying;
        _playerHealth.ResourceOver += OnPlayerDying;

        foreach (var button in _restartButtons)
            button.onClick.AddListener(OnRestartButtonClick);

        foreach (var button in _exitButtons)
            button.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnRestartButtonClick()
    {
        AudioManager.Instance.StopAllSounds();
        SceneChangerSingleton.Instance.LoadScene(Scenes.Game.ToString(), true);
    }

    private void OnPlayerWon()
    {
        Time.timeScale = 0f;
        _buttonCanvas.Disable();
        _winScreen.Enable();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnPlayerDying()
    {
        Time.timeScale = 0f;
        _buttonCanvas.Disable();
        _loseScreen.PlayAnimation();
        Cursor.lockState = CursorLockMode.None;
    }

    private void InitPlayerResourceViews(Resource stamina)
    {
        _playerHealthBar.Init(_playerHealth);
        _playerStaminaBar.Init(stamina);
        _playerMoneyBar.Init(_money);
        _playerHealthText.Init(_playerHealth);
        _playerStaminaText.Init(stamina);
        _playerMoneyText.Init(_money);
    }

    private void OnPlayButtonClick()
    {
        AudioManager.Instance.StopAllSounds();
        SceneChangerSingleton.Instance.LoadScene(Scenes.Menu.ToString());
    }
}
