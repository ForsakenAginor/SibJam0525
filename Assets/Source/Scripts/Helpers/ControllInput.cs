using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllInput : MonoBehaviour
{
    public ControllsBase controlls { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        controlls = new ControllsBase();
    }

    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        controlls.Enable();
    }

    public void Deactivate()
    {
        controlls.Disable();
    }
}
