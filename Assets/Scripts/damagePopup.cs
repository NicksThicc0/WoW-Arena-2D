using System.Collections;
using UnityEngine;

public class damagePopup : MonoBehaviour
{

    [SerializeField] private SpriteRenderer[] _spriteRenderer;

    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private Sprite plusSprite;
    [SerializeField] private Sprite minusSprite;
    [Header("Colors")]
    [SerializeField] private Color plusColor;
    [SerializeField] private Color minusColor;



    public void setDamagePopup(int amount)
    {
        //
        if (amount == 0)
        {
            Destroy(gameObject);
            return;
        }
        //clamping amount
        amount = Mathf.Clamp(amount, -999, 999);

        // Setting Color
        for (int i = 0; i < _spriteRenderer.Length; i++)
        {
            Color newColor = Color.white;
            if (amount > 0)
            {
                newColor = plusColor;
            }
            else
            {
                newColor = minusColor;
            }
            _spriteRenderer[i].color = newColor;
        }


        //Splitting amount into soloed ints
        int hundred = (amount / 100) % 10;
        int ten = (amount / 10) % 10;
        int one = amount % 10;
        //Setting Number Sprites
        _spriteRenderer[1].sprite = getNumberSprite(hundred);
        _spriteRenderer[2].sprite = getNumberSprite(ten);
        _spriteRenderer[3].sprite = getNumberSprite(one);


        if (hundred == 0 && ten == 0 && one != 0)
        {
            _spriteRenderer[2].sprite = getNumberSprite(one);
            _spriteRenderer[1].gameObject.SetActive(false);
            _spriteRenderer[3].gameObject.SetActive(false);
        }
        else
        {
            #region Enabling Number Objects
            if (hundred != 0)
            {
                _spriteRenderer[1].gameObject.SetActive(true);
            }
            else
            {
                _spriteRenderer[1].gameObject.SetActive(false);
            }
            //
            if (ten != 0)
            {
                _spriteRenderer[2].gameObject.SetActive(true);
            }
            else if (ten == 0 && hundred == 0)
            {
                _spriteRenderer[2].gameObject.SetActive(false);
            }
            //
            if (one != 0)
            {
                _spriteRenderer[3].gameObject.SetActive(true);
            }
            else if (one == 0 && ten == 0 && hundred == 00)
            {
                _spriteRenderer[3].gameObject.SetActive(false);
            }
            #endregion

        }


        //floating up
        StartCoroutine(floatUp());
    }

    IEnumerator floatUp()
    {
        float elasped = 0;
        float alpha = 1;


        while (elasped < 1.25f)
        {
            elasped += 1 * Time.deltaTime;
            alpha -= 1 * Time.deltaTime;

            transform.position += Vector3.up * Time.deltaTime;
            for (int i = 0; i < _spriteRenderer.Length; i++)
            {
                _spriteRenderer[i].color = new Color(_spriteRenderer[i].color.r, _spriteRenderer[i].color.g, _spriteRenderer[i].color.b, alpha);
            }
            yield return null;
        }
        Destroy(gameObject);

    }


    Sprite getNumberSprite(int amount)
    {

        return numberSprites[Mathf.Abs(amount)];
    }

}
