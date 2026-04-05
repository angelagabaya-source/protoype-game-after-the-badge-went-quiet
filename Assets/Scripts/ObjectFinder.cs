using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectFinder : MonoBehaviour
{
    private GameManager gm;
    public LayerMask itemLayer; 
    public float maxDistance = 1000f; 

    [Header("Discovery Effects")]
    public GameObject vfxPrefab; // <-- The slot for your Particle System Prefab

    void Start()
    {
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Don't click through UI menus
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // Debug line in Scene View to see your click
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 2f);

            if (Physics.Raycast(ray, out hit, maxDistance, itemLayer))
            {
                GameObject hitObj = hit.collider.gameObject;
                
                if (hitObj.CompareTag("HiddenObject"))
                {
                    // SPAWN THE EFFECT
                    if (vfxPrefab != null)
                    {
                        // hit.point = the exact surface spot where the ray touched the 3D mesh
                        Instantiate(vfxPrefab, hit.point, Quaternion.identity);
                    }

                    if (gm != null)
                    {
                        gm.CrossOffItem(hitObj.name);

                        // Handle ItemGroups or single objects
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
            }
            else
            {
                // Penalty for clicking into empty 3D space
                if (gm != null) gm.SubtractTime(5f);
            }
        }
    }
}