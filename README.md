# Jump Cloud Software Engineer Assignment
This library provides the ability to track the average time for input actions.  Currently, the two type of actions that are supported are "jump" and "run".

## System Requirements
This library requires the following in order to build and run the library:  
1. Windows 10
2. [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/downloads/)
3. [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
4. Command Prompt or Powershell
5. [Git](https://git-scm.com/downloads) downloaded with an active GitHub account

## Getting Started
1. Open Command Prompt or Powershell and clone the repo from GitHub using the following command:
```
    git clone https://github.com/wpe19d/JC_Assignment.git
```
2. Navigate to the project directory
```
    cd JC_Assignment
```
## Building
To build the project, run:
```
    .\build.bat
```

## Testing
To test the project, run:
```
    .\test.bat
```
## Usage
There are two action methods contained in this library:
1. AddAction(string Input) : string
    - Adds an action of type "jump" or "run" and its respective time to the ActionService
        - Example Input: Json string of action information
        ```json
            {"action":"run","time":500}
        ```
        - Example Output: Empty string if successfull.  Error Message if Invalid.
2. GetStats() : string
    - Example Output: json array of action types and their average times
        ```json
            [
                  {
                    "action": "jump",
                    "avg": 100
                  },
                  {
                    "action": "run",
                    "avg": 375
                  }
                ]
        ```

Example usage of the ActionService library
```csharp
    ActionService actionService = new ActionService();

            var actionOne = "{\"action\":\"run\",\"time\":500}";
            var actionTwo = "{\"action\":\"jump\",\"time\":100}";
            var actionThree = "{\"action\":\"run\",\"time\":250}";

            actionService.AddAction(actionOne);
            actionService.AddAction(actionTwo);
            actionService.AddAction(actionThree);

            var stats = actionService.GetStats();

            /*
                Output of the stats variable: 
                [
                  {
                    "action": "jump",
                    "avg": 100
                  },
                  {
                    "action": "run",
                    "avg": 375
                  }
                ]
             */
```

## Assumptions
1. Case sensitivity of the inputs and outputs does not matter
2. GetStats() may return an empty array in the case that no actions have been added.
3. Time will be within the bounds of an int (-2,147,483,648 to 147,483,648)
