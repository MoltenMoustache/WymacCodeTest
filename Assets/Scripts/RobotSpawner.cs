using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] Text counter;
    int robotsSpawned = 0;

	private void Awake()
	{
        counter.text = robotsSpawned.ToString();
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyUp(KeyCode.Return))
		{
            // Spawns a robot at 0,0,0 and increments the counter
            Instantiate(robotPrefab, Vector3.zero, Quaternion.identity);
            robotsSpawned++;
            counter.text = robotsSpawned.ToString();
        } 
    }
}
