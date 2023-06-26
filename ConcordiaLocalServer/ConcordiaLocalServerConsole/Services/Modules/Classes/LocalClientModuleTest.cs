namespace ConcordiaLocalServerConsole.Services.Modules.Classes;

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;

using Abstract;
using Classes;
using Exceptions;

public class LocalClientTestModule : ILocalClientTestModule
{
    private NetworkStream? _stream;

    public LocalClientTestModule()
    {
        _stream = null;
    }

    public string Name => "LocalClinetTestMenu";
    public string Command => "Test";

    private const string StringToUpper = "STU";
    private const string StringToLower = "STL";
    private const string StringRepeat = "SRP";

    public void Start()
    {
        throw new ExitException($"Exit From {Name}.");
    }

    public async Task StartAsync(NetworkStream stream)
    {
        _stream = stream;

        var operations = Options.Operations();
        operations.Add(StringToUpper, "Returns the string upperized.");
        operations.Add(StringToLower, "Returns the string lowerized.");
        operations.Add(StringRepeat, "Returns the string repeater.");

        var buffer = new byte[0];
        var bytesRead = 0;

        while (true)
        {
            var initString = new StringBuilder();
            initString.Append($"################################");
            initString.Append(Environment.NewLine);
            initString.Append($"LocalClientTestMenu:");
            initString.Append(Environment.NewLine);
            foreach (var operation in operations)
            {
                initString.Append($"[{operation.Key}]:\t{operation.Value}{Environment.NewLine}");
            }
            var menuBuffer = Encoding.ASCII.GetBytes(initString.ToString());
            await _stream.WriteAsync(menuBuffer, 0, menuBuffer.Length);

            var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
            await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

            // problem: if the client is stupid and writes too much (more than 4096 bytes) 
            // he is stupid and should not use the server at all... 
            buffer = new byte[4096];
            bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            try
            {
                await ManageInput(input.Trim());
            }
            catch (ExitException e)
            {
                var exitString = new StringBuilder();
                exitString.Append(e.Message);
                exitString.Append(Environment.NewLine);
                exitString.Append("Returning to MENU.");
                exitString.Append(Environment.NewLine);
                exitString.Append($"################################");
                exitString.Append(Environment.NewLine);
                var runBuffer = Encoding.ASCII.GetBytes(exitString.ToString());
                await _stream.WriteAsync(runBuffer, 0, runBuffer.Length);

                break;
            }
        }
    }

    private async Task ManageInput(string input)
    {
        switch (input)
        {
            case StringToUpper: await StringUpperizerAsync(); break;
            case StringToLower: await StringLowerizerAsync(); break;
            case StringRepeat: await StringRepeaterAsync(); break;
            case Options.EXIT: throw new ExitException($"Exit From {Name}.");
            default: await InvalidInput(input); break;
        }
    }

    private async Task InvalidInput(string input)
    {
        var invalidString = new StringBuilder();
        invalidString.Append($"Input Not Valid: {input}.");
        invalidString.Append(Environment.NewLine);
        var invalidBuffer = Encoding.ASCII.GetBytes(invalidString.ToString());
        await _stream.WriteAsync(invalidBuffer, 0, invalidBuffer.Length);
    }

    public async Task StringUpperizerAsync() => await StringProcesserAsync(Upperizer);
    public async Task StringLowerizerAsync() => await StringProcesserAsync(Lowerizer);
    public async Task StringRepeaterAsync() => await StringProcesserAsync(Repeater);
    private static string Upperizer(string str) => str.ToUpper();
    private static string Lowerizer(string str) => str.ToLower();
    private static string Repeater(string str) => str;

    private async Task StringProcesserAsync(Func<string, string> function)
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        await StringModifierAsync(request, function);
    }

    private async Task StringModifierAsync(string request, Func<string, string> function)
    {
        var responseString = new StringBuilder();
        responseString.Append("Response: ");
        responseString.Append(function(request));
        responseString.Append(Environment.NewLine);

        byte[] responseBuffer = Encoding.ASCII.GetBytes(responseString.ToString());
        await _stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
    }
}