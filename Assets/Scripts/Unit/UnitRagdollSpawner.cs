using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Transform ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        ragdoll.GetComponentInChildren<SkinnedMeshRenderer>().material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        UnitRagdoll unitRagdoll = ragdoll.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
