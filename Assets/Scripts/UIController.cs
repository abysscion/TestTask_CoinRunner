using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI coinsCounter;
    public CanvasGroup winPanel;

    private void Start()
    {
        coinsCounter.text = "0";
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void RestartButton_OnClick()
    {
        SceneManager.LoadScene(0);
    }

    private void OnCoinPickedUp(int newAmount)
    {
        coinsCounter.text = $"{newAmount}";
    }

    private void OnFinishReached()
    {
        winPanel.alpha = 1;
        winPanel.blocksRaycasts = true;
        winPanel.interactable = true;
    }

    private void Subscribe()
    {
        playerController.OnCoinPickedUp += OnCoinPickedUp;
        playerController.OnFinishReached += OnFinishReached;
    }

    private void Unsubscribe()
    {
        playerController.OnCoinPickedUp -= OnCoinPickedUp;
        playerController.OnFinishReached -= OnFinishReached;
    }
}
