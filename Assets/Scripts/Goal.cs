using UnityEngine;

// the exit pad at the end of each maze
public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LevelManager.Instance != null)
            LevelManager.Instance.Won();
    }
}
