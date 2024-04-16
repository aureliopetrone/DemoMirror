using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror;

namespace DemoMirror {
public class TankController : MonoBehaviour
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
    public float wheelRotationForce = 500.0f;
    public float wheelForwardFrictionStiffness = 0.9f;
    public float wheelForwardFrictionExtremumSlip = 0.4f;
    public float wheelForwardFrictionExtremumValue = 0.9f;
    public float wheelForwardFrictionAsymptoteSlip = 0.65f;
    public float wheelForwardFrictionAsymptoteValue = 0.7f;
    public float wheelSidewaysFrictionStiffness = 0.3f;
    public float wheelSidewaysFrictionExtremumSlip = 0.9f;
    public float wheelSidewaysFrictionExtremumValue = 0.7f;
    public float wheelSidewaysFrictionAsymptoteSlip = 0.4f;
    public float wheelSidewaysFrictionAsymptoteValue = 0.34f;
    public float wheelMass = 50.0f;
    public float wheelRadius = 0.45f;

    void Start()
    {
        SetupWheel(leftBackWheelCollider);
        SetupWheel(rightBackWheelCollider);
        SetupWheel(leftFrontWheelCollider);
        SetupWheel(rightFrontWheelCollider);
    }

    [ContextMenu("Update Wheels Properties", false, 0)]
    void UpdateWheelsProperties()
    {
        SetupWheel(leftBackWheelCollider);
        SetupWheel(rightBackWheelCollider);
        SetupWheel(leftFrontWheelCollider);
        SetupWheel(rightFrontWheelCollider);
    }

    void FixedUpdate()
    {
        syncMeshPosition(leftBackWheelCollider, leftBackWheel);
        syncMeshPosition(rightBackWheelCollider, rightBackWheel);
        syncMeshPosition(leftFrontWheelCollider, leftFrontWheel);
        syncMeshPosition(rightFrontWheelCollider, rightFrontWheel);
    }

    public void MoveForward(float speed)
    {
        float torque = speed * wheelRotationForce;
        ApplyMotorTorque(leftFrontWheelCollider, torque);
        ApplyMotorTorque(leftBackWheelCollider, torque);
        ApplyMotorTorque(rightFrontWheelCollider, torque);
        ApplyMotorTorque(rightBackWheelCollider, torque);
    }

    public void MoveBackward(float speed)
    {
        float torque = -speed * wheelRotationForce;
        ApplyMotorTorque(leftFrontWheelCollider, torque);
        ApplyMotorTorque(leftBackWheelCollider, torque);
        ApplyMotorTorque(rightFrontWheelCollider, torque);
        ApplyMotorTorque(rightBackWheelCollider, torque);
    }

    public void TurnLeft(float speed)
    {
        float leftTorque = -speed * wheelRotationForce;
        float rightTorque = speed * wheelRotationForce;
        ApplyMotorTorque(leftFrontWheelCollider, leftTorque);
        ApplyMotorTorque(leftBackWheelCollider, leftTorque);
        ApplyMotorTorque(rightFrontWheelCollider, rightTorque);
        ApplyMotorTorque(rightBackWheelCollider, rightTorque);
    }

    public void TurnRight(float speed)
    {
        float leftTorque = speed * wheelRotationForce;
        float rightTorque = -speed * wheelRotationForce;
        ApplyMotorTorque(leftFrontWheelCollider, leftTorque);
        ApplyMotorTorque(leftBackWheelCollider, leftTorque);
        ApplyMotorTorque(rightFrontWheelCollider, rightTorque);
        ApplyMotorTorque(rightBackWheelCollider, rightTorque);
    }

    public void RotateTurretLeft()
    {
        rotateTurret(true);
    }

    public void RotateTurretRight()
    {
        rotateTurret(false);
    }

    public void Shoot()
    {
        shoot();
    }

    private void syncMeshPosition(WheelCollider wheelCollider, GameObject wheel)
    {
        UnityEngine.Vector3 position;
        UnityEngine.Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheel.transform.position = position;
        wheel.transform.rotation = rotation;
    }

    private void ApplyMotorTorque(WheelCollider wheel, float torque)
    {
        wheel.motorTorque = torque;
    }

    private void rotateTurret(bool direction)
    {
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

        wheel.forwardFriction = forwardFrictionCurve;
        wheel.sidewaysFriction = frictionCurve;
        wheel.mass = wheelMass;
        wheel.radius = wheelRadius;
        wheel.suspensionDistance = 0.3f;
        wheel.wheelDampingRate = 50f;
    }

    private void shoot()
    {
        UnityEngine.Vector3 bulletStartingPosition = turretMesh.transform.position + turretMesh.transform.rotation * new UnityEngine.Vector3(0, 0, 1) * 1.7f + new UnityEngine.Vector3(0, 1, 0) * 0.35f;
        GameObject bullet = Instantiate(Resources.Load("Bullet"), bulletStartingPosition, turretMesh.transform.rotation) as GameObject;
    }
}
}