using System;
using System.IO;

namespace Camefor.Tools.NetCore.Util.Log
{
    /// <summary>
    /// 描   述  ：                          
    /// 版   本  ： V1.0.0                            
    /// 创 建 人 ： rhyswang                                  
    /// 日    期 ：                         
    /// 创 建 人 ：                                   
    /// 创建时间 ：                                  
    /// 修 改 人 ：                                   
    /// 修改时间 ：                                   
    /// 修改描述 ：                                   
    /// </summary> 
    /// <summary>
    /// 事件日志类
    /// </summary>
    public class EventLog
    {
        static public string LogServer = "127.0.0.1";
        static public int LogPort = 8989;
        private string _logFile = null;
        static public bool Debug = true;
        private StreamWriter _logWriter = null;

        public EventLog()
        {
            //if ((Assembly.GetEntryAssembly() != null) && (Assembly.GetEntryAssembly().Location != null))
            //_logFile = Assembly.GetEntryAssembly().Location;
            _logFile = AppDomain.CurrentDomain.BaseDirectory + "EventLog\\";

            if (!Directory.Exists(_logFile))
            {
                Directory.CreateDirectory(_logFile);
            }
            //Debug = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugFlag"]);
        }

        public EventLog(string Path)
        {
            //if ((Assembly.GetEntryAssembly() != null) && (Assembly.GetEntryAssembly().Location != null))
            //_logFile = Assembly.GetEntryAssembly().Location;
            _logFile = AppDomain.CurrentDomain.BaseDirectory + Path + "\\";

            if (!Directory.Exists(_logFile))
            {
                Directory.CreateDirectory(_logFile);
            }
            //Debug = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugFlag"]);
        }

        ~EventLog()
        {
            if (_logWriter != null)
                _logWriter.Close();
        }

        private void WriteLog(string Section, string Msg)
        {
            if (!Msg.Equals(""))
            {
                if (_logWriter != null)
                    _logWriter.WriteLine(Section + Msg);
            }
        }
        private void AddLog(string ModuleName, string Msg, string OtherMsg, bool IsError)
        {
            try
            {
                if (_logFile != null)
                    _logWriter = File.AppendText(_logFile + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + ".log");
                string kind = IsError ? "错误" : "正常";
                WriteLog("类别：", kind);
                WriteLog("时间：", DateTime.Now.ToString());
                WriteLog("模块：", ModuleName);
                WriteLog("调试信息：", Msg);
                WriteLog("", OtherMsg);
                WriteLog("--", "--------------------------------------------------------------------------");
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (_logWriter != null)
                        _logWriter.Close();
                }
                catch
                {
                }
            }
        }
        public void AddException(string ModuleName, string Msg, Exception e)
        {
            string mask = "信息：{0}\n对象：{1}\n方法：{2}\n堆栈：{3}\n";
            string OtherMsg = String.Format(mask, e.Message, e.Source, e.TargetSite, e.StackTrace);
            e = e.InnerException;
            while (e != null)
            {
                OtherMsg += "\n" + String.Format(mask, e.Message, e.Source, e.TargetSite, e.StackTrace);
                e = e.InnerException;
            }
            AddLog(ModuleName, Msg, OtherMsg, true);

        }
        public void AddLog(string ModuleName, string Msg, bool IsError)
        {
            if (Debug || IsError)
            {
                AddLog(ModuleName, Msg, "", IsError);
            }
        }
        public void AddLog(string ModuleName, string Msg)
        {
            AddLog(ModuleName, Msg, false);
        }
    }
}
