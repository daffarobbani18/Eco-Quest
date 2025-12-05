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

    [Header("UI Scene Ini (Wajib Diisi di Inspector)")]
    public GameObject panelWinScene2;
    public TMP_Text textSkorAkhirScene2;
    public TMP_Text textWaktuAkhirScene2;
    
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

        // Matikan Spawner Awal
        if (mesinSpawner != null) mesinSpawner.enabled = false;

        // Cek apakah ada briefing (dengan null check lengkap)
        bool adaBriefing = (briefingScript != null && 
                           dataLevelIni != null && 
                           dataLevelIni.barisDialogSortir != null && 
                           dataLevelIni.barisDialogSortir.Length > 0);

        if (adaBriefing)
        {
            Debug.Log("[3] Memulai Briefing...");
            briefingScript.SetupSequenceKhusus(dataLevelIni, dataLevelIni.barisDialogSortir);

            if (briefingScript.tombolMulai != null)
            {
                briefingScript.tombolMulai.onClick.RemoveAllListeners();
                briefingScript.tombolMulai.onClick.AddListener(MulaiMain);
            }

            // BEKUKAN WAKTU UNTUK BRIEFING (Hanya jika briefing siap)
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("[3] Tidak ada Briefing. Langsung main.");
            MulaiMain();
        }
    }

    public void MulaiMain()
    {
        Debug.Log("[GAME START] Game Dimulai.");
        Time.timeScale = 1; // Pastikan waktu jalan

        // Hilangkan UI Briefing
        if (briefingScript != null)
        {
            briefingScript.panelDialog.SetActive(false);
            if (briefingScript.panelIntro != null) briefingScript.panelIntro.SetActive(false);
        }

        // Nyalakan Spawner
        if (mesinSpawner != null) mesinSpawner.enabled = true;

        // Mulai Timer di GameManager
        if (GameManager.Instance != null) GameManager.Instance.MulaiLevel();
    }
}