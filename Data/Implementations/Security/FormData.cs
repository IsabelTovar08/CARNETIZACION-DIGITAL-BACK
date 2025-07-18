using Data.Classes.Base;
using Entity;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class FormData : BaseData<Form>
    {
        public FormData(ApplicationDbContext context, ILogger<Form> logger) : base(context, logger)
        {

        }
    }
}