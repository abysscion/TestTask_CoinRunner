using UnityEngine;

public class CoinHelper : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public PlayerController playerController;

    private Vector3 _rotationAngles;

    private void Start()
    {
        _rotationAngles = new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), Random.Range(-60, 60));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerController.PickUpCoin();
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(_rotationAngles * Time.deltaTime);
    }
}
