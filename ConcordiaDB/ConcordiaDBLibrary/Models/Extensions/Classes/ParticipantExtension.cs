namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class ParticipantExtension
{
    public static string ToShortString(this Participant participant)
    {
        return $"{nameof(Participant.ExperimentId)}:{participant.ExperimentId},{nameof(Participant.Scientist)}:{participant.Scientist?.FullName}";
    }

    public static string ToLongString(this Participant participant)
    {
        throw new NotImplementedException();
    }
}

