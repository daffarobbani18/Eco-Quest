using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    [Header("Kecepatan Ban Berjalan")]
    public float speed = 2.0f; // Bisa diatur di Inspector nanti

    void Update()
    {
        // LOGIKA PERGERAKAN:
        // Geser posisi benda ini ke arah KANAN (Vector2.right)
        // dikali kecepatan (speed)
        // dikali Time.deltaTime (agar gerakannya mulus di HP apa saja)

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}