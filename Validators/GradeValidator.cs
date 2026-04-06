using FluentValidation;
using SchoolManagement.DTOs.Request;

namespace SchoolManagement.Validators;

public class RecordGradeRequestValidator : AbstractValidator<RecordGradeRequest>
{
    public RecordGradeRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(100).WithMessage("Subject cannot exceed 100 characters");

        RuleFor(x => x.Marks)
            .GreaterThanOrEqualTo(0).WithMessage("Marks cannot be negative");

        RuleFor(x => x.MaxMarks)
            .GreaterThan(0).WithMessage("Max marks must be greater than 0");

        RuleFor(x => x.Marks)
            .LessThanOrEqualTo(x => x.MaxMarks).WithMessage("Marks cannot exceed max marks");

        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Term is required")
            .MaximumLength(20).WithMessage("Term cannot exceed 20 characters");
    }
}
