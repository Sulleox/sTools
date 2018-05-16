	using System;
	using System.IO;
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
							if(pylonList.Count == 0) break;

							if(pylonList[i] != null) pylonList[i] = (GameObject) EditorGUILayout.ObjectField(i + " - ", pylonList[i], typeof(GameObject), true);
							else
							{
								pylonList.Remove(pylonList[i]);
								if(pylonList.Count > 1) GenerateBridge();
							}
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
				deckRotate = EditorGUILayout.Toggle("Activate Deck Rotation : ", deckRotate);
				if(deckRotate) deckLookAt = EditorGUILayout.Toggle("Activate Deck LookAt : ", deckLookAt);
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
				if(generateRope)
				{
					ropeMat = (Material) EditorGUILayout.ObjectField("Rope Material : ", ropeMat, typeof(Material), true);
					ropeSize = EditorGUILayout.Slider("Rope Size : ",ropeSize, 0, 0.5f);
					ropeAttachYPos = EditorGUILayout.Slider("Rope Attach Y Offset : ", ropeAttachYPos, 0, 10);
					ropeYOffest = EditorGUILayout.Slider("Rope Y Offset : ", ropeYOffest, 0, 2);
				}
				if(EditorGUI.EndChangeCheck())
				{
					GenerateBridge();
				}
			}

			//GENERATOR
			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUI.BeginChangeCheck();
				rotatePylons = EditorGUILayout.Toggle("Rotate Pylon : ", rotatePylons);
				if(EditorGUI.EndChangeCheck())
				{
					GenerateBridge();
				}
				using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
				{
					if(GUILayout.Button("Preview Bridge")) GenerateBridge();
					if(GUILayout.Button("Reset Bridge")) ResetBridge();
				}
				using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
				{
					if(GUILayout.Button("Change Prefab folder")) SelectPrefabFolder();
					if(GUILayout.Button("Save Bridge as Prefab")) SaveBridgeAsPrefab();
				}
			}
		}
		#endregion

		#region GENERATOR
		/* BRIDGE */
		void GenerateBridge()
		{
			ResetBridge();
			RotatePylons();
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

		string prefabFolder = string.Empty;
		string prefabDestination = string.Empty;
		void SelectPrefabFolder()
		{
			prefabFolder = EditorUtility.OpenFolderPanel("Select prefab folder", "folder", "myPrefab");
			SaveBridgeAsPrefab();
		}
		
		List<GameObject> bridgePrefabs = new List<GameObject>();
		void SaveBridgeAsPrefab()
		{
			GameObject newPrefab = new GameObject("BridgePrefab_" + bridgePrefabs.Count);
			newPrefab.transform.position = pylonList[0].transform.position;
			foreach(GameObject pylon in pylonList)
			{
				pylon.transform.parent = newPrefab.transform;
			}

			if(prefabDestination == string.Empty || !Directory.Exists(prefabFolder))
				SelectPrefabFolder();
			else
			{
				prefabDestination = prefabFolder.Replace(Application.dataPath, string.Empty);
				PrefabUtility.CreatePrefab("Assets" + prefabDestination + "/" + newPrefab.name + ".prefab", newPrefab);
			}
		}

		/* PYLONS */
		#region PYLON
		bool rotatePylons = true;
		GameObject pylonPrefab;
		List<GameObject> pylonList = new List<GameObject>();
		List<Vector3> lastPylonPositions = new List<Vector3>();
		void ResetPylonList()
		{
			pylonList.Clear();
			lastPylonPositions.Clear();
			EditorApplication.update -= CheckPylonsPosition;
			Debug.Log("[BridgeConstructor] Pylon list clear, new size : " + pylonList.Count);
		}
		void GenerateNewPylon()
		{
			//GAMEOBJECT
			GameObject newPylon;
			if(pylonPrefab != null) newPylon = GameObject.Instantiate(pylonPrefab);
			else newPylon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			//POSITION
			Vector3 spawnPosition;
			if(pylonList.Count > 0) spawnPosition = lastPylonPositions[lastPylonPositions.Count-1] + Vector3.right * 20f;
			else spawnPosition = Vector3.zero;
			newPylon.transform.position = spawnPosition;

			newPylon.name = "Pylon " + pylonList.Count;
			pylonList.Add(newPylon);
			lastPylonPositions.Add(newPylon.transform.position);

			if(pylonList.Count > 1) GenerateBridge();
			EditorApplication.update += CheckPylonsPosition;
		}
		void AddPylon(GameObject[] newPylon)
		{
			for(int i = newPylon.Length - 1; i >= 0; i--)
			{
				if(!pylonList.Contains(newPylon[i]))
				{
					pylonList.Add(newPylon[i]);
					lastPylonPositions.Add(newPylon[i].transform.position);
					EditorApplication.update += CheckPylonsPosition;
				}
				else Debug.Log("[BridgeConstructor] Pylon already registered : " + newPylon[i]);
			}

			if(pylonList.Count > 1) GenerateBridge();
		}
		void RemovePylon(GameObject[] newPylon)
		{
			foreach(GameObject pylon in newPylon)
			{
				if(pylonList.Contains(pylon))
				{
					pylonList.Remove(pylon);
					lastPylonPositions.Remove(pylon.transform.position);
				}
			}
		}
		void RotatePylons()
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

		void CheckPylonsPosition()
		{
			if(pylonList.Count > 0 && lastPylonPositions.Count > 0)
			{
				for(int i = 0; i < pylonList.Count; i++)
				{
					if(pylonList[i] == null)
					{
						MonoBehaviour.DestroyImmediate(pylonList[i]);
						pylonList.RemoveAt(i);
						lastPylonPositions.RemoveAt(i);
						GenerateBridge();
						Repaint();
					}
					else
					{
						if(pylonList[i].transform.position != lastPylonPositions[i])
						{
							lastPylonPositions[i] = pylonList[i].transform.position;
							GenerateBridge();
						}
					}
				}
			}
		}
		
		#endregion

		/* DECK */
		#region DECK
		bool deckRotate = true;
		bool deckLookAt;
		int deckNumber = 8;
		float gravityForce = 3;
		GameObject deckPrefab;
		List<GameObject> deckList;
		void GeneratePlank(GameObject firstPylon, GameObject secondPylon)
		{
			if(deckNumber > 0)
			{
				List<GameObject> tempDeckList = new List<GameObject>(0);

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

					Vector3 deckPosition = deckPivot.transform.position + deckPivot.transform.forward * (deckDistance * (i + 1));
					float gravityFactor = Mathf.Abs(((deckDistance * (i + 1)) - (bridgeDistance/2)) / (bridgeDistance/2));
					deckPosition.y -= gravityForce * (1 - Mathf.Pow(gravityFactor, 2));
					newDeck.transform.position = deckPosition;
					
					newDeck.transform.parent = deckPivot.transform;
					deckList.Add(newDeck);
					tempDeckList.Add(newDeck);
				}

				//DECKS LOCAL ROTATION
				if(deckRotate)
				{
					for(int y = 0; y < tempDeckList.Count; y++)
					{
						if(deckLookAt)
						{
							if(y < (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y + 1].transform);
							else if(y > (tempDeckList.Count / 2)) tempDeckList[y].transform.LookAt(tempDeckList[y - 1].transform);
							else tempDeckList[y].transform.forward = deckPivot.transform.forward;
						}
						else tempDeckList[y].transform.forward = firstPylon.transform.forward;
					}
				}

				if(generateRope) GenerateRope(firstPylon, secondPylon, tempDeckList);
				MonoBehaviour.DestroyImmediate(gravityPoint);
			}
			else Debug.Log("[BridgeConstructor] There is less than 1 deckPrefab change it in deckPrefab number field");
		}
		#endregion

		/* ROPES */
		#region ROPES
		bool generateRope = true;
		Material ropeMat;
		List<GameObject> ropeList;
		float ropeSize = 0.15f;
		float ropeAttachYPos = 1f;
		float ropeYOffest = 0.5f;
		void GenerateRope(GameObject firstPylon, GameObject secondPylon, List<GameObject> tempDeckList)
		{
			//DISTANCE
			float bridgeDistance = Vector3.Distance(firstPylon.transform.position, secondPylon.transform.position);
			float ropeDistance = bridgeDistance / (deckNumber + 1);

			for(int i = 0; i < 2; i++)
			{
				//PARENT GAMEOBJECT
				GameObject rope = new GameObject("ropeRenderer");
				rope.transform.position = firstPylon.transform.position;
				Vector3 localRight = firstPylon.transform.right;

				//RENDERER
				LineRenderer ropeRenderer = rope.AddComponent<LineRenderer>();
				ropeRenderer.sharedMaterial = ropeMat;
				ropeRenderer.positionCount = tempDeckList.Count + 2;
				ropeRenderer.startWidth = ropeSize;
				ropeRenderer.endWidth = ropeSize;

				if(i == 0) 
				{
					ropeRenderer.SetPosition(0, firstPylon.transform.position + localRight + (Vector3.up * ropeAttachYPos) + (Vector3.up * ropeYOffest));
					ropeRenderer.SetPosition(tempDeckList.Count + 1, secondPylon.transform.position + localRight + (Vector3.up * ropeAttachYPos) + (Vector3.up * ropeYOffest));
				}
				else
				{
					ropeRenderer.SetPosition(0, firstPylon.transform.position - localRight + (Vector3.up * ropeAttachYPos) + (Vector3.up * ropeYOffest));
					ropeRenderer.SetPosition(tempDeckList.Count + 1, secondPylon.transform.position - localRight + (Vector3.up* ropeAttachYPos) + (Vector3.up * ropeYOffest));
				} 

				for(int j = 0; j < tempDeckList.Count; j++)
				{
					Vector3 vertexPosition = Vector3.zero;
					float gravityFactor = Mathf.Abs(((ropeDistance * (j + 1)) - (bridgeDistance/2)) / (bridgeDistance/2));
					if(i == 0) 
					{
						vertexPosition = tempDeckList[j].transform.position + localRight  + (Vector3.up * Mathf.Pow(gravityFactor, 2)* ropeAttachYPos) + (Vector3.up * ropeYOffest);
					}
					else 
					{
						vertexPosition = tempDeckList[j].transform.position - localRight + (Vector3.up * Mathf.Pow(gravityFactor, 2) * ropeAttachYPos)+ (Vector3.up * ropeYOffest);
					}
					ropeRenderer.SetPosition(j+1, vertexPosition);
				}
				
				rope.transform.parent = firstPylon.transform;
				//ropeRenderer.useWorldSpace = false;
				ropeList.Add(rope);
			}
		}
		#endregion

		#endregion
	}
