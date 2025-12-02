using UnityEngine;

public class FailZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- BARIS BARU UNTUK CEK ---
        // Ini akan mencetak nama benda apa pun yang masuk, biarpun tag-nya salah.
        Debug.Log("ADA YANG MASUK! Nama bendanya: " + collision.gameObject.name);
        // ----------------------------

        if (collision.CompareTag("Sampah"))
        {
            Debug.Log("ZONA GAGAL: Sampah masuk ke keranjang residu!");
            Destroy(collision.gameObject);
        }
    }
}