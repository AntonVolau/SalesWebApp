using SalesUpdater.Interfaces.Core.DataTransferObject;
using System.Collections.Generic;

namespace SalesUpdater.Interfaces.DAL
{
    public interface IUnit
    {
        void Add(params SaleDTO[] models);

        IEnumerable<SaleDTO> GetAll();
    }
}
