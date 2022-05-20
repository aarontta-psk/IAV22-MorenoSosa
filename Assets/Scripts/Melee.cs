using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV22_MorenoSosa
{
    public class Melee : Enemy
    {
        GameObject player;

        NavMeshAgent myNav;
        private Vector3 newDestination = Vector3.zero;

        public const float stayTime = 2f;
        float currentTime;

        private void Start()
        {
            currentTime = 0;


            myNav = transform.GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // --------- STATES ---------
        public void Wander()
        {
            Debug.Log("Wander to a different spot");
            myNav.SetDestination(newDestination);
        }

        public void ResetTimer()
        {
            currentTime = 0f;
        }

        public void WanderLocation(float wanderRadius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y;

            NavMeshHit hit;
            Vector3 finalPosition = transform.position;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
                finalPosition = hit.position;

            newDestination = finalPosition;
            Debug.Log(newDestination);
        }

        public void Stay()
        {
            Debug.Log("Stay on spot");
            currentTime += Time.deltaTime;
        }

        public void ResetDestination()
        {
            newDestination = Vector3.zero;
        }

        public void Repositioning()
        {
            Debug.Log("Repositioning");
        }

        // --------- TRANSITIONS ---------
        public bool TimeRunOut()
        {
            return currentTime >= stayTime;
        }

        public bool ArrivedDestination()
        {
            // ignore Y axis
            Vector3 auxDest = newDestination;
            auxDest.y = transform.position.y;

            return (transform.position - auxDest).magnitude <= 0.1;
        }

        public bool PlayerVisible()
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                out hit, Mathf.Infinity, LayerMask.GetMask("Walls", "Player", "Enemy")))
                return false;

            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            return hit.collider.tag == "Player";
        }
    }
}