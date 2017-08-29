using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public int width_x;
	public int width_z;

	GameObject leftWall;
	GameObject rightWall;
	GameObject topWall;
	GameObject bottomWall;

	public float wallWidth = 0.1f;
	public float wallHeight = 1.0f;

	public Transform player;
	public Transform guard;

	public Vector2 playerStartPos;
	public Vector2 guardStartPos;

	// Use this for initialization
	void Start () {
		GenerateWalls();
		MovePlayer((int)playerStartPos.x, (int)playerStartPos.y);
		MoveGuard((int)guardStartPos.x, (int)guardStartPos.y);
	}

	// Update is called once per frame
	void Update () {
		
	}

	// Also moves camera
	public void GenerateWalls(){
		float halfWall = wallWidth / 2.0f;
		float halfWidthX = (float) width_x / 2.0f;
		float halfWidthZ = (float) width_z / 2.0f;
		
		leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leftWall.transform.parent = transform;
		leftWall.transform.position = transform.position + new Vector3(-halfWidthX - halfWall, 0f, 0f);
		leftWall.transform.localScale = new Vector3(wallWidth, wallHeight, (float)width_z + (wallWidth * 2.0f));
		leftWall.transform.Translate(new Vector3(halfWidthX, 0f, halfWidthZ));

		rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		rightWall.transform.parent = transform;
		rightWall.transform.position = transform.position + new Vector3(halfWidthX+ halfWall, 0f, 0f);
		rightWall.transform.localScale = new Vector3(wallWidth, wallHeight, (float)width_z + (wallWidth * 2.0f));
		rightWall.transform.Translate(new Vector3(halfWidthX, 0f, halfWidthZ));

		topWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		topWall.transform.parent = transform;
		topWall.transform.position = transform.position + new Vector3(0f, 0f, halfWidthZ + halfWall);
		topWall.transform.localScale = new Vector3((float)width_x, wallHeight, wallWidth);
		topWall.transform.Translate(new Vector3(halfWidthX, 0f, halfWidthZ));

		bottomWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bottomWall.transform.parent = transform;
		bottomWall.transform.position = transform.position + new Vector3(0f, 0f, -halfWidthZ - halfWall);
		bottomWall.transform.localScale = new Vector3((float)width_x, wallHeight, wallWidth);
		bottomWall.transform.Translate(new Vector3(halfWidthX, 0f, halfWidthZ));

		Camera.main.orthographicSize = Mathf.Ceil(Mathf.Max(halfWidthX, halfWidthZ));
		Camera.main.transform.position = new Vector3(halfWidthX, wallHeight + 1.0f, halfWidthZ);

	}

	public void ClearWalls(){
		Destroy(leftWall);
		Destroy(rightWall);
		Destroy(topWall);
		Destroy(bottomWall);
	}

	public void MovePlayer(int x, int y){
		if(x < 0 || x >= width_x || y < 0 || y >= width_z){
			Debug.LogError("Cannot move player - position is invalid");
			return;
		}
		player.position = new Vector3(x, 0f, y);
	}

	public void MoveGuard(int x, int y){
		if(x < 0 || x >= width_x || y < 0 || y >= width_z){
			Debug.LogError("Cannot move guard - position is invalid");
			return;
		}
		guard.position = new Vector3(x, 0f, y);
	}



}
