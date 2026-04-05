using UnityEngine;
public class DestroyTimer : MonoBehaviour {
    void Start() { Destroy(gameObject, 2f); } // Deletes itself after 2 seconds
}