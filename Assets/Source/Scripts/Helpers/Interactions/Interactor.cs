using NSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class Interactor : MonoBehaviour, IInteractor
{
    Camera viever;
    ControllsBase controlls;
    [SerializeField] float interactionDist;
    [SerializeField] LayerMask interactionLayerMask;
    

    IFocusable currentFocus;
    // Start is called before the first frame update
    void Start()
    {
        viever = GetComponent<Camera>();
        controlls = GetComponentInParent<ControllInput>().controlls;
        //controlls.CommonControlls.MouseMove.performed += CheckFocus;
        controlls.CharacterControlls.Interact.started += (context) => { if (currentFocus != null && currentFocus is IInteractable) (currentFocus as IInteractable).Interact(this, true);  };
        controlls.CharacterControlls.Interact.canceled += (context) => { if (currentFocus != null && currentFocus is IInteractable) (currentFocus as IInteractable).Interact(this, false); };

    }

    private void Update()
    {
        CheckFocus();
    }

    void CheckFocus()
    {
        
        if(Physics.Raycast(viever.transform.position,viever.transform.forward, out RaycastHit f, interactionDist, interactionLayerMask))
        {
            Debug.DrawLine(viever.transform.position, f.point, Color.red);
           
            IFocusable c = f.collider.GetComponentInParent<IFocusable>();
            if (currentFocus != c)
            {
                if (currentFocus != null)
                {
                    currentFocus.Unfocus(this);
                }
                currentFocus = c;
                if (currentFocus != null)
                {
                    currentFocus.Focus(this);
                }
            }
        }
        else if (currentFocus != null)
        {
           
            currentFocus.Unfocus(this);
            currentFocus = null;
        }
    }

    
}
