using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectFinder : MonoBehaviour
{
    private GameManager gm;
    public LayerMask itemLayer; 
    public float maxDistance = 1000f; 

    [Header("Discovery Effects")]
    public GameObject vfxPrefab; 

    void Start() { gm = Object.FindFirstObjectByType<GameManager>(); }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, itemLayer))
            {
                GameObject hitObj = hit.collider.gameObject;
                
                if (hitObj.CompareTag("HiddenObject"))
                {
                    // SFX: Play Correct Sound
                    if (PersistentMusic.Instance) PersistentMusic.Instance.PlaySFX(PersistentMusic.Instance.correctSound);

                    if (vfxPrefab != null)
                    {
                        // Add a tiny offset to hit.normal so VFX doesn't clip into the mesh
                        Instantiate(vfxPrefab, hit.point + (hit.normal * 0.05f), Quaternion.identity);
                    }

                    if (gm != null)
                    {
                        gm.CrossOffItem(hitObj.name);
                        if (hitObj.transform.parent != null && hitObj.transform.parent.CompareTag("ItemGroup"))
                            hitObj.transform.parent.gameObject.SetActive(false);
                        else
                            hitObj.SetActive(false);
                    }
                }
            }
            else
            {
                // SFX: Play Wrong Sound
                if (PersistentMusic.Instance) PersistentMusic.Instance.PlaySFX(PersistentMusic.Instance.wrongSound);
                if (gm != null) gm.SubtractTime(5f);
            }
        }
    }
}