namespace ConcordiaLocalServerConsole.Services.Modules.Classes;

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;

using Abstract;
using Classes;
using Exceptions;

public class LocalClientModule : ILocalClientModule
{
    // private readonly IPriorityGateway _prioritiesGway;
    // private readonly IParticipantsGateway _participantsGway;
    // private readonly IExperimentsGateway _experimentsGway;
    // private readonly IRemarksGateway _remarksGway;
    // private readonly IScientistsGateway _scientistsGway;
    // private readonly IStatesRepositoryGateway _statesGway;

    private NetworkStream? _stream;

    public LocalClientModule()
    {
        _stream = null;
    }

    public string Name => "LocalClinetMenu";
    public string Command => "Client";

    private const string GetAllExperiments = "GAE";
    private const string GetExperimentsByState = "GES";
    private const string GetExperimentById = "GEI";
    private const string GetAllRemarksInExperiment = "GRA";
    private const string GetLastRemarkInExperiment = "GRL";
    private const string PostRemarkInExperiment = "PRE";
    private const string PostScientist = "PSC";
    private const string PutStateInExperiment = "PSE";

    public void Start()
    {
        throw new ExitException($"Exit From {Name}.");
    }

    public async Task StartAsync(NetworkStream stream)
    {
        _stream = stream;

        var operations = Options.Operations();
        operations.Add(GetAllExperiments, "Search All Experiments");
        operations.Add(GetExperimentsByState, "Search Experiments By State");
        operations.Add(GetExperimentById, "Search Experiment By Id");
        operations.Add(GetAllRemarksInExperiment, "Search All Remarks In Experiment (by Id)");
        operations.Add(GetLastRemarkInExperiment, "Search Last Remark In Experiment (by Id)");
        operations.Add(PostRemarkInExperiment, "Create Remark In Experiment");
        operations.Add(PostScientist, "Create Scientists In Database");
        operations.Add(PutStateInExperiment, "Modify State in Experiment");

        var buffer = new byte[0];
        var bytesRead = 0;

        while (true)
        {
            var initString = new StringBuilder();
            initString.Append($"################################");
            initString.Append(Environment.NewLine);
            initString.Append($"LocalClientMenu:");
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
            case GetAllExperiments: await SelectAllExperiments(); break;
            case GetExperimentsByState: await SelectExperimentsByState(); break;
            case GetExperimentById: await SelectExperimentById(); break;
            case GetAllRemarksInExperiment: await SelectLastRemarkInExperiment(); break;
            case GetLastRemarkInExperiment: await SelectLastRemarkInExperiment(); break;
            case PostRemarkInExperiment: await InsertRemarkInExperiment(); break;
            case PostScientist: await InsertScientist(); break;
            case PutStateInExperiment: await UpdateStateInExperiment(); break;
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

    private async Task ResponseAsync(string response)
    {
        var responseString = new StringBuilder();
        responseString.Append("Response: ");
        responseString.Append(Environment.NewLine);
        responseString.Append(response);
        responseString.Append(Environment.NewLine);
        byte[] responseBuffer = Encoding.ASCII.GetBytes(responseString.ToString());
        await _stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
    }

    public async Task SelectAllExperiments()
    {
        var response = string.Empty;
        var builder = new StringBuilder();
        try
        {
            // var experiments = _experimentsGway.GetAll();
            var experiments = new string[] { "Id1;Experiment1", "..." };
            foreach (var experiment in experiments)
            {
                builder.Append($"{experiment.ToString()}{Environment.NewLine}");
                // builder.Append($"{experiment.ToShortString()}{Environment.NewLine}");
            }
            // experiments.OrderBy(e => e.DateDue).OrderBy(e => e.Ordering())
        }
        catch (Exception ex)
	    {
            builder.Append($"ERROR = {ex.Message}");
	    }

        response = builder.ToString();
        await ResponseAsync(response);
    }


    public async Task SelectExperimentsByState()
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var states = new string[] { "Id1:Start", "Id2;Working", "Id3;Finish" };
        var statesString = new StringBuilder();
        try
        {
            statesString.Append("States: ");
            statesString.Append(Environment.NewLine);
            foreach (var state in states /* _statesGway.GetAll() */ )
            {
                statesString.Append($"{state.ToString()}{Environment.NewLine}");
            }
            statesString.Append($"Write a State Id or Name.");
            statesString.Append(Environment.NewLine);
        }
        catch (Exception ex)
        {
            statesString.Append($"ERROR = {ex.Message}");
        }
        var stateBuffer = Encoding.ASCII.GetBytes(statesString.ToString());
        await _stream.WriteAsync(stateBuffer, 0, stateBuffer.Length);

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var stateInput = input;
        var response = string.Empty;
        var builder = new StringBuilder();
        try 
	    {
            // IEnumerable<Experiment>? experimentsInState = null;
            IEnumerable<string>? experimentsInState = null;
            if (int.TryParse(stateInput, out var stateId))
            {
                // experimentsInState = _experimentsGway.GetAll().Where(e => e.State.Id == stateId);
                // experimentsInState = _experimentsGway.GetAll().Where(e => e.StateId == stateId);
            }
            else 
	        {
                // experimentsInState = _experimentsGway.GetAll().Where(e => e.State.Name == stateInput);
            }
            // experimentsInState.OrderBy(e => e.DateDue).OrderBy(e => e.Ordering())
            experimentsInState = new string[] 
	        { "Id:1,Name:Ex1,Priority:Priority.Name,Due:DueDate", 
		      "Id:2,Name:Ex2,Priority:Priority.Name,Due:DueDate", 
		      "..." };
            foreach (var experiment in experimentsInState)
            {
                builder.Append($"{experiment.ToString()}{Environment.NewLine}");
                // builder.Append($"{experiment.ToShortString()}{Environment.NewLine}");
            }
        }
        catch (Exception ex)
        {
            builder.Append($"ERROR = {ex.Message}");
        }
        response = builder.ToString();
        await ResponseAsync(response);
    }


    public async Task SelectExperimentById()
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var experimentIdString = new StringBuilder();
        experimentIdString.Append("Write an Experiment Id.");
        experimentIdString.Append(Environment.NewLine);
        var experimentIdBuffer = Encoding.ASCII.GetBytes(experimentIdString.ToString());
        await _stream.WriteAsync(experimentIdBuffer, 0, experimentIdBuffer.Length);

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var experimentInput = input;
        var response = string.Empty;
        var builder = new StringBuilder();
        try
        {
            // Experiment? experiment = null;
            // ...
            // ...
            string? experiment = null;
            string? scientists = null;
            string? remark = null;
            if (int.TryParse(experimentInput, out var experimentId))
            {
                // experiment = _experimentsGway.GetById(experimentId);

                // participants = _participantsGway.GetAll().Where(p => p.ExperimentId == experimentId);
                // scientists = participants.Select(p => p.Scientist.FullName).Aggregate((acc, s) => $"{acc}, {s}").TrimEnd(',');
                // remark = _remarksGway.GetAll().Where(r => r.ExperimentId == experimentId).OrderByDescending(r => r.Date).Last();

                experiment = "Id:1,Data:[...]";
                scientists = "Pippo";
                remark = "Ciao sono pippo";

                // builder.Append(experiment.ToLongString());
                // builder.Append(Environment.NewLine);
                // builder.Append($"LastRemark: {remark.Text}");
                // builder.Append(Environment.NewLine);
                // builder.Append($"Scientists: {scientists}");

                builder.Append(experiment.ToString());
                builder.Append(Environment.NewLine);
                builder.Append(remark.ToString());
                builder.Append(Environment.NewLine);
                builder.Append(scientists.ToString());
            }
            else
            {
                throw new Exception("Input not a number.");
            }
        }
        catch (Exception ex)
        {
            builder.Append($"ERROR = {ex.Message}");
        }
        response = builder.ToString();
        await ResponseAsync(response);
    }


