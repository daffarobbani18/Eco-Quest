using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Wajib ada untuk IEnumerator

public class ProcessingLevelManager : MonoBehaviour
{
    public static ProcessingLevelManager Instance;

    [Header("Referensi Sistem")]
    public WasteSpawner mesinSpawner;
    public BriefingSequence briefingScript;

    [Header("Data Level")]
    public LevelData dataLevelIni;
    
    [Tooltip("Isi 1 untuk Kantin, 2 untuk Lab IPA, 3 untuk Gudang")]
    public int urutanLevel = 1;
    
    [Tooltip("Nama scene tujuan level selanjutnya (kosongkan jika level terakhir)")]
    public string namaSceneSelanjutnya = "";

    [Header("UI Scene Ini (Wajib Diisi di Inspector)")]
    public GameObject panelWinScene2;
    public GameObject panelLoseSceneIni;
    public TMP_Text textSkorAkhirScene2;
    public TMP_Text textWaktuAkhirScene2;
    
    [Header("Sorting Guide Panel")]
    [Tooltip("Panel panduan sortir yang muncul setelah briefing")]
    public GameObject panelSortingGuide;
    
    [Header("UI HUD (Optional - Untuk Manual Linking)")]
    [Tooltip("Opsional: Drag Text_Skor jika auto-find gagal")]
    public TMP_Text textSkorHUD;
    [Tooltip("Opsional: Drag Text_Timer jika auto-find gagal")]
    public TMP_Text textTimerHUD;

    void Awake()
    {
        Instance = this;
    }

