using SocialStuff.Data;
using SocialStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Interfaces
{
    public interface IReportService
    {
        Report GetReportById(int id);
        void AddReport(Report report);
        bool CheckIfReportExists(int messageID, int reporterUserID);
        void IncreaseReportCount(int reportedID);
        void LogReportedMessages(List<Report> reports);
        public void SendReport(Report report);
    }
}
