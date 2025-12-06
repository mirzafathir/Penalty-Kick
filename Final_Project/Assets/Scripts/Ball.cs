using UnityEngine;
using UnityEngine.InputSystem;
 
public class BallController : MonoBehaviour
{
    [Header("Shoot Settings")]
    public float shootForce = 10f;
    public float extraUpwardForce = .6f;
    public Transform target;          // drag target (objek kuning) ke sini

    [Header("Keeper")]
    // public KeeperDive keeper;   // drag KeeperRoot (dengan KeeperController)
    public GoalieDive keeper;   // drag KeeperRoot (dengan KeeperController)

    private Rigidbody rb;

    private Vector3 startPos;
    private Quaternion startRot;

    private enum State
    {
        ReadyToShoot,
        BallInFlight
    }

    private State currentState = State.ReadyToShoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("BallController: Rigidbody belum dipasang!");

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (currentState == State.ReadyToShoot)
            {
                ShootAndDive();
            }
            else if (currentState == State.BallInFlight)
            {
                ResetAll();
            }
        }
    }

    // ------------ SHOOT ------------

    private void ShootAndDive()
    {
        if (rb == null || target == null)
        {
            return;
        }

        // arah dari bola ke target
        Vector3 dir = (target.position - transform.position).normalized;

        // reset velocity dulu
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // beri gaya
        Vector3 force = dir * shootForce + Vector3.up * extraUpwardForce;
        rb.AddForce(force, ForceMode.Impulse);

        // suruh keeper dive
        if (keeper != null)
        {
            Debug.Log("Suruh Kiper Dive!!");
            // keeper.RandomDive();
            keeper.DiveRandom();
        }

        currentState = State.BallInFlight;
    }

    // ------------ RESET ------------

    private void ResetAll()
    {
        // reset bola
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        transform.SetPositionAndRotation(startPos, startRot);

        // reset keeper
        if (keeper != null)
        {
            keeper.ResetKeeper();
        }

        currentState = State.ReadyToShoot;
    }
}
