using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 30;

    [SerializeField] private Transform grenadeExplosionVFXPrefab;
    [SerializeField] private TrailRenderer trailrenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action OnGrenadeBehaviourComplete;
    private float moveSpeed = 10f;
    private float stoppingDistance = 0.2f;

    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1f - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                    targetUnit.Damage(damageAmount);
            }

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                    destructibleCrate.Damage();
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            Instantiate(grenadeExplosionVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

            trailrenderer.transform.parent = null;

            Destroy(gameObject);

            OnGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviourComplete)
    {
        this.OnGrenadeBehaviourComplete = OnGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
