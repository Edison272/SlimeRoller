using UnityEngine;

public class ShadowObstacle : MonoBehaviour
{
    private Collider obstacleCollider;
    private readonly System.Collections.Generic.List<Collider> ignoredColliders = new System.Collections.Generic.List<Collider>();

    private void Awake()
    {
        obstacleCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider otherCollider = collision.collider;
        if (obstacleCollider == null || otherCollider == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("ShadowPlayer"))
        {
            if (!ignoredColliders.Contains(otherCollider))
            {
                Physics.IgnoreCollision(otherCollider, obstacleCollider, true);
                ignoredColliders.Add(otherCollider);
            }
        }
    }

    private void FixedUpdate()
    {
        if (obstacleCollider == null)
        {
            obstacleCollider = GetComponent<Collider>();
        }

        if (obstacleCollider == null)
        {
            ignoredColliders.Clear();
            return;
        }

        // Keep this synced every physics step so newly shadowed players phase through instantly.
        GameObject[] shadowPlayers = GameObject.FindGameObjectsWithTag("ShadowPlayer");
        for (int i = 0; i < shadowPlayers.Length; i++)
        {
            if (shadowPlayers[i] == null)
            {
                continue;
            }

            Collider[] playerColliders = shadowPlayers[i].GetComponentsInChildren<Collider>();
            for (int j = 0; j < playerColliders.Length; j++)
            {
                Collider playerCollider = playerColliders[j];
                if (playerCollider == null || playerCollider == obstacleCollider)
                {
                    continue;
                }

                if (!ignoredColliders.Contains(playerCollider))
                {
                    Physics.IgnoreCollision(playerCollider, obstacleCollider, true);
                    ignoredColliders.Add(playerCollider);
                }
            }
        }

        if (ignoredColliders.Count == 0)
        {
            return;
        }

        for (int i = ignoredColliders.Count - 1; i >= 0; i--)
        {
            Collider otherCollider = ignoredColliders[i];
            if (otherCollider == null)
            {
                ignoredColliders.RemoveAt(i);
                continue;
            }

            if (otherCollider.gameObject.CompareTag("ShadowPlayer"))
            {
                continue;
            }

            Physics.IgnoreCollision(otherCollider, obstacleCollider, false);
            ignoredColliders.RemoveAt(i);
        }
    }
}
