namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class ExperimentExtension
{
    public static string ToShortString(this Experiment experiment)
    {
        return $"{nameof(Experiment.Id)}:{experiment.Id},Data:{experiment.State?.Name},{experiment.Priority?.Name},{experiment.DueDate.ToString()}";
    }

    public static string ToLongString(this Experiment experiment)
    {
        throw new NotImplementedException();
    }

    public static int OrderingByPriority(this Experiment experiment)
    {
        if (experiment.Priority is not null)
        {
            if (experiment.Priority.Name.Equals(DBSettings.GetPrioritiesNames()[0], StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            else if (experiment.DueDate < DateTimeOffset.Now.AddDays(5))
            {
                return 1;
            }
            else if (experiment.Priority.Name.Equals(DBSettings.GetPrioritiesNames()[1], StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            else if (experiment.Priority.Name.Equals(DBSettings.GetPrioritiesNames()[2], StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            return 5;
        }
    }

    public static int Ordering(this Experiment experiment)
    {
        if (experiment.State is not null)
        {
            if (!experiment.State.Name.Equals(DBSettings.GetStatesNames()[2], StringComparison.OrdinalIgnoreCase))
            {
                return experiment.OrderingByPriority();
            }
            else
            {
                return 4;
            }
        }
        else
        {
            return 5;
        }
    }
}