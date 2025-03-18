using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model
{
    class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public int ReportedCount { get; set; }

        public User() { }
        public User(int userId, string username, string phoneNumber, int reportedCount)
        {
            UserId = userId;
            Username = username;
            PhoneNumber = phoneNumber;
            ReportedCount = reportedCount;
        }
        public User(int userId, string username, string phoneNumber)
        {
            UserId = userId;
            Username = username;
            PhoneNumber = phoneNumber;
            ReportedCount = 0;
        }


        public override string ToString()
        {
            return $"User ID: {UserId}, " +
                $"Username: {Username}, " +
                $"Phone Number: {PhoneNumber}, " +
                $"Reported Count: {ReportedCount}";
        }


        public void IncreaseReportCount()
        {
            ReportedCount++;
        }

        public void resetReportCount()
        {
            ReportedCount = 0;
        }
    }
}
