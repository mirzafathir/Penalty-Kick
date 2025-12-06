using UnityEngine;

public class GoalieDive : MonoBehaviour
{
    public Animator keeperAnimator;
    private string diveLeftTrigger = "DiveLeft";
    private string diveRightTrigger = "DiveRight";

    [Header("Reset ke tengah saat reset manual")]
    public bool resetPositionOnReset = true;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake()
    {
        if (keeperAnimator == null)
            keeperAnimator = GetComponentInChildren<Animator>();

        startPos = transform.position;
        startRot = transform.rotation;
    }

    // dipanggil saat bola ditendang
    public void DiveRandom()
    {
            Debug.Log("Diving Random");
        if (keeperAnimator == null) return;

        int r = Random.Range(0, 3);
        if (r == 0){
            keeperAnimator.SetTrigger(diveRightTrigger);
            Debug.Log("DiveRight Triggered");
            }
        else if(r==1){
            keeperAnimator.SetTrigger(diveLeftTrigger);
            }
    }

    // dipanggil saat reset (Space kedua)
    public void ResetKeeper()
    {
        transform.SetPositionAndRotation(startPos, startRot);
        keeperAnimator.transform.localPosition = Vector3.zero;
        keeperAnimator.transform.localRotation = Quaternion.identity;
    }
}
