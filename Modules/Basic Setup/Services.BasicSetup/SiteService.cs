using System.Collections.Generic;
using System.Linq;
using System;
using Model;
using Models.Company.Models;
using Infrastructure.IO;
using Models.BasicSetup.ViewModels;

namespace Services.BasicSetup
{
    public interface ISiteService : IDisposable
    {
        Site Create(Site pt);
        void Delete(int id);
        void Delete(Site s);
        Site Find(int Id);
        void Update(Site s);
        IEnumerable<Site> GetSiteList();
        int NextId(int id);
        int PrevId(int id);
        Site FindByPerson(int id);

        #region HelpList Getter
        /// <summary>
        /// *General Function*
        /// This function will create the help list for Projects
        /// </summary>
        /// <param name="searchTerm">user search term</param>
        /// <param name="pageSize">no of records to fetch for each page</param>
        /// <param name="pageNum">current page size </param>
        /// <returns>ComboBoxPagedResult</returns>
        ComboBoxPagedResult GetList(string searchTerm, int pageSize, int pageNum);
        #endregion

        #region HelpList Setters
        /// <summary>
        /// *General Function*
        /// This function will return the object in (Id,Text) format based on the Id
        /// </summary>
        /// <param name="Id">Primarykey of the record</param>
        /// <returns>ComboBoxResult</returns>
        ComboBoxResult GetValue(int Id);

        /// <summary>
        /// *General Function*
        /// This function will return list of object in (Id,Text) format based on the Ids
        /// </summary>
        /// <param name="Id">PrimaryKey of the record(',' seperated string)</param>
        /// <returns>List<ComboBoxResult></returns>
        List<ComboBoxResult> GetListCsv(string Id);
        #endregion

    }

    public class SiteService : ISiteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Site> _SiteRepository;
        public SiteService(IUnitOfWork unitOfWork, IRepository<Site> SiteRepo)
        {
            _unitOfWork = unitOfWork;
            _SiteRepository = SiteRepo;
        }
        public SiteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _SiteRepository = unitOfWork.Repository<Site>();
        }

        public Site Find(int id)
        {
            return _unitOfWork.Repository<Site>().Query().Include(m => m.City).Get().Where(m => m.SiteId == id).FirstOrDefault();

        }

        public Site Create(Site s)
        {
            s.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<Site>().Insert(s);
            return s;
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<Site>().Delete(id);
        }

        public void Delete(Site s)
        {
            _unitOfWork.Repository<Site>().Delete(s);
        }

        public void Update(Site s)
        {
            s.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<Site>().Update(s);
        }


        public IEnumerable<Site> GetSiteList()
        {
            var pt = (from p in _SiteRepository.Instance
                      orderby p.SiteName
                      select p);

            return pt;
        }
        public int NextId(int id)
        {
            int temp = 0;
            if (id != 0)
            {
                temp = (from p in _SiteRepository.Instance
                        orderby p.SiteName
                        select p.SiteId).AsEnumerable().SkipWhile(p => p != id).Skip(1).FirstOrDefault();
            }
            else
            {
                temp = (from p in _SiteRepository.Instance
                        orderby p.SiteName
                        select p.SiteId).FirstOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public int PrevId(int id)
        {

            int temp = 0;
            if (id != 0)
            {

                temp = (from p in _SiteRepository.Instance
                        orderby p.SiteName
                        select p.SiteId).AsEnumerable().TakeWhile(p => p != id).LastOrDefault();
            }
            else
            {
                temp = (from p in _SiteRepository.Instance
                        orderby p.SiteName
                        select p.SiteId).AsEnumerable().LastOrDefault();
            }
            if (temp != 0)
                return temp;
            else
                return id;
        }

        public Site FindByPerson(int id)
        {
            return (from p in _SiteRepository.Instance
                    where p.PersonId == id
                    select p).FirstOrDefault();
        }

        public ComboBoxPagedResult GetList(string searchTerm, int pageSize, int pageNum)
        {
            var list = (from pr in _SiteRepository.Instance
                        where (string.IsNullOrEmpty(searchTerm) ? 1 == 1 : (pr.SiteName.ToLower().Contains(searchTerm.ToLower())))
                        orderby pr.SiteName
                        select new ComboBoxResult
                        {
                            text = pr.SiteName,
                            id = pr.SiteId.ToString()
                        }
              );

            var temp = list
               .Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = list.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = temp;
            Data.Total = count;

            return Data;
        }

        public ComboBoxResult GetValue(int Id)
        {
            ComboBoxResult ProductJson = new ComboBoxResult();

            IEnumerable<Site> Sites = from pr in _SiteRepository.Instance
                                            where pr.SiteId == Id
                                            select pr;

            ProductJson.id = Sites.FirstOrDefault().SiteId.ToString();
            ProductJson.text = Sites.FirstOrDefault().SiteName;

            return ProductJson;
        }

        public List<ComboBoxResult> GetListCsv(string Ids)
        {
            string[] subStr = Ids.Split(',');
            List<ComboBoxResult> ProductJson = new List<ComboBoxResult>();
            for (int i = 0; i < subStr.Length; i++)
            {
                int temp = Convert.ToInt32(subStr[i]);
                IEnumerable<Site> Sites = from pr in _SiteRepository.Instance
                                                where pr.SiteId == temp
                                                select pr;
                ProductJson.Add(new ComboBoxResult()
                {
                    id = Sites.FirstOrDefault().SiteId.ToString(),
                    text = Sites.FirstOrDefault().SiteName
                });
            }
            return ProductJson;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
