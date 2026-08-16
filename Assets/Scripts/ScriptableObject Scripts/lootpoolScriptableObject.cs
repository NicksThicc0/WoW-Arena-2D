using UnityEngine;



[CreateAssetMenu(fileName = "New Lootpool", menuName = "Inventory/Loot Pool")]
public class lootpoolScriptableObject : ScriptableObject
{

    public items[] dropableItems; 





    [System.Serializable]
    public class items
    {
        public itemScriptableObject item;
        public int rarirty = 10;
        public Vector2 dropAmounts;

    }


}
