using DG.Tweening;
using TMPro;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private float _scaleDuration = 1f;

    [SerializeField] private Color _targetTextColor = Color.red;
    [SerializeField] private float _colorDuration = 1f;
    [SerializeField] private TMP_Text _textToAnimate;

    private int _yoyoLoops = -1;
    private Vector3 _originalScale;
    private Color _originalTextColor;
    private Sequence _animationSequence;

    private void Start()
    {
        _originalScale = transform.localScale;
        _originalTextColor = _textToAnimate.color;
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

    public void StartAnimation()
    {
        StopAnimation();
        _animationSequence = DOTween.Sequence();

        _animationSequence.Join(
            transform.DOScale(_targetScale, _scaleDuration).SetEase(Ease.Linear));
        _animationSequence.Join(
            _textToAnimate.DOColor(_targetTextColor, _colorDuration).SetEase(Ease.Linear));

        _animationSequence.SetLoops(_yoyoLoops, LoopType.Yoyo);
    }

    public void StopAnimation()
    {
        if (_animationSequence != null && _animationSequence.IsActive())
        {
            _animationSequence.Kill();
        }

        transform.localScale = _originalScale;

        if (_textToAnimate != null)
        {
            _textToAnimate.color = _originalTextColor;
        }
    }

}
