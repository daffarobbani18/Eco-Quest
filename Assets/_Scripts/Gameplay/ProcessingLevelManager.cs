using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProcessingLevelManager : MonoBehaviour
{
    public static ProcessingLevelManager Instance;

    [Header("Referensi Sistem")]
    public WasteSpawner mesinSpawner;
    public BriefingSequence briefingScript;

    [Header("Data Level")]
    public LevelData dataLevelIni;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("==================================================");
        Debug.Log("[1] ProcessingLevelManager: START dimulai.");

        // CEK REFERENSI DI INSPECTOR
        if (mesinSpawner == null) Debug.LogError("[ERROR] Mesin Spawner belum dimasukkan di Inspector!");
        if (briefingScript == null) Debug.LogError("[ERROR] Briefing Script belum dimasukkan di Inspector!");
        if (dataLevelIni == null) Debug.LogError("[ERROR] Data Level (Scriptable Object) belum dimasukkan di Inspector!");

        // 1. Matikan Spawner
        if (mesinSpawner != null)
        {
            mesinSpawner.enabled = false;
            Debug.Log("[2] Spawner dimatikan sementara.");
        }

        // 2. Setup Briefing
        if (briefingScript != null && dataLevelIni != null)
        {
            // Cek dulu isi dialog di data level
            if (dataLevelIni.barisDialogSortir.Length == 0)
            {
                Debug.LogError("[ERROR] Data Level 'Baris Dialog Sortir' KOSONG (Size 0)! Dialog tidak akan muncul.");
            }
            else
            {
                Debug.Log("[3] Data ditemukan. Mengirim perintah SetupSequenceKhusus ke BriefingScript...");
                Debug.Log("--> Jumlah dialog yang dikirim: " + dataLevelIni.barisDialogSortir.Length + " baris.");

                briefingScript.SetupSequenceKhusus(dataLevelIni, dataLevelIni.barisDialogSortir);

                // Sambungkan tombol
                if (briefingScript.tombolMulai != null)
                {
                    briefingScript.tombolMulai.onClick.RemoveAllListeners();
                    briefingScript.tombolMulai.onClick.AddListener(MulaiMain);
                    Debug.Log("[4] Tombol Mulai berhasil disambungkan ke fungsi MulaiMain.");
                }
                else
                {
                    Debug.LogError("[ERROR] Tombol Mulai di dalam BriefingScript masih NULL/KOSONG!");
                }
            }
        }
        else
        {
            Debug.LogWarning("[WARNING] Data Level atau Briefing Script hilang! Langsung force start game.");
            MulaiMain();
        }
    }

    public void MulaiMain()
    {
        Debug.Log("[GAME START] Tombol Mulai Ditekan / Game Dimulai Paksa.");

        if (briefingScript != null)
        {
            briefingScript.panelDialog.SetActive(false);
            if (briefingScript.panelIntro != null) briefingScript.panelIntro.SetActive(false);
        }

        if (mesinSpawner != null)
        {
            mesinSpawner.enabled = true;
            Debug.Log("[GAME START] Mesin Spawner Dinyalakan.");
        }

        if (GameManager.Instance != null && dataLevelIni != null)
        {
            Debug.Log("[GAME START] GameManager Timer dijalankan: " + dataLevelIni.batasWaktuDetik + " detik.");
            GameManager.Instance.MulaiLevel(dataLevelIni.batasWaktuDetik);
        }
        else
        {
            Debug.LogError("[ERROR] GameManager Instance tidak ditemukan atau Data Level kosong!");
        }
    }
}