using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;

    [Header("Force Settings")]
    public float headForce = 2500f;
    public float tentacleForce = 50f;
    public float range = 10f;

    [Header("Sucker settings")]
    public LayerMask SuckableLayers;
    public float maxSuckerStamina = 5f;
    public float staminaRegenSpeed = 1f;

    [Header("Body Parts")]
    public Rigidbody2D headRB;
    public Sucker L_sucker;
    public Sucker R_sucker;
    public Transform L_shoulder;
    public Transform R_shoulder;



    public void Start()
    {
        _input = gameObject.GetComponent<PlayerInput>();
        L_sucker.OtherSucker = R_sucker;
        R_sucker.OtherSucker = L_sucker;
        L_sucker.MaxStamina = maxSuckerStamina;
        R_sucker.MaxStamina = maxSuckerStamina;
    }

    public void FixedUpdate()
    {
        HandleSucker(_input.SuctionLeftHeld, _input.SuctionLeftPressed, _input.MoveLeftValue, L_sucker, L_shoulder);
        HandleSucker(_input.SuctionRightHeld, _input.SuctionRightPressed, _input.MoveRightValue, R_sucker, R_shoulder);

        if ((R_sucker.Sucking) || (L_sucker.Sucking))
        {

            // Determine the target position based on the opposite of the stick input
            Vector2 targetOffset;
            Transform shoulder;
            if (R_sucker.Sucking)
            {
                targetOffset = -_input.MoveRightValue.normalized * range;
                shoulder = R_shoulder;
            }
            else
            {
                targetOffset = -_input.MoveLeftValue.normalized * range;
                shoulder = L_shoulder;
            }
            Vector2 targetPosition = (Vector2)shoulder.position + targetOffset;

            // Calculate the direction and magnitude of the force to apply
            Vector2 direction = (targetPosition - headRB.position).normalized;
            float distance = Vector2.Distance(headRB.position, targetPosition);
            Vector2 force = direction * headForce * distance * Time.fixedDeltaTime;

            headRB.AddForce(force);
        }
    }

    public void HandleSucker(bool suctionHeld, bool suctionPressed, Vector2 moveInput, Sucker sucker,  Transform shoulder)
    {

        if (sucker.Sucking)
        {
            if (sucker.OtherSucker.JustStartedSucking || !suctionHeld)
            {
                sucker.StopSucking();
            }
        } else

        if (!sucker.Sucking)
        {
            if (suctionPressed && sucker.CanSuck)
            {
                sucker.Suck();
            }
            MoveSuckerAddForce(moveInput, shoulder, sucker);
        }
    }


    public void MoveSucker(Vector2 input, Transform shoulder, Sucker sucker)
    {
        Rigidbody2D suckerRB = sucker.GetComponent<Rigidbody2D>();

        if (input !=  Vector2.zero)
        {
            Vector2 targetOffset = input * range;
            Vector2 targetPosition = (Vector2)shoulder.position + targetOffset;
            Vector2 newPosition = Vector2.MoveTowards(suckerRB.position, targetPosition, tentacleForce * Time.fixedDeltaTime);

            suckerRB.MovePosition(newPosition);
        }
    }

    public void MoveSuckerAddForce(Vector2 input, Transform shoulder, Sucker sucker)
    {
        Rigidbody2D suckerRB = sucker.GetComponent<Rigidbody2D>();

        if (input != Vector2.zero)
        {
            Vector2 targetOffset = input * range;
            Vector2 targetPosition = (Vector2)shoulder.position + targetOffset;

            // Calculate direction from current position to the target position
            Vector2 direction = (targetPosition - suckerRB.position).normalized;

            // Calculate force vector, could adjust the magnitude as necessary
            Vector2 force = direction * tentacleForce;

            // Apply force towards the target position
            suckerRB.AddForce(force);
        }
    }

}
