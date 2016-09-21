using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Infrastructure;
using Model.Models;

using Core.Common;
using System;
using Model;
using System.Threading.Tasks;
using Data.Models;

namespace Service
{
    public interface IStockInHandSettingService : IDisposable
    {
        StockInHandSetting Create(StockInHandSetting pt);
        void Delete(int id);
        void Delete(StockInHandSetting pt);
        StockInHandSetting Find(int id);      
        void Update(StockInHandSetting pt);
        StockInHandSetting Add(StockInHandSetting pt);
        StockInHandSetting GetTrailBalanceSetting(string UserName);
    }

    public class StockInHandSettingService : IStockInHandSettingService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUnitOfWorkForService _unitOfWork;
        private readonly Repository<StockInHandSetting> _StockInHandSettingRepository;
        RepositoryQuery<StockInHandSetting> StockInHandSettingRepository;
        public StockInHandSettingService(IUnitOfWorkForService unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _StockInHandSettingRepository = new Repository<StockInHandSetting>(db);
            StockInHandSettingRepository = new RepositoryQuery<StockInHandSetting>(_StockInHandSettingRepository);
        }

        public StockInHandSetting Find(int id)
        {
            return _unitOfWork.Repository<StockInHandSetting>().Find(id);
        }

        public StockInHandSetting Create(StockInHandSetting pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<StockInHandSetting>().Insert(pt);
            return pt;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<StockInHandSetting>().Delete(id);
        }

        public void Delete(StockInHandSetting pt)
        {
            _unitOfWork.Repository<StockInHandSetting>().Delete(pt);
        }

        public void Update(StockInHandSetting pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<StockInHandSetting>().Update(pt);
        }

        public StockInHandSetting Add(StockInHandSetting pt)
        {
            _unitOfWork.Repository<StockInHandSetting>().Insert(pt);
            return pt;
        }

        public StockInHandSetting GetTrailBalanceSetting(string UserName)
        {

            return (from p in db.StockInHandSetting
                    where p.UserName == UserName
                    select p
                        ).FirstOrDefault();

        }

        public void Dispose()
        {
        }

    }
}
