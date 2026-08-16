using UnityEngine;
using UnityEngine.Events;

public class enemyClass : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector2 moveTowardsPos;

    [Header("Attacking")]

    public enemyAttacks[] attacks;
    public bool isAttacking = false;

    public float attackTimer = 5;
    public float attackCooldown = 1;

    [Header("Movement")]
    public float stoppingDistance = 1;
    public float currentMoveSpeed;

    public float walkSpeed = 2;

    [SerializeField] private float traction = 5;

    public bool canMove = true;

    [Header("GFX")]
    public Transform gfx;
    public Animator anim;

    [Header("Enemy Scripts")]
    public healthClass healthScript;

    Rigidbody2D rb;


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<healthClass>();
        anim = GetComponent<Animator>();
    }




    public virtual void handleMovement()
    {

        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, getMovementSpeed(), traction * Time.deltaTime);

        float distanceFromPos = Vector2.Distance(transform.position, moveTowardsPos);




        Vector2 dir = (moveTowardsPos - (Vector2)transform.position).normalized;
        rb.position += dir * currentMoveSpeed * Time.deltaTime;




    }



    public virtual void handleAnimation()
    {

        //Look Towards Direction
        int x = moveTowardsPos.x > transform.position.x ? 1 : -1;
        gfx.transform.localScale = new Vector3(x, 1, 1);

    }



    public int chooseAttack()
    {
        float totalWeight = 0;
        float distanceFromPlayer = Vector2.Distance(transform.position, target.position);

        for (int i = 0; i < attacks.Length; i++)
        {
            if (distanceFromPlayer > attacks[i].neededRange) break;
            totalWeight += attacks[i].weight;
        }
        //
        float randomValue = Random.Range(0, totalWeight);
        float currentSum = 0;

        for (int i = 0; i < attacks.Length; i++)
        {
            if (distanceFromPlayer > attacks[i].neededRange) break;
            currentSum += attacks[i].weight;
            if (randomValue <= currentSum)
            {
                return i;
            }

        }


        return -1;
    }



    public float getMovementSpeed()
    {

        float newMoveSpeed = 0;

        newMoveSpeed = walkSpeed;


        if (!canMove)
        {
            newMoveSpeed = 0;
        }

        return newMoveSpeed;

    }

    [System.Serializable]
    public class enemyAttacks
    {
        public string attackName;

        public float attackTime;
        [Header("Attack Range")]
        public Color attackColor = Color.white;
        public float neededRange = 2.5f;
        [Header("Events")]
        public UnityEvent attackEvent;

        [Range(0, 1)] public float weight;

    }
}
