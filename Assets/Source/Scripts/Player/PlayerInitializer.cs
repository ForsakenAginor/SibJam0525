using System;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerControll _controller;
    [SerializeField] private SprintController _sprintController;

    private Resource _stamina;

    public void Init(Resource stamina)
    {
        _stamina = stamina != null ? stamina : throw new ArgumentNullException(nameof(stamina));
        _sprintController.Init(_stamina, _controller);
        _controller.Init(_sprintController);
    }
}
