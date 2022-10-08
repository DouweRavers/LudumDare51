using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Enumerations
public enum CommandType
{
    SET, CLEAR,
    ADD, SUB, MUL, DIV,
    SKIP, ISGREATER, ISSMALLER, ISNULL,
    JUMP,
    ERROR
}
#endregion

#region Structs
public struct Command
{
    public CommandType Type;
    public int FirstMemory;
    public int SecondMemory;
    public int ThirdMemory;
    public int Value;
    public Command(CommandType type, int first = -1, int second = -1, int third = -1, int value = -1)
    {
        Type = type;
        FirstMemory = first;
        SecondMemory = second;
        ThirdMemory = third;
        Value = value;

    }
}
#endregion

#region Behavior
public class CodeManager : MonoBehaviour
{
    public static CodeManager Instance { get; private set; }
    public string SourceCode { get; set; } = "";
    public int Line = 0;
    public int[] Memory { get; private set; } = new int[16];
    public Command[] Commands = new Command[0];
    public int ErrorLine = -1;


    readonly char[] _whiteSpace = new char[] { ' ' };

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public bool RunCommand()
    {
        if (Line < 0 || Commands.Length <= Line) return false;
        var command = Commands[Line];
        Line++;

        int value = command.Value;
        switch (command.Type)
        {
            case CommandType.SET:
                if (0 <= command.FirstMemory) value = Memory[command.FirstMemory];
                Memory[command.SecondMemory] = value;
                break;
            case CommandType.CLEAR:
                Memory[command.FirstMemory] = 0;
                break;
            case CommandType.ADD:
                Memory[command.ThirdMemory] = Memory[command.FirstMemory] + Memory[command.SecondMemory];
                break;
            case CommandType.SUB:
                Memory[command.ThirdMemory] = Memory[command.FirstMemory] - Memory[command.SecondMemory];
                break;
            case CommandType.MUL:
                Memory[command.ThirdMemory] = Memory[command.FirstMemory] * Memory[command.SecondMemory];
                break;
            case CommandType.DIV:
                if (Memory[command.SecondMemory] == 0) Debug.Log("0 division: infinite not supported");
                Memory[command.ThirdMemory] = Memory[command.FirstMemory] / Memory[command.SecondMemory];
                break;
            case CommandType.SKIP:
                Line++;
                break;
            case CommandType.ISGREATER:
                if (Memory[command.FirstMemory] > Memory[command.SecondMemory]) Line++;
                break;
            case CommandType.ISSMALLER:
                if (Memory[command.FirstMemory] < Memory[command.SecondMemory]) Line++;
                break;
            case CommandType.ISNULL:
                if (Memory[command.FirstMemory] == 0) Line++;
                break;
            case CommandType.JUMP:
                if (0 <= command.FirstMemory) value = Memory[command.FirstMemory];
                Line = value;
                break;
            default:
            case CommandType.ERROR:
                ErrorLine = Line;
                LevelManager.Instance.Stop();
                break;
        }
        return Line < Commands.Length;
    }

    public void CompileCode()
    {
        List<Command> commandsList = new List<Command>();
        string[] lines = SourceCode.Split('\n');
        foreach (var line in lines)
        {
            if (line.Length == 0) continue;
            string[] parsedLine = line.TrimStart().TrimEnd().Split(_whiteSpace).Where(s => 0 != s.Length).ToArray();
            string commandString = parsedLine[0].ToUpper();
            string[] parameterString = parsedLine.Skip(1).ToArray();
            commandsList.Add(CreateCommand(commandString, parameterString));
        }

        ErrorLine = commandsList.FindIndex(c => c.Type == CommandType.ERROR);
        Commands = commandsList.ToArray();
    }

    public void ResetMemory()
    {
        Memory = new int[16];
        Line = 0;
    }

    Command CreateCommand(string commandString, string[] parameterString)
    {
        int first = -1, second = -1, third = -1, value = 0;
        if (0 < parameterString.Length)
        {
            if (parameterString[0].StartsWith('#') && int.TryParse(parameterString[0].TrimStart('#'), out first)) ;
            else if (int.TryParse(parameterString[0], out value)) ;
            else { return new Command(CommandType.ERROR); }
        }
        if (1 < parameterString.Length)
        {
            if (parameterString[1].StartsWith('#') && int.TryParse(parameterString[1].TrimStart('#'), out second)) ;
            else if (int.TryParse(parameterString[1], out value)) ;
            else { return new Command(CommandType.ERROR); }
        }
        if (2 < parameterString.Length)
        {
            if (parameterString[2].StartsWith('#') && int.TryParse(parameterString[2].TrimStart('#'), out third)) ;
            else if (int.TryParse(parameterString[2], out value)) ;
            else { return new Command(CommandType.ERROR); }
        }
        if (3 < parameterString.Length)
        {
            if (!int.TryParse(parameterString[3], out value)) { return new Command(CommandType.ERROR); }
        }

        Command command = new Command(CommandType.ERROR);
        switch (commandString.ToUpper())
        {
            case "SET":
                command = new Command(CommandType.SET, first, second, value: value);
                break;
            case "CLEAR":
                if (first == -1) break;
                command = new Command(CommandType.CLEAR, first);
                break;
            case "ADD":
                if (first == -1 || second == -1 || third == -1) break;
                command = new Command(CommandType.ADD, first, second, third);
                break;
            case "SUB":
                if (first == -1 || second == -1 || third == -1) break;
                command = new Command(CommandType.SUB, first, second, third);
                break;
            case "MUL":
                if (first == -1 || second == -1 || third == -1) break;
                command = new Command(CommandType.MUL, first, second, third);
                break;
            case "DIV":
                if (first == -1 || second == -1 || third == -1) break;
                command = new Command(CommandType.DIV, first, second, third);
                break;
            case "SKIP":
                command = new Command(CommandType.SKIP);
                break;
            case "ISGREATER":
                if (first == -1 || second == -1) break;
                command = new Command(CommandType.ISGREATER, first, second);
                break;
            case "ISSMALLER":
                if (first == -1 || second == -1) break;
                command = new Command(CommandType.ISSMALLER, first, second);
                break;
            case "ISNULL":
                if (first == -1) break;
                command = new Command(CommandType.ISNULL, first);
                break;
            case "JUMP":
                command = new Command(CommandType.JUMP, first, value: value);
                break;
            default:
                break;
        }
        return command;
    }

}
#endregion