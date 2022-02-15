﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    /*
    private CharacterController enemyAI;
    private GameObject player;
    float speed = 5.0f;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        velocity = direction * speed;
        velocity.Normalize();
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(velocity),
            speed * Time.deltaTime);
        enemyAI.Move(velocity * Time.deltaTime);
    }

    */
    private NavMeshAgent enemyAI;
    private GameObject player;
    void Start()
    {
        enemyAI = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!GameObject.FindObjectOfType<GameManager>().gameStarted) return;
        enemyAI.SetDestination(player.transform.position);
    }
/*
    public Task activateEnemyAI()
    {

    }
*/
}
