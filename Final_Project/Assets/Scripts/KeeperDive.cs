using System.Collections;
using UnityEngine;

public class KeeperDive : MonoBehaviour
{
    [Header("Animator")]
    public Animator keeperAnimator;
    public string chooseParamName = "Choose";

    [Header("Blend Tree Values")]
    public float idleValue = 0f;
    public float diveRightValue = 0.5f;
    public float diveLeftValue = 1f;

    [Header("Blend Settings")]
    public float blendSpeed = 5f;       // semakin besar, semakin cepat blend
    public float autoResetTime = 0.8f;  // setelah sekian detik, balik ke idle

    [Header("Reset Transform (optional)")]
    public bool resetPositionOnReset = true;

    private Vector3 startPos;
    private Quaternion startRot;

    private int chooseParamHash;
    private float currentValue;
    private float targetValue;

    private Coroutine autoResetCoroutine;

    private void Awake()
    {
        if (keeperAnimator == null)
            keeperAnimator = GetComponentInChildren<Animator>();

        if (keeperAnimator == null)
            Debug.LogError("KeeperDive: Animator belum di-assign!");

        chooseParamHash = Animator.StringToHash(chooseParamName);

        startPos = transform.position;
        startRot = transform.rotation;

        currentValue = idleValue;
        targetValue = idleValue;
        SetChooseValue(currentValue);
    }

    private void Update()
    {
        // Smooth blend menuju targetValue
        if (Mathf.Approximately(currentValue, targetValue) == false)
        {
            currentValue = Mathf.MoveTowards(
                currentValue,
                targetValue,
                blendSpeed * Time.deltaTime
            );
            SetChooseValue(currentValue);
        }
    }

    // ------------ DIPANGGIL DARI BALL ------------

    public void RandomDive()
    {
        int r = Random.Range(0, 2); // 0 = kanan, 1 = kiri

        if (r == 0)
            SetDiveRight();
        else
            SetDiveLeft();

        StartAutoReset();
    }

    public void ResetKeeper()
    {
        // reset posisi (opsional)
        if (resetPositionOnReset)
        {
            transform.SetPositionAndRotation(startPos, startRot);
        }

        // kembali ke idle
        targetValue = idleValue;
        currentValue = idleValue;
        SetChooseValue(currentValue);
    }

    // ------------ INTERNAL: DIVE / BLEND ------------

    private void SetDiveLeft()
    {
        targetValue = diveLeftValue;
    }

    private void SetDiveRight()
    {
        targetValue = diveRightValue;
    }

    private void StartAutoReset()
    {
        if (autoResetCoroutine != null)
            StopCoroutine(autoResetCoroutine);

        autoResetCoroutine = StartCoroutine(AutoResetCoroutine());
    }

    private IEnumerator AutoResetCoroutine()
    {
        yield return new WaitForSeconds(autoResetTime);
        targetValue = idleValue; // balik smooth ke idle
    }

    private void SetChooseValue(float value)
    {
        if (keeperAnimator == null) return;
        keeperAnimator.SetFloat(chooseParamHash, value);
    }
}
