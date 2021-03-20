using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TimedRespawn : MonoBehaviour
    {
        [SerializeField] GameObject objectToSpawn;
        [SerializeField] Vector2 spawnTimerRange;
        [SerializeField] float spawnTimer;

        void Start()
        {
            spawnTimer = Random.Range(spawnTimerRange.x, spawnTimerRange.y);
            Respawn();
        }

        void Update()
        {
            if (transform.childCount <= 0)
            {
                StartCoroutine("Wait");
                return;
            }
            StopAllCoroutines();
        }

        void Respawn()
        {
            Instantiate(objectToSpawn, transform);
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(spawnTimer);
            Respawn();
        }
    }
}
