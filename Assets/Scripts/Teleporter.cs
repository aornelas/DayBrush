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
    private float targetSnapMargin = 0.25f;
    private Color32 enabledTargetColor = new Color32(64, 128, 0, 255);
    private Color32 disabledTargetColor = new Color32(128, 0, 0, 255);
    private Color32 pastTargetColor = Color.gray;
    private Material pastTargetMaterial;

    private float targetInitY;
    private HashSet<GameObject> pastTargets;

    /// Status flags
    private bool _aimingAtGround;

    void Start ()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laser.SetPositions(initLaserPositions);
        laser.SetWidth(0.001f, 0.001f);

        targetInitY = target.transform.position.y;
        pastTargets = new HashSet<GameObject>();
        pastTargetMaterial = GameObject.Instantiate<Material>(target.gameObject.GetComponent<MeshRenderer>().material);
        pastTargetMaterial.color = pastTargetColor;
    }

    void Update ()
    {
        if (floorButtons.activeSelf) {
            return;
        }

        Vector3 v = GvrController.Orientation * Vector3.forward;

//        if (GvrController.AppButton) {
        if (GvrController.ClickButton) {
            ShootLaserFromPointer(transform.position, v, 200f);
            OrientTargetsToPlayer();
            UpdateTargetColor();
            ShowPastTargets();
            SnapTargetToNearbyPastTargets();
//            MakeTargetFloat();
//            laser.enabled = true;
            target.SetActive(true);
        }

//        if (GvrController.AppButtonUp) {
        if (GvrController.ClickButtonUp) {
            if (_aimingAtGround) {
                TeleportToTarget();
            }
            laser.enabled = false;
            target.SetActive(false);
            HidePastTargets();
        }
    }

    /// <summary>
    /// Records the current position of the player to later be able to return to.
    /// </summary>
    public void RecordPosition ()
    {
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.z);
        GameObject pastTarget = GameObject.Instantiate<GameObject>(target);

        pastTarget.gameObject.GetComponent<MeshRenderer>().material = pastTargetMaterial;
        pastTarget.transform.position = new Vector3(position.x, pastTarget.transform.position.y, position.y);
        pastTarget.SetActive(false);
        pastTargets.Add(pastTarget);
    }

    public void ClearRecordedPositions ()
    {
        pastTargets.Clear();
    }

    private void ShootLaserFromPointer (Vector3 pointerPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(pointerPosition, direction);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, length)) {
            GameObject gameObj = raycastHit.transform.gameObject;
            if (true) {//gameObj.tag == "Ground") {
                // Show the target and follow track to the pointer
                target.transform.position = new Vector3(raycastHit.point.x, target.transform.localPosition.y,
                        raycastHit.point.z);
            }
        }

        Vector3 targetPosition = pointerPosition + (length * direction);
        laser.SetPosition(0, pointerPosition);
        laser.SetPosition(1, targetPosition);
    }

    private void SnapTargetToNearbyPastTargets ()
    {
        float targetX = target.transform.position.x;
        float targetZ = target.transform.position.z;

        foreach (GameObject pastTarget in pastTargets) {
            if (pastTarget.activeSelf) {
                float pastTargetX = pastTarget.transform.position.x;
                float pastTargetZ = pastTarget.transform.position.z;
                if (Mathf.Abs(targetX - pastTargetX) < targetSnapMargin &&
                        Mathf.Abs(targetZ - pastTargetZ) < targetSnapMargin) {
                    pastTarget.SetActive(false);
                    target.transform.position = pastTarget.transform.position;
                } else {
                    pastTarget.SetActive(true);
                }
            }
        }
    }

    private void OrientTargetsToPlayer ()
    {
        Vector3 playerPosition =
            new Vector3(player.transform.position.x, target.transform.position.y, player.transform.position.z);
        Vector3 down = new Vector3(180, 0);

        target.transform.LookAt(playerPosition);
        target.transform.Rotate(down);

        foreach (GameObject pastTarget in pastTargets) {
            pastTarget.transform.LookAt(playerPosition);
            pastTarget.transform.Rotate(down);
        }
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

    private void ShowPastTargets ()
    {
        Vector3 playerPosition = player.transform.position;
        foreach (GameObject pastTarget in pastTargets) {
            Vector3 pastTargetPosition = pastTarget.transform.position;
            if (pastTargetPosition.x != playerPosition.x || pastTargetPosition.z != playerPosition.z) {
                pastTarget.SetActive(true);
            }
        }
    }

    private void HidePastTargets ()
    {
        foreach (GameObject pastTarget in pastTargets) {
            pastTarget.SetActive(false);
        }
    }

    private void MakeTargetFloat ()
    {
        float newTargetY = targetInitY + targetFloatingAmplitude * Mathf.Sin(targetFloatingSpeed * Time.time);
        target.transform.position = new Vector3(target.transform.position.x, newTargetY, target.transform.position.z);
        
    }

    private void TeleportToTarget ()
    {
        Vector3 targetPosition = target.transform.position;
        player.transform.position = new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z);
    }
}
