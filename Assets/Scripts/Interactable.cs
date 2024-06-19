using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promtMessage;

    public void BaseInteract(Interactable gameObject)
    {
        Interact(gameObject);
    }
    
   protected virtual void Interact(Interactable gameObject)
    {

    }
}
