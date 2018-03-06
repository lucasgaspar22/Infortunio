using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator anim;
    private CircleCollider2D myBoxCollider;
    private Vector2 spawnPosition;
    private bool isBoosted = false;
    private float maxFallTime;
    private float currentFallTime;
    private bool invert = false;
    [SerializeField] int Speed;
    [SerializeField] Transform ground;
    [SerializeField] int JumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask breakLayer;
    [SerializeField] GameObject lifeBehavior;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource breakSound;
    [HideInInspector] public bool flipped = true;
    [HideInInspector] public bool doubleJump = false;
    [HideInInspector] public bool hitted = false;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBoxCollider = GetComponent<CircleCollider2D>();
        spawnPosition = gameObject.transform.position;
        maxFallTime = 1.5f;
        currentFallTime = 0;
    }

    void Update()
    {
        Move();
        FallChecker();
        DebugRespawn();
    }

    void Move()
    {

        if (IsGrounded())
        {
            doubleJump = false;
            isBoosted = false;
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontal));
        rigidBody.velocity = new Vector2(horizontal * Speed, rigidBody.velocity.y);

        anim.SetBool("IsGrounded", IsGrounded());
        flipped = horizontal >= 0 ? false : true;
        if (horizontal > 0) invert = true;
        else if (horizontal < 0) invert = false;
        Flip(invert);
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (Input.GetButtonDown("Jump") && (IsGrounded() || !doubleJump)) {
            rigidBody.AddForce(new Vector2(0, JumpForce));
            jumpSound.Play();
            if (!doubleJump && !IsGrounded())
            {
                doubleJump = true;
                jumpSound.Play();
            }
        }

        if ((Input.GetButtonDown("Vertical")|| Input.GetAxis("BoostAxis")!=0) && !isBoosted)
        {
            isBoosted = true;
            rigidBody.AddForce(new Vector2(0, -JumpForce));
        }
    }

    void FallChecker()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        Debug.DrawRay(transform.position, Vector2.down,Color.red, 2f,false);
        if (!IsGrounded() && hit.collider.gameObject.tag == "Death")//(hit.collider.gameObject.tag != "Ground" && hit.collider.gameObject.tag != "Breakable Platformer" && hit.collider.gameObject.tag != "Horizontal Moving Platformer" && hit.collider.gameObject.tag != "Vertical Moving Platformer"))
        {
            Debug.Log(hit.collider.gameObject.tag);
            currentFallTime += Time.deltaTime;
            if (currentFallTime >= maxFallTime)
            {
                Respawn();
                UpdateLife();
            }
        }
        else
        {
            currentFallTime = maxFallTime != 0 ? 0 : currentFallTime;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Death")
        {
            Respawn();
            UpdateLife();
        }
        if (collision.collider.gameObject.tag == "Horizontal Moving Platformer")
        {
            HorizontalPaltformBehavior platformer = collision.collider.gameObject.GetComponent<HorizontalPaltformBehavior>();
            transform.localPosition += 2*platformer.platformSpeed * platformer.direction * Time.deltaTime;
        }

        if (collision.collider.gameObject.tag == "Breakable Platformer" && isBoosted)
        {

            if (isBoosted)
            {
                breakSound.Play();
                Destroy(collision.collider.gameObject);
            }
            isBoosted = false;
        } 
        hitted = collision.collider.gameObject.tag == "Enemy" ? true : false;
        if (hitted )
        {
            UpdateLife();
            Respawn();
        }
		bool saved = collision.collider.gameObject.tag == "Similar" ? true : false;
		if (saved) {
			StartCoroutine (WinScene ());
		}
    }

	IEnumerator WinScene(){
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("VictoryScene");
	}

    void Flip(bool invert)
    {
        flipped = !flipped;
        Vector3 myScale = transform.localScale;
        if (invert) myScale.x = 0.69f;
        else myScale.x = -0.69f;
        transform.localScale = myScale;
    }

    //just used for debugging purposes to reset player
    void DebugRespawn()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Respawn();
        }
    }

    //use this for in game logic
    void Respawn()
    {
        currentFallTime = 0;
        gameObject.transform.position = GetSpawnPosition();
        rigidBody.velocity = new Vector2(0, 0);
    }

    Vector2 GetSpawnPosition()
    {
        return spawnPosition;
    }
    
    void UpdateLife()
    {
        lifeBehavior.GetComponent<LifeBehaviour>().UpdateLife();
    }

    bool isBreakable()
    {
        return Physics2D.OverlapCircle(ground.position, 0.15F, breakLayer);
    }

    bool IsGrounded()
    {
        
        return (Physics2D.OverlapCircle(ground.position, 0.15F, groundLayer) || Physics2D.OverlapCircle(ground.position, 0.15F, breakLayer));
    }
}
