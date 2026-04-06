namespace SchoolManagement.Helpers;

/// <summary>
/// Utility class for pagination calculations
/// </summary>
public class PaginationHelper
{
    public static int CalculateTotalPages(int totalItems, int pageSize)
    {
        return (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }

    public static bool IsValidPagination(int pageNumber, int pageSize)
    {
        return pageNumber > 0 && pageSize > 0 && pageSize <= 100;
    }
}

/// <summary>
/// Utility class for date-related operations
/// </summary>
public class DateHelper
{
    public static DateTime GetStartOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    public static DateTime GetEndOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
    }

    public static DateTime GetStartOfCurrentSchoolYear()
    {
        var today = DateTime.UtcNow;
        return today.Month >= 4 ? new DateTime(today.Year, 4, 1) : new DateTime(today.Year - 1, 4, 1);
    }

    public static DateTime GetEndOfCurrentSchoolYear()
    {
        var startOfYear = GetStartOfCurrentSchoolYear();
        return startOfYear.AddYears(1).AddDays(-1);
    }

    public static int GetMonthsDifference(DateTime startDate, DateTime endDate)
    {
        return (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);
    }
}

/// <summary>
/// Utility class for validation helpers
/// </summary>
public class ValidationHelper
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            phoneNumber, @"^\d{10}$|^\+\d{1,3}\d{7,12}$");
    }

    public static bool IsAdult(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
        return age >= 18;
    }
}

/// <summary>
/// Utility class for grade calculations
/// </summary>
public class GradeHelper
{
    public static string CalculateGrade(decimal marks, decimal maxMarks)
    {
        var percentage = (marks / maxMarks) * 100;
        return percentage switch
        {
            >= 90 => "A+",
            >= 80 => "A",
            >= 70 => "B",
            >= 60 => "C",
            >= 50 => "D",
            _ => "F"
        };
    }

    public static decimal CalculatePercentage(decimal marks, decimal maxMarks)
    {
        return maxMarks > 0 ? (marks / maxMarks) * 100 : 0;
    }

    public static decimal CalculateGPA(List<decimal> percentages)
    {
        if (!percentages.Any())
            return 0;

        var average = percentages.Average();
        return average / 20; // Convert to 4.0 scale
    }
}
