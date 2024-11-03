using System;
using _Assets._Code.Scripts.Game.HealthSystem.Survivors;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Assets._Code.Scripts.Game.ChaseSystem.Mobs
{
    public class MobChaser : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;
        
        private void Start()
        {
            if (IsServer)
            {
                target = FindObjectsByType<SurvivorHealth>(FindObjectsSortMode.None)[
                    Random.Range(0, FindObjectsByType<SurvivorHealth>(FindObjectsSortMode.None).Length)].transform;
                agent.SetDestination(target.position);
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}