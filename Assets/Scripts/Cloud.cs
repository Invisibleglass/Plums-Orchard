using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private GameObject[] clamps;
    private GameObject currentClamp;
    private Vector3 targetPosition;
    public float timeBetweemTeleports = 0.2f;
    public float teleportDistance = 1.0f; // Distance to teleport towards clamps
    public float switchDistance = 0.1f; // Distance threshold to switch direction

    void Start()
    {
        if (clamps == null)
        {
            clamps = GameObject.FindGameObjectsWithTag("cloudClamps");
        }
        currentClamp = clamps[1]; // Start with the left clamp
        StartCoroutine(TeleportBetweenClamps());
    }

    IEnumerator TeleportBetweenClamps()
    {
        while (true)
        {
            // Teleport towards the current clamp
            Vector3 direction = new Vector3(currentClamp.transform.position.x - transform.position.x, 0, 0).normalized;
            targetPosition = transform.position + direction * teleportDistance;
            transform.position = targetPosition;

            // Check distance to current clamp
            float distanceToClamp = transform.position.x - currentClamp.transform.position.x;

            // Switch to the other clamp if close enough
            if (distanceToClamp <= switchDistance)
            {
                if (currentClamp == clamps[1])
                    Destroy(gameObject);
            }

            yield return new WaitForSeconds(timeBetweemTeleports); // Wait for half a second before teleporting again
        }
    }
}