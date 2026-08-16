using UnityEngine;
using static healthClass;


[CreateAssetMenu(fileName = "New Buff", menuName = " Buff")]
public class buffScriptableObject : ScriptableObject
{
    public Sprite buffIcon;

    [Header("Buff Stats")]
    public float buffDuration = 3;

    public float maxHealthBuffAmount;
    public float maxStaminaBuffAmount;
    public float maxManaBuffAmount;
    //
    public float defenseBuffAmount;
    public float maxSpeedBuffAmount;



    public void applyBuff(healthClass health)
    {
        //
        if (health == null) return;

        for (int i = 0; i < health.currentBuffs.Count; i++)
        {
            if (health.currentBuffs[i].whatBuff == this)
            {
                health.currentBuffs[i].currentBuffDuration = buffDuration;
                return;
            }
        }
        health.currentBuffs.Add(new buff { whatBuff = this, currentBuffDuration = this.buffDuration });
        //

        health.buffedHealth += maxHealthBuffAmount;
        health.buffedStamina += maxStaminaBuffAmount;
        health.buffedDefense += defenseBuffAmount;
        health.buffedSpeed += maxSpeedBuffAmount;

        health.buffedDefense = Mathf.Clamp(health.buffedDefense, 0, 999);
        health.buffedSpeed = Mathf.Clamp(health.buffedSpeed, 0, 999);


    }
}
