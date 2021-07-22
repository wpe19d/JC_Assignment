# Jump Cloud Software Engineer Assignment
This library provides the ability to track the average time for input actions.

## System Requirements
The library requires the following in order to build and run the library:  
1. Windows 10
2. [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/downloads/)
3. [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
4. Command Prompt or Powershell
5. [Git](https://git-scm.com/downloads) downloaded

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
There are two methods in this library available via the Action Service:
1. AddAction(string input) : string
    - Adds an action (*jump* or *run*) and its respective time to the ActionService
        - Example Input: Json string of action information
          ```json
          {"action":"run","time":500}
          ```
        - Example Output: If the action is added successfully, an empty string will be returned.  If there is an issue with adding the action, an error message will be returned.
2. GetStats() : string
    - Retreives the average time statistic for each action type
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

## Example
Here is an example of how this library could be utilized in another application.
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
1. Only Jump and Run actions should be supported
2. Case sensitivity of the inputs and outputs does not matter
3. GetStats() may return an empty array in the case that no actions have been added.
4. Time will not be negative.
5. Time will be within the bounds of an int (maximum of 2,147,483,648)
