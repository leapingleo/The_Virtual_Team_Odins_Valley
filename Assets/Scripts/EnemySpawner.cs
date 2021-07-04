using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float distance;
    public GameObject shieldEnemy;
    public GameObject standardEnemy;
    public bool useStandardEnemy;
    public GameObject spawnParticles;

    private GameObject enemy;
    private GameObject player;
    private ParticleSystem spawnParticle;
    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnParticle = Instantiate(spawnParticles, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
        
        if (useStandardEnemy)
        {
            enemy = Instantiate(standardEnemy, transform.position, Quaternion.identity, transform);
        }
        else
        {
            enemy = Instantiate(shieldEnemy, transform.position, Quaternion.identity, transform);
        }
        enemy.GetComponent<EnemyAI>().InitialiseValues();
        enemy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFrom = Vector3.Distance(player.transform.position, transform.position);
        int numHits = enemy.GetComponent<EnemyAI>().NumHits;
        Debug.Log(numHits);

        if (!spawned &&
            numHits > 0 &&
            distanceFrom <= distance)
        {
            spawnParticle.transform.position = enemy.transform.position;
            spawnParticle.Play();
            spawned = true;
            enemy.SetActive(true);
        }
        else if (spawned &&
            numHits > 0 &&
            distanceFrom > distance)
        {
            spawned = false;
            spawnParticle.transform.position = enemy.transform.position;
            spawnParticle.Play();
            enemy.SetActive(false);
            enemy.transform.position = transform.position;
        }
    }
}
