using FluentValidation;
using SchoolManagement.DTOs.Request;

namespace SchoolManagement.Validators;

public class MarkAttendanceRequestValidator : AbstractValidator<MarkAttendanceRequest>
{
    public MarkAttendanceRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.Status)
            .InclusiveBetween(0, 3).WithMessage("Invalid attendance status");

        RuleFor(x => x.AttendanceDate)
            .NotEmpty().WithMessage("Attendance date is required");
    }
}
