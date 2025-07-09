using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum FishSwimPattern
{
    Random,
    PointPosition,
}

public enum FishSwimSpeed
{
    Constant,
    RandomSpeed,
}
public class AnimationTrigger : MonoBehaviour
{
    [Header("FISH SETTING")]
    public FishSwimPattern FishSwimPattern;
    public FishSwimSpeed FishSwimSpeed;
    [Space]
    public bool checkCollision = true;
    public bool globalBounds = true;
    public bool drawGizmoPath = true;
    [Space]
    [Header("MOVEMENT SETTING")]
    public float moveSpeed = 5;
    [Range(0.1f, 1.5f)]
    public float turnSpeed = 1;
    public float turnSpeedOnCollisionMultiplier = 5;
    public float turnSpeedOnCollisionMultiplierDuration = 0.5f;
    public float reachedDistance = 5f;
    public float offsetDistance = 5f;
    // public float radiusDetection = 4f;
    [Space]
    [Header("For Random Speed Setting")]
    public Vector2 randomSpeed = new Vector2(2, 10);
    [Space]
    [Header("BOUNDARY SETTING")]
    public Vector3 boundMin = new Vector3(-50f, 0, -50f);
    public Vector3 boundMax = new Vector3(50f, 0, 50f);
    [Space]
    [Header("TARGET SETTING")]
    public GameObject targetTransform;
    public float scaleAmount;

    // [Header("FISH PREFAB")]
    GameObject fishPrefab;

    private Animator anim;
    private Vector3 target;

