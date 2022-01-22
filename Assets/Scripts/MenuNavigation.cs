//based on original script taken from:
// http://unity.grogansoft.com/simple-script-for-gamepad-ui-navigation/
// URL updated to:
// https://www.grogansoft.com/2016/12/18/simple-script-for-gamepad-ui-navigation/
// Edited specifically for use by Jon B. "Cheezyrock" Honeycutt

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
	private Button previousButton;
	[SerializeField] private float scaleAmount = 1.4f;
	[SerializeField] private GameObject defaultButton = null;

	private Vector3 scaledVector = Vector3.one;

	void Start()
	{
		if (defaultButton != null)
		{
			EventSystem.current.SetSelectedGameObject(defaultButton);
		}
		
		if (scaleAmount != 1)
        {
			scaledVector = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        }
	}
	void Update()
	{
		var selected = EventSystem.current.currentSelectedGameObject;

		if (selected != null)
		{
			Button selectedAsButton = selected.GetComponent<Button>();

			if (selectedAsButton != null && selectedAsButton != previousButton)
			{
				HighlightButton(selectedAsButton);
			}

			if (previousButton != null && previousButton != selectedAsButton)
			{
				UnHighlightButton(previousButton);
			}

			previousButton = selectedAsButton;
		}
	}

	void OnDisable()
	{
		if (previousButton != null) UnHighlightButton(previousButton);
	}

	void HighlightButton(Button button)
	{
		button.transform.localScale = scaledVector;
	}

	void UnHighlightButton(Button button)
	{
		button.transform.localScale = Vector3.one;
	}
}