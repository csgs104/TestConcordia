﻿namespace ConcordiaTrelloLibrary.Gateways.Classes;

using TrelloDotNet.Model;
using Abstract;
using Abstract.ITrelloGateways;
using Models.Classes;
using Models.Extensions;
using Networks;

public class TrelloScientistGateway : ITrelloScientistGateway
{
	public TrelloScientistGateway()
	{ }

    public Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete()
    {
        throw new NotImplementedException();
    }
}