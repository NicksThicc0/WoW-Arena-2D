using UnityEngine;

public class BowScript : MonoBehaviour
{
    public bowScriptableObject bowData;

    playerScript player;

    [SerializeField] private float currentDrawAmount = 0;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform GFX;


    public GameObject currentArrow;

    [SerializeField] private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<playerScript>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        handleInputs();
        visuals();
    }


    void handleInputs()
    {

        if(Input.GetMouseButton(0) && player.currentStamina >= bowData.drawStaminaUsage)
        {
            anim.SetFloat("drawSpeed", 1 / (bowData.drawSpeed / bowData.maxDraw));

            player.useStamina(bowData.drawStaminaUsage * Time.deltaTime);

            currentDrawAmount += Time.deltaTime * bowData.drawSpeed;
            currentDrawAmount = Mathf.Clamp(currentDrawAmount, 0, bowData.maxDraw);
            anim.SetBool("isDrawing", true);
        }
        else if(Input.GetMouseButtonUp(0)&& currentDrawAmount >= 1)
        {
            // Shoot the arrow
            ShootArrow();
            currentDrawAmount = 0;
            anim.SetBool("isDrawing", false);
            anim.Play("Shoot");
        }
    }


    void ShootArrow()
    {
        Rigidbody2D rb = Instantiate(currentArrow, shootPoint.position, transform.rotation).GetComponent<Rigidbody2D>();
        rb.GetComponent<Projectile>().setDamage(currentDrawAmount);


        if (!player.isFacingRight)
        {
            rb.GetComponent<SpriteRenderer>().flipY = true;
        }

        rb.AddForce(transform.right * currentDrawAmount * 100);

        Destroy(rb.gameObject, 5);
    }  

    void visuals()
    {

        float sin = Mathf.Sin(Time.time * currentDrawAmount) * .01f;

        GFX.localPosition = new Vector3(0, sin, 0);
    }

}
