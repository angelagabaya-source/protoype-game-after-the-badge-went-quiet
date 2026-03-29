using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectFinder : MonoBehaviour
{
    public GameManager gm;

    void Update()
    {
        // 1. Check for Mouse Click (New Input System)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 2. IMPORTANT: Ignore click if it's on a UI Button/Panel
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; 
            }

            // 3. Raycast to find objects
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("HiddenObject"))
                {
                    gm.CrossOffItem(hit.collider.gameObject.name);
                    hit.collider.gameObject.SetActive(false);
                }
                else
                {
                    gm.SubtractTime(5f);
                }
            }
        }
    }
}