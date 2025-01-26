using System;

namespace SmartSchoolLifeAPI.Core.Models.Helpers
{
    internal class Content
    {
        public string Message { set; get; }

        public object MessageDetails { set; get; }
    }

    struct Messages
    {
        static Content content = new Content();


        /// <summary>
        /// Reason : In case Insert row into table.
        /// </summary>
        public static Content Inserted(dynamic model)
        {
            content.Message = "Resource added successfully 😎";
            content.MessageDetails = model;
            return content;
        }

        /// <summary>
        /// Reason : In case update row in a table.
        /// </summary>
        public static Content Updated()
        {
            content.Message = "Resource updated successfully 😉";
            content.MessageDetails = "updatedAt: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");

            return content;
        }

        /// <summary>
        /// Reason : In case delete row from a table.
        /// </summary>
        public static Content Deleted()
        {
            content.Message = "Resource deleted successfully 🙄";
            content.MessageDetails = "deleteddAt: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");

            return content;
        }

        /// <summary>
        /// Reason : In case function return null or zero.
        /// </summary>
        public static Content NoData(string txt)
        {
            content.Message = "Empty Result 😜";
            content.MessageDetails = "No " + txt + " found";

            return content;
        }

        /// <summary>
        /// Reason : In case Exception or an unexpected behaviour has accoured.
        /// <para>ex : Exception object  <para>
        /// </summary>
        public static Content Exception(Exception ex)
        {
            content.Message = "Exception 😱😱😱😱😱";
            content.MessageDetails = ex.Message;

            return content;
        }

        /// <summary>
        /// Reason : In case specific Error (you know it) has occurred.
        /// <para>ex : Exception object  <para>
        /// </summary>
        public static Content ErrorMessage(string errorDescription)
        {
            content.Message = "Error 🤔";
            content.MessageDetails = errorDescription;

            return content;
        }

        /// <summary>
        /// Reason : In case function return null or zero.
        /// </summary>
        public static Content CustomMessage(string txt)
        {
            content.Message = "Result 💡";
            content.MessageDetails = txt;

            return content;
        }
    }
}