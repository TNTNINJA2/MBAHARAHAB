using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private InputAction movementControls;
    [SerializeField]
    private InputAction jumpControls;
    [SerializeField]
    private InputAction boxInteractControls;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private Collider2D baseCollider;
    [SerializeField]
    private Collider2D boxPickupRange;

    
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float movementAcceleration;
    [SerializeField]
    private float jumpVelocity;

    private GameObject currentlyHeldBox = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        movementControls.Enable();
        jumpControls.Enable();
        boxInteractControls.Enable();
    }

    private void OnDisable()
    {
        movementControls.Disable();
        jumpControls.Disable();
        boxInteractControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleBoxInteractions();

    }

    private void handleMovement()
    {
        if (IsOnGround())
        {
            rigidBody.velocity = new Vector2(Mathf.Clamp(movementControls.ReadValue<float>() * maxMoveSpeed,
         -maxMoveSpeed, maxMoveSpeed), rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x + movementControls.ReadValue<float>() * movementAcceleration * Time.deltaTime,
                -maxMoveSpeed, maxMoveSpeed), rigidBody.velocity.y);
        }
        if (movementControls.ReadValue<float>() >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }


        if (jumpControls.ReadValue<float>() == 1)
        {
            if (IsOnGround())
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpVelocity);
            }

        }
    }

    private void handleBoxInteractions()
    {
        if (currentlyHeldBox == null)
        {
            //if player just began pressing shift and there is a box in the range, make it the currently held box
            if (boxInteractControls.ReadValue<float>() == 1)
            {
                Debug.Log("trying to find a box to pickup");
                GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
                foreach (GameObject box in boxes)
                {
                    Debug.Log(box.ToString() + " is being checked for tag 'Box'");
                    if (boxPickupRange.IsTouching(box.GetComponent<Collider2D>()))
                    {
                        currentlyHeldBox = box;
                        Debug.Log(box.ToString() + " is now the currently held box");
                        break;
                    }
                }
            }
        } else
        {
            if (boxInteractControls.ReadValue<float>() == 1)
            {
                currentlyHeldBox = null;
            }
            //currentlyHeldBox.transform.position = new Vector3(transform.position.x, transform.position.y + 2, 0);
        }
    }

        

    public bool IsOnGround()
    {
        List<Collider2D> groundColliders = new List<Collider2D>();
        baseCollider.OverlapCollider(new ContactFilter2D().NoFilter(), groundColliders);
        if (groundColliders.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
