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

	#region ONGUI
	/* ON GUI */
	void OnGUI()
	{
		//PYLONS
		using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				if(GUILayout.Button("Add Select Pylon")) AddPylon(Selection.gameObjects);
				if(GUILayout.Button("Remove Select Pylon")) RemovePylon(Selection.gameObjects);
			}
			using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				if(GUILayout.Button("Generate New Pylon")) GenerateNewPylon();
				if(GUILayout.Button("Reset Pylon List")) ResetPylonList();
			}

			pylonPrefab = (GameObject) EditorGUILayout.ObjectField ("Pylon Prefab  : ", pylonPrefab, typeof(GameObject), true);
			GUILayout.Label("Pylon List : " + pylonList.Count);

			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				if(pylonList.Count > 0)
				{
					for(int i = 0; i < pylonList.Count; i++)
					{
						GUILayout.Label(i + " - " + pylonList[i].name);
					}
				}
			}
		}

		//DECK
		using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
		{
			EditorGUI.BeginChangeCheck();
			deckPrefab = (GameObject) EditorGUILayout.ObjectField ("Deck Prefab : ", deckPrefab, typeof(GameObject), true);
			deckNumber = EditorGUILayout.IntField("Deck Number : ", deckNumber);
			rotateDeck = EditorGUILayout.Toggle("Rotate Deck : ", rotateDeck);
			gravityForce = EditorGUILayout.Slider("Gravity Force : ", gravityForce, 0, 10);
			if(EditorGUI.EndChangeCheck())
			{
				GenerateBridge();
			}
		}
		//ROPE
		using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
		{
			EditorGUI.BeginChangeCheck();
			generateRope = EditorGUILayout.Toggle("Generate Rope : ", generateRope);
			ropeMat = (Material) EditorGUILayout.ObjectField("Rope Material : ", ropeMat, typeof(Material), true);
			ropeSize = EditorGUILayout.Slider("Rope Size : ",ropeSize, 0, 1);
			if(EditorGUI.EndChangeCheck())
			{
				GenerateBridge();
			}
		}

		//GENERATOR
		using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
		{
			rotatePylons = EditorGUILayout.Toggle("Rotate Pylon : ", rotatePylons);
			using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				if(GUILayout.Button("Generate Bridge")) GenerateBridge();
				if(GUILayout.Button("Reset Bridge")) ResetBridge();
			}
		}
	}
	#endregion

	#region GENERATOR
	/* BRIDGE */
	bool rotatePylons;
	void GenerateBridge()
	{
		ResetBridge();
		GeneratePylons();
	}

	/* PYLONS */
	List<GameObject> pylonList = new List<GameObject>();
	void ResetPylonList()
	{
		pylonList.Clear();
		Debug.Log("[BridgeConstructor] Pylon list clear, new size : " + pylonList.Count);
	}

	GameObject pylonPrefab;
	void GenerateNewPylon()
	{
		GameObject newPylon;
		if(pylonPrefab != null) newPylon = GameObject.Instantiate(pylonPrefab);
		else newPylon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		newPylon.transform.position = Vector3.zero;
		newPylon.name = "Pylon " + pylonList.Count;
		pylonList.Add(newPylon);
	}
	void AddPylon(GameObject[] newPylon)
	{
		for(int i = newPylon.Length - 1; i >= 0; i--)
		{
			if(!pylonList.Contains(newPylon[i]))
			{
				pylonList.Add(newPylon[i]);
			}
			else Debug.Log("[BridgeConstructor] Pylon already registered : " + newPylon[i]);
		}
	}
	void RemovePylon(GameObject[] newPylon)
	{
		foreach(GameObject pylon in newPylon)
		{
			if(pylonList.Contains(pylon))
			{
				pylonList.Remove(pylon);
			}
		}
	}
	void GeneratePylons()
	{
		if(pylonList.Count >= 2)
		{
			for(int i = 0; i < pylonList.Count; i++)
			{
				if(i == (pylonList.Count - 1)) 
				{
					if(rotatePylons)
					{
						Vector3 lookedPosition = pylonList[i-1].transform.position;
						lookedPosition.y = pylonList[i].transform.position.y;
						pylonList[i].transform.LookAt(lookedPosition);
					} 
				}
				else
				{
					if(rotatePylons)
					{
						Vector3 lookedPosition = pylonList[i+1].transform.position;
						lookedPosition.y = pylonList[i].transform.position.y;
						pylonList[i].transform.LookAt(lookedPosition);
					} 
					GeneratePlank(pylonList[i], pylonList[i+1]);
				}
			}
		}
		else Debug.Log("[BridgeConstructor] There is less than 2 pylons registered. Length = " + pylonList.Count);
	}

	/* DECK & ROPE*/
	bool rotateDeck;
	int deckNumber = 8;
	float gravityForce = 3;
	GameObject deckPrefab;
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

			//DECKS GLOBAL ROTATION
			GameObject deckPivot =  new GameObject();
			deckPivot.name = "Bridge_Pivot";
			deckPivot.transform.position = firstPylon.transform.position;
			deckPivot.transform.LookAt(secondPylon.transform);
			deckPivot.transform.parent = firstPylon.transform;
			deckList.Add(deckPivot);

			//GENERATE DECKS
			for(int i = 0; i < deckNumber; i++)
			{
				GameObject newDeck;
				if(deckPrefab != null) newDeck = GameObject.Instantiate(deckPrefab);
				else newDeck = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newDeck.name = "Bridge_Deck_" + i;
				Vector3 plankPosition = deckPivot.transform.position + deckPivot.transform.forward * (deckDistance * (i + 1));
				float gravityFactor = Mathf.Abs(((deckDistance * (i + 1)) - (bridgeDistance/2)) / (bridgeDistance/2));
				plankPosition.y -= gravityForce * (1 - Mathf.Pow(gravityFactor, 2));
				newDeck.transform.position = plankPosition;
				
				newDeck.transform.parent = deckPivot.transform;
				deckList.Add(newDeck);
				tempDeckList.Add(newDeck);
			}

			//DECKS LOCAL ROTATION
			if(rotateDeck)
			{
				for(int y = 0; y < tempDeckList.Count; y++)
				{
					if(y < (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y + 1].transform);
					else if(y > (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y - 1].transform);
					else tempDeckList[y].transform.forward = deckPivot.transform.forward;
				}
			}

			if(generateRope) GenerateRope(firstPylon, secondPylon, tempDeckList);
			MonoBehaviour.DestroyImmediate(gravityPoint);
		}
		else Debug.Log("[BridgeConstructor] There is less than 1 deckPrefab change it in deckPrefab number field");
	}

	//GENERATE ROPES
	bool generateRope;
	Material ropeMat;
	List<GameObject> ropeList;
	float ropeSize = 0.15f;
	void GenerateRope(GameObject firstPylon, GameObject secondPylon, List<GameObject> tempDeckList)
	{
		for(int i = 0; i < 2; i++)
		{
			GameObject rope = new GameObject("ropeRenderer");
			rope.transform.position = firstPylon.transform.position;
			rope.transform.parent = firstPylon.transform;
			LineRenderer ropeRenderer = rope.AddComponent<LineRenderer>();
			ropeRenderer.sharedMaterial = ropeMat;
			ropeRenderer.positionCount = tempDeckList.Count + 2;
			ropeRenderer.startWidth = ropeSize;
			ropeRenderer.endWidth = ropeSize;
			if(i == 0) 
			{
				ropeRenderer.SetPosition(0, firstPylon.transform.right + firstPylon.transform.position);
				ropeRenderer.SetPosition( tempDeckList.Count + 1, firstPylon.transform.right + secondPylon.transform.position);
			}
			else
			{
				ropeRenderer.SetPosition(0, firstPylon.transform.position - firstPylon.transform.right);
				ropeRenderer.SetPosition( tempDeckList.Count + 1, secondPylon.transform.position - firstPylon.transform.right);
			} 
			for(int j = 0; j < tempDeckList.Count; j++)
			{
				Vector3 vertexPosition;
				if(i == 0) vertexPosition = firstPylon.transform.right + tempDeckList[j].transform.position + Vector3.up;
				else vertexPosition = tempDeckList[j].transform.position - firstPylon.transform.right + Vector3.up;
				ropeRenderer.SetPosition(j+1, vertexPosition);
			}
			ropeList.Add(rope);
		}
	}

	void ResetBridge()
	{
		foreach(GameObject deckPrefab in deckList)
		{
			MonoBehaviour.DestroyImmediate(deckPrefab);
		}
		deckList.Clear();

		foreach(GameObject rope in ropeList)
		{
			MonoBehaviour.DestroyImmediate(rope);
		}
		ropeList.Clear();
	}
	#endregion
}
