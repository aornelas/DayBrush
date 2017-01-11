using UnityEngine;
using System.Collections.Generic;

public class DayBrushController : MonoBehaviour {

	public GameObject pencil;
	public GameObject paint;
	public float strokeOffsetZ = -0.5f;
	public AudioClip undoSFX;
	public AudioClip redoSFX;

	private Stack<GameObject> strokes;
	private Stack<GameObject> undoneStrokes;
	private Vector2 touchStartPos;
	private Material paintMaterial;
	private float swipeThreshold = 0.5f;
	private int currentPaintIndex = -1;
	private Color[] paintColors = {
		Color.white,
		Color.gray,
		Color.black,
		Color.red,
		Color.green,
		Color.blue,
		Color.cyan,
		Color.magenta,
		Color.yellow
	};

	void Awake ()
	{
		strokes = new Stack<GameObject>();
		undoneStrokes = new Stack<GameObject>();
		paintMaterial = paint.gameObject.GetComponent<TrailRenderer>().material;
		NextPaint();
	}
	
	void Update ()
	{
		if (GvrController.ClickButtonDown) {
			StartStroke();
		}

		if (GvrController.ClickButtonUp) {
			EndStroke();
		}

		if (GvrController.TouchDown) {
			touchStartPos = GvrController.TouchPos;
		}

		if (GvrController.TouchUp) {
			if (SwipedLeft()) {
				UndoStroke();
			} else if (SwipedRight()) {
				RedoStroke();
			} else if (SwipedDown()) {
				PreviousPaint();
			} else if (SwipedUp()) {
				NextPaint();
			}
		}

	}

	private void StartStroke ()
	{
		GameObject stroke = GameObject.Instantiate<GameObject>(paint);
		stroke.transform.SetParent(pencil.transform);
		stroke.transform.localPosition = new Vector3(0, 0, strokeOffsetZ);
		stroke.gameObject.GetComponent<TrailRenderer>().enabled = true;
		strokes.Push(stroke);
	}

	private void EndStroke ()
	{
		GameObject stroke = strokes.Peek();
		stroke.transform.SetParent(this.transform);
		undoneStrokes = new Stack<GameObject>();
	}

	private void NextPaint ()
	{
		currentPaintIndex = (currentPaintIndex + 1) % paintColors.Length;
		SetPaint();
	}

	private void PreviousPaint ()
	{
		currentPaintIndex = (currentPaintIndex - 1 + paintColors.Length) % paintColors.Length;
		SetPaint();
	}

	private void SetPaint()
	{
		Color newColor = paintColors[currentPaintIndex];
		pencil.gameObject.GetComponent<MeshRenderer>().material.color = newColor;
		Material newPaintMaterial = Material.Instantiate(paintMaterial);
		newPaintMaterial.SetColor("_EmissionColor", newColor);
		paint.gameObject.GetComponent<TrailRenderer>().material = newPaintMaterial;
	}

	private void UndoStroke ()
	{
		if (strokes.Count > 0) {
			GameObject stroke = strokes.Pop();
			stroke.SetActive(false);
			undoneStrokes.Push(stroke);
			AudioSource.PlayClipAtPoint(undoSFX, stroke.transform.position);
		}
	}

	private void RedoStroke ()
	{
		if (undoneStrokes.Count > 0) {
			GameObject stroke = undoneStrokes.Pop();
			stroke.SetActive(true);
			strokes.Push(stroke);
			AudioSource.PlayClipAtPoint(redoSFX, stroke.transform.position);
		}
	}

	private bool SwipedLeft ()
	{
		return (touchStartPos.x - GvrController.TouchPos.x) >= swipeThreshold;
	}

	private bool SwipedRight ()
	{
		return (GvrController.TouchPos.x - touchStartPos.x) >= swipeThreshold;
	}

	private bool SwipedDown ()
	{
		return (touchStartPos.y - GvrController.TouchPos.y) >= swipeThreshold;
	}

	private bool SwipedUp ()
	{
		return (GvrController.TouchPos.y - touchStartPos.y) >= swipeThreshold;
	}
}
