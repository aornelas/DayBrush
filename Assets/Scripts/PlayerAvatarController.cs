using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAvatarController : NetworkBehaviour {

    private GameObject head;

    void Start ()
    {
        if (isLocalPlayer) {
            transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        }
        head = transform.Find("head").gameObject;
    }

    void Update ()
    {
        if (isLocalPlayer) {
            head.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        }
    }

    public override void OnStartLocalPlayer()
    {
        GameObject head = transform.Find("head").gameObject;
        GameObject visor = head.transform.Find("visor").gameObject;
        GameObject body = transform.Find("body").gameObject;

        // hide player avatar locally
        head.GetComponent<MeshRenderer>().enabled = false;
        visor.GetComponent<MeshRenderer>().enabled = false;
        body.GetComponent<MeshRenderer>().enabled = false;
    }
}
