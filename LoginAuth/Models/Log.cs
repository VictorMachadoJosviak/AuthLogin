using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginAuth.Models
{
    [Table("ApplicationLogs")]
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string AppName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ErrorMessage { get; set; }

        public string ExceptionType { get; set; }

        public string FileName { get; set; }

        public int LineNumber { get; set; }

        public string MethodName { get; set; }

        public string ClassName { get; set; }

        public string ImpactLevel { get; set; }

        public string StackTrace { get; set; }

        public string InnerException { get; set; }

        public string InnerExceptionMessage { get; set; }
    }
}