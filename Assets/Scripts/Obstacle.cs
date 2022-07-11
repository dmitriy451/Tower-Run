using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool notDestoyed = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTower playerTower) && notDestoyed)
        {
            float DisplaceScale = 1.3f;
            Vector3 distanceCheckerNewPosition = playerTower.DistanceChecker.position;
            distanceCheckerNewPosition.y += playerTower.Humans[0].transform.localScale.y * DisplaceScale;
            playerTower.DistanceChecker.position = distanceCheckerNewPosition;
            playerTower.CheckCollider.center = playerTower.DistanceChecker.localPosition;
            notDestoyed = false;
            Destroy(playerTower.Humans[0].gameObject);
            playerTower.Humans.RemoveAt(0);
            playerTower.SetAnimationToHumans();
        }
    }
}
