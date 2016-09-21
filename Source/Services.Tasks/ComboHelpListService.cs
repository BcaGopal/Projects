using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using System;
using Model.ViewModels;
using Model.DatabaseViews;
namespace Service
{
    public interface IComboHelpListService : IDisposable
    {
        #region Getters

        /// <summary>
        /// *General Function*
        /// This function will create the help list for Users
        /// </summary>
        /// <param name="searchTerm">user search term</param>
        /// <param name="pageSize">no of records to fetch for each page</param>
        /// <param name="pageNum">current page size </param>
        /// <returns>ComboBoxPagedResult</returns>
        ComboBoxPagedResult GetUsers(string searchTerm, int pageSize, int pageNum);

        #endregion

        #region Setters

        /// <summary>
        /// *General Function*
        /// This function will return the object based on the Id
        /// </summary>
        /// <param name="Id">PrimaryKey of the record</param>
        /// <returns>ComboBoxResult</returns>
        ComboBoxResult GetUser(string Id);

        /// <summary>
        /// *General Function*
        /// This function will return list of object based on the Ids
        /// </summary>
        /// <param name="Id">PrimaryKey of the record(',' seperated string)</param>
        /// <returns>List<ComboBoxResult></returns>
        List<ComboBoxResult> GetMultipleUsers(string Id);

        #endregion
    }

    public class ComboHelpListService : IComboHelpListService
    {
        private readonly IRepository<_Users> _UserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ComboHelpListService(IRepository<_Users> User,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _UserRepository = User;
        }

        public ComboBoxPagedResult GetUsers(string searchTerm, int pageSize, int pageNum)
        {

            var Query = (from ur in _UserRepository.Instance
                        where (string.IsNullOrEmpty(searchTerm) ? 1 == 1 : (ur.UserName.ToLower().Contains(searchTerm.ToLower())))
                        orderby ur.UserName
                        select new ComboBoxResult
                        {
                            text = ur.UserName,
                            id = ur.UserName,
                        }
            );

            var records = Query.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();

            var count = Query.Count();

            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = records;
            Data.Total = count;

            return Data;

        }



        public ComboBoxResult GetUser(string Id)
        {
            ComboBoxResult ProductJson = new ComboBoxResult();

            IEnumerable<_Users> users = from p in _UserRepository.Instance
                                       where p.UserName == Id
                                       select p;

            ProductJson.id = users.FirstOrDefault().UserName.ToString();
            ProductJson.text = users.FirstOrDefault().UserName;

            return ProductJson;
        }

        public List<ComboBoxResult> GetMultipleUsers(string Ids)
        {
            string[] subStr = Ids.Split(',');
            List<ComboBoxResult> ProductJson = new List<ComboBoxResult>();
            for (int i = 0; i < subStr.Length; i++)
            {
                string temp = subStr[i];
                IEnumerable<_Users> users = from p in _UserRepository.Instance
                                           where p.UserName == temp
                                           select p;
                ProductJson.Add(new ComboBoxResult()
                {
                    id = users.FirstOrDefault().UserName.ToString(),
                    text = users.FirstOrDefault().UserName
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
