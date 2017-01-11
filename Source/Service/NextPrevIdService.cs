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
using System.Data.SqlClient;
using System.Configuration;

namespace Service
{
    public interface INextPrevIdService : IDisposable
    {
        int GetNextPrevId(int DocId, int DocTypeId, string UserName, string ForAction, string HeaderTableName, string HeaderTablePK, string NextPrev);
        int GetNextPrevIdWithoutDivisionAndSite(int DocId, int DocTypeId, string UserName, string ForAction, string HeaderTableName, string HeaderTablePK, string NextPrev);
    }

    public class NextPrevIdService : INextPrevIdService
    {
        ApplicationDbContext db;
        private readonly IUnitOfWorkForService _unitOfWork;
     
        public NextPrevIdService(IUnitOfWorkForService unitofWork)
        {
            this.db = new ApplicationDbContext();
            _unitOfWork = unitofWork;
        }


        public int GetNextPrevId(int DocId, int DocTypeId, string UserName, string ForAction, string HeaderTableName, string HeaderTablePK, string NextPrev)
        {

            SqlParameter SqlParameterUserName = new SqlParameter("@UserName", UserName);
            SqlParameter SqlParameterForAction = new SqlParameter("@ForAction", ForAction);
            SqlParameter SqlParameterHeaderTableName = new SqlParameter("@HeaderTableName", HeaderTableName);
            SqlParameter SqlParameterHeaderTablePkFieldName = new SqlParameter("@HeaderTablePkFieldName", HeaderTablePK);
            SqlParameter SqlParameterDocId = new SqlParameter("@DocId", DocId);
            SqlParameter SqlParameterDocTypeId = new SqlParameter("@DocTypeId", DocTypeId);
            SqlParameter SqlParameterNextPrevious = new SqlParameter("@NextPrevious", NextPrev);

            int Id = db.Database.SqlQuery<int>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spGetNextPreviousId @UserName, @ForAction, @HeaderTableName, @HeaderTablePkFieldName, @DocId, @DocTypeId, @NextPrevious", SqlParameterUserName, SqlParameterForAction, SqlParameterHeaderTableName,SqlParameterHeaderTablePkFieldName,SqlParameterDocId,SqlParameterDocTypeId,SqlParameterNextPrevious).FirstOrDefault();

            return Id;
        }

        public int GetNextPrevIdWithoutDivisionAndSite(int DocId, int DocTypeId, string UserName, string ForAction, string HeaderTableName, string HeaderTablePK, string NextPrev)
        {

            SqlParameter SqlParameterUserName = new SqlParameter("@UserName", UserName);
            SqlParameter SqlParameterForAction = new SqlParameter("@ForAction", ForAction);
            SqlParameter SqlParameterHeaderTableName = new SqlParameter("@HeaderTableName", HeaderTableName);
            SqlParameter SqlParameterHeaderTablePkFieldName = new SqlParameter("@HeaderTablePkFieldName", HeaderTablePK);
            SqlParameter SqlParameterDocId = new SqlParameter("@DocId", DocId);
            SqlParameter SqlParameterDocTypeId = new SqlParameter("@DocTypeId", DocTypeId);
            SqlParameter SqlParameterNextPrevious = new SqlParameter("@NextPrevious", NextPrev);

            int Id = db.Database.SqlQuery<int>("" + ConfigurationManager.AppSettings["DataBaseSchema"] + ".spGetNextPreviousIdWithoutDivisionAndSite @UserName, @ForAction, @HeaderTableName, @HeaderTablePkFieldName, @DocId, @DocTypeId, @NextPrevious", SqlParameterUserName, SqlParameterForAction, SqlParameterHeaderTableName, SqlParameterHeaderTablePkFieldName, SqlParameterDocId, SqlParameterDocTypeId, SqlParameterNextPrevious).FirstOrDefault();

            return Id;
        }

        public void Dispose()
        {
        }
       
    }
}