    // UBAH void Start() MENJADI IEnumerator Start()
    IEnumerator Start()
    {
        Debug.Log("==================================================");
        Debug.Log("[1] ProcessingLevelManager: Menunggu GameManager siap...");

        // 1. JEDA SEJENAK (PENTING!)
        // Memberi waktu agar GameManager dari Scene 1 mendarat sempurna di Scene 2
        yield return new WaitForSeconds(0.1f);

        // Pastikan Waktu Jalan Dulu (Reset Pembekuan dari Scene 1)
        Time.timeScale = 1;

        // 2. SETUP GAMEMANAGER
        if (GameManager.Instance != null)
        {
            Debug.Log("[2] GameManager Ditemukan. Melakukan Setup Level Baru...");

            // Hitung target sampah dari inventory (atau default 5 jika testing)
            int targetSampah = (GameManager.Instance.trashInventory != null) ? GameManager.Instance.trashInventory.Count : 0;
            if (targetSampah <= 0) targetSampah = 5;

            // Setup GameManager
            // ⚠️ UNTUK TESTING: Ubah 'true' menjadi 'false' untuk disable timer
            GameManager.Instance.SetupLevelBaru(
                true,  // ← Ubah ke 'false' untuk unlimited time saat testing
                targetSampah,
                dataLevelIni.batasWaktuDetik,
                panelWinScene2,
                panelLoseSceneIni,
                textSkorAkhirScene2,
                textWaktuAkhirScene2
            );
            
            // Set index level untuk progression system
            GameManager.Instance.SetIndexLevel(urutanLevel);
            
            // Manual override jika HUD di-assign di Inspector
            if (textSkorHUD != null)
            {
                GameManager.Instance.scoreTextUI = textSkorHUD;
                Debug.Log("✅ Text Skor HUD di-override manual dari Inspector");
            }
            if (textTimerHUD != null)
            {
                GameManager.Instance.timerTextUI = textTimerHUD;
                Debug.Log("✅ Text Timer HUD di-override manual dari Inspector");
            }
        }
        else
        {
            Debug.LogError("[CRITICAL] GameManager TIDAK DITEMUKAN! Pastikan 'DontDestroyOnLoad' jalan.");
        }

        // 3. LOGIKA BRIEFING & SPAWNER
        Debug.Log("==================================================");
        Debug.Log("[3] MEMULAI LOGIKA BRIEFING & SORTING GUIDE");

        // Matikan Spawner Awal
        if (mesinSpawner != null) 
        {
            mesinSpawner.enabled = false;
            Debug.Log("✅ Spawner dimatikan");
        }
        
        // Matikan Sorting Guide Panel di awal
        if (panelSortingGuide != null) 
        {
            panelSortingGuide.SetActive(false);
            Debug.Log("✅ Panel Sorting Guide dimatikan di awal");
        }
        else
        {
            Debug.LogError("❌ panelSortingGuide NULL! Tidak di-assign di Inspector!");
        }

        // Cek apakah ada briefing (dengan null check lengkap)
        Debug.Log($"📋 CEK BRIEFING:");
        Debug.Log($"   - briefingScript: {(briefingScript != null ? "✅" : "❌ NULL")}");
        Debug.Log($"   - dataLevelIni: {(dataLevelIni != null ? "✅" : "❌ NULL")}");
        if (dataLevelIni != null)
        {
            Debug.Log($"   - barisDialogSortir: {(dataLevelIni.barisDialogSortir != null ? $"✅ ({dataLevelIni.barisDialogSortir.Length} dialog)" : "❌ NULL")}");
        }
        
        bool adaBriefing = (briefingScript != null && 
                           dataLevelIni != null && 
                           dataLevelIni.barisDialogSortir != null && 
                           dataLevelIni.barisDialogSortir.Length > 0);

        Debug.Log($"   HASIL: {(adaBriefing ? "✅ ADA BRIEFING" : "❌ TIDAK ADA BRIEFING")}");

        if (adaBriefing)
        {
            Debug.Log("[3a] Path: BRIEFING MODE");
            briefingScript.SetupSequenceKhusus(dataLevelIni, dataLevelIni.barisDialogSortir);

            if (briefingScript.tombolMulai != null)
            {
                Debug.Log("✅ Tombol Mulai ditemukan - Listener diset ke BukaPanduanSortir()");
                briefingScript.tombolMulai.onClick.RemoveAllListeners();
                briefingScript.tombolMulai.onClick.AddListener(BukaPanduanSortir);
            }
            else
            {
                Debug.LogError("❌ briefingScript.tombolMulai NULL!");
            }

            Time.timeScale = 0;
            Debug.Log("⏸️ Time.timeScale = 0 (Pause untuk briefing)");
        }
        else
        {
            Debug.Log("[3b] Path: SKIP BRIEFING - Langsung ke Sorting Guide");
            Debug.Log("🚀 Memanggil BukaPanduanSortir() langsung...");
            BukaPanduanSortir();
        }
        
        Debug.Log("==================================================");
    }

    /// <summary>
    /// Buka Sorting Guide Panel setelah briefing selesai
    /// Dipanggil oleh tombol "Mulai" di Briefing
    /// </summary>
    public void BukaPanduanSortir()
    {
        Debug.Log("==================================================");
        Debug.Log("[BRIEFING → GUIDE] BukaPanduanSortir() DIPANGGIL");
        Debug.Log($"⏱️ Time.timeScale saat ini: {Time.timeScale}");
        
        // Matikan panel briefing
        if (briefingScript != null)
        {
            Debug.Log("🔴 Mematikan Panel Briefing...");
            briefingScript.panelDialog.SetActive(false);
            if (briefingScript.panelIntro != null) 
            {
                briefingScript.panelIntro.SetActive(false);
            }
            Debug.Log("✅ Panel Briefing dimatikan");
        }
        else
        {
            Debug.LogWarning("⚠️ briefingScript NULL!");
        }
        
        // Nyalakan Sorting Guide Panel
        if (panelSortingGuide != null)
        {
            Debug.Log($"🟢 Menyalakan Panel Sorting Guide (sebelum: {panelSortingGuide.activeSelf})...");
            panelSortingGuide.SetActive(true);
            Debug.Log($"✅ Sorting Guide Panel ditampilkan (sekarang: {panelSortingGuide.activeSelf})");
        }
        else
        {
            Debug.LogError("❌ panelSortingGuide NULL! Tidak di-assign di Inspector! Langsung main.");
            MulaiMain();
            return;
        }
        
        // PENTING: Time.timeScale tetap 0 karena game belum mulai
        Debug.Log($"⏸️ Time.timeScale tetap: {Time.timeScale} (Game masih pause)");
        Debug.Log("📋 Menunggu user klik tombol 'Lanjut' di Sorting Guide...");
        Debug.Log("==================================================");
    }

