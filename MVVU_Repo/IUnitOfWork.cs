using System;

namespace MVVU_Repo
{
    public interface IUnitOfWork : IDisposable
    {
        AdminMasteMVVU_Repository IAdminMaster { get; }
        int SaveChanges();
    }
}