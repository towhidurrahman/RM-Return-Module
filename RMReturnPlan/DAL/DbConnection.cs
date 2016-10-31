using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;

namespace RMReturnPlan.DAL
{
    class DbConnection
    {
        private string _dsnName;

        public string dsnName
        {
            get { return _dsnName; }
            set { this._dsnName = value; }
        }

        public string cnnStr = "";

        public OdbcConnection dbConnect()
        {

            cnnStr = "Data Provider=SQLOLEDB.1;DSN=" + dsnName + ";";

            return new OdbcConnection(cnnStr);
        }
    }
}
