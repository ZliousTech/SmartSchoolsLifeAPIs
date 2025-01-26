using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SmartSchoolLifeAPI.Models.Helpers
{
    internal class Content
    {
        public string Message { set; get; }

        public string MessageDetails { set; get; }
    }

    struct Messages
    {
        static Content content = new Content();


        /// <summary>
        /// Reason : In case Insert row into table.
        /// </summary>
        public static Content Inserted()
        {
            return content;
        }

        /// <summary>
        /// Reason : In case update row in a table.
        /// </summary>
        public static Content Updated()
        {
            content.Message = "Resource updated successfully";
            content.MessageDetails = "updatedAt: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");

            return content;
        }

        /// <summary>
        /// Reason : In case delete row from a table.
        /// </summary>
        public static Content Deleted()
        {
            content.Message = "Resource deleted successfully";
            content.MessageDetails = "deleteddAt: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");

            return content;
        }

        /// <summary>
        /// Reason : In case function return null or zero.
        /// </summary>
        public static Content NoData(string table)
        {
            content.Message = "Empty Result";
            content.MessageDetails = "No " + table + " found";

            return content;
        }

        /// <summary>
        /// Reason : In case Exception has accoured.
        /// <para>ex : Exception object  <para>
        /// </summary>
        public static Content Exception(Exception ex)
        {
            content.Message = "Exception";
            content.MessageDetails = ex.Message;

            return content;
        }
    }
}