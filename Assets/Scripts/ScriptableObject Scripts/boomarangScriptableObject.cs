using UnityEngine;

[CreateAssetMenu(fileName = "NewBoomarang", menuName = "Inventory/Boomarang")]
public class boomarangScriptableObject : itemScriptableObject
{
    [Header("Boomarang Properties")]
    public Sprite boomarangSprite;


    public bool isMagic;

    public float damage = 5;
    public float throwRange = 5;



    public int maxBounces = 2;
    public float throwSpeed = 10f;

     
    public float detectRange = 1;
    public float returnTime = 2.5f;


 

}
