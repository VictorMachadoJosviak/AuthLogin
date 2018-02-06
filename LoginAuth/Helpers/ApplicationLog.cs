using LoginAuth.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LoginAuth.Helpers
{
    public static partial class ApplicationLog
    {        
        public static void Save(this Exception ex)
        {
            Save(ex, ImpactLevel.Low, "");
        }
          
        public static void Save(this Exception ex, ImpactLevel impactLevel)
        {
            Save(ex, impactLevel, "");
        }  
        public static void Save(this Exception ex, ImpactLevel impactLevel, string errorDescription)
        {
            using (var db = new DataContext())
            {
                Log log = new Log();

                if (errorDescription != null && errorDescription != "")
                {
                    log.ErrorMessage = errorDescription;
                }
                log.ExceptionType = ex.GetType().FullName;
                var stackTrace = new StackTrace(ex, true);
                var allFrames = stackTrace.GetFrames().ToList();
                foreach (var frame in allFrames)
                {
                    log.FileName = frame.GetFileName();
                    log.LineNumber = frame.GetFileLineNumber();
                    var method = frame.GetMethod();
                    log.MethodName = method.Name;
                    log.ClassName = frame.GetMethod().DeclaringType.ToString();
                }

                log.ImpactLevel = impactLevel.ToString();
                try
                {
                    log.AppName = Assembly.GetCallingAssembly().GetName().Name;
                }
                catch
                {
                    log.AppName = "";
                }

                log.ErrorMessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                if (ex.InnerException != null)
                {
                    log.InnerException = ex.InnerException.ToString();
                    log.InnerExceptionMessage = ex.InnerException.Message;
                }


                try
                {
                    log.CreatedAt = DateTime.Now;
                    db.Logs.Add(log);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }
        public enum ImpactLevel
        {
            High = 0,
            Medium = 1,
            Low = 2,
        }
    }
}