using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Assignment
{
    public class CardData : BaseData<Card>, ICardData
    {
        public CardData(ApplicationDbContext context, ILogger<Card> logger) : base(context, logger)
        {

        }
    }
}