    public async Task SelectAllRemarksInExperiment()
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var idString = new StringBuilder();
        idString.Append("Write an Experiment Id.");
        idString.Append(Environment.NewLine);
        var idBuffer = Encoding.ASCII.GetBytes(idString.ToString());
        await _stream.WriteAsync(idBuffer, 0, idBuffer.Length);

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var experimentInput = input;
        var response = string.Empty;
        var builder = new StringBuilder();
        try
        {
            // Experiment? experiment = null;
            string? experiment = null;
            if (int.TryParse(experimentInput, out var experimentId))
            {
                // var experiment = _experimentsGway.GetById(id);
                // var remarksInExperiment = _remarksGway.GetAll().Where(r => r.ExperimentId == experimentId).OrderByDescending(r => r.Date);
                // var remarksInExperiment = _remarksGway.GetAll().Where(r => r.Experiment.Id == experimentId).OrderByDescending(r => r.Date);
                experiment = "Id:1,Data:[...]";
                var remarksInExperiment = new string[] { "Id1;r1", "Id2;r2", "Id3;r3", };

                builder.Append(experiment.ToString());
                builder.Append(Environment.NewLine);
                builder.Append($"Remarks:");
                builder.Append(Environment.NewLine);
                foreach (var remarkInExperiment in remarksInExperiment)
                {
                    builder.Append($"{remarkInExperiment.ToString()}{Environment.NewLine}");
                }
            }
            else
            {
                throw new Exception("Input not a number.");
            }
        }
        catch (Exception ex)
        {
            builder.Append($"ERROR = {ex.Message}");
        }
        response = builder.ToString();
        await ResponseAsync(response);
    }


    public async Task SelectLastRemarkInExperiment()
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var idString = new StringBuilder();
        idString.Append("Write an Experiment Id.");
        idString.Append(Environment.NewLine);
        var idBuffer = Encoding.ASCII.GetBytes(idString.ToString());
        await _stream.WriteAsync(idBuffer, 0, idBuffer.Length);

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var experimentInput = input;
        var response = string.Empty;
        var builder = new StringBuilder();
        try
        {
            // Experiment? experiment = null;
            string? experiment = null;
            if (int.TryParse(experimentInput, out int experimentId))
            {
                // var experiment = _experimentsGway.GetById(id);
                // var remarksInExperiment = _remarksGway.GetAll().Where(r => r.ExperimentId == experimentId).OrderByDescending(r => r.Date);
                // var remarksInExperiment = _remarksGway.GetAll().Where(r => r.Experiment.Id == experimentId).OrderByDescending(r => r.Date);
                // var remarkLast = remarksInExperiment.Last();
                experiment = "Id:1,Data:[...]";
                var remarksInExperiment = new string[] { "Id1;r1", "Id2;r2", "Id3;r3", };
                var remarkLast = remarksInExperiment.Last();

                builder.Append(experiment.ToString());
                builder.Append(Environment.NewLine);
                builder.Append($"LastRemark:");
                builder.Append(Environment.NewLine);
                builder.Append(remarkLast.ToString());
            }
            else
            {
                throw new Exception("Input not a number.");
            }
        }
        catch (Exception ex)
        {
            builder.Append($"ERROR = {ex.Message}");
        }
        response = builder.ToString();
        await ResponseAsync(response);
    }

    public async Task InsertRemarkInExperiment()
    {
        var buffer = new byte[0];
        var bytesRead = 0;
        var input = string.Empty;
        var inputBuffer = new byte[0];

        var experimentIdString = new StringBuilder();
        experimentIdString.Append("Write an Experiment Id.");
        experimentIdString.Append(Environment.NewLine);
        var experimentIdBuffer = Encoding.ASCII.GetBytes(experimentIdString.ToString());
        await _stream.WriteAsync(experimentIdBuffer, 0, experimentIdBuffer.Length);

        inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var experimentInput = input;


        var scientistIdString = new StringBuilder();
        scientistIdString.Append("Write a Remark Text.");
        scientistIdString.Append(Environment.NewLine);
        var scientistIdBuffer = Encoding.ASCII.GetBytes(scientistIdString.ToString());
        await _stream.WriteAsync(scientistIdBuffer, 0, scientistIdBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        var scientistInput = input;


        var textString = new StringBuilder();
        textString.Append("Write a Remark Text.");
        textString.Append(Environment.NewLine);
        var textBuffer = Encoding.ASCII.GetBytes(textString.ToString());
        await _stream.WriteAsync(textBuffer, 0, textBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        var text = input;


        var response = string.Empty;
        var builder = new StringBuilder();
        try
        {
            var parseExperimentId = int.TryParse(experimentInput, out var experimentId);
            var parseScientistId = int.TryParse(experimentInput, out var scientistId);
            // if (!parseExperimentId && _experimentsGway.GetById(experimentId) is null) throw new Exception("Experiment input not an id.");
            // if (!parseScientistId && _scientistsGway.GetById(scientistId) is null) throw new Exception("Scientist input not an id.");
            // var remark = new Remark(null, null, text, DateTimeOffset.Now, ExperimentId, ScientistId);
            // builder.Append(_remarksGway.Insert(remark).ToString());
            var remark = "Id:null,Code:null,Text:" + text + ",Date:DateTimeOffset.Now,ExperiemntId:" + experimentInput + "ScientistId" + scientistInput;
        }
        catch (Exception ex)
	    {
            builder.Append($"ERROR = {ex.Message}");
        }
        response = builder.ToString();
        await ResponseAsync(response);
    }

    public async Task InsertScientist()
    {
        var buffer = new byte[0];
        var bytesRead = 0;

        var fullNameString = new StringBuilder();
        fullNameString.Append("Write your Scientist FullName.");
        fullNameString.Append(Environment.NewLine);
        var fullNameBuffer = Encoding.ASCII.GetBytes(fullNameString.ToString());
        await _stream.WriteAsync(fullNameBuffer, 0, fullNameBuffer.Length);

        var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var fullName = input;
        var response = string.Empty;
        var builder = new StringBuilder();

        try
        {
            // var scientist = _scientistsGway.Insert(new Scientist(null, fullName));
            var scientist = "Scientist Science Scientific";
            builder.Append(scientist.ToString());
        }
        catch (Exception ex) 
	    {
            builder.Append($"ERROR = {ex.Message}");
        }

        response = builder.ToString();
        await ResponseAsync(response);
    }

    public async Task UpdateStateInExperiment()
    {
        var buffer = new byte[0];
        var bytesRead = 0;
        var input = string.Empty;
        var inputBuffer = new byte[0];

        var experimentIdString = new StringBuilder();
        experimentIdString.Append("Write an Experiment Id.");
        experimentIdString.Append(Environment.NewLine);
        var experimentIdBuffer = Encoding.ASCII.GetBytes(experimentIdString.ToString());
        await _stream.WriteAsync(experimentIdBuffer, 0, experimentIdBuffer.Length);

        inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var experimentId = input;


        var states = new string[] { "Id1:Start", "Id2;Working", "Id3;Finish" };
        // var states = _statesGway.GetAll();
        var statesString = new StringBuilder();
        statesString.Append("States:");
        statesString.Append(Environment.NewLine);
        foreach (var state in states)
        {
            statesString.Append($"{state.ToString()}{Environment.NewLine}");
        }
        statesString.Append($"Write a State Name or Id.");
        statesString.Append(Environment.NewLine);
        var stateBuffer = Encoding.ASCII.GetBytes(statesString.ToString());
        await _stream.WriteAsync(stateBuffer, 0, stateBuffer.Length);

        inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
        await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

        // problem: if the client is stupid and writes too much (more than 4096 bytes) 
        // he is stupid and should not use the server at all... 
        buffer = new byte[4096];
        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        var stateId = input;
        var stateName = input;


        var response = string.Empty;
        var builder = new StringBuilder();

        // try {}
        var experiment = "Id1;Experiment1";
        // var check = int.TryParse(input, out int id);
        // int id = int.Parse(input);
        // var experimentOld = _experimentsGway.GetById(id);
        // var experinentNew = new Experiment(experimentOld.Id, experimentOld.Code, stateId, ...);
        // var experimentUpdate = _experimentsGway.Update(experinentNew).ToString();
        // catch {}

        response = builder.ToString();
        await ResponseAsync(response);
    }
}