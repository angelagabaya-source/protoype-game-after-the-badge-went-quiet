using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        // This 'Instance' check is what makes it work across scene loads!
        if (PersistentMusic.Instance != null)
        {
            PersistentMusic.Instance.PlaySFX(PersistentMusic.Instance.clickSound);
        }
    }
}