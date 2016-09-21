﻿using Core.Common;
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
using JobInvoiceReturnDocumentEvents;

namespace Web
{


    public class JobInvoiceReturnEvents : JobInvoiceReturnDocEvents
    {
        //For Subscribing Events
        public JobInvoiceReturnEvents()
        {
            Initialized = true;
        }

    }
}
