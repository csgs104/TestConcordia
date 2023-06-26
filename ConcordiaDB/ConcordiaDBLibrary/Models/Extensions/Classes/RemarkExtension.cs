namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class RemarkExtension
{
    public static string ToShortString(this Remark remark)
    {
        return $"{nameof(Remark.Id)}:{remark.Id},{nameof(Remark.Text)}:{remark.Text}";
    }

    public static string ToLongString(this Remark remark)
    {
        throw new NotImplementedException();
    }
}

