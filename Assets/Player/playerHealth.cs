using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class playerHealth : healthClass
{

    public override void takeDamage(float amount, Vector2 hitPos)
    {
        if (iFrames > 0) return;
        base.takeDamage(amount, hitPos);
        PlayerStatusUI.instance.takeDamage(amount);




    }



}
