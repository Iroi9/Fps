using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class Keypad : Interactable
{
    /// 
    /// Class Kepad definse the interactable Object Keypad
    /// Keypads can open Doors in the Game by interacting with them
    /// 

    /// GameObject door references the GameObject affected by the Keypad
    [SerializeField] private GameObject door;

    /// bool doorOpen stores the state of the door
    private bool doorOpen;
    
    /// <summary>
    /// This method changes the stored state of the door
    /// and triggers animation
    /// </summary>
    /// <param name="gameObject"></param> this parameter is not used, is present because of inhetance
    protected override void Interact(GameObject gameObject)
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("isOpen", doorOpen);
    }
}
