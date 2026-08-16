using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float currentDamage = 0;

    public bool breakOnImpact;
    public bool canPerice = false;

    public healthClass ownersHealth;
    public List<healthClass> hitObjects;
    [Header("GFX")]
    [SerializeField] private GameObject breakEffect;

    public void setDamage(float amount)
    {
        currentDamage = amount;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
   
        if (collision.gameObject.GetComponent<healthClass>() != null)
        {


            healthClass healthScript = collision.gameObject.GetComponent<healthClass>();
            if (healthScript == ownersHealth) return;


            if (hitObjects.Contains(healthScript)) return;

            hitObjects.Add(healthScript);
            healthScript.takeDamage(currentDamage, transform.root.position);


            if (!canPerice)
            {
                Destroy(gameObject);
            }
            return;
        }
        else if (collision.gameObject.GetComponentInParent<healthClass>() != null)
        {
            healthClass healthScript = collision.gameObject.GetComponentInParent<healthClass>();
            if (healthScript == ownersHealth) return;

            if (hitObjects.Contains(healthScript)) return;

            hitObjects.Add(healthScript);



            healthScript.takeDamage(currentDamage, transform.root.position);


            if (!canPerice)
            {
                Destroy(gameObject);
            }
            return;

        }

        if (breakOnImpact)
        {
            Destroy(gameObject);
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breakOnImpact)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        
        if(breakEffect != null)
        {
            GameObject newEffect = Instantiate(breakEffect, transform.position, Quaternion.identity);
            Destroy(newEffect, 1);

        }


    }
}