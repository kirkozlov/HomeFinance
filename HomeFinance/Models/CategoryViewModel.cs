using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Models
{
    public class CategoryViewModelBase
    {
        public int? Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Domain.Localization.Common))]
        public string Name { get; set; }

        public bool Outgo { get; set; }

        public CategoryViewModelBase(CategoryDto category)
        {
            Id = category.Id;
            Name = category.Name;
            Outgo = category.Outgo;
        }


        // Needed for view
        public CategoryViewModelBase()
        {
           
        }
    }

    public class AddEditCategoryViewModel: CategoryViewModelBase
    {

        public int? ParentId { get; set; }

        public IEnumerable<CategoryViewModelBase>? PossibleParents { get; set; }

        public string? Comment { get; set; } = null;

        public AddEditCategoryViewModel(CategoryDto category):base(category)
        {
            ParentId = category.ParentId;
            Comment = category.Comment;
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
