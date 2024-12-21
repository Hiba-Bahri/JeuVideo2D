using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    [SerializeField] private int strikes;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        if (strikes-- != 0)
        {
            if (currentCheckpoint == null)
            {
                uiManager.GameOver();
                return;
            }
            playerHealth.Respawn(); //Restore player health and reset animation
            transform.position = currentCheckpoint.position; //Move player to checkpoint location

            //Move the camera to the checkpoint's room
            Camera.main.GetComponent<CameraController>().moveToNewRoom(currentCheckpoint.parent);
        }
        else
        {
            uiManager.GameOver();
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}