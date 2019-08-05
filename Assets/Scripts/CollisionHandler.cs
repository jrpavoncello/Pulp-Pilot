using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
    }

    private void StartDeathSequence()
    {
        SendMessage("OnPlayerDeath");

        StartRigidBodyDrop();

        SendMessage("SpawnEffects");

        BehaviourHelpers.DelayInvoke(this, () =>
        {
            levelChanger.OnReloadLevel();
        },
        this.levelLoadDelay);
    }

    private void StartRigidBodyDrop()
    {
        this.rigidBody.isKinematic = false;
        this.rigidBody.useGravity = true;
    }

    [SerializeField]
    private LevelChanger levelChanger;

    [SerializeField]
    private float levelLoadDelay = 1f;

    private Rigidbody rigidBody;
}
