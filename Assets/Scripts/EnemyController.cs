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

            this.scoreboard.ScoreHit(scorePerHit);

            // Ensures the player can't die from hitting them
            this.boxCollider.enabled = false;

            SendMessage("SpawnEffects");

            Destroy(this.gameObject);
        }
    }

    [SerializeField]
    private Scoreboard scoreboard;

    [SerializeField]
    private int scorePerHit = 12;

    private BoxCollider boxCollider;
    private bool isDead = false;
}
