using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
        this.boxCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!this.isDead)
        {
            this.isDead = true;

            // Ensures the player can't die from hitting them
            this.boxCollider.enabled = false;

            SpawnExplosion();

            BehaviourHelpers.DelayInvoke(this,
                () =>
                {
                    Destroy(this.gameObject);
                },
                timeToDeath);
        }
    }

    private void SpawnExplosion()
    {
        var newEffects = Instantiate(deathEffects, this.transform);

        newEffects.SetActive(true);
    }

    [SerializeField]
    private GameObject deathEffects;

    [SerializeField]
    private float timeToDeath = .5f;

    private BoxCollider boxCollider;
    private bool isDead = false;
}
