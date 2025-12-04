using UnityEngine;

public class BinController : MonoBehaviour
{
    [Header("Setting Tong")]
    // Pilih jenis tong ini di Inspector (Organik / Anorganik / B3)
    public WasteType tipeTongIni;

    // -----------------------------------------------------------
    // LOGIKA UTAMA: SAAT ADA BENDA MASUK AREA TONG
    // -----------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Cek apakah benda yang masuk itu adalah Sampah?
        // Kita cek apakah dia punya script 'WasteItem' yang memegang data sampah
        WasteItem scriptSampah = other.GetComponent<WasteItem>();

        // Jika scriptnya ditemukan (berarti itu memang sampah)
        if (scriptSampah != null)
        {
            // Ambil data jenis sampahnya
            WasteData dataMasuk = scriptSampah.dataSampah;

            // 2. BANDINGKAN: Apakah jenis sampah SAMA dengan jenis tong ini?
            if (dataMasuk.tipeSampah == tipeTongIni)
            {
                // --- JIKA BENAR ---
                Debug.Log("BENAR! " + dataMasuk.namaSampah + " masuk ke tong yang pas. +" + dataMasuk.skorBenar + " poin");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.TambahSkor(dataMasuk.skorBenar);        // Nambah Skor (dari data)
                    GameManager.Instance.KurangiJumlahSampah(); // Kurangi Target Sisa
                }

                // Hancurkan sampah (seolah-olah sudah masuk tong)
                Destroy(other.gameObject);
            }
            else
            {
                // --- JIKA SALAH ---
                Debug.Log("SALAH! " + dataMasuk.namaSampah + " jangan dibuang di sini! -" + dataMasuk.skorSalah + " poin");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.KurangiSkor(dataMasuk.skorSalah); // Hukuman kurangi nilai (dari data)
                    // Sampah TIDAK dihancurkan, biar pemain mindahin ke tong lain
                    // Atau bisa juga dikasih efek mental (bepending sistem drag kamu)
                }
            }
        }
    }
}