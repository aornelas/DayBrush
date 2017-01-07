using UnityEngine;
using System.Collections;

public class DayBrushController : MonoBehaviour {

	public GameObject pencil;
	public GameObject paint;
	private GameObject stroke;
	
	void Update ()
	{
		if (GvrController.ClickButtonDown) {
			stroke = GameObject.Instantiate<GameObject>(paint);
			stroke.transform.SetParent(pencil.transform);
			stroke.transform.localPosition = Vector3.zero + new Vector3(0, 0, -0.5f);
			stroke.gameObject.GetComponent<TrailRenderer>().enabled = true;
		}

		if (GvrController.ClickButtonUp) {
			stroke.transform.SetParent(this.transform);
		}

	}
}
