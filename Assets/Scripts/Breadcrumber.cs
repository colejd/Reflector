using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadcrumber : MonoBehaviour {

	public Transform breadcrumbParent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.x = Mathf.Floor(mousePosition.x + 0.5f);
		mousePosition.y = 0f;
		mousePosition.z = Mathf.Floor(mousePosition.z + 0.5f);

		if (Input.GetMouseButtonDown(0)){
			Vector3 rayOrigin = mousePosition;
			rayOrigin.y = 10f;
			Ray ray = new Ray(rayOrigin, Vector3.down);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
				if(hit.transform.tag == "Breadcrumb"){
					//Delete the breadcrumb
					Destroy(hit.transform.gameObject);
				}
			}
			else{
				GameObject breadcrumb = GameObject.CreatePrimitive(PrimitiveType.Cube);
				breadcrumb.transform.parent = breadcrumbParent;
				breadcrumb.transform.position = mousePosition;
				breadcrumb.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				//breadcrumb.GetComponent<Collider>().enabled = false;
				breadcrumb.tag = "Breadcrumb";
				breadcrumb.layer = LayerMask.NameToLayer("Breadcrumb");
			}
		}
		
	}
}
