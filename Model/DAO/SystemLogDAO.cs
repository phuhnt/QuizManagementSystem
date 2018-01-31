using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using PagedList;

namespace Model.DAO
{
    public class SystemLogDAO
    {
        QuizManagementSystemDbContext db = null;

        public SystemLogDAO()
        {
            db = new QuizManagementSystemDbContext();
        }

        public int Insert(string eventName, string performedBy, TimeSpan exTime, DateTime exDate, string clientIp)
        {
            var log = new SystemLog();
            log.EventName = eventName;
            log.PerformedBy = performedBy;
            log.ExTime = exTime;
            log.ExDate = exDate;
            log.ClientIP = clientIp;
            db.SystemLogs.Add(log);
            db.SaveChanges();
            return log.Id;
        }

        public IEnumerable<SystemLog> GetAllSystemLogPageList(string searchString, int page = 1, int pageSize = 10)
        {
            IQueryable<SystemLog> model = db.SystemLogs;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.EventName.Contains(searchString) ||
                                    x.PerformedBy.Contains(searchString) ||
                                    x.ClientIP.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ExDate).ThenBy(x => x.ExTime).ToPagedList(page, pageSize);
        }
    }
}
