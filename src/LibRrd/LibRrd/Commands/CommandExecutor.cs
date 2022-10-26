using LibRrd.Commands.Configurators;
using LibRrd.Exceptions;

namespace LibRrd.Commands;

public class CommandExecutor
{
    public Command ExecuteCommand(string filename, ICommandConfigurator configurator)
    {
        var command = new Command(filename, configurator.Configure());
        command.Execute();
        if (command.ErrorOutput != string.Empty) throw new RrdException(command.ErrorOutput);
        return command;
    } 
}