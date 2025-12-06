using System.Collections;
using UnityEngine;
using TMPro;

public class GoalScript : MonoBehaviour
{
    public TextMeshProUGUI goalText;
    public float displayTime = 1.5f;

    private void Awake()
    {
        goalText.gameObject.SetActive(false);
    }

    public void ShowGoalText()
    {
        StartCoroutine(ShowTextRoutine());
    }

    private IEnumerator ShowTextRoutine()
    {
        goalText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        goalText.gameObject.SetActive(false);
    }
}
