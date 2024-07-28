using API.Data;
using API.DTOs.CategoryDTOs;
using FluentValidation;
using System.Linq;

namespace API.Validators
{
    public class CategoryValidator : AbstractValidator<CategorySaveDto>
    {
        private readonly DataContext _context;
        private readonly Guid? _categoryId; 

        public CategoryValidator(DataContext context, Guid? categoryId = null)
        {
            _context = context;
            _categoryId = categoryId; // تعيين المعرف الممرر

            RuleFor(c => c.CategoryName)
                .NotEmpty()
                .MaximumLength(100)
                .Must(BeUniqueCategoryName)
                .WithMessage("Category name must be unique.");
        }

        private bool BeUniqueCategoryName(string categoryName)
        {
            if (_categoryId.HasValue) 
            {
                return !_context.Categories
                    .Any(x => x.CategoryName == categoryName && !x.Id.Equals(_categoryId));
            }
            else 
            {
                return !_context.Categories
                    .Any(x => x.CategoryName == categoryName);
            }
        }
    }
}
