//Debug.Log("Hejo :)");
//Debug.LogWarning("Ostrze¿enie");
//Debug.LogError("B³¹d!");

/* Praca Domowa 
 * --------------------------------------------
 * Dorobiæ skakanie na spacjê (zrób zmienn¹ prêdkoœci skoku konfigurowaln¹ w inspektorze jak speed)
 * Czym jest prefab i jak siê nim pos³ugiwaæ
 * ZnaleŸæ grafiki ninjy (klatki)
 * Dorobiæ platformy na które nasz ninja mo¿e wskoczyæ
*/
//---------------------------------------------------------------------------kod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ninja : MonoBehaviour
{  //variables - zmienne && wartoœci

    [Header("Movment Parameters")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // ile czasu gracz mo¿e wisieæ w powietrzu przed skokiem
    private float coyoteCounter; // ile czasu up³ynê³o od momentu, gdy gracz uciek³ z krawêdzi

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //si³a skoku na œcianê poziom¹
    [SerializeField] private float wallJumpY; //si³a pionowego skoku przez œcianê

    [Header("Sounds")]
    [SerializeField] private AudioClip JumpSound;
   
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Health healthComponent;
    private float wallJumpCooldown;
    private float horizontalInput;


    // Start is called before the first frame update
     void Start()
    {
        //references - odnoœniki
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        healthComponent = GetComponent<Health>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Nasza postaæ ma komponent Rigid Body 2D. Pobierzmy(:
        //body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //obrot postaci prawo-lewo
        this.UpdateSpritesDirection();

        //skakanie na spacji
        //if (Input.GetKey(KeyCode.Space) && isGrounded())
        //  Jump();

        //set animator parameters - parametry animacji
        anim.SetBool("run", MathHelper.IsNearlyEqual(body.velocity.x, 0) == false);
        anim.SetBool("grounded", isGrounded());

        //print(onWall()+", "+isGrounded());

        //skakanie na scianie - wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            horizontalInput = (healthComponent.IsDead ? 0 : Input.GetAxis("Horizontal"));

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(onWall() && isGrounded() == false)
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 8;

            if (!healthComponent.IsDead && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else 
            wallJumpCooldown += Time.deltaTime;

        // regulowana wysokosc skoku

        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y /2);

        if(onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(isGrounded())
            {
                coyoteCounter = coyoteTime; // zresetuj licznik kojotów, gdy s¹ na ziemi
                jumpCounter = extraJumps;  // zresetuj licznik dodatkowych skokow
            }
            else
                coyoteCounter -= Time.deltaTime; //rozpoczêcie licznika kojotów, gdy nie s¹ na ziemi
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return; //jeœli licznik kojotów wynosi 0 lub mniej i nie znajduje siê na œcianie i nie ma dodatkowych skokow nie rób niczego

        SoundManager.Instance.PlaySound(JumpSound);

        if (isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpPower);
       else
        {
            // jeœli nie le¿y na ziemi, a licznik kojotów jest wiêkszy ni¿ 0, wykonaj normalny skok
            if(coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if(jumpCounter > 0) //jeœli mamy dodatkowe skoki, to skaczemy i zmniejszamy licznik skoków
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }
        //resetowanie licznika kojotów do 0 w celu unikniêcia podwójnych skoków
        coyoteCounter = 0;



    }

    
    private void UpdateSpritesDirection()
    {
        if (body.velocity.x > 0.01f)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        else if (body.velocity.x < -0.01f)
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }


    //skakanie po œcianie
   private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer );
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall() && !healthComponent.IsDead;
    }
}


class MathHelper
{
    static public bool IsNearlyEqual(float a, float b, float ERROR_TOLERANCE = 0.0001f)
    {
        return Mathf.Abs(a - b) < ERROR_TOLERANCE;
    }
}
