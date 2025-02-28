using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Extensions;

public static class FormFileExtension
{
    public static IRuleBuilderOptions<T, IFormFile> BaseFileValidation<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder,
        int maxNameLength = 150,
        params string[] allowedExtensions)
    {
        if (maxNameLength > 255) throw new DomainException("File name cannot be longer than 255 characters.");

        return ruleBuilder
            .Must(x => x.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(x => x.FileName.Length <= maxNameLength)
            .WithMessage($"File name must be less than {maxNameLength} characters.")
            .Must(x => !x.FileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            .WithMessage("File name contains invalid characters.")
            .Must(x => Path.GetFileName(x.FileName) == x.FileName)
            .WithMessage("File name is invalid.")
            .Must(x => allowedExtensions
                .Any(ext => Path
                    .GetExtension(x.FileName)
                    .Equals(ext, StringComparison.CurrentCultureIgnoreCase)
                )
            )
            .WithMessage($"Only {string.Join(", ", allowedExtensions)} are allowed.");
    }
}