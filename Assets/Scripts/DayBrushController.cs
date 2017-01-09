using UnityEngine;
using System.Collections.Generic;

public class DayBrushController : MonoBehaviour {

	public GameObject pencil;
	public GameObject paint;

	private Stack<GameObject> strokes;

	void Awake ()
	{
		strokes = new Stack<GameObject>();
	}
	
	void Update ()
	{
		if (GvrController.ClickButtonDown) {
			GameObject stroke = GameObject.Instantiate<GameObject>(paint);
			stroke.transform.SetParent(pencil.transform);
			stroke.transform.localPosition = Vector3.zero + new Vector3(0, 0, -0.5f);
			stroke.gameObject.GetComponent<TrailRenderer>().enabled = true;
			strokes.Push(stroke);
		}

		if (GvrController.ClickButtonUp) {
			GameObject stroke = strokes.Peek();
			stroke.transform.SetParent(this.transform);
		}

		if (GvrController.AppButtonDown) {
			GameObject stroke = strokes.Pop();
			stroke.SetActive(false);
		}

	}
}
