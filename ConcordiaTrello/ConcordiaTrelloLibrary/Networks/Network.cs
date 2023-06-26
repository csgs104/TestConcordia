namespace ConcordiaTrelloLibrary.Networks;

using TrelloDotNet;

public class TrelloNetwork : TrelloClient
{
	public TrelloNetwork(string key, string token) 
	: base(key, token)
	{ }
}