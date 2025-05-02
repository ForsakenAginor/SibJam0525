using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string IsWalking = nameof(IsWalking);
    private const string IsRunning = nameof(IsRunning);
    private const string IsShooting = nameof(IsShooting);
    private const string IsDying = nameof(IsDying);

    [SerializeField] private Animator _animator;

    public void Walk()
    {
        _animator.SetBool(IsWalking, true);
        _animator.SetBool(IsRunning, false);
        _animator.SetBool(IsShooting, false);
        _animator.SetBool(IsDying, false);
    }

    public void Run()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(IsRunning, true);
        _animator.SetBool(IsShooting, false);
        _animator.SetBool(IsDying, false);
    }

    public void Shoot()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(IsRunning, false);
        _animator.SetBool(IsShooting, true);
        _animator.SetBool(IsDying, false);
    }

    public void Stop()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(IsRunning, false);
        _animator.SetBool(IsShooting, false);
        _animator.SetBool(IsDying, false);
    }

    public void Die()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(IsRunning, false);
        _animator.SetBool(IsShooting, false);
        _animator.SetBool(IsDying, true);
    }
}


