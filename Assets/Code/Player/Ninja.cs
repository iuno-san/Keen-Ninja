//Debug.Log("Hejo :)");
//Debug.LogWarning("Ostrze�enie");
//Debug.LogError("B��d!");

/* Praca Domowa 
 * --------------------------------------------
 * Dorobi� skakanie na spacj� (zr�b zmienn� pr�dko�ci skoku konfigurowaln� w inspektorze jak speed)
 * Czym jest prefab i jak si� nim pos�ugiwa�
 * Znale�� grafiki ninjy (klatki)
 * Dorobi� platformy na kt�re nasz ninja mo�e wskoczy�
*/
//---------------------------------------------------------------------------kod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ninja : MonoBehaviour
{  //variables - zmienne && warto�ci

    [Header("Movment Parameters")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // ile czasu gracz mo�e wisie� w powietrzu przed skokiem
    private float coyoteCounter; // ile czasu up�yn�o od momentu, gdy gracz uciek� z kraw�dzi

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //si�a skoku na �cian� poziom�
    [SerializeField] private float wallJumpY; //si�a pionowego skoku przez �cian�

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
        //references - odno�niki
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        healthComponent = GetComponent<Health>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Nasza posta� ma komponent Rigid Body 2D. Pobierzmy(:
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
                coyoteCounter = coyoteTime; // zresetuj licznik kojot�w, gdy s� na ziemi
                jumpCounter = extraJumps;  // zresetuj licznik dodatkowych skokow
            }
            else
                coyoteCounter -= Time.deltaTime; //rozpocz�cie licznika kojot�w, gdy nie s� na ziemi
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return; //je�li licznik kojot�w wynosi 0 lub mniej i nie znajduje si� na �cianie i nie ma dodatkowych skokow nie r�b niczego

        SoundManager.Instance.PlaySound(JumpSound);

        if (isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpPower);
       else
        {
            // je�li nie le�y na ziemi, a licznik kojot�w jest wi�kszy ni� 0, wykonaj normalny skok
            if(coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if(jumpCounter > 0) //je�li mamy dodatkowe skoki, to skaczemy i zmniejszamy licznik skok�w
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }
        //resetowanie licznika kojot�w do 0 w celu unikni�cia podw�jnych skok�w
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


    //skakanie po �cianie
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
