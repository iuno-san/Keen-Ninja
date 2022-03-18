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

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    // Start is called before the first frame update
     void Start()
    {
        //references - odnoœniki
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();    
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Nasza postaæ ma komponent Rigid Body 2D. Pobierzmy(:
        //body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //obrot postaci prawo-lewo
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );

        //skakanie na spacji
        //if (Input.GetKey(KeyCode.Space) && isGrounded())
        //  Jump();

        //set animator parameters - parametry animacji
        anim.SetBool("run", MathHelper.IsNearlyEqual(body.velocity.x, 0) == false);
        anim.SetBool("grounded", isGrounded());

        print(onWall()+", "+isGrounded());

        //skakanie na scianie - wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(onWall() && isGrounded() == false)
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 8;

            if(Input.GetKey(KeyCode.Space))
                Jump();
        }
        else 
            wallJumpCooldown += Time.deltaTime;
    }
    //metoda na skakanie - jump method
   private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            wallJumpCooldown = 0;
        }
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
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}


class MathHelper
{
    static public bool IsNearlyEqual(float a, float b, float ERROR_TOLERANCE = 0.0001f)
    {
        return Mathf.Abs(a - b) < ERROR_TOLERANCE;
    }
}