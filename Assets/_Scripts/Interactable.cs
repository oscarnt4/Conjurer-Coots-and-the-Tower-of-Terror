using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] public string promptMessage;

    //Function to be called from player script
    public void BaseInteract()
    {
        Interact();
        Debug.Log("interacted with " + gameObject.name);
    }

    protected virtual void Interact()
    {
        //Function to be overwritten by subclasses
    }

    //Function to be called from player script
    public void BaseReleaseInteract()
    {
        ReleaseInteract();
        Debug.Log("Finished interacting with " + gameObject.name);
    }

    protected virtual void ReleaseInteract()
    {
        //Function to be overwritten by subclasses

    }
}
