using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandParser
{
	// Filters out all non-command lines
	// Returns a list of 'Command's, parsed from the supplied .txt file
	public static Command[] GetCommands(string[] fileLines)
	{
		fileLines = FilterCommands(fileLines);
		Command[] commands = new Command[fileLines.Length];

		for (int i = 0; i < fileLines.Length; i++)
			commands[i] = GetCommand(fileLines[i]);

		return commands;
	}

	// Assembles a command from a given line
	static Command GetCommand(string line)
	{
		Command command = new Command();

		// If the line doesn't contain a space, then there are no parameters
		if (!line.Contains(" "))
		{
			// Return a title case version of the line
			command.methodName = line.TitleCase();
			return command;
		}

		// Otherwise, seperate the method name from the parameters (denoted by the space)
		string[] splitLines = line.Split(' ');

		command.methodName = splitLines[0].TitleCase();

		// Get all the parameters, seperated by commas
		command.parameters = splitLines[1].Split(',');
		return command;
	}

	// Returns the file, with all blank and commented lines removed.
	static string[] FilterCommands(string[] fileLines)
	{
		List<string> commandLines = new List<string>();

		// Iterates through the file, adding lines containing commands to a new array (ignores comments and whitespace)
		for (int i = 0; i < fileLines.Length; i++)
		{
			if (!fileLines[i].StartsWith("//") && !string.IsNullOrEmpty(fileLines[i]))
				commandLines.Add(fileLines[i]);
		}

		return commandLines.ToArray();
	}

	// String extension to capitalse the first initial of a string, and makes the rest of the chars lower case
	public static string TitleCase(this string s)
	{
		// If the string is null, return the string without modification
		if (string.IsNullOrEmpty(s))
			return s;
		// If the string is only a single character long, return a capitalised char as a string
		if (s.Length == 1)
			return s.ToUpper();

		// Otherwise, set the string to lower and capitalise the first letter
		s = s.ToLower();
		return s.Remove(1).ToUpper() + s.Substring(1);
	}
}

public struct Command
{
	public string methodName;
	public string[] parameters;

	public Command(string methodName, string[] parameters)
	{
		this.methodName = methodName;
		this.parameters = parameters;
	}
}