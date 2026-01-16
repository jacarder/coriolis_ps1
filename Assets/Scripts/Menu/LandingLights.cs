using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPadLights : MonoBehaviour
{
    [Header("Light Prefab & Spawn Settings")]
    public GameObject lightPrefab;           // Small light prefab
    public Transform[] landingPads;          // Landing pads transforms
    public int maxLights = 20;               // Max lights active at once
    public float spawnInterval = 0.5f;       // How often to spawn a light

    [Header("Movement Settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float minDistance = 2f;           // Distance from pad to start/end movement
    public float maxDistance = 10f;

    [Header("Light Behavior Probabilities")]
    [Range(0f, 1f)] public float probabilityToward = 0.4f;
    [Range(0f, 1f)] public float probabilityPast = 0.3f;
    [Range(0f, 1f)] public float probabilityAway = 0.3f;

    private List<GameObject> activeLights = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLights());
    }

    IEnumerator SpawnLights()
    {
        while (true)
        {
            if (activeLights.Count < maxLights && landingPads.Length > 0 && lightPrefab != null)
            {
                SpawnLight();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnLight()
    {
        Transform pad = landingPads[Random.Range(0, landingPads.Length)];

        // Decide behavior
        float rand = Random.value;
        Vector3 startPos = pad.position;
        Vector3 targetPos = pad.position;
        float distance = Random.Range(minDistance, maxDistance);

        if (rand < probabilityToward) // Flying toward pad
        {
            startPos += Random.onUnitSphere * distance;  // Start somewhere around pad
            targetPos = pad.position;
        }
        else if (rand < probabilityToward + probabilityPast) // Flying past pad
        {
            startPos += Random.onUnitSphere * distance;
            Vector3 dir = (pad.position - startPos).normalized;
            targetPos = pad.position + dir * distance;   // Go past pad
        }
        else // Flying away from pad
        {
            startPos = pad.position;
            targetPos += Random.onUnitSphere * distance; // Go away from pad
        }

        GameObject lightObj = Instantiate(lightPrefab, startPos, Quaternion.identity);
        LightMover mover = lightObj.AddComponent<LightMover>();
        mover.Initialize(targetPos, Random.Range(minSpeed, maxSpeed), this);
        activeLights.Add(lightObj);
    }

    public void RemoveLight(GameObject light)
    {
        if (activeLights.Contains(light))
            activeLights.Remove(light);
    }
}

public class LightMover : MonoBehaviour
{
    private Vector3 target;
    private float speed;
    private LandingPadLights manager;

    public void Initialize(Vector3 targetPos, float moveSpeed, LandingPadLights managerRef)
    {
        target = targetPos;
        speed = moveSpeed;
        manager = managerRef;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            manager.RemoveLight(gameObject);
            Destroy(gameObject);
        }
    }
}
