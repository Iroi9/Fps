using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    /// <summary>
    /// This class handels the interaction of the player
    /// </summary> 
    [SerializeField]private float distance = 3f;
    [SerializeField]private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    /// <summary>
    /// On start iniziales Components
    /// </summary>
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        
    }

    /// <summary>
    /// On each frame shoots out a ray and gets the Interactable hit by it
    /// also updates UI with promtMessage of the Interactable
    /// and calls the Interactable's function
    /// </summary>
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance, mask))
        {
            if(hit.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promtMessage);
                if (inputManager.onFootActions.Interact.triggered)
                {
                    interactable.BaseInteract(interactable.gameObject);
                }
            }
        }
    }
}
