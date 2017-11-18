using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortEffectManager : ManagerTemplate<ShortEffectManager>
{

    public List<GameObject> effectList = new List<GameObject>();

    public void addEffect(GameObject effectPrefab)
    {
        effectList.Add(effectPrefab);
    }

    public void deleteEffect()
    {
        foreach(var effect in effectList)
        {
            if (!effect.GetComponent<ParticleSystem>().IsAlive())
            {
                GameObject temp = effect.GetComponent<GameObject>();

                effectList.Remove(temp);
                Destroy(temp);
                break;
            }
        }
    }

}
