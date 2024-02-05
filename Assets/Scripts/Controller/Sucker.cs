using UnityEngine;

public class Sucker : MonoBehaviour
{

    private SpriteRenderer suctionSprite;
    public Color suctionPointColor;
    public FixedJoint2D fixedSuctionJoint;

    public Sucker OtherSucker;
    public Vector2 SuckPosition = Vector2.zero;
    public GameObject suctionPoint;
    public bool JustStartedSucking { get { return framesSinceStartedSucking < 3; } }
    private int framesSinceStartedSucking = 0;

    public bool Sucking { get; private set; }

    private float maxStamina;
    public float staminaChargeSpeed = 2f;
    public float MaxStamina
    {
        get { return maxStamina; }
        set
        {
            maxStamina = value;
            CurStamina = maxStamina;
        }
    }
    public float CurStamina { get; private set; }

    private void Start()
    {

        suctionSprite = suctionPoint.GetComponent<SpriteRenderer>();
        suctionSprite.color = suctionPointColor;
        suctionPoint.SetActive(false);
    }

    private void Update()
    {
        suctionSprite.color = suctionPointColor;
        Debug.Log("CurStamina = " + CurStamina);
    }
    public bool TouchingSuckable
    {
        get { return TouchedSuckable != null; }
    }
    public GameObject TouchedSuckable;

    // this is the same as Touching suckable, but here because we plan to add stamina. 
    public bool CanSuck
    {
        get { return TouchingSuckable; }
    }

    public void FixedUpdate()
    {
        if (framesSinceStartedSucking < 3)
        {
            framesSinceStartedSucking++;
        }

        if (Sucking)
        {
            CurStamina = Mathf.Max(CurStamina - Time.fixedDeltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colliderLayer = collision.gameObject.layer;
        if (colliderLayer == LayerMask.NameToLayer("terrain") || colliderLayer == LayerMask.NameToLayer("kelp"))
        {
            TouchedSuckable = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (TouchingSuckable && collision.gameObject == TouchedSuckable)
        {
            TouchedSuckable = null;
        }
    }

    public void Suck()
    {
        if (TouchingSuckable)
        {
            suctionPoint.SetActive(true);
            fixedSuctionJoint.connectedBody = TouchedSuckable.GetComponent<Rigidbody2D>();
            framesSinceStartedSucking = 0;
            Sucking = true;
        }

    }

    public void StopSucking()
    {
        if (Sucking)
        {
            fixedSuctionJoint.connectedBody = null;
            CurStamina = MaxStamina;
            suctionPoint.SetActive(false);
            Sucking = false;
        }

    }
}
