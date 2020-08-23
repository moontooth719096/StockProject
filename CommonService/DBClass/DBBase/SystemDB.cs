using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CommonService.DBClass.DBBase
{
    class SystemDB
    {
        #region DBTest
        static public bool ConnetTest(string str_conn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region DB_Action_Single
        static public T DB_Action_Single<T>(string str_conn, string SqlString, object sql_value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    return conn.QueryFirst<T>(SqlString, sql_value);
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return default(T);
                throw ex;
            }
        }
        #endregion

        #region DB_Action_Single
        static public T DB_SPAction_Single<T>(string str_conn, string sp_name, object sql_value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    return conn.QuerySingleOrDefault<T>(sp_name, sql_value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return default(T);
                throw ex;
            }
        }
        #endregion

        #region DB_Action
        static public List<T> DB_Action<T>(string str_conn, string SqlString, object sql_value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    return conn.Query<T>(SqlString, sql_value).ToList();
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return default(List<T>);
                throw ex;
            }
        }
        #endregion

        #region DB_Action SP資料庫連線
        /// <summary>
        /// 資料庫連線
        /// </summary>
        /// <param name="str_conn">連線字串</param>
        /// <param name="sp_name">SP 名稱</param>
        /// <param name="sql_value">輸入的值與類型</param>
        static public List<T> DB_SPAction<T>(string str_conn, string sp_name, object sql_value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    return conn.Query<T>(sp_name, sql_value, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return null;
                throw ex;
            }
        }
        #endregion

        #region DB_ParameterOut 資料庫連線
        /// <summary>
        /// 資料庫Out
        /// </summary>
        /// <param name="str_conn">連線字串</param>
        /// <param name="sp_name">SP 名稱</param>
        /// <param name="sql_value">輸入的值與類型</param>
        static public void DB_ParameterOut(string str_conn, string sp_name, DynamicParameters sql_value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    //conn.Execute(sp_name, sql_value, commandType: CommandType.StoredProcedure);
                    //return sql_value;
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return null;
                throw ex;
            }
        }
        #endregion

        #region DB_Action_OutReturnput
        static public void DB_Action_Output(string str_conn, string sp_name, ref DynamicParameters parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(str_conn))
                {
                    conn.Execute(sp_name, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(ex.ToString());
                //return null;
                throw ex;
            }
        }
        #endregion
    }


}
