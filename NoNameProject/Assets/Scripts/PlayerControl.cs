using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private bool facingRight = true;
    private bool jump = false;
    private bool isJump = false;

    public float speed = 16f;
    public float jumpForce = 1200f;

    private Transform groundCheck;
    public bool grounded = false;

    public Animator anim;
    public float groundRadius = 0.1f;
    public LayerMask whatIsGround;

    private Vector3 pos;
    private Camera mainCamera;

    void Start()
    {
        groundCheck = transform.Find("groundCheck");
        mainCamera = (Camera) GameObject.FindGameObjectWithTag("MainCamera").GetComponent("Camera");

        whatIsGround = LayerMask.GetMask("Default", "Ground");
        anim = (Animator)GameObject.FindWithTag("PlayerPivot").GetComponent("Animator");
    }

    void Update()
    {
        pos = mainCamera.WorldToScreenPoint(transform.position);

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
        else if (grounded)
        {
            anim.SetBool("Grounded", true);
        }
        else if (!grounded)
        {
            anim.SetBool("Grounded", false);
        }

        if (Input.GetButton("Fire1") )
        {
            LookAtCursor();
            AttackAnimationStart();
        }
    }

    void FixedUpdate()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position += move * speed * Time.deltaTime;

        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h));

        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }

        if (isJump)
        {
            anim.SetBool("Jump", false);
            isJump = false;
        }

        if (jump)
        {
            anim.SetBool("Jump", true);

            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

            jump = false;
            isJump = true;
        }
       
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void LookAtCursor()
    {
        if (Input.mousePosition.x < pos.x && facingRight)
        {
            Flip();
        }
        else if (Input.mousePosition.x > pos.x && !facingRight)
        {
            Flip();
        }

    }

    public void AttackAnimationStart()
    {
        anim.SetBool("Attack", true);
    }

}