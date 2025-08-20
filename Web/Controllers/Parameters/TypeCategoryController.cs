using Business.Interfases;
using Entity.DTOs.Parameter;
using Entity.Models.Parameter;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class TypeCategoryController : GenericController<TypeCategory, TypeCategoryDto, TypeCategoryDto>
    {
        private readonly IBaseBusiness<TypeCategory, TypeCategoryDto, TypeCategoryDto> _business;

        public TypeCategoryController(IBaseBusiness<TypeCategory, TypeCategoryDto, TypeCategoryDto> business, ILogger<TypeCategoryController> logger) : base(business, logger)
        {
            _business = business;
        }
    }
}
