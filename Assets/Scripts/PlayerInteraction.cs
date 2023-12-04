using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    
    public LayerMask interactableLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            TryInteract();
        }
    }

    private void TryInteract()
    {
        Debug.Log("Try interact");
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0));

        if (!Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
        {
            return;
        }
        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        Debug.Log(hit.transform.name);

        interactable?.Interact();
    }
}
