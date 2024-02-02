using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput _input;

    public Rigidbody2D headRB;
    public Sucker L_sucker;
    public Sucker R_sucker;

    public float maxHeadForce = 100f;
    public float tentacleForce = 50f;
    public float range = 10f;
    public float headPositionLenience;

    public Transform L_shoulder;
    public Transform R_shoulder;

    public float tentacleGravity = 8f;
    public float headGravity = 8f;

    public void Start()
    {
        _input = gameObject.GetComponent<PlayerInput>();
        L_sucker.OtherSucker = R_sucker;
        R_sucker.OtherSucker = L_sucker;
    }

    public void FixedUpdate()
    {
        HandleSucker(_input.SuctionLeftHeld, _input.SuctionLeftPressed, _input.MoveLeftValue, L_sucker, L_shoulder);
        HandleSucker(_input.SuctionRightHeld, _input.SuctionRightPressed, _input.MoveRightValue, R_sucker, R_shoulder);

        if ((R_sucker.Sucking) || (L_sucker.Sucking))
        {
            headRB.gravityScale = 0;

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
            Vector2 force = direction * maxHeadForce * distance * Time.fixedDeltaTime;

            headRB.AddForce(force);
        }
        else
        {
            headRB.gravityScale = headGravity;
        }
    }

    public void HandleSucker(bool suctionHeld, bool suctionPressed, Vector2 moveInput, Sucker sucker,  Transform shoulder)
    {

        if (sucker.Sucking)
        {
            if (!sucker.TouchingSuckable || sucker.OtherSucker.JustStartedSucking || !suctionHeld)
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
            MoveSucker(moveInput, shoulder, sucker);
        }
    }


    public void MoveSucker(Vector2 input, Transform shoulder, Sucker sucker)
    {
        Rigidbody2D suckerRB = sucker.GetComponent<Rigidbody2D>();
        if (input == Vector2.zero)
        {
            suckerRB.gravityScale = tentacleGravity;
        }
        else
        {
            suckerRB.gravityScale = 0;
            Vector2 targetOffset = input * range;
            Vector2 targetPosition = (Vector2)shoulder.position + targetOffset;
            Vector2 newPosition = Vector2.MoveTowards(suckerRB.position, targetPosition, tentacleForce * Time.fixedDeltaTime);

            suckerRB.MovePosition(newPosition);
        }
    }
}
