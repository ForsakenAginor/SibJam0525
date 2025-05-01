using System;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerControll _controller;
    [SerializeField] private SprintController _sprintController;
    [SerializeField] private PlayerHealth _playerHealth;

    private Resource _stamina;
    private Resource _health;

    public void Init(Resource stamina, Resource health)
    {
        _stamina = stamina != null ? stamina : throw new ArgumentNullException(nameof(stamina));
        _health = health != null ? health : throw new ArgumentNullException(nameof(health));
        _playerHealth.Init(_health);
        _sprintController.Init(_stamina, _controller);
        _controller.Init(_sprintController);
    }
}
