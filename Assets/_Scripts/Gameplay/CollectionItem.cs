using UnityEngine;

public class CollectionItem : MonoBehaviour
{
    [Header("Data Sampah")]
    public WasteData dataSampahIni;

    [Header("Settings Gerakan")]
    public float kecepatanTerbang = 15f; // Saya naikkan sedikit biar lebih responsif
    private bool isCollected = false;

    private Transform targetWadah;
    private Vector3 skalaAwal; // Ukuran asli sampah sebelum terbang
    private float jarakTotal;  // Jarak dari posisi awal klik ke wadah

    void Start()
    {
        // Simpan ukuran asli (misal skala 1,1,1)
        skalaAwal = transform.localScale;

        // Cari wadah otomatis
        GameObject wadahObj = GameObject.FindGameObjectWithTag("Wadah");
        if (wadahObj != null)
        {
            targetWadah = wadahObj.transform;
        }
        else
        {
            Debug.LogError("ERROR: Tidak ketemu objek Tag 'Wadah'!");
        }
    }

    void OnMouseDown()
    {
        // CEK STATUS: Hanya boleh diklik jika game sedang playing (Briefing sudah tutup)
        if (CollectionLevelManager.Instance != null && !CollectionLevelManager.Instance.isGamePlaying)
        {
            return; // Jangan lakukan apa-apa
        }

        if (!isCollected && targetWadah != null)
        {
            MulaiTerbang();
        }
    }

    void MulaiTerbang()
    {
        isCollected = true;
        GetComponent<Collider2D>().enabled = false; // Matikan agar tidak bisa diklik lagi

        // Hitung jarak total saat ini untuk referensi pengecilan
        jarakTotal = Vector3.Distance(transform.position, targetWadah.position);
    }

    void Update()
    {
        if (isCollected && targetWadah != null)
        {
            // 1. Gerakkan sampah ke wadah
            transform.position = Vector3.MoveTowards(transform.position, targetWadah.position, kecepatanTerbang * Time.deltaTime);

            // 2. Hitung sisa jarak saat ini
            float jarakSekarang = Vector3.Distance(transform.position, targetWadah.position);

            // 3. LOGIKA BARU: Atur ukuran berdasarkan persentase jarak
            // Jika jarakSekarang mendekati 0, ukuran juga mendekati 0
            // Rumus: (Jarak Sekarang / Jarak Total) * Ukuran Awal
            float persentase = jarakSekarang / jarakTotal;
            transform.localScale = skalaAwal * persentase;

            // 4. Cek jika sudah sangat dekat (hampir 0)
            if (jarakSekarang < 0.1f)
            {
                MasukKeTas();
            }
        }
    }

    void MasukKeTas()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddTrashToInventory(dataSampahIni);
            GameManager.Instance.KurangiJumlahSampah();
        }
        if (CollectionLevelManager.Instance != null)
        {
            CollectionLevelManager.Instance.LaporSampahTerambil();
        }
        Destroy(gameObject);
    }
}