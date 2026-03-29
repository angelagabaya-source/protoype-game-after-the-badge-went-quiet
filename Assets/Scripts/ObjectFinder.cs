using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ObjectFinder : MonoBehaviour
{
    private GameManager gm;
    public LayerMask itemLayer; // Set this to "HiddenItems" in the Inspector

    void Start()
    {
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 1. Ignore if clicking UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            // 2. Raycast ONLY against the Item Layer
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // We added "itemLayer" here so it ignores the floor/walls
            if (Physics.Raycast(ray, out hit, 100f, itemLayer))
            {
                Debug.Log("Hit valid item: " + hit.collider.gameObject.name);

                if (hit.collider.CompareTag("HiddenObject"))
                {
                    if (gm != null)
                    {
                        gm.CrossOffItem(hit.collider.gameObject.name);
                        hit.collider.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // If we didn't hit anything on the Item Layer, it's a miss
                if (gm != null) gm.SubtractTime(5f);
                Debug.Log("Missed! No hidden item found at this position.");
            }
        }
    }
}