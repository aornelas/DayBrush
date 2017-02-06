using UnityEngine;
using System.Collections;

public class FloorButtonsController : MonoBehaviour {

    public GameObject buttonPanel;
    public MeshRenderer controller;
    public GameObject laser;
    public GameObject reticle;

    private float minDownY = -0.5f;

    private bool _forceHidden = false;
      
	void Update ()
    {
        Vector3 controllerDirection = GvrController.Orientation * Vector3.forward;
        bool controllerPointingDown = controllerDirection.y < minDownY;
        if (!GvrController.AppButton && !GvrController.ClickButton) {
            if (controllerPointingDown) {
                if (!_forceHidden) {
                    buttonPanel.SetActive(true);
                    controller.enabled = true;
                    laser.SetActive(true);
                    reticle.SetActive(true);
                }
            } else {
                buttonPanel.SetActive(false);
                reticle.SetActive(false);
                _forceHidden = false;
            }
        }
	}

    public void HideFloorButtons ()
    {
        buttonPanel.SetActive(false);
        controller.enabled = false;
        laser.SetActive(false);
        reticle.SetActive(false);
        _forceHidden = true;
    }
}
