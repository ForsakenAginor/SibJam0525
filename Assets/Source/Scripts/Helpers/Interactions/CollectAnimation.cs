using UnityEngine;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;

[RequireComponent(typeof(Renderer))]
public class CollectAnimation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float _rotationDuration = 2f;
    [SerializeField] private RotateMode _rotateMode = RotateMode.FastBeyond360;
    private Vector3 _rotationEndValue = new Vector3(0, 360, 0);

    [Header("Scale Settings")]
    [SerializeField] private Vector3 _scaleEndValue = Vector3.zero;
    [SerializeField] private float _scaleDuration = 2f;

    [Header("Material Settings")]
    [SerializeField] private float _alphaClippingEndValue = 1f;
    [SerializeField] private float _alphaClippingDuration = 2f;
    [SerializeField] private string _shaderPropertyName = "_AlphaClipping";

    [SerializeField] private EventReference _sound;
    private EventInstance eventInstance;

    private Material _targetMaterial;

    private Sequence _animationSequence;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        _targetMaterial = new Material(renderer.material);
        renderer.material = _targetMaterial;

        eventInstance = RuntimeManager.CreateInstance(_sound);
        eventInstance.setParameterByName("ItemsType", 2);
    }

    private void OnDestroy()
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        eventInstance.release();
    }

    public void StartAnimation()
    {
        eventInstance.start();
        _animationSequence = DOTween.Sequence();

        _animationSequence.Join(
            transform.DORotate(_rotationEndValue, _rotationDuration, _rotateMode)
                .SetEase(Ease.InSine)
        );

        _animationSequence.Join(
            transform.DOScale(_scaleEndValue, _scaleDuration)
                .SetEase(Ease.InQuad)
        );

        _animationSequence.Join(
            transform.DOMoveY(transform.position.y + 0.5f, _scaleDuration)
                .SetEase(Ease.InQuad)
        );

        _animationSequence.Join(
            _targetMaterial.DOFloat(_alphaClippingEndValue, _shaderPropertyName, _alphaClippingDuration)
            .SetEase(Ease.Linear)
        );

        _animationSequence.Play().OnComplete(() =>
        {
            Destroy(_targetMaterial);
            Destroy(gameObject);
        });
    }
}
