using System.Diagnostics;

namespace LibRrd.Commands;

public class Command : ICommand
{
    private string _filename;

    private string _args;

    public string? Output { get; private set; }
    
    public string? ErrorOutput { get; private set; }

    public Command(string filename, string args)
    {
        _filename = filename;
        _args = args;
    }
    
    public void Execute()
    {
        var startInfo = new ProcessStartInfo(_filename, _args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        var proc = Process.Start(startInfo);
        Output = proc?.StandardOutput.ReadToEnd();
        ErrorOutput = proc?.StandardError.ReadToEnd();
        proc?.WaitForExit();
    }
}