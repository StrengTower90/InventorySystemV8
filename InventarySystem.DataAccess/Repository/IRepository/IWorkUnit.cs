using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository.IRepository
{
    public interface IWorkUnit : IDisposable // this class allows to release any resource unused in the system
    {
        IStoreRepository Store { get; }
        ICategoryRepository Category { get;  }
        Task Save();
    }
}
