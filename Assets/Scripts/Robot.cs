using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Robot : MonoBehaviour
{
	// Could remove the const keyword and have the robot dynamically grab a path, or simply serialize it to add it in the inspector
	// The path is stored on the robot itself so that each robot can have its own set of commands
	const string commandPath = "Assets/Data/RobotCommands.txt";

	Command[] commands;
	int commandIndex = 0;

	bool isMoving = false;
	bool isRotating = false;

	// Start is called before the first frame update
	void Start()
	{
		GetCommandsFromFile();

		StartExecution();

	}

	// Reads all lines from a text file using the commandPath, run the lines through the CommandParser to get all the commands
	void GetCommandsFromFile()
	{
		string[] file = System.IO.File.ReadAllLines(commandPath);
		commands = CommandParser.GetCommands(file);
	}

	// Initiates the command loop
	void StartExecution()
	{
		if (commands.Length == 0)
		{
			Debug.LogError("No commands to execute, exiting execution.");
			return;
		}

		// Purposefully executes the 'Start' command
		commandIndex = 0;
		StartCoroutine(Start(commands[commandIndex].parameters));
	}

	// Increments the commandIndex and starts the next command
	void ExecuteNextCommand()
	{
		// If the previously executed command was the last command in the array, stop execution
		if (commandIndex == commands.Length - 1)
			return;

		commandIndex++;

		if (CheckValidCommand(commands[commandIndex].methodName))
			StartCoroutine(commands[commandIndex].methodName, commands[commandIndex].parameters);
		else
		{
			Debug.LogError("Command not recognised, exiting execution");
			return;
		}
	}

	// Checks if command is valid and returns the result
	bool CheckValidCommand(string command)
	{
		// Gets the coroutine by the command name
		MethodInfo method = GetType().GetMethod(command, BindingFlags.NonPublic | BindingFlags.Instance);

		// Returns true if the method exists and has a return type of IEnumerator
		return method != null && method.ReturnType == typeof(IEnumerator);
	}

	#region Commands

	// Sets the Robot immediately to world co-ordinates X,Y,Z
	IEnumerator Start(string[] paras)
	{
		// Parses the x, y, z positions from the first three parameters
		float x, y, z;
		if (float.TryParse(paras[0], out x) &&
			float.TryParse(paras[1], out y) &&
			float.TryParse(paras[2], out z))
		{
			transform.position = new Vector3(x, y, z);
		}
		else
		{
			Debug.LogError("Could not parse paras to a float during Start coroutine");
			ExecuteNextCommand();
			yield return null;
		}

		ExecuteNextCommand();
		yield return null;
	}

	// Waits X seconds before executing the next command
	IEnumerator Wait(string[] paras)
	{
		// Parses the wait duration from the first parameter
		if (!float.TryParse(paras[0], out float time))
		{
			Debug.LogError("Could not parse paras[0] to a float during Wait coroutine");
			ExecuteNextCommand();
			yield return null;
		}

		yield return new WaitForSeconds(time);

		ExecuteNextCommand();
	}

	// Rotate the robot around the Y axis D degrees at DS degrees per second.
	// The rotation is relative from the robot's current rotation.
	// Zero DS means to rotate immediatley.
	IEnumerator Rotate(string[] paras)
	{
		// Parses the target rotation from the first parameter
		if (!float.TryParse(paras[0], out float target))
		{
			Debug.LogError("Could not parse paras[0] to a float during Rotate coroutine");
			ExecuteNextCommand();
			yield return null;
		}

		// Parses the rotation speed (degrees per second) from the second parameter
		if (!float.TryParse(paras[1], out float speed))
		{
			Debug.LogError("Could not parse paras[1] to a float during Rotate coroutine");
			ExecuteNextCommand();
			yield return null;
		}


		Quaternion fromRot = transform.rotation;
		Quaternion targetRot = Quaternion.Euler(0, transform.eulerAngles.y + target, 0);
		float time = 0;
		float duration = Quaternion.Angle(fromRot, targetRot) / speed;

		ExecuteNextCommand();

		isRotating = true;
		while (isRotating)
		{
			time += Time.deltaTime;
			transform.rotation = Quaternion.Lerp(fromRot, targetRot, time / duration);

			if (fromRot.y == targetRot.y)
			{
				isRotating = false;
				yield return null;
			}

			yield return 0;
		}
	}

	// Set the robot moving in the robot's current forward direction at V units per second.
	IEnumerator Move(string[] paras)
	{
		// Parses the movement speed (units per second) from the first parameter
		if (!float.TryParse(paras[0], out float distancePerSecond))
		{
			Debug.LogError("Could not parse paras[0] to a float during Move coroutine");
			ExecuteNextCommand();
			yield return null;
		}

		isMoving = true;
		ExecuteNextCommand();
		while (isMoving)
		{
			transform.Translate(Vector3.forward * distancePerSecond * Time.deltaTime, Space.Self);
			yield return 0;
		}

		yield return null;
	}

	// Stop Moving and or Rotating the robot.
	IEnumerator Stop()
	{
		isMoving = false;
		isRotating = false;

		ExecuteNextCommand();
		yield return null;
	}

	// Destroy and remove the robot from the scene.
	IEnumerator Destroy()
	{
		DestroyImmediate(gameObject);
		yield return null;
	}
	#endregion Commands
}
