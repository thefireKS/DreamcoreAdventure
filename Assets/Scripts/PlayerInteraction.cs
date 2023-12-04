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

    private void OnDrawGizmos()
    {
        Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0)), Camera.main.transform.forward);
    }

    private void TryInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0));
        Physics.Raycast(ray, out var hit, interactionRange, interactableLayer);
        //Physics.Raycast(ray, out var hit2, interactionRange);
        //Debug.Log($"{hit2.transform.name}");
        if (!hit.transform)
        {
            return;
        }
        
        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        
        interactable?.Interact();
    }
}
