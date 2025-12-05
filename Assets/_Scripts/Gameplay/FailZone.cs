using UnityEngine;

public class FailZone : MonoBehaviour
{
    [Header("Penalty Settings")]
    [Tooltip("Skor yang dikurangi saat sampah masuk fail zone")]
    public int denda = 5;

    [Header("Audio Settings (Optional)")]
    [Tooltip("AudioSource untuk memutar sound effect gagal")]
    public AudioSource source;
    
    [Tooltip("Sound effect saat sampah masuk fail zone")]
    public AudioClip failSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug log untuk tracking
        Debug.Log($"⚠️ [FAIL ZONE] Ada yang masuk: {collision.gameObject.name}");

        // Cek 1: Apakah object punya tag "Sampah"?
        bool isSampahByTag = collision.CompareTag("Sampah");
        
        // Cek 2: Atau apakah object punya component "DragController"?
        bool isSampahByComponent = collision.GetComponent<DragController>() != null;

        // Jika salah satu kondisi TRUE, berarti ini sampah
        if (isSampahByTag || isSampahByComponent)
        {
            Debug.Log($"❌ [FAIL ZONE] Sampah masuk zona gagal! Denda: -{denda} poin");

            // 1. Kurangi skor via GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.KurangiSkor(denda);
            }
            else
            {
                Debug.LogWarning("⚠️ [FAIL ZONE] GameManager tidak ditemukan!");
            }

            // 2. Mainkan sound effect (jika diassign)
            if (source != null && failSound != null)
            {
                source.PlayOneShot(failSound);
            }

            // 3. Hancurkan sampah
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log($"ℹ️ [FAIL ZONE] Bukan sampah, diabaikan: {collision.gameObject.name}");
        }
    }
}