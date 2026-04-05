using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectFinder : MonoBehaviour
{
    private GameManager gm;
    public LayerMask itemLayer; 
    public float maxDistance = 1000f; 

    void Start()
    {
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // DRAW THE LINE: If this line doesn't touch your object, the collider is missing!
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 3f);

            if (Physics.Raycast(ray, out hit, maxDistance, itemLayer))
            {
                GameObject hitObj = hit.collider.gameObject;
                
                // DIAGNOSTIC LOG: This tells us exactly what Unity sees
                Debug.Log($"<color=cyan>HIT DETECTED:</color> {hitObj.name} | Layer: {LayerMask.LayerToName(hitObj.layer)} | Tag: {hitObj.tag}");

                if (hitObj.CompareTag("HiddenObject"))
                {
                    if (gm != null)
                    {
                        gm.CrossOffItem(hitObj.name);

                        if (hitObj.transform.parent != null && hitObj.transform.parent.CompareTag("ItemGroup"))
                        {
                            hitObj.transform.parent.gameObject.SetActive(false);
                        }
                        else
                        {
                            hitObj.SetActive(false);
                        }
                    }
                }
                else 
                {
                    Debug.LogWarning($"<color=orange>REJECTED:</color> {hitObj.name} is on the right layer but is NOT tagged 'HiddenObject'!");
                }
            }
            else
            {
                // If the Raycast fails completely, check if there's a hidden wall blocking it
                if (Physics.Raycast(ray, out RaycastHit wallHit, maxDistance))
                {
                    Debug.Log($"<color=red>BLOCKED:</color> You clicked, but hit '{wallHit.collider.gameObject.name}' on layer '{LayerMask.LayerToName(wallHit.collider.gameObject.layer)}' which is NOT in your ItemLayer mask.");
                }
                
                if (gm != null) gm.SubtractTime(5f);
            }
        }
    }
}