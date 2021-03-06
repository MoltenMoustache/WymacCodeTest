# Josh Moten's Programming test for Wymac Gaming

Hey there!
Thank you for allowing me to take part in the next stage of the hiring process, the programming test was actually a lot of fun!
I thought I'd leave some information here about my design choices and why I made them.

## Classes

### CommandParser.cs
For parsing the commands from RobotCommands.txt to valid commands, I created a static class with a variety of string manipulation methods.

**Order of operation:**
- Robot calls GetCommands() passing in the full text file in the form of a string array
- All comments and whitespace is filtered out through the 'FilterCommands()' method
- Each command is then grabbed through the 'GetCommand()' method
- GetCommand splits the string by space, which results in the command name and the parameters as two seperate elements.
- The command name is formatted to maintain the coding convention of PascalCase method names.
- From there, the parameter element is split into a string[] using string.Split(','), with each element in the array being a seperate parameter.
- Array of commands are finally returned to the Robot

### Robot.cs
Each robot reads the text file and send it to the CommandParser for parsing. I've decided to have each robot read their own file so that in the future, each robot could be given their own individual Commands.txt file.

Once the robot has received their commands, they execute the Start() coroutine and get going.

Each 'Command' coroutine takes a string array as a parameter, this is so each command can parse the parameters into whatever it needs. Meaning methods like 'Rotate' can parse the parameter into a float, but future methods could keep them as a string if need be.

The use of Coroutines was partly due to the fact that Coroutines can be called from a string, however it also means that each command can fully house their execution from within their own method, without having to have a hook into Update().

### RobotSpawner.cs
The robot spawner simply spawns a robot at 0,0,0 when Enter is pressed, it also increments the UI counter.
In retrospect the UI counter could have been contained within its own class, the Robot Spawner referencing it and calling an Increment() method when a robot is spawned.

## Final Thoughts
Again, I actually quite enjoyed the programming test. I believe the code is quite extensible, clean and well documented. I initially was a bit concerned with the 3 hour estimated time, but ended up completing it in around 2 hours.

Thank you for giving me the opportunity to move onto the next stage of the hiring process, and I hope to hear back from you soon!