using System;
using UnityEngine;

public class GameLifeCycle : MonoBehaviour
{
    [SerializeField] private LevelEscape _escapeCollider;
    [SerializeField] private SwitchableElement _moneyBar;

    private Resource _money;
    private GameStage _stage;

    public event Action PlayerWon;

    private void OnDestroy()
    {
        _money.ResourceFulfill += OnMoneyFulfill;
        _escapeCollider.PlayerEscaped -= OnPlayerEscaped;
    }

    public void Init(Resource money)
    {
        _money = money != null ? money : throw new ArgumentNullException(nameof(money));
        _stage = GameStage.Collecte;
        _money.ResourceFulfill += OnMoneyFulfill;
        _escapeCollider.PlayerEscaped += OnPlayerEscaped;
    }

    private void OnPlayerEscaped()
    {
        PlayerWon?.Invoke();
    }

    private void OnMoneyFulfill()
    {
        _stage = GameStage.Escape;
        _moneyBar.Disable();
        _escapeCollider.Enable();
    }
}

public enum GameStage
{
    Collecte,
    Escape,
}
