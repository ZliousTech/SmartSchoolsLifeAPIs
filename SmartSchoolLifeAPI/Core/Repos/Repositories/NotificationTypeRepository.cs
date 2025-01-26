using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        public IEnumerable<NotificationsTypesModel> GetAll()
        {
            List<dynamic> notificationsTypes = new List<dynamic>();
            string query = "SELECT * FROM NotificationsTypes";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        notificationsTypes = reader.MapAll();
                    }
                }
            }

            return notificationsTypes != null ? notificationsTypes.MapListTo<NotificationsTypesModel>() : null;
        }

        public NotificationsTypesModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public NotificationsTypesModel Add(NotificationsTypesModel entity)
        {
            throw new NotImplementedException();
        }

        public void Update(NotificationsTypesModel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}