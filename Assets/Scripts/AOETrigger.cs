using System.Collections.Generic;
using UnityEngine;

public class AOETrigger : MonoBehaviour
{
    public enum AOEType
    {
        Damage,
        Heal,
        Buff,
        Debuff
    }
    public AOEType typeOfAOE;

    [SerializeField] private buffScriptableObject[] buffToApply;

    [SerializeField] private List<healthClass> healthScripts;





    public float healthChangeAmount;


    void Update()
    {
        for (int i = 0; i < healthScripts.Count; i++)
        {
            if (typeOfAOE == AOEType.Heal)
            {
                healthScripts[i].takeDamage(-healthChangeAmount * Time.deltaTime, transform.root.position);
            }


            for (int b = 0; b < buffToApply.Length; b++)
            {
                healthScripts[i].addBuff(buffToApply[b]);
            }

        }



    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<healthClass>() != null)
        {
            if (healthScripts.Contains(collision.GetComponent<healthClass>())) return;
            healthScripts.Add(collision.GetComponent<healthClass>());
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<healthClass>() != null)
        {
            if (!healthScripts.Contains(collision.GetComponent<healthClass>())) return;
            healthScripts.Remove(collision.GetComponent<healthClass>());
        }
    }


}
