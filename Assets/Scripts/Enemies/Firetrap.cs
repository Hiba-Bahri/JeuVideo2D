using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; // Whether the trap has been triggered
    private bool active;    // Whether the trap is active (dealing damage)
    private bool playerDamaged; // Whether the player has been damaged by this trap's activation

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
        }
    }

    private void Update()
    {
        if (active && !playerDamaged)
        {
            Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("Player"));
            if (player != null)
            {
                DamagePlayer(player);
                playerDamaged = true; // Ensure the player is only damaged once during this activation
            }
        }
    }

    private void DamagePlayer(Collider2D player)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true; // Prevent re-triggering
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);

        spriteRend.color = Color.white;
        active = true; // Firetrap is now active and can deal damage
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);

        active = false;      // Firetrap is no longer active
        triggered = false;   // Allow retriggering
        playerDamaged = false; // Reset the damage status for the player
        anim.SetBool("activated", false);
    }

    private void OnDrawGizmos()
    {
        // Draw the firetrap's area to debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 0.5f, 0));
    }
}
