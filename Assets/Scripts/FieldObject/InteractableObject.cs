using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public bool Interact(PlayerInteraction player)
    {
        Debug.Log("Interacted with " + gameObject.name);
        return true;
    }
}
