using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndgameAnimation : MonoBehaviour
{
    [SerializeField] private SwitchableElement _panel;
    [SerializeField] private SwitchableElement _button;
    [SerializeField] private float _duration;

    public void PlayAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        _panel.Enable();
        sequence.Append(_panel.transform.DOScale(Vector3.one, _duration).SetEase(Ease.Linear));
        sequence.AppendCallback(() => _button.Enable());
        sequence.Append(_button.transform.DOScale(Vector3.one, _duration).SetEase(Ease.Linear));
        sequence.OnComplete(() => _button.GetComponent<Button>().interactable = true);
        sequence.Play();
    }
}
