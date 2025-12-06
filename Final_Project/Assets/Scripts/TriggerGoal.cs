using UnityEngine;
using UnityEngine.Events;
 
public class TriggerGoal : MonoBehaviour
{
    public UnityEvent onGoal; 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER HIT: " + other.name);

        if (other.CompareTag("Ball"))
        {
            Debug.Log("GOAL!");
            onGoal?.Invoke();
        }
    }

}
