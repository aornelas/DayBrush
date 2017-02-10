using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAvatarController : NetworkBehaviour {

    private GameObject head;

    void Start ()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        head = transform.Find("head").gameObject;
    }

    void Update ()
    {
        if (isLocalPlayer) {
            head.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        }
    }
}
