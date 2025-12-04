using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Buttons")]
    [Tooltip("Array berisi semua script LevelButton di scene")]
    public LevelButton[] allLevelButtons;

    [Header("Debug Settings")]
    [Tooltip("Aktifkan untuk reset progress (unlock hanya level 1)")]
    public bool resetProgressOnStart = false;

    void Start()
    {
        // Debug: Reset progress jika diperlukan
        if (resetProgressOnStart)
        {
            PlayerPrefs.SetInt("LevelTerbuka", 1);
            PlayerPrefs.Save();
            Debug.Log("🔄 Progress direset - Hanya Level 1 yang terbuka");
        }

        // Ambil data progress pemain dari PlayerPrefs
        // Key: "LevelTerbuka" - Menyimpan level tertinggi yang bisa dimainkan
        // Default: 1 - Level 1 selalu terbuka untuk pemain baru
        int highestLevel = PlayerPrefs.GetInt("LevelTerbuka", 1);
        Debug.Log($"📊 Progress Pemain: Level tertinggi terbuka = {highestLevel}");

        // Validasi array tidak kosong
        if (allLevelButtons == null || allLevelButtons.Length == 0)
        {
            Debug.LogError("❌ LevelSelectionManager: Array allLevelButtons kosong! Assign di Inspector.");
            return;
        }

        // Loop semua tombol level dan atur status lock/unlock
        foreach (LevelButton button in allLevelButtons)
        {
            // Skip jika button null (element kosong di array)
            if (button == null)
            {
                Debug.LogWarning("⚠️ Ada element NULL di array allLevelButtons!");
                continue;
            }

            // Logika Lock/Unlock berdasarkan progress
            if (button.levelIndex <= highestLevel)
            {
                // Level sudah dibuka (player sudah unlock)
                button.SetStatus(false); // false = UNLOCK
            }
            else
            {
                // Level masih terkunci (player belum mencapai level ini)
                button.SetStatus(true); // true = LOCK
            }
        }

        Debug.Log($"✅ Level Selection Manager siap - Total {allLevelButtons.Length} level");
    }

    /// <summary>
    /// Unlock level berikutnya setelah player menyelesaikan level
    /// Dipanggil dari script lain (misalnya: saat player menang di level)
    /// </summary>
    /// <param name="levelCompleted">Level yang baru saja diselesaikan</param>
    public void UnlockNextLevel(int levelCompleted)
    {
        int currentHighest = PlayerPrefs.GetInt("LevelTerbuka", 1);
        
        // Hanya update jika level yang diselesaikan adalah level tertinggi
        if (levelCompleted >= currentHighest)
        {
            int newHighest = levelCompleted + 1;
            PlayerPrefs.SetInt("LevelTerbuka", newHighest);
            PlayerPrefs.Save();
            
            Debug.Log($"🎉 Level {newHighest} berhasil di-unlock!");
        }
    }

    /// <summary>
    /// Reset semua progress (untuk testing atau tombol reset)
    /// </summary>
    public void ResetAllProgress()
    {
        PlayerPrefs.SetInt("LevelTerbuka", 1);
        PlayerPrefs.Save();
        
        Debug.Log("🔄 Semua progress direset!");
        
        // Refresh tampilan
        Start();
    }

    /// <summary>
    /// Unlock semua level (untuk testing atau cheat)
    /// </summary>
    public void UnlockAllLevels()
    {
        if (allLevelButtons != null && allLevelButtons.Length > 0)
        {
            int maxLevel = allLevelButtons.Length;
            PlayerPrefs.SetInt("LevelTerbuka", maxLevel);
            PlayerPrefs.Save();
            
            Debug.Log($"🔓 Semua {maxLevel} level di-unlock!");
            
            // Refresh tampilan
            Start();
        }
    }
}
