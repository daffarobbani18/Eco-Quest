using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool isDragging = false;
    private ConveyorMovement movementScript;
    private Rigidbody2D rb;
    private Vector3 startPosition; // Posisi awal sebelum di-drag

    void Start()
    {
        movementScript = GetComponent<ConveyorMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        startPosition = transform.position; // Ingat posisi kalau-kalau player batal buang

        if (movementScript != null) movementScript.enabled = false;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckDropTarget(); // Cek kita jatuh di mana?
    }

    void CheckDropTarget()
    {
        // Tembakkan sinar laser kecil di posisi sampah berada sekarang
        // untuk melihat apakah ada benda lain di situ (seperti Tong Sampah)
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        bool foundBin = false;

        foreach (Collider2D hit in hits)
        {
            // Apakah benda yang kena itu punya script 'BinController'?
            BinController bin = hit.GetComponent<BinController>();

            if (bin != null)
            {
                foundBin = true;
                ProsesPemilahan(bin); // Jalankan logika penilaian
                break; // Keluar loop, kita sudah nemu tong
            }
        }

        // Kalau dilepas bukan di atas tong, kembalikan ke ban berjalan?
        // Atau biarkan jatuh? Untuk sekarang kita biarkan saja dia diam di sana
        // atau Anda bisa aktifkan lagi movementScript-nya.
        if (!foundBin)
        {
            // Opsional: Balik lagi jalan kalau tidak kena tong
            // if (movementScript != null) movementScript.enabled = true;
        }
    }

    void ProsesPemilahan(BinController bin)
    {
        WasteItem myItem = GetComponent<WasteItem>();

        if (myItem != null && myItem.dataSampah != null)
        {
            if (myItem.dataSampah.tipeSampah == bin.tipeTongIni)
            {
                Debug.Log("BENAR! +" + myItem.dataSampah.skorBenar + " poin");
                // PANGGIL GAMEMANAGER:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.TambahSkor(myItem.dataSampah.skorBenar);
                    GameManager.Instance.KurangiJumlahSampah(); // Kurangi counter target
                }
            }
            else
            {
                Debug.Log("SALAH! -" + myItem.dataSampah.skorSalah + " poin");
                // PANGGIL GAMEMANAGER:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.KurangiSkor(myItem.dataSampah.skorSalah);
                    
                    // ⭐ JUDGMENT PHASE: Catat sampah yang SALAH MASUK ke tong ini
                    // Parameter: (Tong yang salah terima, Data sampah lengkap)
                    GameManager.Instance.RecordMistake(
                        bin.tipeTongIni,          // Tong mana yang salah terima (contoh: Tong B3)
                        myItem.dataSampah         // Data sampah lengkap (Apel, Organik, icon, dll)
                    );
                }
            }
        }
        Destroy(gameObject);
    }
}