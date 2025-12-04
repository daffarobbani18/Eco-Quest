using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings (Di-update oleh LevelManager)")]
    public bool levelPakaiTimer = true;
    public int totalSampahLevelIni; // Target jumlah sampah agar menang

    [Header("UI References (Akan berubah tiap scene)")]
    public GameObject winPanel;       // Panel Menang
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

    // --- FUNGSI BARU (PENTING): UPDATE SETTING SAAT PINDAH SCENE ---
    // Fungsi ini dipanggil oleh ProcessingLevelManager saat scene Pengolahan dimulai
    public void SetupLevelBaru(bool pakaiTimer, int targetSampah, float durasi, GameObject panelWin, TMP_Text txtSkor, TMP_Text txtWaktu)
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
        textSkorAkhir = txtSkor;
        textWaktuAkhir = txtWaktu;

        // 3. Cari UI HUD otomatis di scene baru 
        // (Pastikan nama objek di Canvas kamu adalah "Text_Skor" dan "Text_Timer")
        GameObject objSkor = GameObject.Find("Text_Skor");
        GameObject objTimer = GameObject.Find("Text_Timer");

        if (objSkor != null) scoreTextUI = objSkor.GetComponent<TMP_Text>();
        if (objTimer != null) timerTextUI = objTimer.GetComponent<TMP_Text>();

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
            if (sisaWaktu <= 0) GameOver();
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
        if (totalSkor < 0) totalSkor = 0;
        UpdateUI();
    }

    public void KurangiJumlahSampah()
    {
        totalSampahLevelIni--;

        Debug.Log("Sisa Sampah Target: " + totalSampahLevelIni);

        // Cek Menang
        if (totalSampahLevelIni <= 0)
        {
            LevelSelesai();
        }
    }

    void LevelSelesai()
    {
        isGameActive = false;
        Debug.Log("LEVEL SELESAI - MENANG!");

        if (winPanel != null)
        {
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
        }

        // Matikan waktu physics saat menang
        Time.timeScale = 0;
    }

    void GameOver()
    {
        isGameActive = false;
        Debug.Log("GAME OVER - WAKTU HABIS");
    }

    void UpdateUI()
    {
        if (scoreTextUI != null) scoreTextUI.text = "Skor: " + totalSkor;

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
                timerTextUI.text = "";
            }
        }
    }

    // ---------------------------------------------------------
    // FUNGSI INVENTARIS
    // ---------------------------------------------------------

    public void AddTrashToInventory(WasteData newTrash)
    {
        if (trashInventory == null) trashInventory = new List<WasteData>();
        trashInventory.Add(newTrash);
    }

    public void ClearInventory()
    {
        if (trashInventory != null) trashInventory.Clear();
    }
}