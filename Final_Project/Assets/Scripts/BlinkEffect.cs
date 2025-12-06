using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    [Header("Blink Timing")]
    public float minDelay = 3f;        // jeda minimum antar kedip
    public float maxDelay = 7f;        // jeda maksimum antar kedip
    public float closeDuration = 0.08f; // waktu menutup (cepat)
    public float holdDuration = 0.05f;  // mata tertutup penuh
    public float openDuration = 0.1f;   // waktu membuka

    [Header("Blink Shape")]
    public float coverAmount =1f;    // seberapa banyak layar ditutup (0.9 = hampir full)

    private float nextBlinkTime;
    private float blinkTimer;
    private float blinkAmount;          // 0 = terbuka, 1 = tertutup

    private enum BlinkState { Idle, Closing, Hold, Opening }
    private BlinkState state = BlinkState.Idle;

    private Texture2D blackTex;

    private void Awake()
    {
        // bikin texture hitam 1x1
        blackTex = new Texture2D(1, 1);
        blackTex.SetPixel(0, 0, Color.black);
        blackTex.Apply();
    }

    private void Start()
    {
        ScheduleNextBlink();
    }

    private void Update()
    {
        switch (state)
        {
            case BlinkState.Idle:
                if (Time.time >= nextBlinkTime)
                {
                    // mulai kedip
                    state = BlinkState.Closing;
                    blinkTimer = 0f;
                }
                break;

            case BlinkState.Closing:
                blinkTimer += Time.deltaTime;
                float tClose = Mathf.Clamp01(blinkTimer / closeDuration);
                blinkAmount = tClose;

                if (blinkTimer >= closeDuration)
                {
                    state = BlinkState.Hold;
                    blinkTimer = 0f;
                }
                break;

            case BlinkState.Hold:
                blinkTimer += Time.deltaTime;
                blinkAmount = 1f;

                if (blinkTimer >= holdDuration)
                {
                    state = BlinkState.Opening;
                    blinkTimer = 0f;
                }
                break;

            case BlinkState.Opening:
                blinkTimer += Time.deltaTime;
                float tOpen = Mathf.Clamp01(blinkTimer / openDuration);
                blinkAmount = 1f - tOpen;

                if (blinkTimer >= openDuration)
                {
                    blinkAmount = 0f;
                    state = BlinkState.Idle;
                    ScheduleNextBlink();
                }
                break;
        }
    }

    private void ScheduleNextBlink()
    {
        float delay = Random.Range(minDelay, maxDelay);
        nextBlinkTime = Time.time + delay;
    }

    private void OnGUI()
    {
        if (blinkAmount <= 0f) return;
        if (blackTex == null) return;

        // seberapa besar bagian layar tertutup
        float halfHeight = Screen.height * 0.5f * coverAmount * blinkAmount;

        Rect topRect = new Rect(0, 0, Screen.width, halfHeight);
        Rect bottomRect = new Rect(0, Screen.height - halfHeight, Screen.width, halfHeight);

        GUI.depth = -100; // pastikan di atas gambar lain
        GUI.DrawTexture(topRect, blackTex);
        GUI.DrawTexture(bottomRect, blackTex);
    }
}
