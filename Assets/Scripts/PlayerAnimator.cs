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
        playerController.OnFinishReached += OnLMBReleased;
    }

    private void Unsubscribe()
    {
        playerController.OnLMBReleased -= OnLMBReleased;
        playerController.OnLMBPressed -= OnLMBPressed;
        playerController.OnFinishReached -= OnLMBReleased;
    }

    private void OnLMBPressed()
    {
        foreach (var animator in _limbAnimators)
            animator.PlayRunningAnimation();
    }

    private void OnLMBReleased()
    {
        foreach (var animator in _limbAnimators)
            animator.PlayIdleAnimation();
    }
}
