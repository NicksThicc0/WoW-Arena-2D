using System.Collections;
using UnityEngine;

public class nodeScript : healthClass
{
    [Header("NODE")]
    [SerializeField] private ParticleSystem hitParticle;

    public override void takeDamage(float amount, Vector2 hitPos)
    {
        base.takeDamage(amount, hitPos);
        StartCoroutine(hitShake(.05f));

        hitParticle.Play();
    }


    IEnumerator hitShake(float strength = .1f)
    {

        Vector2 defaultPos = mainGFX.transform.localPosition;

        float elasped = 0;
        while(elasped < .1f)
        {
            elasped += 1 * Time.deltaTime;
            mainGFX.transform.localPosition = new Vector2(mainGFX.transform.localPosition.x + Random.Range(-strength, strength), mainGFX.transform.localPosition.y + Random.Range(-strength, strength));
            
            yield return null;
        }
        mainGFX.transform.localPosition = defaultPos;
    }

}
