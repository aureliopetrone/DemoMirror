using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror;

public class TankControls : NetworkBehaviour
{
    // Colliders
    public WheelCollider leftFrontWheelCollider;
    public WheelCollider rightFrontWheelCollider;
    public WheelCollider leftBackWheelCollider;
    public WheelCollider rightBackWheelCollider;

    // Meshes

    public GameObject leftFrontWheel;
    public GameObject rightFrontWheel;
    public GameObject leftBackWheel;
    public GameObject rightBackWheel;

    public GameObject turretMesh;

    // Settings
    private float wheelRotationForce = 500.0f;
    private float wheelForwardFrictionStiffness = 0.9f;
    private float wheelForwardFrictionExtremumSlip = 0.4f;
    private float wheelForwardFrictionExtremumValue = 0.9f;
    private float wheelForwardFrictionAsymptoteSlip = 0.65f;
    private float wheelForwardFrictionAsymptoteValue = 0.7f;
    private float wheelSidewaysFrictionStiffness = 0.3f;
    private float wheelSidewaysFrictionExtremumSlip = 0.9f;
    private float wheelSidewaysFrictionExtremumValue = 0.7f;
    private float wheelSidewaysFrictionAsymptoteSlip = 0.4f;
    private float wheelSidewaysFrictionAsymptoteValue = 0.34f;
    private float wheelMass = 50.0f;
    private float wheelRadius = 0.45f;



    // Inputs
    private float leftTracksInput;
    private float rightTracksInput; // Corrected variable name
    private float leftTracksInputBack;
    private float rightTracksInputBack;
    private float leftTurretInput;
    private float rightTurretInput;
    private bool shootInput;

    void Start()
    {
        // Setup friction for the wheels                    3

        
        SetupWheel(leftBackWheelCollider);
        SetupWheel(rightBackWheelCollider);
        SetupWheel(leftFrontWheelCollider);
        SetupWheel(rightFrontWheelCollider);
    }

    void Update()
    {
        getInputs();

        if (shootInput)
        {

            shoot();
        }
        
    }

    void FixedUpdate()
    {
        syncMeshPosition(leftBackWheelCollider, leftBackWheel);
        syncMeshPosition(rightBackWheelCollider, rightBackWheel);
        syncMeshPosition(leftFrontWheelCollider, leftFrontWheel);
        syncMeshPosition(rightFrontWheelCollider, rightFrontWheel);

        float leftTorque = leftTracksInput * wheelRotationForce + leftTracksInputBack * -wheelRotationForce;
        float rightTorque = rightTracksInput * wheelRotationForce + rightTracksInputBack * -wheelRotationForce;

        Debug.Log("Left torque: " + leftTorque);
        Debug.Log("Right torque: " + rightTorque);


        ApplyMotorTorque(leftFrontWheelCollider, leftTorque); // Use wheelRotationForce
        ApplyMotorTorque(leftBackWheelCollider, leftTorque); // Use wheelRotationForce

        ApplyMotorTorque(rightFrontWheelCollider, rightTorque); // Use wheelRotationForce
        ApplyMotorTorque(rightBackWheelCollider, rightTorque); // Use wheelRotationForce

        if (leftTurretInput == 1 && rightTurretInput == 1)
        {
            // Do nothing
        }
        else if (leftTurretInput == 1)
        {
            rotateTurret(true);
        }
        else if (rightTurretInput == 1)
        {
            rotateTurret(false);
        }

       

    }

    private void syncMeshPosition(WheelCollider wheelCollider, GameObject wheel)
    {

        UnityEngine.Vector3 position;
        UnityEngine.Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        // Get the mesh of the wheel. The weel is a WheelCollider, so we need to get
        // the GameObject that contains the collider and then transform the position

        // Set the position and rotation of the wheel mesh
        wheel.transform.position = position;
        wheel.transform.rotation = rotation;

    }

    private void getInputs()
    {
        if(isLocalPlayer == false)
        {
            return;
        }
        
        leftTracksInput = Input.GetKey(KeyCode.A) ? 1 : 0;
        rightTracksInput = Input.GetKey(KeyCode.D) ? 1 : 0;
        leftTracksInputBack = Input.GetKey(KeyCode.Z) ? 1 : 0;
        rightTracksInputBack = Input.GetKey(KeyCode.X) ? 1 : 0;
        leftTurretInput = Input.GetKey(KeyCode.Q) ? 1 : 0;
        rightTurretInput = Input.GetKey(KeyCode.E) ? 1 : 0;
        shootInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void ApplyMotorTorque(WheelCollider wheel, float torque)
    {
        Debug.Log("Applying motor torque: " + torque);
        wheel.motorTorque = torque;
    }

    private void rotateTurret(bool direction)
    {
        // Rotate the turret
        var rotationVector = new UnityEngine.Vector3(0, 0, 0);
        if (direction)
        {
            rotationVector.y = 1;
        }
        else
        {
            rotationVector.y = -1;
        }
        turretMesh.transform.Rotate(rotationVector);
    }

    private void SetupWheel(WheelCollider wheel)
    {

        // Create the curve
        WheelFrictionCurve frictionCurve = new WheelFrictionCurve
        {
            stiffness = wheelSidewaysFrictionStiffness,
            extremumSlip = wheelSidewaysFrictionExtremumSlip,
            extremumValue = wheelSidewaysFrictionExtremumValue,
            asymptoteSlip = wheelSidewaysFrictionAsymptoteSlip,
            asymptoteValue = wheelSidewaysFrictionAsymptoteValue
        };

        WheelFrictionCurve forwardFrictionCurve = new WheelFrictionCurve
        {
            stiffness = wheelForwardFrictionStiffness,
            extremumSlip = wheelForwardFrictionExtremumSlip,
            extremumValue = wheelForwardFrictionExtremumValue,
            asymptoteSlip = wheelForwardFrictionAsymptoteSlip,
            asymptoteValue = wheelForwardFrictionAsymptoteValue
        };


        // Set the curve
        wheel.forwardFriction = forwardFrictionCurve;
        wheel.sidewaysFriction = frictionCurve;

        // Set the mass, radius, suspension distance wheel damping rate and force app point
        wheel.mass = 50;
        wheel.radius = 0.45f;
        wheel.suspensionDistance = 0.3f;
        wheel.wheelDampingRate = 50f;
    }

    private void shoot()
    {
        // Shoot a bullet
        // 1 - Create a bullet (a capsule with a collider and a rigidbody)
        // 2 - Add a force to the bullet
        // 3 - Destroy the bullet at impact

        Debug.Log("Shooting");

        UnityEngine.Vector3 bulletStartingPosition = turretMesh.transform.position + turretMesh.transform.rotation * new UnityEngine.Vector3(0, 0, 1) * 1.7f + new UnityEngine.Vector3(0, 1, 0) * 0.35f;
        GameObject bullet = Instantiate(Resources.Load("Bullet"), bulletStartingPosition, turretMesh.transform.rotation) as GameObject;
        // Set force 
        // bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000, ForceMode.Impulse);

    }

}
