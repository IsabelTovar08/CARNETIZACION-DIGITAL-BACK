using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class CardTemplateData : BaseData<CardTemplate>, ICardTemplateData
    {
        public CardTemplateData(ApplicationDbContext context, ILogger<CardTemplate> logger) : base(context, logger)
        {
        }
    }
}
