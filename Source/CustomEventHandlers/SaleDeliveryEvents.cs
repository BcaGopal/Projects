using Core.Common;
using CustomEventArgs;
using Data.Models;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentEvents;
using SaleDeliveryDocumentEvents;
using System.Data;

namespace Web
{


    public class SaleDeliveryEvents : SaleDeliveryDocEvents
    {
        //For Subscribing Events
        public SaleDeliveryEvents()
        {
            Initialized = true;
            _onHeaderSave += SaleDeliveryEvents__onHeaderSave;
            _onLineSave += SaleDeliveryEvents__onLineSave;
            _onLineSaveBulk += SaleDeliveryEvents__onLineSaveBulk;
            _onLineDelete += SaleDeliveryEvents__onLineDelete;
            _onHeaderDelete += SaleDeliveryEvents__onHeaderDelete;
            _onHeaderSubmit += SaleDeliveryEvents__onHeaderSubmit;
            _beforeLineSave += SaleDeliveryEvents__beforeLineSaveDataValidation;
            _beforeLineDelete += SaleDeliveryEvents__beforeLineDeleteDataValidation;
            _beforeLineSaveBulk += SaleDeliveryEvents__beforeLineSaveBylkDataValidation;
            _afterHeaderSubmit += SaleDeliveryEvents_afterHeaderSubmitEvent;

        }

        private void SaleDeliveryEvents_afterHeaderSubmitEvent(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
            throw new NotImplementedException();
        }


        void SaleDeliveryEvents__onHeaderSubmit(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
            int Id = EventArgs.DocId;

            string ConnectionString = (string)System.Web.HttpContext.Current.Session["DefaultConnectionString"];

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();


                    using (SqlCommand cmd = new SqlCommand("Web.sp_CreatePurchaseOnBranch"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@SaleDeliveryHeaderId", Id);
                        cmd.CommandTimeout = 1000;
                        cmd.ExecuteNonQuery();
                    }

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool SaleDeliveryEvents__beforeLineSaveDataValidation(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
            return true;
        }

        bool SaleDeliveryEvents__beforeLineDeleteDataValidation(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
            return true;
        }


        bool SaleDeliveryEvents__beforeLineSaveBylkDataValidation(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
            return true;
        }


        void SaleDeliveryEvents__onHeaderDelete(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
        }

        void SaleDeliveryEvents__onLineDelete(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
        }

        void SaleDeliveryEvents__onLineSaveBulk(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
        }

        void SaleDeliveryEvents__onLineSave(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
        }


        void SaleDeliveryEvents__onHeaderSave(object sender, SaleEventArgs EventArgs, ref ApplicationDbContext db)
        {
        }
    }
}
