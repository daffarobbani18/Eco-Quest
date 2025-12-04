using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BriefingSequence : MonoBehaviour
{
    [Header("UI Intro (Judul)")]
    public GameObject panelIntro;
    public TMP_Text textJudul;
    public TMP_Text textInfo;
    public float durasiAnimasi = 3.0f;

    [Header("UI Dialog (Guru)")]
    public GameObject panelDialog;
    public TMP_Text textDialogIsi;
    public Button tombolNext;
    public Button tombolMulai;

    [Header("Data Internal")]
    private string[] barisKalimat;
    private int indeksKalimat = 0;

    void Start()
    {
        // Debugging awal
        if (CollectionLevelManager.Instance != null)
        {
            Debug.Log("[BRIEFING] Mode Kantin Terdeteksi (CollectionLevelManager ada).");
            SetupSequence(CollectionLevelManager.Instance.dataLevelIni);
        }
        else
        {
            // Jika CollectionManager null, berarti kita mungkin di scene Pengolahan
            // Di scene Pengolahan, script ini menunggu perintah manual dari ProcessingLevelManager
            Debug.Log("[BRIEFING] Mode Standby (Menunggu perintah Manager).");
        }
    }

    // --- FUNGSI 1: UNTUK KANTIN (STANDARD) ---
    public void SetupSequence(LevelData data)
    {
        if (data == null) { Debug.LogError("[BRIEFING] Data Level NULL!"); return; }

        textJudul.text = data.namaLevel;
        textInfo.text = "Target: " + data.targetJumlahSampah;
        barisKalimat = data.barisDialogGuru;

        StartCoroutine(MainkanIntro());
    }

    // --- FUNGSI 2: UNTUK PENGOLAHAN (YANG HILANG TADI) ---
    // Fungsi inilah yang dicari oleh ProcessingLevelManager!
    public void SetupSequenceKhusus(LevelData data, string[] dialogKhusus)
    {
        Debug.Log("[BRIEFING] SetupSequenceKhusus dipanggil.");

        if (data == null) return;

        // Kita ubah Judulnya manual
        textJudul.text = "FASE PENGOLAHAN";
        textInfo.text = "Waktu: " + data.batasWaktuDetik + "s";

        // Kita paksa pakai dialog sortir, bukan dialog guru biasa
        barisKalimat = dialogKhusus;

        StartCoroutine(MainkanIntro());
    }

    // ---------------------------------------------------------

    IEnumerator MainkanIntro()
    {
        // Matikan dialog, Nyalakan Intro
        if (panelDialog != null) panelDialog.SetActive(false);
        if (tombolMulai != null) tombolMulai.gameObject.SetActive(false);

        if (panelIntro != null)
        {
            panelIntro.SetActive(true);
            Debug.Log("[BRIEFING] Mainkan Intro...");
        }

        yield return new WaitForSeconds(durasiAnimasi);

        // Matikan Intro, Nyalakan Dialog
        if (panelIntro != null) panelIntro.SetActive(false);
        MulaiDialog();
    }

    void MulaiDialog()
    {
        if (panelDialog != null) panelDialog.SetActive(true);

        indeksKalimat = 0;
        TampilkanKalimat();

        if (tombolNext != null)
        {
            tombolNext.onClick.RemoveAllListeners();
            tombolNext.onClick.AddListener(LanjutKalimat);
        }
    }

    void TampilkanKalimat()
    {
        if (barisKalimat != null && indeksKalimat < barisKalimat.Length)
        {
            if (textDialogIsi != null) textDialogIsi.text = barisKalimat[indeksKalimat];
        }
    }

    void LanjutKalimat()
    {
        indeksKalimat++;
        if (barisKalimat != null && indeksKalimat < barisKalimat.Length)
        {
            TampilkanKalimat();
        }
        else
        {
            SelesaiDialog();
        }
    }

    void SelesaiDialog()
    {
        if (textDialogIsi != null) textDialogIsi.text = "Siap melaksanakan tugas?";
        if (tombolNext != null) tombolNext.gameObject.SetActive(false);

        // Munculkan tombol mulai
        if (tombolMulai != null)
        {
            tombolMulai.gameObject.SetActive(true);
            Debug.Log("[BRIEFING] Tombol Mulai Dimunculkan.");
        }
    }
}