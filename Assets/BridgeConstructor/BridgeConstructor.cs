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

	void OnGUI()
	{
		using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		{
			if(GUILayout.Button("Add Select Pylon")) AddPylon(Selection.gameObjects);
			if(GUILayout.Button("Reset Pylon List")) ResetPylonList();
		}
		for(int i = 0; i < pylonList.Count; i++)
		{
			GUILayout.Label(i + " - " + pylonList[i].name);
		}

		using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		{
			if(GUILayout.Button("Generate Bridge")) GenerateBridge();
			if(GUILayout.Button("Reset Bridge")) ResetBridge();
		}

		plankNumber = EditorGUILayout.IntField("Number of plank : ", plankNumber);
		gravityForce = EditorGUILayout.Slider("Gravity Force : ", gravityForce, 0, 10);

	}

	/* BRIDGE */
	void GenerateBridge()
	{
		ResetBridge();
		RotatePylons();
	}

	/* PYLONS */
	List<GameObject> pylonList;
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
		else Debug.Log("[BridgeConstructor] There is less than 2 pylon registered. Length = " + pylonList.Count);
	}

	/* PLANK */
	int plankNumber;
	float gravityForce;
	List<GameObject> plankList;
	void GeneratePlank(GameObject firstPylon, GameObject secondPylon)
	{
		if(plankNumber > 0)
		{
			float bridgeDistance = Vector3.Distance(firstPylon.transform.position, secondPylon.transform.position);
			float plankDistance = bridgeDistance / (plankNumber + 1);

			GameObject gravityPoint = new GameObject("Temporary_Gravity_Point");
			Vector3 gravityPointPosition = firstPylon.transform.position + firstPylon.transform.forward * (bridgeDistance/2);
			gravityPointPosition.y -= gravityForce;
			gravityPoint.transform.position = gravityPointPosition;
			gravityPoint.transform.LookAt(secondPylon.transform);

			for(int i = 0; i < plankNumber; i++)
			{
				GameObject newPlank = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newPlank.name = "Bridge_Plank_" + i;
				newPlank.transform.position = firstPylon.transform.position + firstPylon.transform.forward * (plankDistance * (i + 1));
				if(i > plankNumber/2) newPlank.transform.LookAt(gravityPoint.transform);
				else if(i <= plankNumber/2) newPlank.transform.LookAt(secondPylon.transform);
				newPlank.transform.parent = firstPylon.transform;
				plankList.Add(newPlank);
			}

			MonoBehaviour.DestroyImmediate(gravityPoint);
		}
		else Debug.Log("[BridgeConstructor] There is less than 1 plank change it in plank number field");
	}
	void ResetBridge()
	{
		foreach(GameObject plank in plankList)
		{
			MonoBehaviour.DestroyImmediate(plank);
		}
		plankList.Clear();
	}
}
