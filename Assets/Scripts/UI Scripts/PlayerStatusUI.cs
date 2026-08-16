using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public static PlayerStatusUI instance;

    public playerScript player;


    [Header("Health")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image whiteHealthBar;
    Coroutine healthBarDrop;
    Coroutine popBarCoroutine;
    [Header("Stamina")]
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image whiteStaminaBar;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void Update()
    {
        handleStaminaBarVisuals();
    }


    //Health Bar
    public void takeDamage(float amount)
    {

        //Health Bar Drop

        if (player.health.currentHealth <= player.health.getMaxHealth())
        {
            if (healthBarDrop != null)
            {
                StopCoroutine(healthBarDrop);
            }
            healthBarDrop = StartCoroutine(LerpBarFillAmount(healthBar, whiteHealthBar, player.health.currentHealth, amount));

        }


        if (amount > 0)
        {
            //Pop Bar
            if (popBarCoroutine != null)
            {
                StopCoroutine(popBarCoroutine);
            }
            popBarCoroutine = StartCoroutine(popBar(whiteHealthBar.transform));
        }

    }

    IEnumerator popBar(Transform bar, float maxDur = .1f)
    {
        float duration = 0;

        bar.transform.localScale = Vector3.one * 1.2f;
        bar.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-2, 2));
        yield return new WaitForSeconds(.05f);

        while (duration < maxDur)
        {
            duration += Time.deltaTime;
            bar.transform.localScale = Vector3.Lerp(bar.transform.localScale, Vector3.one, duration / maxDur);
            bar.transform.localRotation = Quaternion.Lerp(bar.transform.localRotation, Quaternion.Euler(0, 0, 0), duration / maxDur);
            yield return null;
        }


    }

    IEnumerator LerpBarFillAmount(Image baseBar, Image whiteBar, float targetFillAmount, float amount)
    {
        //Healing
        if (amount < 0)
        {
            //Smoothly Moving The White Bar To The Target Fill Amount
            whiteBar.fillAmount = 1 / (player.health.getMaxHealth() / targetFillAmount);



            yield return new WaitForSeconds(0.5f);

            //Decreasing The Base Bar To The Target Fill Amount
            while (baseBar.fillAmount != 1 / (player.health.getMaxHealth() / targetFillAmount))
            {
                baseBar.fillAmount = Mathf.MoveTowards(baseBar.fillAmount, 1 / (player.health.getMaxHealth() / targetFillAmount), 1 * Time.deltaTime);

                yield return null;
            }
        }
        //Taking Damage
        else
        {
            //Smoothly Moving The Base Bar To The Target Fill Amount
            baseBar.fillAmount = 1 / (player.health.getMaxHealth() / targetFillAmount);


            yield return new WaitForSeconds(0.5f);

            //Decressing The white Bar To The Target Fill Amount
            while (whiteBar.fillAmount != 1 / (player.health.getMaxHealth() / targetFillAmount))
            {
                whiteBar.fillAmount = Mathf.MoveTowards(whiteBar.fillAmount, 1 / (player.health.getMaxHealth() / targetFillAmount), 1 * Time.deltaTime);

                yield return null;
            }
        }




    }

    //StaminaBar

    void handleStaminaBarVisuals()
    {
        staminaBar.fillAmount = 1 / (player.getMaxStamina() / player.currentStamina);
        if (player.currentStamina < player.getMaxStamina())
        {
            whiteStaminaBar.fillAmount = Mathf.MoveTowards(whiteStaminaBar.fillAmount, 1 / (player.getMaxStamina() / player.currentStamina), .1f * Time.deltaTime);
        }
        else
        {
            whiteStaminaBar.fillAmount = 1 / (player.getMaxStamina() / player.currentStamina);
        }
    }

    IEnumerator stamina()
    {
        yield return new WaitForSeconds(0.5f);
        while (whiteStaminaBar.fillAmount != 1 / (player.getMaxStamina() / player.currentStamina))
        {
            whiteStaminaBar.fillAmount = Mathf.MoveTowards(whiteStaminaBar.fillAmount, 1 / (player.getMaxStamina() / player.currentStamina), 1 * Time.deltaTime);
            yield return null;
        }

    }



}
