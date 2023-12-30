using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ableton_Project_Timer
{
    internal class Timer
    {
        public Timer(MainWindow mainWin) 
        {
            this.db = new DBLogic();
            this.timerLoopThread = new Thread(new ThreadStart(MainTimerThread));
            mainWindow = mainWin;
            try
            {
                LoadDataToUI();
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            timerLoopThread.Start();
        }

        public void MainTimerThread()
        {
            while (this.threadActive)
            {
                DateTime t0 = DateTime.Now;

                if (!this.firstLoop && IsTimerActive())
                {
                    IncrementTime();
                    LoadDataToUI();
                }

                if (IsAbletonActive())
                {
                    if (!IsTimerActive()) { StartTimer(); }
                }
                else
                {
                    if (IsTimerActive()) { StopTimer(); }
                }

                DateTime t1 = DateTime.Now;
                Console.WriteLine(1000 - (t1 - t0).Milliseconds);
                Thread.Sleep(1000 - (t1 - t0).Milliseconds);
            }
        }

        public void EndThread()
        {
            this.threadActive = false;
            this.timerLoopThread.Join();
        }

        public void StartTimer()
        {
            this.timerActive = true;
            this.firstLoop = false;
        }
        
        public void StopTimer()
        {
            this.timerActive = false;
            this.firstLoop = true;
        }

        private bool IsTimerActive()
        {
            return this.timerActive;
        }

        private void IncrementTime()
        {
            string abletonTitle = GetAbletonProjectName();
            if (abletonTitle != "")
            {
                string currentTime = this.db.GetProjectTime(abletonTitle);
                if (currentTime != null)
                {
                    if (checkTimeFormat.IsMatch(currentTime) || checkTimeFormatWithDate.IsMatch(currentTime))
                    {
                        TimeSpan time = TimeSpan.Parse(currentTime);
                        time = time.Add(TimeSpan.FromSeconds(1));
                        this.db.WriteProjectTime(abletonTitle, time.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Regex Fail - Reset");
                        this.db.WriteProjectTime(abletonTitle, "00:00:01");
                    }
                } else
                {
                    Console.WriteLine("Null Error - Reset");
                    this.db.WriteProjectTime(abletonTitle, "00:00:01");
                }
            }
        }

        public string GetAbletonProjectName()
        {
            try
            {
                string windowTitle = GetActiveWindowTitle();
                Match m = findProjectTitle.Match(windowTitle);
                if (m != null)
                {
                    string projTitle = m.Groups[0].Value;
                    if (projTitle.IndexOf("*") != -1)
                    {
                        projTitle = projTitle.Replace("*", "");
                    }
                    return projTitle;
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"GetAbletonProjectName Error: {ex.ToString()}");
            }
            
            return "";
        }

        public bool IsAbletonActive()
        {
            if (findAbleton.IsMatch(GetActiveWindowTitle())){ return true; }
            return false;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "";
        }

        private void LoadDataToUI()
        {
            mainWindow.PopulateTableData(db.GetDataAsTable());
        }
        

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private Thread timerLoopThread;
        private bool threadActive = true;

        private bool timerActive = false;
        private bool firstLoop = true;

        private MainWindow mainWindow;
        
        Regex findAbleton = new Regex(".*\\-\\s\\bAbleton Live\\b\\s[0-9]*\\s[A-z]*");
        Regex findProjectTitle = new Regex(".*?.(?=((\\s*(\\[.*\\])\\s)|\\s)\\-\\s\\bAbleton Live\\b\\s[0-9]*\\s[A-z]*)");
        Regex checkTimeFormat = new Regex("^[0-9]*\\:[0-9][0-9]\\:[0-9][0-9]$");
        Regex checkTimeFormatWithDate = new Regex("^[0-9]*\\.[0-9][0-9]\\:[0-9][0-9]\\:[0-9][0-9]$");

        public DBLogic db;

    }


}
