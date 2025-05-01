using UnityEngine;

public class SprintController : MonoBehaviour
{
    private const int Step = 1;

    [SerializeField] private int _drainValue = 15;
    [SerializeField] private int _recoveryValue = 5;
    [SerializeField] private int _threshold = 5;

    private Resource _stamina;
    private ISprinter _sprinter;
    private float _accumulator;

    public bool CanSprint => _stamina.Amount > _threshold;

    private void Update()
    {
        if (_sprinter.IsSprinting)
        {
            _accumulator += _drainValue * Time.deltaTime;

            if (_accumulator > Step)
            {
                _accumulator -= Step;
                _stamina.Spent(Step);
            }
        }
        else
        {
            _accumulator -= _recoveryValue * Time.deltaTime;

            if (_accumulator < 0)
            {
                _accumulator += Step;
                _stamina.Add(Step);
            }
        }
    }

    public void Init(Resource stamina, ISprinter sprinter)
    {
        _stamina = stamina;
        _sprinter = sprinter;
    }

}
