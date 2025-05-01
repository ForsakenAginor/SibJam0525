using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSomeShit : MonoBehaviour
{
    [SerializeField] private ResourceView _view;
    [SerializeField] private Button _button;

    private Resource _health;

    private void Start()
    {
        _health = new Resource(10, 50);
        _view.Init(_health);

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _health.Spent(1);
    }
}
