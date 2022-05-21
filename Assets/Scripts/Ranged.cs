using UnityEngine;
using UnityEngine.AI;

namespace IAV22_MorenoSosa
{
    public class Ranged : Enemy
    {
        GameObject player;

        NavMeshAgent myNav;
        private Vector3 newDestination = Vector3.zero;

        public const float stayTime = 2f;
        float currentTime;

        public const float distRep = 6f;
        Vector3 repositioning = Vector3.zero;

        [SerializeField]
        private GameObject bullet;
        public const float cadency = 0.75f;
        float shootTime = 0;
        int shoots = 0;
        public int numBulletShoot = 3;
        int maxBullets, currentBullets;

        private Health health;

        float auxTime = 0;
        bool changeState = false;


        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            myNav = transform.GetComponent<NavMeshAgent>();

            health = transform.GetComponent<Health>();

            currentTime = 0;

            maxBullets = currentBullets = 3 * numBulletShoot;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
                health.TakeDamage(10);
            Debug.Log(health.Amount);
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
        }

        public void Stay()
        {
            Debug.Log("Stay on spot");
            currentTime += Time.deltaTime;
        }

        public void ResetDestination()
        {
            newDestination = transform.position;
        }

        public void Repositioning()
        {
            Debug.Log("Repositioning");
            myNav.SetDestination(repositioning);
        }

        public void RepositionSpace()
        {
            NavMeshHit hit;
            int dir = Random.Range(0, 2) * 2 - 1;
            repositioning = transform.position + (dir * Vector3.Cross(transform.position, player.transform.position).normalized * distRep);

            if (Physics.Raycast(repositioning, (player.transform.position - repositioning).normalized,
                Mathf.Infinity, LayerMask.GetMask("Obstacles")))
                repositioning = transform.position + (-dir * Vector3.Cross(transform.position, player.transform.position).normalized * distRep);

            if (Physics.Raycast(repositioning, (player.transform.position - repositioning).normalized,
                Mathf.Infinity, LayerMask.GetMask("Obstacles")))
                repositioning = transform.position;

            if (!Physics.Raycast(repositioning, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Floor")) &&
                !Physics.Raycast(repositioning, Vector3.up, Mathf.Infinity, LayerMask.GetMask("Floor")))
                repositioning = transform.position;

            Debug.Log(repositioning);
        }

        public void Reload()
        {
            Debug.Log("Reloading");
            currentBullets = maxBullets;
            changeState = true;
        }

        public void Shoot()
        {
            Debug.Log("Shoot");
            shootTime += Time.deltaTime;

            if (shootTime > cadency)
            {
                Debug.Log("Gun");
                shoots++;
                currentBullets--;

                // gunshot
                GameObject gunshot;
                gunshot = Instantiate(bullet, transform.position, transform.rotation);
                gunshot.GetComponent<Rigidbody>().velocity = (player.transform.position - transform.position).normalized * 10;

                shootTime = 0;
            }
        }

        public void ResetShoot()
        {
            shootTime = 0;
            shoots = 0;
        }

        public void Cover()
        {
            Debug.Log("Cover");
        }

        //public void TakeCover()
        //{
        //    if (closestEnemy_ != null && !isCovered_)
        //    {
        //        coverTimer_ = Random.Range(minCoverTime_, maxCoverTime_);
        //        coverTimerRunning_ = true;
        //        NearestObstacle();
        //        CalculateBestCoverPos();
        //        isCovered_ = true;
        //    }
        //}

        //void NearestObstacle()
        //{
        //    float distance = enemyDistanceRadius_;
        //    // Busca obstaculos en radio
        //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDistanceRadius_);
        //    foreach (var hitCollider in hitColliders)
        //    {
        //        if (hitCollider.gameObject.tag == "Obstacle")
        //        {
        //            float obstacleDistance = Vector3.Distance(transform.position, hitCollider.transform.position);
        //            //Calcular si es el obstáculo más cercano
        //            if (obstacleDistance < distance)
        //            {
        //                distance = obstacleDistance;
        //                closestCover_ = hitCollider.transform;
        //            }
        //        }
        //    }
        //}

        //void CalculateBestCoverPos()
        //{
        //    bestCover_.position = closestCover_.position + (closestCover_.position - closestEnemy_.position).normalized * 2f;
        //    objectivePos_ = bestCover_;
        //}

        public void Grenade()
        {
            Debug.Log("GrenDE");
        }

        public void ResetChange()
        {
            changeState = false;
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
                out hit, Mathf.Infinity, LayerMask.GetMask("Walls", "Player")))
                return false;

            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            return hit.collider.tag == "Player";
        }

        public bool SuccesfullReposition()
        {
            // ignore Y axis
            Vector3 auxDest = repositioning;
            auxDest.y = transform.position.y;

            return (transform.position - auxDest).magnitude <= 0.1;
        }

        public bool ReturnReposition()
        {
            return shoots == 3 && currentBullets > 0;
        }

        public bool NeedToReload()
        {
            return currentBullets <= 0;
        }

        public bool CheckHealth()
        {
            return health.Amount <= health.InitialAmmount / 2;
        }
    }
}