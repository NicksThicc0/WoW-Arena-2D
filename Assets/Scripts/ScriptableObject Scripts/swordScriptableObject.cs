using UnityEngine;

[CreateAssetMenu(fileName = "New Sword", menuName = "Inventory/Sword")]
public class swordScriptableObject : itemScriptableObject
{
    [Header("Sword Stats")]
    public int damage = 5;
    public float requiredStamina = 5;
    public float knockBack = 0;

    public int attackSpeed = 1;
    public float staminaRegenCoolDown = 1;

    [Header("Sword Attacks")]
    public bool autoSwing = false;
    public AnimationClip baseAttack;
    public AnimationClip heavyAttack;
    [Header("gfx")]
    public Vector2 holdPos;
    public Quaternion holdRot;
}
