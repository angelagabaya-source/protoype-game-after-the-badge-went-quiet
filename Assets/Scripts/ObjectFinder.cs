using UnityEngine;
using UnityEngine.InputSystem; // Required for Mouse.current

public class ObjectFinder : MonoBehaviour
{
    [Header("Settings")]
    public string targetLayerName = "HiddenItems"; 
    public string targetTag = "HiddenObject";
    public float timePenalty = 5f; // How many seconds to lose on a miss

    void Update()
    {
        // Check for mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PerformClick();
        }
    }

    void PerformClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // Create a LayerMask that ONLY looks for the "HiddenItems" layer
        int layerMask = LayerMask.GetMask(targetLayerName);

        // Shoot the ray
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // SUCCESS: We hit a True Object on the correct layer
            if (hit.collider.CompareTag(targetTag))
            {
                Debug.Log("Found: " + hit.collider.name);
                FoundObject(hit.collider.gameObject);
            }
        }
        else
        {
            // MISS: The laser hit nothing on the HiddenItems layer (it hit clutter/walls)
            ApplyPenalty();
        }
    }

    void FoundObject(GameObject obj)
    {
        // For now, we hide the object. 
        // Later, we can add a 'Ding' sound or a particle effect here.
        obj.SetActive(false);
    }

    void ApplyPenalty()
    {
        Debug.LogWarning("Missed! Subtracting time...");
        
        // Find the GameManager in the scene and tell it to subtract time
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        
        if (gm != null)
        {
            gm.SubtractTime(timePenalty);
        }
        else
        {
            Debug.LogError("No GameManager found in the scene! Make sure you created one.");
        }
    }
}