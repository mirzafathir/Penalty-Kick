using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool useLimits = true;

    // batas kiri/kanan di sumbu Z (depan–belakang gawang)
    private Vector2 limitZ = new Vector2(-9f, -3.45f);


    void Update()
    {
        if (Keyboard.current == null) return;

        Vector2 moveInput = Vector2.zero;

        // kiri/kanan → gerak di Z
        if (Keyboard.current.leftArrowKey.isPressed)
            moveInput.x -= 1f;   // nanti kita pakai sebagai Z-
        if (Keyboard.current.rightArrowKey.isPressed)
            moveInput.x += 1f;   // Z+

        

        if (moveInput.sqrMagnitude > 0f)
        {
            moveInput = moveInput.normalized;

            // X input → Z movement, Y input → Y movement
            Vector3 delta = new Vector3(
                0f,                // X: tidak dipakai
                0f,       // naik/turun di Y
                moveInput.x        // kiri/kanan di Z
            ) * moveSpeed * Time.deltaTime;

            Vector3 newPos = transform.position + delta;

            if (useLimits)
            {
                // Vector2 cuma punya .x dan .y → pakai itu untuk min/max
                newPos.z = Mathf.Clamp(newPos.z, limitZ.x, limitZ.y);

            }

            transform.position = newPos;
        }
    }
}
