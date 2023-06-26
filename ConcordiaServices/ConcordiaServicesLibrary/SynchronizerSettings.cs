namespace ConcordiaServicesLibrary;

using ConcordiaDBLibrary;
using ConcordiaTrelloLibrary;

public static class SynchronizerSettings
{
    private static int SynchronizationTime = 0;

    public static void SynchronizePrioritiesNames(List<string> names)
    {
        DBSettings.SetPrioritiesNames(names);
        TrelloSettings.SetTrelloPrioritiesNames(names);
    }

    public static void SynchronizeStatesNames(List<string> names)
    {
        DBSettings.SetStatesNames(names);
        TrelloSettings.SetTrelloStatesNames(names);
    }

    public static int GetSynchronizationTime()
    {
        return SynchronizationTime;
    }

    public static void SetSynchronizationTime(int time)
    {
        SynchronizationTime = time;
    }

    public static TimeSpan SynchronizationTimeDelay()
    {
        DateTimeOffset currentTime = DateTimeOffset.Now.AddMinutes(4);
        // Console.WriteLine($"Current: {currentTime}.");
        DateTimeOffset restartTime = currentTime.Date.AddDays(1);
        // Console.WriteLine($"Restart: {restartTime}.");
        int currentMinutes = currentTime.TimeOfDay.Minutes;
        // Console.WriteLine($"CurrentMinutes: {currentMinutes}.");
        var currentHours = currentTime.TimeOfDay.Hours;
        // Console.WriteLine($"currentHours: {currentHours}.");
        for (int minutes = 0; minutes < 1440; minutes += SynchronizationTime)
        {
            if (minutes > (currentHours * 60 + currentMinutes))
            {
                restartTime = currentTime.Date.AddMinutes(minutes);
                // Console.WriteLine($"minutes: {minutes}.");
                break;
            }
        }
        // Console.WriteLine($"Now: {DateTimeOffset.Now}.");
        // Console.WriteLine($"Restart: {restartTime}.");
        var delay = restartTime - DateTimeOffset.Now;
        return delay;
    }
}