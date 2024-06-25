using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the abstract Class Interactable
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    public string promtMessage;

    /// <summary>
    /// public method to execute the interaction
    /// </summary>
    /// <param name="gameObject"></param> is the interacted object
    public void BaseInteract(GameObject gameObject)
    {
        Interact(gameObject);
    }
    /// <summary>
    /// method to overide
    /// </summary>
    /// <param name="gameObject"></param> is the interacted object
    protected virtual void Interact(GameObject gameObject)
    {

    }
}
