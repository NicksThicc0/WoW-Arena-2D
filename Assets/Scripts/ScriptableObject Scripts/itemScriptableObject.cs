using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class itemScriptableObject : ScriptableObject
{
    [Header("Graphics")]
    public Sprite inventoryIcon;
    public Sprite worldIcon;
    [Header("Item")]
    public int maxStackAmount = 999;

}
