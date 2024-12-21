using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private UIManager uiManager;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Jump Sound")]
    [SerializeField] private AudioClip jumpSound;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        uiManager = FindObjectOfType<UIManager>();

    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);

        anim.SetBool("grounded", isGrounded());

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
            
        if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if(onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
            body.gravityScale = 2;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if(isGrounded())
            {
                coyoteTimeCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }
        
        private void MoveToNextLevel()
        {
            if (DoorKey.gotKey == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
            uiManager.showKeyText();
            }
        }
        
        /* if (wallJumpCooldown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 2;

            if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                    if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
                    {
                        SoundManager.instance.PlaySound(jumpSound);
                    }
                }
        }
        else
            wallJumpCooldown += Time.deltaTime; */

    private void Jump()
    {
        if(coyoteTimeCounter <= 0 && !onWall() && jumpCounter <= 0)
            return;

        SoundManager.instance.PlaySound(jumpSound);

        if(onWall())
            WallJump();
        else{
            if(isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if(coyoteTimeCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            coyoteTimeCounter = 0;
        }

        /* if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            }
            wallJumpCooldown = 0;
        } */

    }
    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the player collides with the final level door
        if (collision.gameObject.CompareTag("FinalLevelDoor"))
        {
            MoveToNextLevel();
        }
    }
}