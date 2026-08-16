using UnityEngine;


[CreateAssetMenu(fileName = "New Bow", menuName = "Inventory/Bow")]
public class bowScriptableObject : itemScriptableObject
{
    [Header("Bow Spec")]
    public float drawSpeed;
    public float maxDraw;

    public float drawStaminaUsage = 1;
    

}
