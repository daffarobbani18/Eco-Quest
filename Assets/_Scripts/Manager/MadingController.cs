using UnityEngine;

public class MadingController : MonoBehaviour
{
    public LevelSelectionUI scriptUI; // Referensi ke script UI

    void OnMouseDown()
    {
        if (scriptUI != null)
        {
            scriptUI.BukaPanel(); // Panggil fungsi buka panel
        }
    }
}