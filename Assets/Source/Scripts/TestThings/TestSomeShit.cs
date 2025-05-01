using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSomeShit : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private PlayerHealth _playerHealth;

    private Resource _health;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _playerHealth.TakeDamage(10);
    }
}
