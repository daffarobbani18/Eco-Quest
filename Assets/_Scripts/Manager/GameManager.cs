using UnityEngine;
using System.Collections.Generic;
using TMPro; // Wajib untuk UI TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    // Kita buat static agar mudah diakses dari scene manapun tanpa drag-drop
    public TMP_Text scoreTextUI;
    public TMP_Text timerTextUI;

    [Header("Game State")]
    public int totalSkor = 0;
    public float sisaWaktu = 60f;
    public bool isGameActive = false;

    // Inventaris (dari Fase 1)
    public List<WasteData> trashInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            trashInventory = new List<WasteData>();
        }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        if (isGameActive && sisaWaktu > 0)
        {
            sisaWaktu -= Time.deltaTime;
            UpdateUI(); // Update tampilan tiap frame

            if (sisaWaktu <= 0) GameOver();
        }
    }

    public void TambahSkor(int nilai)
    {
        totalSkor += nilai;
        UpdateUI();
    }

    public void KurangiSkor(int nilai)
    {
        totalSkor -= nilai;
        if (totalSkor < 0) totalSkor = 0;
        UpdateUI(); // Update tampilan

        // Opsional: Mainkan suara salah di sini
    }

    void UpdateUI()
    {
        // Cek apakah UI tersedia
        if (scoreTextUI != null)
        {
            scoreTextUI.text = "Skor: " + totalSkor;
        }

        if (timerTextUI != null)
        {
            int minutes = Mathf.FloorToInt(sisaWaktu / 60F);
            int seconds = Mathf.FloorToInt(sisaWaktu - minutes * 60);
            timerTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Tambahkan 'float durasi' di dalam kurung
    public void MulaiLevel(float durasi)
    {
        // Set sisa waktu sesuai angka yang dikirim (misal 60 detik)
        sisaWaktu = durasi;

        totalSkor = 0;
        isGameActive = true;

        // Cari objek teks di scene secara otomatis
        GameObject objSkor = GameObject.Find("Text_Skor");
        GameObject objTimer = GameObject.Find("Text_Timer");

        if (objSkor != null) scoreTextUI = objSkor.GetComponent<TMP_Text>();
        if (objTimer != null) timerTextUI = objTimer.GetComponent<TMP_Text>();

        // PENTING: Panggil update UI sekali di awal biar angkanya langsung muncul
        UpdateUI();
    }

    void GameOver()
    {
        isGameActive = false;
        Debug.Log("GAME OVER");
        // Nanti munculkan Panel Laporan di sini
    }

    // ---------------------------------------------------------
    // FUNGSI INVENTARIS (Untuk Fase 1: Koleksi)
    // ---------------------------------------------------------

    // Fungsi ini dipanggil oleh CollectionItem.cs saat sampah masuk wadah
    public void AddTrashToInventory(WasteData newTrash)
    {
        // Pastikan list tidak null (untuk jaga-jaga)
        if (trashInventory == null)
        {
            trashInventory = new List<WasteData>();
        }

        trashInventory.Add(newTrash);
        Debug.Log("GameManager: Berhasil menyimpan " + newTrash.namaSampah + " ke inventaris.");
    }

    // Fungsi untuk mengosongkan tas (misal saat restart level)
    public void ClearInventory()
    {
        if (trashInventory != null)
        {
            trashInventory.Clear();
        }
        Debug.Log("GameManager: Inventaris dikosongkan.");
    }
}