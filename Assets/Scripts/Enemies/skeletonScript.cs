using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class skeletonScript : enemyClass
{
    public enum skeletonStates { Attacking }




    [Header("Attacking")]
    private float attackRange;


    [Header("throwPoint")]
    [SerializeField] private Transform piviot;
    [SerializeField] private Transform throwPoint; 


    [Header("Throwing Bone Attack")]
    [SerializeField] private GameObject boneProjectile;
    [SerializeField] private float throwForce = 100;

    [SerializeField] private float boneDamage = 2;
    [Header("")]



    [Header("Debugging")]
    [SerializeField] private bool debug;


    public override void Start()
    {
        base.Start();

        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i].neededRange > attackRange)
            {
                attackRange = attacks[i].neededRange;
            }
        }

    }


    //Seperate into own functions
    private void Update()
    {
        if (target != null)
        {
            Vector2 directionTowardsPlayer = target.position - transform.position;
            float distance = Vector2.Distance(transform.position, target.position);

            moveTowardsPos = target.position;



            if (distance <= attackRange && attackCooldown > 0 && !isAttacking)
            {
                attackCooldown -= 1 * Time.deltaTime;
            }

            if (attackCooldown <= 0 && !isAttacking)
            {
                int whatAttack = chooseAttack();
                if (whatAttack < 0) return;

                attacks[whatAttack].attackEvent.Invoke();
                return;
            }






        }
        //


        handleMovement();

        handleAnimation();
    }





    //Throwing Bone Attack
    public void doBoneAttack()
    {
        StartCoroutine(throwBone());
    }
    IEnumerator throwBone()
    {
        isAttacking = true;
        anim.Play("SkeletonThrow");
        yield return new WaitForSeconds(.13f);
        spawnBone();
        yield return new WaitForSeconds(.2f);
        attackCooldown = 2;//
        isAttacking = false;
    }
    void spawnBone()
    {

        //Rotate Throw Point
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        piviot.rotation = Quaternion.Euler(0, 0, angle);
        //

        Vector2 directionTowardsPlayer = target.position - transform.position;

        Rigidbody2D newBone = Instantiate(boneProjectile, throwPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Projectile newProjectile = newBone.gameObject.GetComponent<Projectile>();


        newBone.AddForce(directionTowardsPlayer * throwForce);

        newProjectile.ownersHealth = healthScript;
        newProjectile.currentDamage = boneDamage;

    }
    //




    public override void handleAnimation()
    {
        base.handleAnimation();

        anim.SetBool("isMoving", currentMoveSpeed > .1f);
    }


    private void OnDrawGizmos()
    {
        if (!debug) return;
        for (int i = 0; i < attacks.Length; i++)
        {
            Gizmos.color = attacks[i].attackColor;
            Gizmos.DrawWireSphere(transform.position, attacks[i].neededRange);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

    }

}
