using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hitboxScript : MonoBehaviour
{
    public float damage;
    public float knockBackAmount;
    public List<healthClass> hurtObj;

    [Header("Hitbox Settings")]

    public bool doKnockback;
    [SerializeField] private bool resetOnExit;
    [SerializeField] private bool continuesHitbox;


    private void Start()
    {
        if (continuesHitbox)
        {
            StartCoroutine(checkHealthScripts());
        }
    }

    public void setupHitbox(float setDamage, float knockBack)
    {
        damage = setDamage;
        knockBackAmount = knockBack;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (continuesHitbox)
        {
            if (collision.gameObject.GetComponent<healthClass>() != null)
            {
                healthClass healthScript = collision.gameObject.GetComponent<healthClass>();
                if (!hurtObj.Contains(healthScript))
                {
                    hurtObj.Add(healthScript);
                    return;
                }
            }
        }

        if (collision.gameObject.GetComponent<healthClass>() != null)
        {
            healthClass healthScript = collision.gameObject.GetComponent<healthClass>();
            if (!hurtObj.Contains(healthScript))
            {
                hurtObj.Add(healthScript);
                

                healthScript.takeDamage(damage, transform.root.position);
                healthScript.takeKnockback(transform.root.position, knockBackAmount);
                return;
            }
        }
        if (collision.gameObject.GetComponentInParent<healthClass>() != null)
        {
            healthClass healthScript = collision.gameObject.GetComponentInParent<healthClass>();
            if (!hurtObj.Contains(healthScript))
            {
                hurtObj.Add(healthScript);
                healthScript.takeDamage(damage, transform.root.position);
                healthScript.takeKnockback(transform.root.position, knockBackAmount);
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!resetOnExit) return;
        if (collision.gameObject.GetComponent<healthClass>() != null)
        {
            healthClass healthScript = collision.gameObject.GetComponent<healthClass>();
            if (hurtObj.Contains(healthScript))
            {
                hurtObj.Remove(healthScript);
                return;
            }
        }
        if (collision.gameObject.GetComponentInParent<healthClass>() != null)
        {
            healthClass healthScript = collision.gameObject.GetComponentInParent<healthClass>();
            if (hurtObj.Contains(healthScript))
            {
                hurtObj.Remove(healthScript);
                return;
            }
        }

    }

    IEnumerator checkHealthScripts()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < hurtObj.Count; i++)
        {
            if(hurtObj[i].iFrames <= 0)
            {
                hurtObj[i].takeKnockback(transform.root.position, knockBackAmount);
                hurtObj[i].takeDamage(damage, transform.root.position);
            }

        }
        StartCoroutine(checkHealthScripts());
    }


    public void resetHitbox()
    {
        hurtObj.Clear();
    }

}
