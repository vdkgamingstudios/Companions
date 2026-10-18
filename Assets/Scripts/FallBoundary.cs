using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBoundary : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Only respond when the player enters.
        if (!other.CompareTag("Player"))
            return;

        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn point is not assigned!");
            return;
        }

        RespawnPlayer(other);
    }

    private void RespawnPlayer(Collider playerCollider)
    {
        Rigidbody playerRb =playerCollider.GetComponentInParent<Rigidbody>();

        if (playerRb != null)
        {
            //Stop all movement before respawning.
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            //Move player to the respawn point.
            playerRb.position = respawnPoint.position;
            playerRb.rotation = respawnPoint.rotation;
        }
        else
        {
            playerCollider.transform.position = respawnPoint.position;

            playerCollider.transform.rotation = respawnPoint.rotation;
        }

        //Update physics transforms after teleporting.
        Physics.SyncTransforms();

        //Instantly move camera to player's new position.
        CameraManager cameraManager =
            FindObjectOfType<CameraManager>();

        if (cameraManager != null)
        {
            cameraManager.SnapCameraToPlayer();
        }

        Debug.Log("Player respawned and camera repositioned.");
    }
}
