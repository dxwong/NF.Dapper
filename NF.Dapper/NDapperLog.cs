using System;
using System.IO;
using System.Text;

namespace NF.Dapper
{
    public class NDapperLog
    {
        public static Action<string> Receive { get; set; }

        public static void write(object err, string fileDir, bool writeLog = true)
        {
            if (!writeLog) { return; }
            try
            {
                err = err.ToString();
                if (fileDir != null) { fileDir = "\\" + fileDir; }
                if (Receive != null) { Receive(err.ToString()); }//回调函数

                string Dir = Directory.GetCurrentDirectory() + "\\log" + fileDir;
                string filename = string.Format("{0}\\{1}.log", Dir, DateTime.Now.ToString("yyyyMMdd"));
                string error = "" + DateTime.Now.ToString() + "\r\n" + err + "\r\n\r\n";
                FileWrite(Dir, filename, error);
            }
            catch { }
        }

        #region 写文件
        static object _fLock = new object();
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="err"></param>
        /// <param name="file"></param>
        static void FileWrite(string Dir, string filename, string logstr)
        {
            lock (_fLock)
            {
                if (!Directory.Exists(Dir)) { Directory.CreateDirectory(Dir); }
                if (!System.IO.File.Exists(filename))
                {
                    System.IO.FileStream f = System.IO.File.Create(filename);
                    f.Close();
                }
                StreamWriter sw = new StreamWriter(filename, true, Encoding.GetEncoding("UTF-8"));
                sw.Write(logstr);
                sw.Flush();
                sw.Close();
            }
        }
        #endregion
    }
}
