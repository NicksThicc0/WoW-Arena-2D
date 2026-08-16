using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class playerScript : MonoBehaviour
{
    Vector2 mx;
    private Rigidbody2D rb;
    [Header("Mouse")]
    public Vector2 mousePos;

    [Header("Movement")]
    public float currentSpeed;
    public bool isRunning = false;
    [Header("Stamina")]
    public bool canUseStamina = true;

    public float currentStamina = 100;
    public float maxStamina = 100;

    [SerializeField] private float stamainaRegenRate = 5f;

    Coroutine staminaRegenCoroutine;



    [Header("playerStats")]
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 10;

    [Header("Visual")]
    public bool isFacingRight = true;
    [SerializeField] private Transform gfx;
    public Animator anim;
    [SerializeField] private SortingGroup playerSortingGroup;

    [Header("Weapon")]
    public Transform weaponPivot;
    [SerializeField] private SortingGroup weaponSortingGroup;
    [Header("Potion")]
    [SerializeField] private SpriteRenderer potionSpriteRenderer;
    [SerializeField] private PotionScriptableObject testPotion;
    bool isDrinkingPotion = false;



    [Header("Player Scripts")]
    public healthClass health;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        handleInputs();
        handleStamina();

        doAnimations();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            health.takeDamage(10, transform.root.position);
        }



    }

    private void FixedUpdate()
    {
        handleMovement();
    }



    void handleInputs()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mx.x = Input.GetAxisRaw("Horizontal");
        mx.y = Input.GetAxisRaw("Vertical");

        //

        Vector2 direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
        //


        if (Input.GetKeyDown(KeyCode.E) && !isDrinkingPotion)
        {
            StartCoroutine(usePotion(25, testPotion));
        }


    }

    void handleMovement()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed(), 2 * Time.deltaTime);

        rb.linearVelocity = mx.normalized * currentSpeed;
    }

    public void useStamina(float amount, float waitForRegenTime = 0)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, getMaxStamina());
        //
        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
            staminaRegenCoroutine = null;
        }
        if (staminaRegenCoroutine == null)
        {
            staminaRegenCoroutine = StartCoroutine(regenStamina(waitForRegenTime));
        }

    }
    void handleStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isDrinkingPotion)
        {
            if (currentStamina > 0)
            {
                isRunning = true;
                if (staminaRegenCoroutine != null)
                {
                    StopCoroutine(staminaRegenCoroutine);
                    staminaRegenCoroutine = null;
                }
                if (mx != Vector2.zero)
                {
                    currentStamina -= 10 * Time.deltaTime;
                }

            }
            else
            {
                isRunning = false;
                if (staminaRegenCoroutine == null)
                {
                    staminaRegenCoroutine = StartCoroutine(regenStamina());
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || currentStamina <= 0)
        {
            isRunning = false;
            if (staminaRegenCoroutine == null)
            {
                staminaRegenCoroutine = StartCoroutine(regenStamina());
            }
        }

    }

    IEnumerator regenStamina(float regenTime = 0)
    {

        float regenSpeed = stamainaRegenRate;
        bool lostAllStamina = false;

        yield return new WaitForSeconds(regenTime);


        if (currentStamina <= 0)
        {
            lostAllStamina = true;
            canUseStamina = false;


            yield return new WaitForSeconds(1.5f);
            regenSpeed = stamainaRegenRate / 1.25f;

        }

        yield return new WaitForSeconds(1.5f);




        while (currentStamina < maxStamina)
        {
            currentStamina += stamainaRegenRate * Time.deltaTime;
            if (lostAllStamina && currentStamina >= maxStamina / 4)
            {
                canUseStamina = true;
                lostAllStamina = false;
            }

            yield return null;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaRegenCoroutine = null;
    }


    void doAnimations()
    {
        anim.SetBool("isMoving", mx != Vector2.zero);

        if (mousePos.x > transform.position.x)
        {
            gfx.localScale = new Vector3(1, 1, 1);
            weaponPivot.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
        else
        {
            gfx.localScale = new Vector3(-1, 1, 1);
            weaponPivot.localScale = new Vector3(1, -1, 1);
            isFacingRight = false;
        }

        if (mousePos.y > transform.position.y)
        {
            weaponSortingGroup.sortingOrder = playerSortingGroup.sortingOrder + -5;
        }
        else
        {
            weaponSortingGroup.sortingOrder = playerSortingGroup.sortingOrder + 5;
        }



    }



    IEnumerator usePotion(float amount, PotionScriptableObject whatPotion)
    {

        //Setting Graphics
        potionSpriteRenderer.sprite = whatPotion.potionGFX;
        //Disabling Weapons
        weaponPivot.gameObject.SetActive(false);
        //
        isDrinkingPotion = true;
        anim.Play("Drink Potions");
        yield return new WaitForSeconds(whatPotion.potionDrinkTime);
        anim.SetTrigger("FinishDrinking");
        //Do Potion Effect
        handlePotionEffect(whatPotion);
        if (whatPotion.potionBuff != null)
        {
            health.addBuff(whatPotion.potionBuff);
        }



        yield return new WaitForSeconds(.1f);
        //Enabling Weapons
        weaponPivot.gameObject.SetActive(true);

        //
        anim.ResetTrigger("FinishDrinking");
        isDrinkingPotion = false;
        //
    }
    void handlePotionEffect(PotionScriptableObject whatPotion)
    {
        if (whatPotion.typeOfPotion == potionType.Health)
        {
            health.takeDamage(-whatPotion.healthAmount, transform.root.position);
        }
        if (whatPotion.typeOfPotion == potionType.Stamina)
        {
            useStamina(-whatPotion.staminaAmount);
        }


    }


    float playerSpeed()
    {

        if (isDrinkingPotion)
        {
            return (walkSpeed + health.buffedSpeed) / 2;
        }
        else if (isRunning)
        {
            return runSpeed + health.buffedSpeed;
        }
        else
        {
            return walkSpeed + health.buffedSpeed;
        }
    }


    public float getMaxStamina()
    {
        return maxStamina + health.buffedStamina;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(weaponPivot.position, .5f);
    }
}
