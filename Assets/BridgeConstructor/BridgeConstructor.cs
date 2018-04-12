using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

public class BridgeConstructor : EditorWindow
{

	[MenuItem("sTools/BridgeConstructor")]
	static void Init()
    {
        BridgeConstructor window = (BridgeConstructor)EditorWindow.GetWindow(typeof(BridgeConstructor));
        window.Show();
    }

	/* ON GUI */
	void OnGUI()
	{
		using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		{
			if(GUILayout.Button("Add Select Pylon")) AddPylon(Selection.gameObjects);
			if(GUILayout.Button("Reset Pylon List")) ResetPylonList();
		}

		if(pylonList.Count > 0)
		{
			for(int i = 0; i < pylonList.Count; i++)
			{
				GUILayout.Label(i + " - " + pylonList[i].name);
			}
		}

		using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		{
			if(GUILayout.Button("Generate Bridge")) GenerateBridge();
			if(GUILayout.Button("Reset Bridge")) ResetBridge();
		}

		deckNumber = EditorGUILayout.IntField("Number of Deck : ", deckNumber);
		gravityForce = EditorGUILayout.Slider("Gravity Force : ", gravityForce, 0, 10);
	}

	/* BRIDGE */
	void GenerateBridge()
	{
		ResetBridge();
		RotatePylons();
	}

	/* PYLONS */
	List<GameObject> pylonList = new List<GameObject>();
	void ResetPylonList()
	{
		pylonList.Clear();
		Debug.Log("[BridgeConstructor] Pylon list clear, new size : " + pylonList.Count);
	}
	void AddPylon(GameObject[] newPylon)
	{
		foreach(GameObject pylon in newPylon)
		{
			if(!pylonList.Contains(pylon))
			{
				pylonList.Add(pylon);
			}
			else Debug.Log("[BridgeConstructor] Pylon already registered : " + newPylon);
		}
	}
	void RotatePylons()
	{
		if(pylonList.Count >= 2)
		{
			for(int i = 0; i < pylonList.Count; i++)
			{
				if(i == (pylonList.Count - 1)) pylonList[i].transform.LookAt(pylonList[i-1].transform);
				else
				{
					pylonList[i].transform.LookAt(pylonList[i+1].transform);
					GeneratePlank(pylonList[i], pylonList[i+1]);
				}
			}
		}
		else Debug.Log("[BridgeConstructor] There is less than 2 pylons registered. Length = " + pylonList.Count);
	}

	/* PLANK */
	int deckNumber = 8;
	float gravityForce = 3;
	List<GameObject> deckList;
	void GeneratePlank(GameObject firstPylon, GameObject secondPylon)
	{
		if(deckNumber > 0)
		{
			List<GameObject> tempDeckList = new List<GameObject>();

			//DISTANCE
			float bridgeDistance = Vector3.Distance(firstPylon.transform.position, secondPylon.transform.position);
			float deckDistance = bridgeDistance / (deckNumber + 1);

			//GRAVITY
			GameObject gravityPoint = new GameObject("Temporary_Gravity_Point");
			Vector3 gravityPointPosition = firstPylon.transform.position + firstPylon.transform.forward * (bridgeDistance/2);
			gravityPointPosition.y -= gravityForce;
			gravityPoint.transform.position = gravityPointPosition;
			gravityPoint.transform.LookAt(secondPylon.transform);

			//GENERATE PLANKS
			for(int i = 0; i < deckNumber; i++)
			{
				GameObject newDeck = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newDeck.name = "Bridge_Deck_" + i;

				Vector3 plankPosition = firstPylon.transform.position + firstPylon.transform.forward * (deckDistance * (i + 1));
				float gravityFactor = Mathf.Abs(((deckDistance * (i + 1)) - (bridgeDistance/2)) / (bridgeDistance/2));
				plankPosition.y -= gravityForce * (1 - Mathf.Pow(gravityFactor, 2));
				newDeck.transform.position = plankPosition;

				newDeck.transform.parent = firstPylon.transform;
				deckList.Add(newDeck);
				tempDeckList.Add(newDeck);
			}

			//PLANKS ROTATION
			for(int y = 0; y < tempDeckList.Count; y++)
			{
				if(y < (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y + 1].transform);
				else if(y > (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y - 1].transform);
				else tempDeckList[y].transform.forward = firstPylon.transform.forward;
			}

			MonoBehaviour.DestroyImmediate(gravityPoint);
		}
		else Debug.Log("[BridgeConstructor] There is less than 1 deck change it in deck number field");
	}
	void ResetBridge()
	{
		foreach(GameObject deck in deckList)
		{
			MonoBehaviour.DestroyImmediate(deck);
		}
		deckList.Clear();
	}
}
