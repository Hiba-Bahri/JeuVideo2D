using UnityEngine;

public class DeathFloor : MonoBehaviour
{

    private UIManager _uiManager;
  private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _uiManager.GameOver();
        }

    }
}
