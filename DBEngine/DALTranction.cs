using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.DALEngine
{
    public class DALTranction
    {
        private SqlConnection conn;
        private SqlTransaction trans;

        public SqlTransaction Transaction
        {
            get { return trans; }
        }

        public SqlConnection Connection
        {
            get { return conn; }
        }

        public DALTranction(IsolationLevel iso)
        {
            conn = new SqlConnection(DBHelper.connectionString);
            conn.Open();
            trans = conn.BeginTransaction(iso);
        }

        public DALTranction()
        {
            conn = new SqlConnection(DBHelper.connectionString);
            conn.Open();
            trans = conn.BeginTransaction();
        }

        public void Commit()
        {
            trans.Commit();
            conn.Close();
        }

        public void Rollback()
        {
            trans.Rollback();
            conn.Close();
        }
        protected void Finalize()
        {
            trans.Rollback();
            conn.Close();
        } 
    }
}
