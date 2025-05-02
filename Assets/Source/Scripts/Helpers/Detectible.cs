using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSpace;

public class Detectible : MonoBehaviour, IDetectible
{
    public static List<IDetectible> entities=new List<IDetectible>();

    [field: SerializeField] public List<Transform> spotPoints { get; private set; }
    [field: SerializeField] public int side { get; set; }
    

    private void OnEnable()
    {
        entities.Add(this);
    }

    private void OnDisable()
    {
        entities.Remove(this);
    }
}