    /// <summary>
    /// Mulai game (spawner, timer, unpause)
    /// Dipanggil oleh tombol "Lanjut" di Sorting Guide Panel
    /// </summary>
    public void MulaiMain()
    {
        Debug.Log("==================================================");
        Debug.Log("[GAME START] MulaiMain() DIPANGGIL");
        Debug.Log($"⏱️ Time.timeScale sebelum: {Time.timeScale}");
        
        // Matikan Sorting Guide Panel
        if (panelSortingGuide != null) 
        {
            Debug.Log($"🔴 Mematikan Sorting Guide Panel (sebelum: {panelSortingGuide.activeSelf})...");
            panelSortingGuide.SetActive(false);
            Debug.Log($"✅ Panel dimatikan (sekarang: {panelSortingGuide.activeSelf})");
        }
        else
        {
            Debug.LogWarning("⚠️ panelSortingGuide NULL saat MulaiMain!");
        }
        
        Time.timeScale = 1; // Pastikan waktu jalan
        Debug.Log($"▶️ Time.timeScale diset ke: {Time.timeScale}");

        // Hilangkan UI Briefing
        if (briefingScript != null)
        {
            briefingScript.panelDialog.SetActive(false);
            if (briefingScript.panelIntro != null) briefingScript.panelIntro.SetActive(false);
            Debug.Log("✅ UI Briefing dimatikan");
        }

        // Nyalakan Spawner
        if (mesinSpawner != null) 
        {
            mesinSpawner.enabled = true;
            Debug.Log("✅ WasteSpawner diaktifkan");
        }
        else
        {
            Debug.LogError("❌ mesinSpawner NULL!");
        }

        // Mulai Timer di GameManager
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.MulaiLevel();
            Debug.Log("✅ GameManager.MulaiLevel() dipanggil");
        }
        else
        {
            Debug.LogError("❌ GameManager.Instance NULL!");
        }
        
        Debug.Log("🎮 GAME STARTED - Sampah mulai spawn!");
        Debug.Log("==================================================");
    }

    /// <summary>
    /// Navigasi ke level selanjutnya
    /// Dipanggil oleh tombol "Next Level" / "Lanjut" di Panel Win
    /// </summary>
    public void KeLevelSelanjutnya()
    {
        // PENTING: Reset time scale agar scene baru tidak freeze
        Time.timeScale = 1f;
        
        // Clear inventory untuk mulai fresh di level baru
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearInventory();
        }
        
        // Cek apakah ada scene selanjutnya
        if (!string.IsNullOrEmpty(namaSceneSelanjutnya))
        {
            Debug.Log($"➡️ Lanjut ke level berikutnya: {namaSceneSelanjutnya}");
            
            // Cek apakah scene ada di Build Settings
            if (SceneExists(namaSceneSelanjutnya))
            {
                SceneManager.LoadScene(namaSceneSelanjutnya);
            }
            else
            {
                Debug.LogError($"❌ Scene '{namaSceneSelanjutnya}' tidak ditemukan di Build Settings!");
                Debug.LogWarning("⚠️ Pastikan scene sudah ditambahkan di File → Build Settings");
                Debug.Log("🏠 Fallback: Kembali ke Hub");
                SceneManager.LoadScene("01_Hub_Klub");
            }
        }
        else
        {
            // Jika kosong (level terakhir), kembali ke Hub
            Debug.Log("🏁 Level terakhir selesai! Kembali ke Hub.");
            SceneManager.LoadScene("01_Hub_Klub");
        }
    }
    
    /// <summary>
    /// Helper function: Cek apakah scene ada di Build Settings
    /// </summary>
    private bool SceneExists(string sceneName)
    {
        // Cek di Build Settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        
        return false;
    }
}