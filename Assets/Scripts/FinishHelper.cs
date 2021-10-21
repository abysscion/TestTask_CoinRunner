using UnityEngine;

public class FinishHelper : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public PlayerController playerController;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
            playerController.ReachFinish();
    }
}
