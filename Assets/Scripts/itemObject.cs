using System.Collections;
using UnityEngine;

public class itemObject : MonoBehaviour
{
    public itemScriptableObject itemToPickup;
    [SerializeField] private SpriteRenderer gfx;

    private void Start()
    {
        gameObject.name = "Item (" + itemToPickup.name + ")";
        gfx.sprite = itemToPickup.worldIcon;

        StartCoroutine(bobSprite());
    }




    public void setupItem()
    {
        gameObject.name = "Item (" + itemToPickup.name + ")";
        gfx.sprite = itemToPickup.worldIcon;
    }



    public void pickupItem()
    {
        //Inventory

        Destroy(gameObject);
    }




    IEnumerator suckInTowardsPlayer(Transform player)
    {
        float distance = Vector2.Distance(transform.position, player.position);
        float scale = 1;

        while (distance > .25f)
        {
            distance = Vector2.Distance(transform.position, player.position);
            scale -= 1.5f * Time.deltaTime;
            scale = Mathf.Clamp01(scale);

            transform.position = Vector2.MoveTowards(transform.position, player.position, .5f * Time.deltaTime);
            transform.localScale = new Vector2(scale, scale);
            yield return null;

        }
        pickupItem();

    }

    IEnumerator bobSprite()
    {
        float elasped = 0;
        while(elasped < 10)
        {

            elasped += 1 * Time.deltaTime;
            gfx.transform.localPosition = new Vector2(0, Mathf.PingPong(1, 1));



            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(suckInTowardsPlayer(collision.transform));
        }
    }

    private void OnDrawGizmos()
    {

    }

}