    private float maxTurnSpeed;
    private float minTurnSpeed;
    private void Awake()
    {
        if (this.gameObject.transform.GetChild(0))
        {
            fishPrefab = this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    private void Start()
    {
        maxTurnSpeed = turnSpeed * turnSpeedOnCollisionMultiplier;
        minTurnSpeed = turnSpeed;

        anim = GetComponent<Animator>();

        // GetComponent<SphereCollider>().radius = radiusDetection;

        // if(FishSwimPattern == FishSwimPattern.Random){
        //     transform.position = transform.parent.position + new Vector3(UnityEngine.Random.Range(boundMin.x,boundMax.x),UnityEngine.Random.Range(boundMin.y,boundMax.y),UnityEngine.Random.Range(boundMin.z,boundMax.z));
        // }
        // PickNewRandomDestination();

        // target = targetTransform.transform.position;

    }

    private void Update()
    {
        // CheckDistance(target);
        TargetGrow();
        LookAtTarget(targetTransform.transform.position);
        Move();
    }

    void PickNewRandomDestination()
    {
        // targetTransform.transform.position = targetTransform.transform.position;
        if (FishSwimPattern == FishSwimPattern.Random)
        {

            Vector3 randomTarget = new Vector3(UnityEngine.Random.Range(boundMin.x, boundMax.x), UnityEngine.Random.Range(boundMin.y, boundMax.y), UnityEngine.Random.Range(boundMin.z, boundMax.z));

            if (!globalBounds) randomTarget += transform.parent.position;

            targetTransform.transform.position = randomTarget;
        }

        // target = targetTransform.transform.position;

        // Vector3 direction = target - transform.position;
        // finalRotation = Quaternion.LookRotation(direction);
    }

    private void CheckDistance(Vector3 target)
    {
        if (GetDistance(target) <= reachedDistance)
        {
            PickNewRandomDestination();
            SetSpeed();
        }
    }

    private float GetDistance(Vector3 target)
    {
        return Vector3.Distance(target, transform.position);
    }

    private void SetSpeed()
    {
        if (FishSwimSpeed == FishSwimSpeed.RandomSpeed)
        {
            moveSpeed = UnityEngine.Random.Range(randomSpeed.x, randomSpeed.y);
        }
    }

    private void LookAtTarget(Vector3 target)
    {
        Quaternion startRotation = transform.rotation;
        Vector3 direction = target - transform.position;
        Quaternion finalRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        if (moveSpeed > 0)
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collided with " + other);

        if (other.tag == "AITarget")
        {
            if (other.gameObject == targetTransform)
            {
                PickNewRandomDestination();
                TargetScaleReset();
            }
        }
        else
        {
            AvoidCollision();
            StartCoroutine(SpikeTurnSpeed());
        }

    }

    private void AvoidCollision()
    {
        // Vector3 behindPos =  transform.position - other.transform.position;

        Vector3 dir = transform.forward * -1;
        Vector3 randomTarget = transform.position + (dir * offsetDistance) + (UnityEngine.Random.insideUnitSphere * 1.75f);

        targetTransform.transform.position = randomTarget;

        // Debug.Log("Dir is " + dir + ", target is " + randomTarget);
        // if(FishSwimPattern == FishSwimPattern.Random){
        //     target = randomTarget;
        // }else{
        //     target = targetTransform.transform.position;
        // }

    }

    // private void OnCollisionEnter(Collision other) {
    //     // Debug.Log("Collision with " + other.gameObject.name);
    //     Vector3 dir = transform.position - other.contacts[0].point;
    //     Vector3 randomTarget = (dir * offsetDistance) + (UnityEngine.Random.insideUnitSphere * 1.75f);
    //     target = randomTarget;
    //     targetTransform.transform.position = randomTarget;

    //     StartCoroutine(SpikeTurnSpeed());
    // }

    // private void OnTriggerStay(Collider other) {
    //     Debug.Log("Collision with " + other.gameObject.name);
    //     // Vector3 dir = transform.position - other.contacts[0].point;

    //     // Vector3 dir = transform.forward * -1;
    //     // Vector3 randomTarget = (dir * offsetDistance) + (UnityEngine.Random.insideUnitSphere * 1.75f);
    //     // target = randomTarget;
    //     // targetTransform.transform.position = randomTarget;

    //     PickNewRandomDestination();

    //     StartCoroutine(SpikeTurnSpeed());
    // }

    private void TargetGrow()
    {
        targetTransform.transform.localScale += new Vector3(scaleAmount, scaleAmount, scaleAmount);
    }

    private void TargetScaleReset()
    {
        targetTransform.transform.localScale = Vector3.one;
    }

    IEnumerator SpikeTurnSpeed()
    {
        turnSpeed = maxTurnSpeed;
        AnimSwimFast();
        yield return new WaitForSeconds(turnSpeedOnCollisionMultiplierDuration);
        turnSpeed = minTurnSpeed;
        AnimSwimNormal();

        // if(turnSpeed <= maxTurnSpeed)
        // {
        //     turnSpeed *= turnSpeedOnCollisionMultiplier;
        //     AnimSwimFast();
        //     yield return new WaitForSeconds(turnSpeedOnCollisionMultiplierDuration);
        // }

        // if(turnSpeed > minTurnSpeed)
        // {
        //     turnSpeed /= turnSpeedOnCollisionMultiplier;
        //     AnimSwimNormal();
        // }
    }

    private void AnimSwimFast()
    {
        fishPrefab.GetComponent<Animator>().SetBool("SwimFast", true);
    }

    private void AnimSwimSlow()
    {
        fishPrefab.GetComponent<Animator>().SetBool("SwimSlow", true);
    }

    private void AnimSwimNormal()
    {
        fishPrefab.GetComponent<Animator>().SetBool("SwimFast", false);
        fishPrefab.GetComponent<Animator>().SetBool("SwimSlow", false);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmoPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetTransform.transform.position);


            // Gizmos.DrawRay(transform.position, transform.forward * raysLength);
            //---
            // if(isHit && behindPos != Vector3.zero){
            //     Gizmos.color = Color.green;
            //     Gizmos.DrawWireSphere(behindPos, 1.75f);
            // }

            // Gizmos.color = Color.white;
            // for (int i = 0; i < numberOfRays; i++)
            // {
            //     Quaternion rotation = transform.rotation;
            //     Quaternion rotationMod = Quaternion.AngleAxis((i/((float)numberOfRays - 1)) * raysAngle * 2 - raysAngle,transform.up);
            //     Vector3 direction = rotation * rotationMod * Vector3.forward;
            //     Gizmos.DrawRay(transform.position,direction * raysLength);
            // }
        }
    }
}
