using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectFinder : MonoBehaviour
{
    private GameManager gm;
    public LayerMask itemLayer; // Set this to your "HiddenItems" layer
    public float maxDistance = 500f; // High distance to fix the "Camera Face" issue

    void Start()
    {
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // 1. Check for Mouse Click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 2. Ignore if clicking UI (Menus/Buttons)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            // 3. Create the Raycast
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // 4. Shoot the Raycast only at the Item Layer
            if (Physics.Raycast(ray, out hit, maxDistance, itemLayer))
            {
                GameObject hitObj = hit.collider.gameObject;
                Debug.Log("Hit: " + hitObj.name);

                // 5. Check if it's a Hidden Object
                if (hitObj.CompareTag("HiddenObject"))
                {
                    if (gm != null)
                    {
                        // Cross it off the list in GameManager using the object's name
                        gm.CrossOffItem(hitObj.name);

                        // --- THE GROUP CLEAR LOGIC ---
                        if (hitObj.transform.parent != null)
                        {
                            // If the frame is inside an 'Empty', hide the whole Empty
                            hitObj.transform.parent.gameObject.SetActive(false);
                            Debug.Log("Cleared Group: " + hitObj.transform.parent.name);
                        }
                        else
                        {
                            // If it's a single object with no parent, just hide itself
                            hitObj.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                // If we hit nothing on the Item Layer, subtract time
                if (gm != null) gm.SubtractTime(5f);
                Debug.Log("Miss! Nothing on the item layer was hit.");
            }
        }
    }
}