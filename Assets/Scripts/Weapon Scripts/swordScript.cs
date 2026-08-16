using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    playerScript player;

    public swordScriptableObject swordData;


    [Header("Hitbox")]
    [SerializeField] private hitboxScript hitbox;

    [Header("Visuals")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform gfx;


    bool alreadySwong = false;

    private float attackCooldown = 0;

    private void Awake()
    {
        player = GetComponentInParent<playerScript>();
        anim = GetComponent<Animator>();

        hitbox.setupHitbox(swordData.damage,swordData.knockBack);
    }


    private void Update()
    {
        handleInputs();


        //Attack Cooldown
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    void handleInputs()
    {
        if (Input.GetMouseButton(0) && canAttack() && !alreadySwong)
        {
            if (!swordData.autoSwing)
            {
                alreadySwong = true;
            }

            hitbox.resetHitbox();

            anim.Play(swordData.baseAttack.name);
            attackCooldown = swordData.baseAttack.length + .25f;

            player.useStamina(swordData.requiredStamina, swordData.staminaRegenCoolDown);
        }

        if (Input.GetMouseButtonUp(0))
        {
            alreadySwong = false;
        }
    }



    bool canAttack()
    {
        bool canAttack = true;

        if (attackCooldown > 0)
        {
            canAttack = false;
        }
        if (player.currentStamina < swordData.requiredStamina)
        {
            canAttack = false;
        }
        if (!player.canUseStamina)
        {
            canAttack = false;
        }

        return canAttack;
    }

    private void OnEnable()
    {
        anim.enabled = true;
    }
    private void OnDisable()
    {
        anim.enabled = false;
        gfx.localRotation = swordData.holdRot;
        gfx.localPosition = swordData.holdPos;
        gfx.localScale = Vector3.one;
    }

    IEnumerator resetGFX()
    {

        yield return new WaitForSeconds(1);
        anim.enabled = true;
    }

}
