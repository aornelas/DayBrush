using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour {

    public LineRenderer laser;
    public GameObject target;
    public GameObject player;
    public GameObject floorButtons;

    private float maxGroundY = 0.15f;
    private float targetFloatingAmplitude = 0.1f;
    private float targetFloatingSpeed = 5.0f;
    private Color32 enabledTargetColor = new Color32(64, 128, 0, 255);
    private Color32 disabledTargetColor = new Color32(128, 0, 0, 255);

    private float targetInitY;
    private HashSet<Vector2> playerPositions;

    /// Status flags
    private bool _aimingAtGround;

    Vector3 currentTargetPos;

    void Start ()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laser.SetPositions(initLaserPositions);
        laser.SetWidth(0.001f, 0.001f);

        targetInitY = target.transform.position.y;
        playerPositions = new HashSet<Vector2>();
    }

    void Update ()
    {
        if (floorButtons.activeSelf) {
            return;
        }

        Vector3 v = GvrController.Orientation * Vector3.forward;

        if (GvrController.AppButton) {
            ShootLaserFromPointer(transform.position, v, 200f);
            OrientTargetToPlayer();
            UpdateTargetColor();
//            MakeTargetFloat();
//            laser.enabled = true;
            target.SetActive(true);
        }

        if (GvrController.AppButtonUp) {
            if (_aimingAtGround) {
                TeleportToTarget();
            }
            laser.enabled = false;
            target.SetActive(false);
        }
    }

    /// <summary>
    /// Records the current position of the player to later be able to return to.
    /// </summary>
    public void RecordPosition ()
    {
        playerPositions.Add(player.transform.position);
        Debug.Log(playerPositions.Count);
    }

    private void ShootLaserFromPointer (Vector3 pointerPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(pointerPosition, direction);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, length)) {
            GameObject gameObj = raycastHit.transform.gameObject;
            if (true) {//gameObj.tag == "Ground") {
                // Show the target and follow track to the pointer
                currentTargetPos = new Vector3(raycastHit.point.x,
                    target.transform.localPosition.y, raycastHit.point.z);
                target.transform.position = currentTargetPos;
            }
        }

        Vector3 targetPosition = pointerPosition + (length * direction);
        laser.SetPosition(0, pointerPosition);
        laser.SetPosition(1, targetPosition);
    }

    private void OrientTargetToPlayer ()
    {
        Vector3 playerPosition =
            new Vector3(player.transform.position.x, target.transform.position.y, player.transform.position.z);
        target.transform.LookAt(playerPosition);
        target.transform.Rotate(new Vector3(180, 0));
    }

    private void UpdateTargetColor ()
    {
        Vector3 controllerDirection = GvrController.Orientation * Vector3.forward;
        _aimingAtGround = controllerDirection.y < maxGroundY;

        Color targetColor = enabledTargetColor;
        if (!_aimingAtGround) {
            targetColor = disabledTargetColor;
        }
        target.gameObject.GetComponent<MeshRenderer>().material.color = targetColor;
    }

    private void MakeTargetFloat ()
    {
        float newTargetY = targetInitY + targetFloatingAmplitude * Mathf.Sin(targetFloatingSpeed * Time.time);
        target.transform.position = new Vector3(target.transform.position.x, newTargetY, target.transform.position.z);
        
    }

    private void TeleportToTarget ()
    {
        player.transform.position = new Vector3(currentTargetPos.x, player.transform.position.y, currentTargetPos.z);
    }
}
