using System.Collections;
using UnityEngine;

public class boomarangScript : MonoBehaviour
{

    public playerScript player;
    public boomarangScriptableObject boomarangData;

    [SerializeField] private GameObject boomarangPrefab;


    [Header("GFX")]
    [SerializeField] private GameObject gfx;
    [SerializeField] private SpriteRenderer boomarangSpriteRenderer;
    bool alreadyThrown = false;


    Coroutine throwBoomarangCoroutine;



    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<playerScript>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boomarangSpriteRenderer.sprite = boomarangData.boomarangSprite;
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();

    }


    void handleInput()
    {
        if (Input.GetMouseButtonDown(0) && !alreadyThrown)
        {

            if (throwBoomarangCoroutine != null)
            {
                StopCoroutine(throwBoomarangCoroutine);
                throwBoomarangCoroutine = null;
            }
            throwBoomarangCoroutine = StartCoroutine(throwBoomarang());
        }
    }



    void spawnBoomarang()
    {
        boomarangProjectile newBoomarang = Instantiate(boomarangPrefab, transform.position, Quaternion.identity).GetComponent<boomarangProjectile>();

        //
        Vector2 dir = ((Vector3)player.mousePos - player.transform.position).normalized * boomarangData.throwRange;
        Vector2 Circle = (Vector2)player.transform.position + dir;

        newBoomarang.setBoomarang(boomarangData, Circle, player, this);
    }

    public void catchBoomarang()
    {
        anim.Play("None");
        //Stop Coroutine
        StopCoroutine(throwBoomarangCoroutine);
        throwBoomarangCoroutine = null;
        //
        alreadyThrown = false;
        gfx.SetActive(true);
        //

    }

    IEnumerator throwBoomarang()
    {
        alreadyThrown = true;
        anim.Play("throwBoomarang");
        yield return new WaitForSeconds(.16f);
        Debug.Log("Throw boomarang");
        spawnBoomarang();
        yield return new WaitForSeconds(.5f);
        gfx.SetActive(false);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(player.transform.position, boomarangData.throwRange);
        //
    }


    private void OnEnable()
    {
        boomarangSpriteRenderer.sprite = boomarangData.boomarangSprite;
    }
}
