using UnityEngine;


public enum potionType
{
    Health,
    Stamina,
    Buff
}
[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class PotionScriptableObject : itemScriptableObject
{
    public potionType typeOfPotion;
    public Sprite potionGFX;

    public buffScriptableObject potionBuff;



    [Header("Potion Stats")]
    public float potionDrinkTime = 3;

    public float healthAmount;
    public float staminaAmount;
    public float manaAmount;




}
