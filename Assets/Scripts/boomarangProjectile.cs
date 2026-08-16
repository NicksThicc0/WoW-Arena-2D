using System.Collections;
using UnityEngine;

public class boomarangProjectile : MonoBehaviour
{
    public boomarangScriptableObject boomarangData;
    public playerScript player;
    public boomarangScript _boomarangScript;


    public Vector3 moveTowardsPos;



    [SerializeField] private int currentBounces = 0;
    private bool returningToPlayer = false;

    float distanceFromPoint;


    [Header("GFX")]
    [SerializeField] private SpriteRenderer boomarangGFX;

    //
    Rigidbody2D rb;
    Collider2D collisions;
    Coroutine returnTimer;





    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collisions = GetComponent<Collider2D>();
    }

    public void setBoomarang(boomarangScriptableObject data, Vector2 _moveToPos, playerScript _player, boomarangScript _boomarang)
    {
        boomarangData = data;
        boomarangGFX.sprite = boomarangData.boomarangSprite;

        moveTowardsPos = _moveToPos;

        player = _player;
        _boomarangScript = _boomarang;

        //


        if (returnTimer != null)
        {
            StopCoroutine(returnTimer);
            returnTimer = null;
        }
        returnTimer = StartCoroutine(returnAfterAWhile());
    }

    private void Update()
    {
        distanceFromPoint = Vector2.Distance(transform.position, moveTowardsPos);
        float distancesFromPlayer = Vector2.Distance(transform.position, player.transform.position);

        collisions.enabled = !returningToPlayer;



        if (returningToPlayer)
        {
            moveTowardsPos = player.weaponPivot.position;

            if (distanceFromPoint <= .1f)
            {
                _boomarangScript.catchBoomarang();
                Destroy(gameObject);
            }
        }
        else
        {
            if (distanceFromPoint <= .1f)
            {
                returnToPlayer();
            }
            if (distancesFromPlayer <= .1f)
            {
                _boomarangScript.catchBoomarang();
                Destroy(gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0) && boomarangData.isMagic)
        {
            returningToPlayer = true;
        }

        handleGFX();
        //





    }

    private void FixedUpdate()
    {
        handleMovement();
    }


    void handleMovement()
    {


        Vector2 dir = (moveTowardsPos - transform.position).normalized;
        rb.linearVelocity = dir * boomarangData.throwSpeed;


        //Returning To Player

    }
    void handleGFX()
    {
        boomarangGFX.transform.Rotate(0, 0, (360 * boomarangData.throwSpeed) * Time.deltaTime);

    }


    void findNextTarget(GameObject lastHitObj)
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, boomarangData.detectRange);
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, hitColliders[i].transform.position);

            if (hitColliders[i].gameObject == player.gameObject)
            {
                continue;
            }

            if (distance < closestDistance && hitColliders[i].transform.root.gameObject != lastHitObj && hitColliders[i].GetComponent<healthClass>())
            {
                closestDistance = distance;
                closestIndex = i;
            }


        }

        if (closestIndex < 0)
        {

            returnToPlayer();
            return;
        }

        moveTowardsPos = hitColliders[closestIndex].transform.position;


    }


    void returnToPlayer()
    {
        moveTowardsPos = player.weaponPivot.position;
        returningToPlayer = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentBounces < boomarangData.maxBounces)
        {
            currentBounces++;
            findNextTarget(collision.transform.root.gameObject);

            if (collision.gameObject.GetComponent<healthClass>() != null)
            {
                healthClass healthScript = collision.gameObject.GetComponent<healthClass>();
                healthScript.takeDamage(boomarangData.damage, transform.root.position);
            }
            if (collision.gameObject.GetComponentInParent<healthClass>() != null)
            {
                healthClass healthScript = collision.gameObject.GetComponentInParent<healthClass>();
                healthScript.takeDamage(boomarangData.damage, transform.root.position);
            }

            Debug.Log(collision.gameObject.name);

        }
        else
        {
            returningToPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, boomarangData.detectRange);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(moveTowardsPos, .5f);
    }


    IEnumerator returnAfterAWhile()
    {
        yield return new WaitForSeconds(boomarangData.returnTime);
        returnToPlayer();
    }
}
