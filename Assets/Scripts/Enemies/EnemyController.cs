using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    #region Stats
    private string enemyName = "empy";
    private string description = "empy";

    private GameObject enemyGun;

    public int health;
    public float speed = 2f;
    public float visionRange;
    #endregion

    public HitpointComponent hitpointComponent;

    public Transform target;

    private NavMeshAgent navAgent;


    private Statemachine stateMachine;
    private Statemachine.State dead, idle, alert, chase, seek, positioning, shooting;

    public float targetPositionPrediction = 1;
    public Vector3 targetLastPosition;

    public void Start()
    {
        hitpointComponent = GetComponent<HitpointComponent>();
        navAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
        hitpointComponent.hitpointMax = health;
        navAgent.speed = speed;


        stateMachine = new Statemachine();

        CreateDyingState();
        CreateIdleState();
        CreateAlertState();
        CreateChaseState();
        CreateSeekState();
        CreatePositioningState();
        CreateShootingState();


        stateMachine.PushState(idle);
    }

    private void Update()
    {
        stateMachine.Update(gameObject);
    }

    public virtual void WakeUp()
    {
        stateMachine.PopState();
        stateMachine.PushState(alert);
    }

    public virtual void SeeTarget()
    {
        if (!Physics.Raycast(transform.position, (target.position - transform.position).normalized, out var hit, visionRange)) return;
        if (hit.transform == target)
        {
            ChaseTarget();
        }

    }

    public virtual void ChaseTarget()
    {
        stateMachine.PopState();
        stateMachine.PushState(chase);
    }

    public virtual void LookForTarget()
    {
        stateMachine.PopState();
        stateMachine.PushState(seek);
    }


    #region States

    private void CreateDyingState()
    {
        dead = (finiteStateMachine, gameObj) =>
        {
            Debug.Log("shoot!");
        };
    }

    private void CreateIdleState()
    {
        idle = (finiteStateMachine, gameObj) =>
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                WakeUp();
            }
        };
    }

    private void CreateAlertState()
    {
        alert = (finiteStateMachine, gameObj) =>
        {
            Debug.Log("wakey");
            ChaseTarget();
        };
    }
    private void CreateChaseState()
    {
        chase = (finiteStateMachine, gameObj) =>
        {
            navAgent.destination = target.position;
            Debug.Log("chasing " + target + "!");

            if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out var hit, visionRange))
            {
                if (hit.transform == target) return;
            }

            float time = targetPositionPrediction;
            while (time >= 0)
            {
                Timer.Negative(ref time);
            }
            targetLastPosition = target.position;

            Debug.Log("I'm gonna look for " + target + "at " + targetLastPosition + "!");
            LookForTarget();
        };
    }
    private void CreateSeekState()
    {
        seek = (finiteStateMachine, gameObj) =>
        {
            Debug.Log("seeking you!");
            navAgent.destination = targetLastPosition;
            if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out var hit, visionRange))
            {
                if (hit.transform != target) return;
            }

            Debug.Log("I'm gonna chase" + target + "!");
            ChaseTarget();
        };
    }

    private void CreatePositioningState()
    {
        positioning = (finiteStateMachine, gameObj) =>
        {
            Debug.Log("repositioning");
        };
    }

    private void CreateShootingState()
    {
        shooting = (finiteStateMachine, gameObj) =>
        {
            Debug.Log("shoot!");
        };
    }
    #endregion
}
