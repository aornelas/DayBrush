using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AvatarPencilController : NetworkBehaviour {

    [SyncVar (hook = "OnColorChanged")]
    Color color;

	void OnColorChanged (Color value)
    {
        color = value;
//        GetComponent<MeshRenderer>().material.SetColor
    }
}
