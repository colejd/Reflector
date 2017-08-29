using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GUIManager : MonoBehaviour {

	public Reflector reflector;
	public RoomGenerator RoomGenerator;
	public Collider playerCollider;
	public Collider guardCollider;

	public InputField lineLengthField;
	public Toggle playerCollisionToggle;
	public Toggle guardCollisionToggle;

	// Use this for initialization
	void Start () {
		PopulateFields();

		playerCollisionToggle.onValueChanged.AddListener((on) => playerCollider.enabled = on);
		guardCollisionToggle.onValueChanged.AddListener((on) => guardCollider.enabled = on);
		//lineLengthField.val
		lineLengthField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return ValidateInt( addedChar ); };
		lineLengthField.onValueChanged.AddListener (delegate {OnLineLengthUpdate ();});

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Populate fields from values of objects= 
    /// </summary>
	public void PopulateFields(){
		lineLengthField.text = reflector.maxBeamDistance.ToString();
		playerCollisionToggle.isOn = playerCollider.enabled;
		guardCollisionToggle.isOn = guardCollider.enabled;
	}

	// Non-numbers are invalid
	private char ValidateInt(char charToValidate)
	{
		if (!Char.IsDigit(charToValidate)){
			charToValidate = '\0';
		}
		return charToValidate;
	}
	
	public void OnLineLengthUpdate(){
		reflector.maxBeamDistance = float.Parse(lineLengthField.text);
	}

}