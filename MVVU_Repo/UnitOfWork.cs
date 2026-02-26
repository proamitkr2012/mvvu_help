using Microsoft.Extensions.Configuration;
using MVVU_Data;
using MVVU_Data.Dapper;
using MVVU_Repo.Utilities;
using System;

namespace MVVU_Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext db;
         private IMailClient _mailClient; 
        IDapperContext _dapperContext;
        protected IConfiguration config;
        public UnitOfWork(DataContext _db, IDapperContext dapperContext, IMailClient mailClient, IConfiguration _config)
        {
            db = _db;
            _dapperContext = dapperContext;
            config = _config;
        }

        private AdminMasteMVVU_Repository _IAdminMaster;
        public AdminMasteMVVU_Repository IAdminMaster
        {
            get
            {
                if (this._IAdminMaster == null)
                {
                    this._IAdminMaster = new AdminMasteMVVU_Repository(db, _dapperContext, config);
                }
                return this._IAdminMaster;
            }
        }


        public int SaveChanges()
        {
            return db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
