using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Level Info")]
    [Tooltip("Urutan level (1, 2, 3, dst)")]
    public int levelIndex;
    
    [Tooltip("Nama scene yang akan dimuat saat tombol diklik")]
    public string sceneName;

    [Header("UI References")]
    [Tooltip("GameObject overlay gembok (aktif jika level terkunci)")]
    public GameObject padlockOverlay;
    
    [Tooltip("Komponen Button dari GameObject ini")]
    public Button buttonComponent;

    void Start()
    {
        // Auto-assign button component jika belum diassign manual
        if (buttonComponent == null)
        {
            buttonComponent = GetComponent<Button>();
        }

        // Setup listener untuk tombol
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnClickLevel);
        }
        else
        {
            Debug.LogError($"❌ LevelButton [{levelIndex}]: Button component tidak ditemukan!");
        }
    }

    /// <summary>
    /// Mengatur status level (terkunci atau terbuka)
    /// </summary>
    /// <param name="isLocked">True = level terkunci, False = level terbuka</param>
    public void SetStatus(bool isLocked)
    {
        if (isLocked)
        {
            // Level Terkunci
            if (padlockOverlay != null)
                padlockOverlay.SetActive(true);
            
            if (buttonComponent != null)
                buttonComponent.interactable = false;
            
            Debug.Log($"🔒 Level {levelIndex} terkunci");
        }
        else
        {
            // Level Terbuka
            if (padlockOverlay != null)
                padlockOverlay.SetActive(false);
            
            if (buttonComponent != null)
                buttonComponent.interactable = true;
            
            Debug.Log($"✅ Level {levelIndex} terbuka");
        }
    }

    /// <summary>
    /// Fungsi yang dipanggil saat tombol level diklik
    /// </summary>
    public void OnClickLevel()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"❌ LevelButton [{levelIndex}]: Scene Name belum diisi!");
            return;
        }

        Debug.Log($"🎮 Loading Level {levelIndex}: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        // Cleanup listener untuk mencegah memory leak
        if (buttonComponent != null)
        {
            buttonComponent.onClick.RemoveListener(OnClickLevel);
        }
    }
}
