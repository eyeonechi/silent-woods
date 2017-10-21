using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public static bool[] keys = new bool[5];
	
	void Start()
	{
		keys[0] = true;
		keys[3] = true;
	}
	
}
