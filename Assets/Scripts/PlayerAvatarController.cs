using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAvatarController : NetworkBehaviour {

    private GameObject head;
    private GameObject pencil;
    private GameObject camera;
    private GameObject actualPencil;

    void Start ()
    {
        if (isLocalPlayer) {
            transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        }

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        actualPencil = GameObject.FindGameObjectWithTag("Pencil");
        pencil.GetComponent<MeshRenderer>().material = actualPencil.GetComponent<MeshRenderer>().material;
    }

    void Update ()
    {
        if (isLocalPlayer) {
            head.transform.rotation = camera.transform.rotation;
            pencil.transform.rotation = actualPencil.transform.rotation;
            pencil.transform.position = actualPencil.transform.position;
        }
    }

    public override void OnStartLocalPlayer()
    {
        head = transform.Find("head").gameObject;
        pencil = transform.Find("pencil").gameObject;
        GameObject visor = head.transform.Find("visor").gameObject;
        GameObject body = transform.Find("body").gameObject;

        // hide player avatar and duplicate pencil locally
        head.GetComponent<MeshRenderer>().enabled = false;
        visor.GetComponent<MeshRenderer>().enabled = false;
        body.GetComponent<MeshRenderer>().enabled = false;
        pencil.GetComponent<MeshRenderer>().enabled = false;
    }
}
