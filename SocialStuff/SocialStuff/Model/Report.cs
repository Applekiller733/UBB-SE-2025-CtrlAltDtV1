// <copyright file="Report.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model
{
    using System;
    using System.Collections.Generic;
    using SocialStuff.Model;

    /// <summary>
    /// Represents a report submitted by a user regarding a message or another user.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        /// <param name="messageID">The ID of the message being reported.</param>
        /// <param name="reporterUserID">The ID of the user who submitted the report.</param>
        /// <param name="status">The current status of the report.</param>
        /// <param name="reason">The reason for the report.</param>
        /// <param name="description">A detailed description of the report.</param>
        public Report(int messageID, int reporterUserID, string status, string reason, string description)
        {
            this.MessageID = messageID;
            this.ReporterUserID = reporterUserID;
            this.Status = status;
            this.Reason = reason;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the ID of the message being reported.
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who submitted the report.
        /// </summary>
        public int ReporterUserID { get; set; }

        /// <summary>
        /// Gets or sets the current status of the report.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the reason for the report.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets a detailed description of the report.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the ID of the message being reported.
        /// </summary>
        /// <returns>The ID of the message.</returns>
        public int GetMessageID()
        {
            return this.MessageID;
        }

        /// <summary>
        /// Gets the ID of the user who submitted the report.
        /// </summary>
        /// <returns>The ID of the reporter user.</returns>
        public int GetReporterUserID()
        {
            return this.ReporterUserID;
        }

        /// <summary>
        /// Gets the current status of the report.
        /// </summary>
        /// <returns>The status of the report.</returns>
        public string GetStatus()
        {
            return this.Status;
        }

        /// <summary>
        /// Gets the reason for the report.
        /// </summary>
        /// <returns>The reason for the report.</returns>
        public string GetReason()
        {
            return this.Reason;
        }

        /// <summary>
        /// Gets a detailed description of the report.
        /// </summary>
        /// <returns>The description of the report.</returns>
        public string GetDescription()
        {
            return this.Description;
        }
    }
}