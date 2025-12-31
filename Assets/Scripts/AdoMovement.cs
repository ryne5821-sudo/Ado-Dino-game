using UnityEngine;

public class AdoMovement : MonoBehaviour
{
    [SerializeField] private Animator adoAnimator;
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody2D rb; 
    
    private bool _isGameStarted = false;
    
    private bool _isTouchingGround = false;

    private bool _isDead = false;
    
    // Update is called once per frame
    void Update()
    {
        //Inputs
        bool isJumpButtonPressed = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) ||  Input.GetKey(KeyCode.W);
        bool isCrouchButtonPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        if(!_isDead)
        {
            if (isJumpButtonPressed)
            {
                if (_isGameStarted && _isTouchingGround)
                {
                    //Jump
                    Jump();
                    Debug.Log("Jump");
                }
                else
                {
                    _isGameStarted = true;
                    
                    GameManager.Instance.gameStarted = true;
                }
            }
        } else if(_isDead)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.position = new Vector2(-6.0f, -4.245642f);
            transform.rotation = Quaternion.identity;
        }
                
        //animate
        adoAnimator.SetBool("StartedGame", _isGameStarted);
        adoAnimator.SetBool("IsCrouching", isCrouchButtonPressed && _isTouchingGround && !isJumpButtonPressed);
        adoAnimator.SetBool("IsDead", _isDead);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            _isTouchingGround = true;
            Debug.Log("touching floor");
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Die
            _isDead = true;
            GameManager.Instance.gameEnded = true;
        }
    }
    
    void Jump()
    { 
        rb.AddForce(Vector2.up * jumpForce);
        _isTouchingGround = false;
    }
}