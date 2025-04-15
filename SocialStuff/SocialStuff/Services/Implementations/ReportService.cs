// <copyright file="ReportService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Microsoft.Data.SqlClient;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    /// <summary>
    /// Provides services for managing and handling reports.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IUserService userService;
        private IRepository repository;
        private List<Report> reports;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="repository">The repository instance for data access.</param>
        /// <param name="userService">The user service instance for user-related operations.</param>
        public ReportService(IRepository repository, IUserService userService)
        {
            this.repository = repository;
            this.reports = repository.GetReportsList();
            this.userService = userService;
        }

        /// <summary>
        /// Retrieves a report by its ID.
        /// </summary>
        /// <param name="id">The ID of the report.</param>
        /// <returns>The report if found; otherwise, null.</returns>
        public Report? GetReportById(int id)
        {
            return this.reports.Find(report => report.MessageID == id);
        }

        /// <summary>
        /// Adds a new report to the list.
        /// </summary>
        /// <param name="report">The report to add.</param>
        public void AddReport(Report report)
        {
            this.reports.Add(report);
        }

        /// <summary>
        /// Checks if a report exists for a specific message and reporter.
        /// </summary>
        /// <param name="messageID">The ID of the message being reported.</param>
        /// <param name="reporterUserID">The ID of the user reporting the message.</param>
        /// <returns>True if the report exists; otherwise, false.</returns>
        public bool CheckIfReportExists(int messageID, int reporterUserID)
        {
            return this.reports.Exists(report => report.MessageID == messageID && report.ReporterUserID == reporterUserID);
        }

        /// <summary>
        /// Increases the report count for a specific user.
        /// </summary>
        /// <param name="reportedID">The ID of the user being reported.</param>
        public void IncreaseReportCount(int reportedID)
        {
            User user = this.userService.GetUserById(reportedID);
            user.IncreaseReportCount();
        }

        /// <summary>
        /// Logs a list of reported messages to the repository.
        /// </summary>
        /// <param name="reports">The list of reports to log.</param>
        public void LogReportedMessages(List<Report> reports)
        {
            foreach (var report in reports)
            {
                this.repository.AddReport(report.MessageID, report.Reason, report.Description, report.Status);
            }
        }

        /// <summary>
        /// Sends a report for further processing.
        /// </summary>
        /// <param name="report">The report to send.</param>
        public void SendReport(Report report)
        {
            // todo - implement this method
        }
    }
}