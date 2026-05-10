using UnityEngine;

public class InteractableOject : MonoBehaviour, IInteractable
{
    public bool Interact(PlayerController player)
    {
        Debug.Log("Interacted with " + gameObject.name);
        return true;
    }
}
