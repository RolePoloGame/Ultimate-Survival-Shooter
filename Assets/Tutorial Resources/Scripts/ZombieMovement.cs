﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private Transform player;               // Reference to the player's position.
    private PlayerHealth playerHealth;      // Reference to the player's health.
    private ZombieHealth enemyHealth;        // Reference to this enemy's health.
    private NavMeshAgent nav;               // Reference to the nav mesh agent.

    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<ZombieHealth>();
        nav = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
        if (!enemyHealth.HealthSystem.IsDead && !playerHealth.HealthSystem.IsDead)
        {
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination(player.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    }
}