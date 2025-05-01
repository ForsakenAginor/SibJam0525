using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    float phase;
    [SerializeField] Vector2 scale;
    [SerializeField] float speed;
    public void Shake(float power)
    {
        phase += Time.deltaTime * power *speed;
        transform.localPosition = new Vector3((Mathf.Cos(phase))*power*scale.x, (Mathf.Sin(phase*2)) * power*scale.y,0);
        transform.localRotation = Quaternion.LookRotation(new Vector3((Mathf.Cos(phase+Mathf.PI)) * power * scale.x, (Mathf.Sin(phase * 2 + Mathf.PI)) * power * scale.y, 2),Vector3.up+Vector3.right*Mathf.Sin(phase)*power*scale.x);
    }
}
