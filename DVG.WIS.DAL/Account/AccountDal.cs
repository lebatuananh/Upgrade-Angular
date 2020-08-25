using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Account
{
    public class AccountDal: NpgsqlHelpers, IAccountDal
    {
        public int Insert(Entities.Account obj)
        {
            string storeName = "Admin_Account_Insert";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        var now = DateTime.Now;
                        command.Parameters.Add(NpgsqlParameter("@_username", obj.UserName));
                        command.Parameters.Add(NpgsqlParameter("@_displayname", obj.DisplayName));
                        command.Parameters.Add(NpgsqlParameter("@_fullname", obj.FullName));
                        command.Parameters.Add(NpgsqlParameter("@_email", obj.Email));
                        command.Parameters.Add(NpgsqlParameter("@_phone", obj.Phone));
                        command.Parameters.Add(NpgsqlParameter("@_birthday", obj.Birthday));
                        command.Parameters.Add(NpgsqlParameter("@_gender", obj.Gender));
                        command.Parameters.Add(NpgsqlParameter("@_address", obj.Address));
                        command.Parameters.Add(NpgsqlParameter("@_desciption", obj.Desciption));
                        command.Parameters.Add(NpgsqlParameter("@_avatar", obj.Avatar));
                        command.Parameters.Add(NpgsqlParameter("@_createddate", now));
                        command.Parameters.Add(NpgsqlParameter("@_lastmodifieddate", now));
                        command.Parameters.Add(NpgsqlParameter("@_lastlogin", now));
                        command.Parameters.Add(NpgsqlParameter("@_lastpasswordchange", now));
                        command.Parameters.Add(NpgsqlParameter("@_status", obj.Status));
                        command.Parameters.Add(NpgsqlParameter("@_createdby", obj.CreatedBy));
                        command.Parameters.Add(NpgsqlParameter("@_lastmodifiedby", obj.LastModifiedBy));
                        command.Parameters.Add(NpgsqlParameter("@_password", obj.Password));
                        command.Parameters.Add(NpgsqlParameter("@_passwordquestion", obj.PasswordQuestion));
                        command.Parameters.Add(NpgsqlParameter("@_passwordanswer", obj.PasswordAnswer));
                        command.Parameters.Add(NpgsqlParameter("@_userType", obj.UserType));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Account obj)
        {
            string storeName = "Admin_Account_Update";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_id", obj.Id));
                        command.Parameters.Add(NpgsqlParameter("@_username", obj.UserName));
                        command.Parameters.Add(NpgsqlParameter("@_displayname", obj.DisplayName));
                        command.Parameters.Add(NpgsqlParameter("@_fullname", obj.FullName));
                        command.Parameters.Add(NpgsqlParameter("@_email", obj.Email));
                        command.Parameters.Add(NpgsqlParameter("@_phone", obj.Phone));
                        command.Parameters.Add(NpgsqlParameter("@_birthday", obj.Birthday));
                        command.Parameters.Add(NpgsqlParameter("@_gender", obj.Gender));
                        command.Parameters.Add(NpgsqlParameter("@_address", obj.Address));
                        command.Parameters.Add(NpgsqlParameter("@_desciption", obj.Desciption));
                        command.Parameters.Add(NpgsqlParameter("@_avatar", obj.Avatar));
                        command.Parameters.Add(NpgsqlParameter("@_lastmodifieddate", DateTime.Now));
                        command.Parameters.Add(NpgsqlParameter("@_lastlogin", obj.LastLogin));
                        command.Parameters.Add(NpgsqlParameter("@_lastpasswordchange", obj.LastPasswordChange));
                        command.Parameters.Add(NpgsqlParameter("@_status", obj.Status));
                        command.Parameters.Add(NpgsqlParameter("@_lastmodifiedby", obj.LastModifiedBy));
                        command.Parameters.Add(NpgsqlParameter("@_password", obj.Password));
                        command.Parameters.Add(NpgsqlParameter("@_passwordquestion", obj.PasswordQuestion));
                        command.Parameters.Add(NpgsqlParameter("@_passwordanswer", obj.PasswordAnswer));
                        command.Parameters.Add(NpgsqlParameter("@_userType", obj.UserType));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.Account> GetList(string keyword, out int totalRows, int UserType = 0, int? pageIndex = 1, int? pageSize = 15)
        {
            totalRows = 0;
            string storeName = "Admin_Account_GetList";
            string storeNameCount = "Admin_Account_GetList_Count";
            var lst = new List<Entities.Account>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_keyword", keyword));
                        command.Parameters.Add(NpgsqlParameter("@_userType", UserType));
                        command.Parameters.Add(NpgsqlParameter("@_pageindex", pageIndex));
                        command.Parameters.Add(NpgsqlParameter("@_pagesize", pageSize));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.Account();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lst.Add(obj);
                                }
                            }
                        }
                    }

                    using (var command = db.CreateCommand(storeNameCount, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_keyword", keyword));
                        command.Parameters.Add(NpgsqlParameter("@_userType", UserType));
                        totalRows = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return lst;
        }

        public IEnumerable<Entities.Account> GetAll()
        {
            string storeName = "Admin_Account_GetAll";
            var lst = new List<Entities.Account>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.Account();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lst.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return lst;
        }

        public Entities.Account GetById(int userId)
        {
            string storeName = "Admin_Account_GetById";
            Entities.Account result = new Entities.Account();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_id", userId));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                EntityBase.SetObjectValue(reader, ref result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return result;
        }

        public Entities.Account GetByUserName(string userName)
        {
            string storeName = "Admin_Account_GetByUserName";
            Entities.Account result = new Entities.Account();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_username", userName));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                EntityBase.SetObjectValue(reader, ref result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return result;
        }

        public Entities.Account GetByEmail(string email)
        {
            string storeName = "Admin_Account_GetByEmail";
            Entities.Account result = new Entities.Account();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_email", email));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                EntityBase.SetObjectValue(reader, ref result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return result;
        }
    }
}
