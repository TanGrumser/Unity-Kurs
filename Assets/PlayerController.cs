using UnityEngine;

// This scipt is attached to the player and is responsible for
public class PlayerController : ScrollingObject {
    
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject deathPatciles;
    [SerializeField] private GameObject hookshotPrefab;

    private Rigidbody2D rb;
    private Animator animator;
    private AnchoredJoint2D joint;
    private HookshotClaw hookshot;
    private HookshotSegment hookshotSegment;
    private HookshotSegment lastSegment;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<AnchoredJoint2D>();
        animator = GetComponentInChildren<Animator>();
        hookshotSegment = GetComponent<HookshotSegment>();
    }

    void Update() {

        if (!IsGrounded()) {
            animator.Play("Airbone");
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                rb.AddForce(Vector2.up * 12f, ForceMode2D.Impulse);
            }

            if (rb.velocity.magnitude < 0.1f) {
                animator.Play("Idle");
                animator.speed = 0.6f;
            } else {
                animator.Play("Run");


                animator.speed = Mathf.Abs(rb.velocity.x / 8f);
            }
        }

        if (rb.velocity.x > 0.001)
            sprite.transform.localScale = new Vector3(-0.3f, 0.3f, 1.0f);
        else if (rb.velocity.x < -0.001)
            sprite.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);

        if (Input.GetMouseButtonDown(0)) {
            Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float angle = Vector2.SignedAngle(Vector2.up, direction);

            //Instantiate(hookshotPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            int layerMask = ~0 ^ (0x1 << 8);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 500, layerMask);

            if (hit.collider != null) {
                AttachHookshot(hit.point);
            }
        } else if (Input.GetMouseButtonUp(0)) {
            DeattachHookshot();
        }
    }

    private void FixedUpdate() {
        float horizontalInput = Input.GetAxis("Horizontal");

        rb.AddForce(Vector2.right * horizontalInput * (hookshot == null ? 80f : 8f));

        if (hookshot == null) {
            rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x, -8f, 8f),
                rb.velocity.y
            );
        }
        

    }

    private void AttachHookshot(Vector2 point) {
        Vector2 offset = point - (Vector2)transform.position;
        hookshot = Instantiate(hookshotPrefab, point, Quaternion.identity).GetComponent<HookshotClaw>();
        lastSegment = hookshot.AttachToPoint(transform.position, point);
        lastSegment.SetParentHookshotSegment(hookshotSegment);
        Debug.Log(lastSegment.gameObject.name);
    }

    private void DeattachHookshot() {
        lastSegment?.RemoveParent();
        hookshot?.DestroyRecursive();
        joint.enabled = false;
        hookshot = null;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag.Equals("Deadzone")) {
            // Game Over
            Debug.Log("Game Over!");
            rb.bodyType = RigidbodyType2D.Static;
            gameOverText.SetActive(true);
            deathPatciles.SetActive(true);
            GameManager.Instance.gameOver = true;
        }
    }

    private bool IsGrounded() {
        int layerMask = ~0 ^ (0x1 << 8);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, layerMask);
        
        if (hit.collider != null) {
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }

        return false;
    }
}
