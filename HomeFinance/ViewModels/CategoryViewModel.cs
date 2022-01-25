using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeFinance.ViewModels
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Domain.Localization.Common))]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public bool Outgo { get; set; }

        public string? Comment { get; set; } = null;

        public CategoryViewModel(CategoryDto category)
        {
            Id = category.Id;
            Name = category.Name;
            Outgo = category.Outgo;
            ParentId = category.ParentId;
            Comment = category.Comment;
        }


        // Needed for view
        public CategoryViewModel()
        {
           
        }
    }

    public class AddEditCategoryViewModel: CategoryViewModel
    {
        public IEnumerable<CategoryViewModel>? PossibleParents { get; set; }


        public AddEditCategoryViewModel(CategoryDto category):base(category)
        {
        }

        public CategoryDto ToDto()
        {
            return new CategoryDto(Id, Name, Outgo, ParentId, Comment) ;
        }

        // Needed For View
        public AddEditCategoryViewModel():base()
        {

        }
    }
}
