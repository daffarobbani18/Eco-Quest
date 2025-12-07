using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings (Di-update oleh LevelManager)")]
    public bool levelPakaiTimer = true;
    public int totalSampahLevelIni; // Target jumlah sampah agar menang
    public int indexLevelSaatIni = 0; // Index level yang sedang dimainkan (untuk progression)

    [Header("UI References (Akan berubah tiap scene)")]
    public GameObject winPanel;       // Panel Menang
    public GameObject losePanel;      // Panel Kalah (Game Over)
    public TMP_Text textSkorAkhir;    // Teks Skor di Panel Menang
    public TMP_Text textWaktuAkhir;   // Teks Waktu di Panel Menang

    [Header("UI References (HUD)")]
    public TMP_Text scoreTextUI;      // Teks Skor di Pojok Layar
    public TMP_Text timerTextUI;      // Teks Timer di Pojok Layar

    [Header("Game State")]
    public int totalSkor = 0;
    public float sisaWaktu = 60f;
    public bool isGameActive = false;
    private float waktuAwal;

    // INVENTARIS (Penting untuk dibawa antar scene)
    public List<WasteData> trashInventory;

    // ============ JUDGMENT PHASE - TRACKING KESALAHAN PER TONG ============
    // Dictionary: Key = Tipe Tong yang SALAH TERIMA sampah, Value = List sampah yang masuk
    // Contoh: mistakesByBin[WasteType.B3] = { ApelData, KulitPisangData }
    public Dictionary<WasteType, List<WasteData>> mistakesByBin = new Dictionary<WasteType, List<WasteData>>()
    {
        { WasteType.Organik, new List<WasteData>() },
        { WasteType.Anorganik, new List<WasteData>() },
        { WasteType.B3, new List<WasteData>() }
    };
    
    // HashSet untuk deduplication per tong (jangan record 5x Apel ke B3)
    private Dictionary<WasteType, HashSet<string>> recordedMistakesPerBin = new Dictionary<WasteType, HashSet<string>>()
    {
        { WasteType.Organik, new HashSet<string>() },
        { WasteType.Anorganik, new HashSet<string>() },
        { WasteType.B3, new HashSet<string>() }
    };
    // ======================================================================

    void Awake()
    {
        // 1. SINGLETON ABADI
        // Kita pastikan GameManager ini hidup terus antar scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // HIDUPKAN LAGI (Jangan dikomen)
            trashInventory = new List<WasteData>();
        }
        else
        {
            // Jika kita pindah ke scene baru dan di sana ada GameManager bawaan,
            // Hancurkan GameManager baru itu, kita pakai yang lama (yang bawa Inventory dari Kantin)
            Destroy(gameObject);
        }
    }

    // --- FUNGSI SET INDEX LEVEL ---
    // Fungsi ini dipanggil oleh LevelManager untuk set level index (untuk progression)
    public void SetIndexLevel(int index)
    {
        indexLevelSaatIni = index;
        Debug.Log($"📍 Index Level diset ke: {index}");
    }

    // --- FUNGSI BARU (PENTING): UPDATE SETTING SAAT PINDAH SCENE ---
    // Fungsi ini dipanggil oleh ProcessingLevelManager saat scene Pengolahan dimulai
    public void SetupLevelBaru(bool pakaiTimer, int targetSampah, float durasi, GameObject panelWin, GameObject panelLose, TMP_Text txtSkor, TMP_Text txtWaktu)
    {
        Debug.Log("GameManager: Setup Level Baru dimulai...");

        // 1. Reset Logic Level
        levelPakaiTimer = pakaiTimer;
        totalSampahLevelIni = targetSampah;
        sisaWaktu = durasi;
        waktuAwal = durasi;

        // 2. Sambungkan UI Scene Baru
        // Karena pindah scene, referensi UI lama hilang, jadi kita update dengan yang baru dikirim LevelManager
        winPanel = panelWin;
        losePanel = panelLose;
        textSkorAkhir = txtSkor;
        textWaktuAkhir = txtWaktu;

        // 3. Cari UI HUD otomatis di scene baru 
        // (Pastikan nama objek di Canvas kamu adalah "Text_Skor" dan "Text_Timer")
        GameObject objSkor = GameObject.Find("Text_Skor");
        GameObject objTimer = GameObject.Find("Text_Timer");

        if (objSkor != null)
        {
            scoreTextUI = objSkor.GetComponent<TMP_Text>();
            Debug.Log("✅ Text_Skor ditemukan dan berhasil di-link!");
        }
        else
        {
            Debug.LogWarning("⚠️ GameObject 'Text_Skor' TIDAK DITEMUKAN! Cek nama di Hierarchy.");
        }

        if (objTimer != null)
        {
            timerTextUI = objTimer.GetComponent<TMP_Text>();
            Debug.Log("✅ Text_Timer ditemukan dan berhasil di-link!");
        }
        else
        {
            Debug.LogWarning("⚠️ GameObject 'Text_Timer' TIDAK DITEMUKAN! Cek nama di Hierarchy.");
        }

        // 4. Reset Status & WAKTU (P3K agar game tidak beku)
        isGameActive = false;
        Time.timeScale = 1; // <--- INI PENTING AGAR GAME JALAN LAGI

        if (winPanel != null) winPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (isGameActive && levelPakaiTimer && sisaWaktu > 0)
        {
            sisaWaktu -= Time.deltaTime;
            UpdateUI();
            
            // Cek waktu habis (tapi hanya jika game masih aktif)
            if (sisaWaktu <= 0 && isGameActive)
            {
                GameOver();
            }
        }
    }

    // --- FUNGSI START MANUAL ---
    public void MulaiLevel() // Dipanggil LevelManager setelah briefing
    {
        isGameActive = true;
        Time.timeScale = 1;
        UpdateUI();
    }

    // Overload untuk kompatibilitas dengan script lama (Kantin) jika masih ada yang panggil pakai durasi
    public void MulaiLevel(float durasi)
    {
        sisaWaktu = durasi;
        waktuAwal = durasi;
        isGameActive = true;
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 1;
        UpdateUI();
    }

    // ---------------------------------------------------------
    // LOGIKA GAMEPLAY
    // ---------------------------------------------------------

    public void TambahSkor(int nilai)
    {
        totalSkor += nilai;
        UpdateUI();
    }

    public void KurangiSkor(int nilai)
    {
        totalSkor -= nilai;
        UpdateUI();
        
        // Cek Game Over jika skor negatif
        if (totalSkor < 0)
        {
            Debug.Log("💀 GAME OVER! Skor turun di bawah 0!");
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        // Jangan trigger lagi jika sudah tidak aktif
        if (!isGameActive) return;
        
        isGameActive = false;
        Debug.Log("💀 TRIGGER GAME OVER - SKOR NEGATIF!");
        
        // Matikan win panel (jaga-jaga)
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        
        // Munculkan lose panel
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            
            // Update text skor akhir (jika ada text di lose panel)
            if (textSkorAkhir != null)
            {
                textSkorAkhir.text = "Skor Akhir: " + totalSkor;
            }
            
            Debug.Log("📊 Panel Game Over ditampilkan dengan skor: " + totalSkor);
        }
        else
        {
            Debug.LogError("❌ losePanel NULL! Panel tidak bisa ditampilkan saat Game Over.");
        }
        
        // Hentikan waktu (freeze game)
        Time.timeScale = 0;
    }

    public void KurangiJumlahSampah()
    {
        totalSampahLevelIni--;

        Debug.Log($"✅ KurangiJumlahSampah() dipanggil! Sisa Sampah Target: {totalSampahLevelIni}");

        // Cek Menang
        if (totalSampahLevelIni <= 0)
        {
            Debug.Log("🎉 WIN CONDITION TERCAPAI! Memanggil LevelSelesai()...");
            LevelSelesai();
            
            // ⭐ Trigger Judgment Phase di ProcessingLevelManager
            if (ProcessingLevelManager.Instance != null)
            {
                ProcessingLevelManager.Instance.StartJudgmentPhase();
            }
            else
            {
                // Fallback jika tidak di scene Processing (misalnya testing)
                Debug.LogWarning("⚠️ ProcessingLevelManager tidak ditemukan. Langsung tampilkan Win Panel.");
                ShowWinPanel();
            }
        }
    }

    void LevelSelesai()
    {
        isGameActive = false;
        Debug.Log("🏆 LEVEL SELESAI - MENANG!");
        
        // ============ SISTEM UNLOCK LEVEL BERIKUTNYA ============
        // Hitung level berikutnya yang harus dibuka
        int levelSelanjutnya = indexLevelSaatIni + 1;
        
        // Ambil data progress lama dari PlayerPrefs
        int progressLama = PlayerPrefs.GetInt("LevelTerbuka", 1);
        
        // Hanya update jika level selanjutnya lebih tinggi dari progress lama
        if (levelSelanjutnya > progressLama)
        {
            PlayerPrefs.SetInt("LevelTerbuka", levelSelanjutnya);
            PlayerPrefs.Save();
            Debug.Log($"🎉 PROGRESS TERSIMPAN! Level {levelSelanjutnya} sekarang terbuka!");
        }
        else
        {
            Debug.Log($"📊 Level {levelSelanjutnya} sudah terbuka sebelumnya. Progress tidak berubah.");
        }
        // =========================================================
        
        // ⭐ JUDGMENT PHASE: Delegate ke ProcessingLevelManager
        // Jangan langsung tampilkan Win Panel, biar PLM yang handle
        // (PLM akan cek mistakes dulu, tampilkan slideshow, baru Win Panel)
        Debug.Log("📋 LevelSelesai() selesai. Menunggu ProcessingLevelManager untuk Judgment Phase...");
        
        // JANGAN Freeze Time di sini, biar Judgment Slideshow bisa jalan
        // Time.timeScale = 0; // <-- DIHAPUS/DIPINDAH ke ShowWinPanel()
    }

    void GameOver()
    {
        // Guard: Jangan override jika sudah menang
        if (!isGameActive)
        {
            Debug.Log("⚠️ GameOver() dipanggil tapi game sudah tidak aktif (mungkin sudah menang). Skip.");
            return;
        }
        
        isGameActive = false;
        Debug.Log("⏱️ GAME OVER - WAKTU HABIS!");
        
        // Tetap tampilkan panel meskipun waktu habis
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            
            // Update text dengan data final
            if (textSkorAkhir != null)
                textSkorAkhir.text = "Skor Akhir: " + totalSkor;

            if (textWaktuAkhir != null)
            {
                textWaktuAkhir.text = "Waktu Habis!";
            }
            
            Debug.Log("📊 Panel Game Over ditampilkan dengan skor: " + totalSkor);
        }
        else
        {
            Debug.LogError("❌ winPanel NULL! Panel tidak bisa ditampilkan saat Game Over.");
        }
        
        // Freeze game
        Time.timeScale = 0;
    }

    void UpdateUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = "Skor: " + totalSkor;
        }

        if (timerTextUI != null)
        {
            if (levelPakaiTimer)
            {
                int minutes = Mathf.FloorToInt(sisaWaktu / 60F);
                int seconds = Mathf.FloorToInt(sisaWaktu - minutes * 60);
                timerTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                timerTextUI.text = ""; // Timer disabled (unlimited time)
            }
        }
        else
        {
            Debug.LogWarning("⚠️ UpdateUI: timerTextUI NULL! Timer tidak bisa diupdate.");
        }
    }

    // ---------------------------------------------------------
    // FUNGSI INVENTARIS
    // ---------------------------------------------------------

    public void AddTrashToInventory(WasteData newTrash)
    {
        if (trashInventory == null) trashInventory = new List<WasteData>();
        trashInventory.Add(newTrash);
        
        // Debug log untuk verifikasi inventory
        Debug.Log($"📦 [INVENTORY] +1 Sampah: {newTrash.namaSampah} (Tipe: {newTrash.tipeSampah})");
        Debug.Log($"📊 [INVENTORY] Total: {trashInventory.Count} sampah");
    }

    public void ClearInventory()
    {
        if (trashInventory != null) trashInventory.Clear();
    }

    // ============ JUDGMENT PHASE - WIN PANEL & TRACKING ============
    
    /// <summary>
    /// Tampilkan Win Panel (dipanggil setelah Judgment Slideshow selesai atau skip)
    /// </summary>
    public void ShowWinPanel()
    {
        Debug.Log("==================================================");
        Debug.Log("[SHOW WIN PANEL] Menampilkan Panel Menang");
        
        // Debug: Cek apakah winPanel NULL
        if (winPanel == null)
        {
            Debug.LogError("❌ ERROR: winPanel NULL! Panel Win tidak bisa ditampilkan!");
            Debug.LogError("   Pastikan Panel Win di-link di ProcessingLevelManager Inspector.");
            return;
        }
        
        Debug.Log($"✅ winPanel ditemukan: {winPanel.name}. Mengaktifkan panel...");

        winPanel.SetActive(true);

        if (textSkorAkhir != null)
            textSkorAkhir.text = "Skor Akhir: " + totalSkor;

        if (textWaktuAkhir != null && levelPakaiTimer)
        {
            // Hitung waktu terpakai
            float terpakai = waktuAwal - sisaWaktu;
            int min = Mathf.FloorToInt(terpakai / 60F);
            int sec = Mathf.FloorToInt(terpakai % 60);
            textWaktuAkhir.text = string.Format("Waktu: {0:00}:{1:00}", min, sec);
        }
        else if (textWaktuAkhir != null)
        {
            textWaktuAkhir.text = "Selesai!";
        }

        // Matikan waktu physics saat menang
        Time.timeScale = 0;
        
        Debug.Log("✅ Win Panel ditampilkan!");
        Debug.Log("==================================================");
    }
    
    /// <summary>
    /// Catat kesalahan pemilahan sampah (dipanggil oleh DragController)
    /// System: Track sampah yang SALAH MASUK ke setiap tong
    /// Deduplication: Hanya catat 1x per jenis sampah per tong
    /// </summary>
    public void RecordMistake(WasteType wrongBinType, WasteData wasteData)
    {
        // Cek deduplication: apakah sampah ini sudah tercatat di tong ini?
        if (recordedMistakesPerBin[wrongBinType].Contains(wasteData.namaSampah))
        {
            Debug.Log($"⚠️ [JUDGMENT] '{wasteData.namaSampah}' sudah dicatat di Tong {wrongBinType}. Skip duplicate.");
            return;
        }
        
        // Catat sampah ke tong yang salah terima
        mistakesByBin[wrongBinType].Add(wasteData);
        recordedMistakesPerBin[wrongBinType].Add(wasteData.namaSampah);
        
        Debug.Log($"❌ [JUDGMENT] Tong {wrongBinType} salah terima: {wasteData.namaSampah} (Seharusnya: {wasteData.tipeSampah})");
        Debug.Log($"📊 [JUDGMENT] Tong {wrongBinType} total salah terima: {mistakesByBin[wrongBinType].Count} sampah");
    }
    
    /// <summary>
    /// Bersihkan semua data kesalahan (dipanggil saat mulai level baru)
    /// </summary>
    public void ClearMistakes()
    {
        // Clear semua list di Dictionary
        foreach (var list in mistakesByBin.Values)
        {
            list.Clear();
        }
        
        // Clear semua HashSet deduplication
        foreach (var set in recordedMistakesPerBin.Values)
        {
            set.Clear();
        }
        
        Debug.Log("🧹 [JUDGMENT] Data kesalahan dibersihkan untuk semua tong");
    }
    
    /// <summary>
    /// Cek apakah ada kesalahan yang tercatat di semua tong
    /// </summary>
    public bool HasMistakes()
    {
        foreach (var list in mistakesByBin.Values)
        {
            if (list.Count > 0) return true;
        }
        return false;
    }
    
    /// <summary>
    /// Get total jumlah tong yang kena salah pilah
    /// </summary>
    public int GetAffectedBinCount()
    {
        int count = 0;
        foreach (var list in mistakesByBin.Values)
        {
            if (list.Count > 0) count++;
        }
        return count;
    }
    
    // ======================================================================
}