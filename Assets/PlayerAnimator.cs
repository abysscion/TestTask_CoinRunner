using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerController playerController;
    
    private LimbAnimator[] _limbAnimators;
    
    private void Start()
    {
        _limbAnimators = gameObject.GetComponentsInChildren<LimbAnimator>();

        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        playerController.OnLMBReleased += OnLMBReleased;
        playerController.OnLMBPressed += OnLMBPressed;
    }

    private void Unsubscribe()
    {
        playerController.OnLMBReleased -= OnLMBReleased;
        playerController.OnLMBPressed -= OnLMBPressed;
    }

    private void OnLMBPressed()
    {
        foreach (var animator in _limbAnimators)
            animator.PlayAnimation();
    }

    private void OnLMBReleased()
    {
        foreach (var animator in _limbAnimators)
            animator.StopAnimation();
    }
}
