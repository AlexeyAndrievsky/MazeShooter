using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    

    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {

        public GameObject roottarget;
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;
        private Transform tgggg;// target to aim for
        public GameObject targ;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
            {
                Destroy(targ);
                character.Move(Vector3.zero, false, false);
            }

            if (roottarget.transform.childCount > 0)
            {
                FindTarget();
            }
            else
            {
                tgggg = target;
            }

                if (tgggg != null)
                agent.SetDestination(tgggg.position);

            
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void FindTarget()
        {
            GameObject[] arrayofWP = GameObject.FindGameObjectsWithTag("MovePoint");
            targ = arrayofWP[0];
            tgggg = targ.transform;

        }
    }
}
