using API.Models;

namespace API.Lib
{
    public static class CategoryExtensions
    {
        public static List<Guid> GetAllChildIds(this Category category)
        {
            var childIds = new List<Guid>();
            
            if (category.Categries != null && category.Categries.Any())
            {
                childIds.AddRange(category.Categries.Select(p => p.Id));

                foreach (var childProduct in category.Categries)
                {
                    childIds.AddRange(childProduct.GetAllChildIds());
                }
            }

            return childIds;
        }
    }
}
