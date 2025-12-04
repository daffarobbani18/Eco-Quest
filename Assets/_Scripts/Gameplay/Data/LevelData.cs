using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataLevelBaru", menuName = "PjBL/Level Data", order = 2)]
public class LevelData : ScriptableObject
{
    [Header("Info Dasar Level")]
    public string namaLevel;

    // --- BAGIAN INI YANG PENTING ---
    [Header("Story Dialog")]
    [TextArea(3, 10)] // Membuat kotak teksnya besar (min 3 baris, max 10)
    public string[] barisDialogGuru;

    // --- TAMBAHAN BARU UNTUK FASE 2 ---
    [Header("Story Dialog (Fase Sortir)")]
    [TextArea(3, 10)]
    public string[] barisDialogSortir; // Dialog Fase 2 (Sortir)

    [Header("Aturan Main")]
    public float batasWaktuDetik;
    public int targetJumlahSampah;

    [Header("Isi Level (Resep Sampah)")]
    public List<WasteData> daftarSampahLevelIni;
}