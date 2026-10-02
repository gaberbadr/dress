using Domain.Entities;
using Zero.Core.Specification;

namespace Domain.Specifications.Categories
{
    public class CategoryWithChildrenSpecification : BaseSpecifications<Category, int>
    {
        public CategoryWithChildrenSpecification(int? parentId = null)
        {
            IsAsNoTracking = true;
            Criteria = c => c.ParentCategoryId == parentId;
            AddInclude(c => c.Children);
        }
    }
}
