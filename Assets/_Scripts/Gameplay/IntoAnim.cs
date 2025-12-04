using UnityEngine;
using System.Collections;

public class IntoAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Durasi animasi dalam detik")]
    public float duration = 0.5f;

    [Tooltip("Waktu tunggu sebelum animasi mulai")]
    public float delay = 0f;

    [Tooltip("Kurva animasi (rekomendasi: EaseOutBack untuk efek membal)")]
    public AnimationCurve animCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    [Header("Initial State")]
    [Tooltip("Skala awal panel (misal 0.5 agar tidak mulai dari nol mutlak, atau 0 untuk pop-up penuh)")]
    public Vector3 startScale = Vector3.zero;

    private void OnEnable()
    {
        // Reset skala ke awal setiap kali panel diaktifkan (SetActive true)
        transform.localScale = startScale;

        // Mulai animasi
        StartCoroutine(AnimatePopup());
    }

    IEnumerator AnimatePopup()
    {
        // Tunggu delay jika ada
        if (delay > 0) yield return new WaitForSeconds(delay);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float percentage = timer / duration;

            // Mengambil nilai dari kurva (untuk efek smooth/bouncy)
            float curveValue = animCurve.Evaluate(percentage);

            // Lerp skala dari startScale ke Vector3.one (1,1,1)
            transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.one, curveValue);

            yield return null;
        }

        // Pastikan skala akhir tepat di 1,1,1
        transform.localScale = Vector3.one;
    }
}