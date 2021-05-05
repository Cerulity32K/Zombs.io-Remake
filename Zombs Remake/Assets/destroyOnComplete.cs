using UnityEngine;
using System.Collections;

public class destroyOnComplete : MonoBehaviour
{
    public ParticleSystem ps;

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}