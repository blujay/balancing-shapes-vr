using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyMovement : MonoBehaviour 
{

    public LayerMask landingLayers;
    public LayerMask floorLayer;
    public float flyRadius;
    public float speed;
    public float directionChangeChance;
    public float takeOffChance;
    public float minChangeDuration;
    public float maxChangeDuration;
    public float minHeight;
    public float maxHeight;
    public float verticalVelocity;
    public float minDistanceFromWall;
    public float restInterval;
    public bool showDebug;
    public Animator flyAnimator;

    public UnityEvent flyTakeoff;
    public UnityEvent flyLanding;

    float elapsed;
    float timeSinceLastDirectionChange;
    float timeSinceLastRest;
    Vector3 direction;
    bool landed;
    Vector3 landedSurfaceNormal;


    void Start () 
    {
		direction = Vector3.forward;
        timeSinceLastRest = Random.Range(0,restInterval);

        flyTakeoff.Invoke();
	}
	
	void Update () 
    {

        
        elapsed += Time.deltaTime;
        timeSinceLastDirectionChange += Time.deltaTime;
        timeSinceLastRest += Time.deltaTime;
        bool changeDirection = elapsed >= minChangeDuration;

        if( landed ){
            if( changeDirection && ( Random.value < takeOffChance ) ){
                TakeOff();
            }
            return;
        }

        RaycastHit hitInfo;
        RaycastHit obsticleInfo;
        float distanceFromLanding;
        bool obsticleInFront = Physics.Raycast( transform.position, direction, out hitInfo, minDistanceFromWall, landingLayers, QueryTriggerInteraction.Ignore );
        distanceFromLanding = hitInfo.distance;
        obsticleInfo = hitInfo;
        bool shouldLand = timeSinceLastRest > restInterval ;
        bool landing = obsticleInFront && shouldLand && ( obsticleInfo.collider.GetComponentInParent<Rigidbody>() == null );

        float heightAboveFloor;
        Vector3 movement = speed * direction * Time.deltaTime;
        if( Physics.Raycast( transform.position, -Vector3.up,out hitInfo, maxHeight, floorLayer, QueryTriggerInteraction.Ignore ) ){
            heightAboveFloor = hitInfo.distance ;
        }
        else {
            heightAboveFloor = maxHeight;
        }

        changeDirection |= heightAboveFloor < minHeight || heightAboveFloor >= maxHeight ;

        if( ( changeDirection || obsticleInFront ) && !landing ){
            if( Random.value < directionChangeChance || obsticleInFront || timeSinceLastDirectionChange > maxChangeDuration ){
                ChangeDirection(heightAboveFloor, obsticleInFront, obsticleInfo);
            }
            elapsed = 0;
        }
        else if( landing && distanceFromLanding <= movement.magnitude ){
            Land( obsticleInfo.point + obsticleInfo.normal * flyRadius, obsticleInfo.normal );
            elapsed = 0;
            return;
        }


        transform.position += speed * direction * Time.deltaTime;



	}

    void OnTriggerEnter()
    {

        if( landed ){
            LogFormat("{0} Disturbed!",name);
            TakeOff();
        }

    }

    private void ChangeDirection(float heightAboveFloor, bool obsticleInFront, RaycastHit obsticleInfo){
        var directionXZ = Random.insideUnitCircle.normalized;
        var directionY = Random.Range(-verticalVelocity * speed,verticalVelocity * speed);

        var oldDirection = direction;

        if( heightAboveFloor < minHeight ){
            directionY = Mathf.Abs( directionY );
        }
        if( heightAboveFloor >= maxHeight ){
            directionY = -Mathf.Abs( directionY );
        }
        direction = new Vector3( directionXZ.x, directionY, directionXZ.y ).normalized;

        if( obsticleInFront ){
            if( Vector3.Dot( obsticleInfo.normal, direction ) < 0 ){
                direction = -direction;
                direction.y = -direction.y;
            }

        }
        else if( Vector3.Dot( oldDirection, direction ) < 0 ){
            direction = -direction;
            direction.y = -direction.y;        
        }

       

        LogFormat("{3} New direction dir{0} pos={1} heightAboveFloor={2}", direction, transform.position, heightAboveFloor, name);
        transform.rotation = Quaternion.LookRotation( direction );
        timeSinceLastDirectionChange = 0;

    }

    private void Land(Vector3 position, Vector3 surfaceNormal)
    {
        LogFormat("{0} Landed",name);
        transform.position = position;
        transform.up = surfaceNormal;
        direction = surfaceNormal;
        landed = true;
        flyLanding.Invoke();
    }

    private void TakeOff()
    {
        LogFormat("{0} TakeOff",name);
        transform.rotation = Quaternion.LookRotation( direction );
        timeSinceLastRest = 0;
        timeSinceLastDirectionChange = 0;
        landed = false;

        flyTakeoff.Invoke();
    }

    private void LogFormat(string message, params object[] parts){
        if( showDebug ){
            Debug.LogFormat(message, parts);
        }

    }

}
