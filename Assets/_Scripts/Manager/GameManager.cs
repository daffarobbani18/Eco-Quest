using UnityEngine;
using System.Collections.Generic;
using TMPro; // Wajib untuk UI TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings (Atur per Scene)")]
    public bool levelPakaiTimer = true; // MATIKAN ini di Inspector Scene Kantin!
    public int totalSampahLevelIni;     // Target jumlah sampah agar menang

    [Header("UI References (Win/Lose)")]
    public GameObject winPanel;       // Drag Panel Win disini
    public TMP_Text textSkorAkhir;    // Drag Text Skor di Panel Win
    public TMP_Text textWaktuAkhir;   // Drag Text Waktu di Panel Win

    [Header("UI References (HUD)")]
    public TMP_Text scoreTextUI;      // Drag Text Skor di pojok layar
    public TMP_Text timerTextUI;      // Drag Text Timer di pojok layar

    [Header("Game State (Otomatis)")]
    public int totalSkor = 0;
    public float sisaWaktu = 60f;
    public bool isGameActive = false;
    private float waktuAwal;

    // Inventaris (dari Fase 1)
    public List<WasteData> trashInventory;

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            // Penting: Jika kamu menaruh Prefab GameManager di SETIAP scene secara manual,
            // HAPUS baris DontDestroyOnLoad di bawah ini agar tidak duplikat/bentrok.
            // DontDestroyOnLoad(gameObject); 

            trashInventory = new List<WasteData>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Logika: Timer hanya jalan jika game aktif DAN level ini butuh timer
        if (isGameActive && levelPakaiTimer && sisaWaktu > 0)
        {
            sisaWaktu -= Time.deltaTime;
            UpdateUI();

            if (sisaWaktu <= 0) GameOver();
        }
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

    // Fungsi KUNCI: Dipanggil saat sampah ditemukan (Kantin) atau dipilah (Pengolahan)
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

    // ---------------------------------------------------------
    // LOGIKA MULAI & SELESAI
    // ---------------------------------------------------------

    public void MulaiLevel(float durasi)
    {
        sisaWaktu = durasi;
        waktuAwal = durasi;
        totalSkor = 0;
        isGameActive = true;

        // Pastikan Win Panel mati saat mulai
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 1; // Waktu jalan normal

        UpdateUI();
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

        // Matikan waktu physics
        Time.timeScale = 0;
    }

    void GameOver()
    {
        isGameActive = false;
        Debug.Log("GAME OVER - WAKTU HABIS");
        // Tambahkan logika panel kalah disini jika mau
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
                // Jika mode santai, sembunyikan timer atau tulis pesan lain
                timerTextUI.text = "";
            }
        }
    }

    // ---------------------------------------------------------
    // FUNGSI INVENTARIS (Tetap Ada)
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