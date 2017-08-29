using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reflector : MonoBehaviour {

	public float maxBeamDistance = 10000f;
	LineRenderer lineRenderer;
	public Text coordText;
	public Text bearingText;
	public Text eqText;
	public Text distance;
	public Vector2 aimBearing = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.x = Mathf.Floor(mousePosition.x + 0.5f);
		mousePosition.y = 0f;
		mousePosition.z = Mathf.Floor(mousePosition.z + 0.5f);
		// Debug.DrawRay(transform.position, mousePosition - transform.position, Color.red, 0.0f);
		bool hitGuard = UpdateLineRenderer(transform.position, mousePosition - transform.position, maxBeamDistance);
		aimBearing = new Vector2(mousePosition.x, mousePosition.z);
		UpdateCanvas(aimBearing);
	}

	void OnDrawGizmos(){
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.y = 0;
		mousePosition.x = Mathf.Floor(mousePosition.x + 0.5f);
		mousePosition.z = Mathf.Floor(mousePosition.z + 0.5f);

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(mousePosition, 0.1f);
		Gizmos.DrawLine(transform.position, mousePosition);
		// RaycastHit hit;
		// if(Physics.Raycast(transform.position, mousePosition - transform.position, out hit, Mathf.Infinity)){
		// 	Gizmos.color = Color.cyan;
		// 	Gizmos.DrawRay(hit.point, hit.normal);
		// 	Gizmos.DrawRay(hit.point, Vector3.Reflect(mousePosition - transform.position, hit.normal));
		// }
	}

	bool UpdateLineRenderer(Vector3 position, Vector3 direction, float maxLength){
		lineRenderer.positionCount = 0;
		float currentLength = 0f;
		int index = 1;
		Vector3 currentPosition = position;
		Vector3 currentDirection = direction;

		List<Vector3> points = new List<Vector3>();
		points.Add(position);
		int maxIndex = 1000;
		bool hitGuard = false;
		while (currentLength < maxLength && index <= maxIndex){
			RaycastHit hit;
			Ray ray = new Ray(currentPosition, currentDirection);
			int ignore = ~(1 << LayerMask.NameToLayer("Breadcrumb"));
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, ignore)){
				if(hit.transform.tag == "Guard"){
					float lengthToCenter = Vector3.Distance(hit.transform.position, currentPosition);
					currentLength += lengthToCenter;
					hitGuard = true;
					points.Add(hit.point);
					break;
				}
				if(hit.transform.tag == "Player"){
					float lengthToCenter = Vector3.Distance(hit.transform.position, currentPosition);
					currentLength += lengthToCenter;
					points.Add(hit.point);
					break;
				}
				float length = Vector3.Distance(hit.point, currentPosition);
				currentDirection = Vector3.Reflect(currentDirection, hit.normal);
				currentPosition = hit.point;
				if(currentLength + length > maxLength){
					Vector3 endPoint = ray.GetPoint(maxLength - currentLength);
					points.Add(endPoint);
					currentLength += maxLength - currentLength;
					break;
				}
				currentLength += length;
				points.Add(hit.point);
			}
			else{
				// Hit a diagonal (went through wall)
				//Debug.Log("Forever");
				break;
			}
			index += 1;
		}
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());

		distance.text = currentLength.ToString();

		return hitGuard;
	}

	Vector2 GetGridPos(){
		return new Vector2((int)transform.position.x, (int)transform.position.z);
	}

	void UpdateCanvas(Vector2 mousePosition){
		coordText.text = "(" + mousePosition.x.ToString() + ", " + mousePosition.y.ToString() + ")";
		bearingText.text = "(" + (mousePosition.x - transform.position.x).ToString() + ", " + (mousePosition.y - transform.position.z ).ToString() + ")";
	
		string eq = GetEq(GetGridPos(), aimBearing);
		//print(eq);
		eqText.text = eq;
	}

	string GetEq(Vector2 playerPos, Vector2 aim){
		Fraction m = new Fraction((int)(aim.y - playerPos.y), (int)(aim.x - playerPos.x));
		string mStr = "";

		if(m.Denominator == 0 || m.Denominator == Mathf.Infinity){
			mStr = "?";
		} 
		else {
			mStr = m.GetReduced().ToString();
		}


		return string.Format("({0})x", mStr);
	}
}
