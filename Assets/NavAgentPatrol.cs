using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentPatrol : MonoBehaviour {

    // Use this for initialization
    public Transform[] PatrolTargets;
    private Transform nextTarget;
    private int targetIndex;
    public enum RepeatState { Looping, Backtrack, NoRepeat }
    public RepeatState LoopState;
    NavMeshAgent agent;
    public bool backwardsTracking = false;
    private bool donePatrol;

    void Start() {
        donePatrol = false;

        if (PatrolTargets.Length > 0)
        {
            targetIndex = 0;
        }

        agent = GetComponent<NavMeshAgent>();
        //nextTarget = PatrolTargets[0];
        SetNewDestination();

        if (PatrolTargets.Length >= targetIndex + 1)
        {
            //targetIndex++;
        }

        if (LoopState == RepeatState.Backtrack || LoopState == RepeatState.Looping && PatrolTargets.Length <= 1)
        {
            LoopState = RepeatState.NoRepeat;
        }
    }

    // Update is called once per frame
    void Update() {

        if (StripHeightComponent(transform.position) == StripHeightComponent(PatrolTargets[targetIndex].position) && !donePatrol)
        {
            Debug.Log("ReachedDestination");
            Debug.Log(LoopState);
            if (LoopState == RepeatState.Backtrack)
            {
                
                if (targetIndex >= PatrolTargets.Length-1)
                {
                    backwardsTracking = true;
                    
                }
                else if (targetIndex == 0)
                {
                    backwardsTracking = false;
                }
                if (backwardsTracking)
                {
                    targetIndex -= 1;
                    
                }
                else
                {
                    targetIndex++;
                    
                }
            }

            else if (LoopState == RepeatState.Looping)
            {
                if (targetIndex >= PatrolTargets.Length-1)
                {
                    targetIndex = 0;
                }
                else
                {
                    targetIndex++;
                }
            }

            else
            {
                if (targetIndex >= PatrolTargets.Length-1)
                {
                    donePatrol = true;
                }
            }

            SetNewDestination();
        }

        
    }

    void SetNewDestination()
    {
        Debug.Log("Destination Set to index " + targetIndex);
        agent.destination = PatrolTargets[targetIndex].position;
    }

    Vector3 StripHeightComponent(Vector3 target)
    {
        return new Vector3(target.x, 0, target.z);
    }
}
