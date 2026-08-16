
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class healthClass : MonoBehaviour
{
    public float maxHealth = 100;
    [Header("Current Stat")]
    public float currentHealth = 100;
    public float currentDefense = 0;

    bool didDie;

    //
    Rigidbody2D rb;

    //White Flash
    private Material flashWhiteMat;
    Coroutine whiteFlash;
    //hit twitch
    Coroutine hitTwitchCoro;



    //Knockback
    [Header("Knockback")]
    public bool canTakeKnockback = true;
    public bool isTakingKnockback = false;
    public float knockbackResistance = 0;
    Coroutine knockback = null;


    //Paritcles
    private GameObject hurtParticle;
    private GameObject damagePopupPrefab;
    [Header("IFrames")]
    public float iFrames;
    [SerializeField] private bool canGetIframes;

    //Sprites

    private SpriteRenderer[] sprites;
    private List<Material> defaultMats = new List<Material>();
    [Header("Buffs")]
    public bool canGetBuffed = true;
    public List<buff> currentBuffs = new List<buff>();

    [Header("Buff Effects")]
    public float buffedHealth;
    public float buffedStamina;
    public float buffedDefense;
    public float buffedSpeed;
    [Header("GFX")]
    public Transform mainGFX;
    [SerializeField] private bool doHitFlash = true;
    [SerializeField] private bool doHitParticles = true;
    public bool doDamagePopup = true;
    [Header("Loot")]
    [SerializeField] private lootpoolScriptableObject lootPool;
    [SerializeField] private bool dropOnDeath = true;
    [SerializeField] private bool dropOnHit = false;
    private GameObject itemGameobj;


    private void Awake()
    {
        hurtParticle = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Particles/Hurt Particle.prefab");
        damagePopupPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Particles/Damage Popup.prefab");
        flashWhiteMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Shaders/flashWhite.mat");
        itemGameobj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drop Item.prefab");
        //
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        getSprites();

    }


    private void Update()
    {
        handleBuffsTime();

        //Handle IFRAMES
        if (iFrames > 0)
        {
            iFrames -= 1 * Time.deltaTime;
        }
    }


    public virtual void takeDamage(float amount, Vector2 hitPos)
    {

        currentHealth = Mathf.Clamp(currentHealth, 0, getMaxHealth());

        if (iFrames > 0) return;


        if (canGetIframes)
        {
            iFrames = 1;
        }


        float baseDamage = amount;

       

        //

        currentHealth -= amount ;

        if (doDamagePopup)
        {
            spawnDamagePopup(Mathf.CeilToInt(-amount));
        }

        //Take Damage
        if (amount > 0)
        {
            if (doHitFlash)
            {
                if (whiteFlash != null)
                {
                    StopCoroutine(whiteFlash);
                    whiteFlash = null;
                }

                whiteFlash = StartCoroutine(flashWhite());
            }


            if (hitTwitchCoro != null)
            {
                StopCoroutine(hitTwitchCoro);
                hitTwitchCoro = null;
            }
            hitTwitchCoro = StartCoroutine(hitTwitch(hitPos));

            if (doHitParticles)
            {
                GameObject hitParticle = Instantiate(hurtParticle, transform.position, Quaternion.identity);
                Destroy(hitParticle, 1);
            }

        }
        //HEAL
        else
        {

        }


        if (currentHealth <= 0)
        {
            Die();
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, getMaxHealth());
    }
    public void takeKnockback(Vector3 hitPos, float knockbackAmount)
    {


        if (!canTakeKnockback) return;

        if (iFrames > 0) return;


        if (knockback != null)
        {
            StopCoroutine(knockback);
            knockback = null;
        }
        knockback = StartCoroutine(handleKnockBack(hitPos, knockbackAmount));

    }

    IEnumerator handleKnockBack(Vector3 hitPos, float knockbackAmount)
    {
        isTakingKnockback = true;

        Vector2 knockBackDir = (transform.position - hitPos).normalized;

        float calcAmount = knockbackAmount - knockbackResistance;
        calcAmount = Mathf.Clamp(calcAmount, 0, 9999);


        float elapsed = 0;
        while (elapsed < .1f)
        {

            transform.position += (Vector3)knockBackDir * calcAmount * Time.deltaTime;

            elapsed += Time.deltaTime;

            yield return null;
        }

        isTakingKnockback = false;
    }


    void Die()
    {
        // Handle death logic here
        didDie = true;
        //Destroy(gameObject);
    }



    //Damage popupo

    void spawnDamagePopup(int amount)
    {
        damagePopup newPopup = Instantiate(damagePopupPrefab, (Vector2)transform.position
            + new Vector2(Random.Range(-.5f, .5f), Random.Range(0, .5f)),
            Quaternion.identity).GetComponent<damagePopup>();
        //
        newPopup.setDamagePopup(amount);
    }


    //Getting health
    public float getMaxHealth()
    {
        return maxHealth + buffedHealth;
    }


    //Flashing White

    public IEnumerator flashWhite()
    {

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material = flashWhiteMat;
        }

        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material = defaultMats[i];
        }


    }
    IEnumerator hitTwitch(Vector2 hitPos)
    {
        if (mainGFX == null)
        {

            yield break;

        }

        bool gotHitFromTheRight = false;
        if (hitPos.x > transform.position.x)
        {
            gotHitFromTheRight = true;
        }



        float amountX = gotHitFromTheRight ? 15 : -15;

        mainGFX.transform.rotation = Quaternion.Euler(0, 0, Random.Range(amountX / 2, amountX));

        yield return new WaitForSeconds(.05f);
        float elasped = 0;
        while (elasped <= .5)
        {
            elasped += 1 * Time.deltaTime;

            mainGFX.transform.rotation = Quaternion.Slerp(mainGFX.rotation, Quaternion.identity, 15 * Time.deltaTime);
            if (elasped >= .1f && didDie)
            {
                Destroy(gameObject);
                //
                if (dropOnDeath)
                {
                    //Drop Loot
                    dropLoot();

                }


            }
            yield return null;
        }
        mainGFX.transform.rotation = Quaternion.identity;

    }


    public void getSprites()
    {
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();





        defaultMats.Clear();
        for (int i = 0; i < sprites.Length; i++)
        {
            defaultMats.Add(sprites[i].material);
        }
    }
    public void dropLoot()
    {
        if (lootPool == null) return;

        for (int i = 0; i < lootPool.dropableItems.Length; i++)
        {
            int chance = Random.Range(0, lootPool.dropableItems[i].rarirty + 1);
            if (chance == 0)
            {
                //Spawn

                int dropAmount = (int)Random.Range(lootPool.dropableItems[i].dropAmounts.x, lootPool.dropableItems[i].dropAmounts.y);


                for (int d = 0; d < dropAmount; d++)
                {
                    Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-.5f, .5f), transform.position.y);
                    itemObject newItem = Instantiate(itemGameobj, spawnPos, Quaternion.identity).GetComponent<itemObject>();
                    newItem.itemToPickup = lootPool.dropableItems[i].item;

                    newItem.setupItem();
                }


            }

        }



    }


    #region buffs
    public void addBuff(buffScriptableObject buffToAdd)
    {
        if (!canGetBuffed) return;
        buffToAdd.applyBuff(this);
    }

    public void removeBuff(buffScriptableObject buffToRemove)
    {
        buffedDefense -= buffToRemove.defenseBuffAmount;
        buffedHealth -= buffToRemove.maxHealthBuffAmount;
        buffedSpeed -= buffToRemove.maxSpeedBuffAmount;
        buffedStamina -= buffToRemove.maxStaminaBuffAmount;
    }

    public void handleBuffsTime()
    {
        for (int i = 0; i < currentBuffs.Count; i++)
        {
            currentBuffs[i].currentBuffDuration -= Time.deltaTime;
            if (currentBuffs[i].currentBuffDuration <= 0)
            {
                // Remove the buff and reset stats
                removeBuff(currentBuffs[i].whatBuff);
                currentBuffs.RemoveAt(i);
                i--;
            }
        }
    }

    float getDefense()
    {

        return currentDefense + buffedDefense;
        
    }


    //
    [System.Serializable]
    public class buff
    {
        public buffScriptableObject whatBuff;
        public float currentBuffDuration;
    }
    #endregion

}
