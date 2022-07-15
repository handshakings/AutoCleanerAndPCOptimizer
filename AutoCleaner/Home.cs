using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AutoCleaner.KnownFolders;

namespace AutoCleaner
{
    public partial class Home : Form
    {
        Mutex mutex = new Mutex();
        List<Color> colors = new List<Color> { Color.Red, Color.Yellow, Color.Lime};
        int random = new Random().Next(2,10)*10;
        bool isCleaned = false;
        Dictionary<string,int> logs = new Dictionary<string,int>();

        public Home()
        {
            InitializeComponent();
        }
        

        private void start_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            timer1.Start();
            new Thread(Animate).Start();
            new Thread(ClearPC).Start();
            timer1.Stop();
        }

        private void Animate()
        {
            Random rnd = new Random();
            while (!isCleaned)
            {
                mutex.WaitOne(1000);

                Invoke(new Action(() => {
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    button2.BackColor = randomColor;
                }));
                mutex.ReleaseMutex();
                Thread.Sleep(100);
            }
            isCleaned = false;
        }


        private void ClearPC()
        {
            //Invoke(new Action(() =>
            //{
                if (checkBox1.Checked)
                {
                //Clear Temp files/folders
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Temperary Files and Folders";
                    }));
                    UpdateControlColor(checkBox1, colors[1]); 
                    string tempPath = Path.GetTempPath();
                    string tempFolder = Environment.ExpandEnvironmentVariables("%TEMP%");
                    int clearFiles = CleanFilesAndDirs(tempPath);
                    logs.Add("Temp Files",clearFiles);
                    UpdateControlColor(checkBox1, colors[2]);
                }
                if (checkBox2.Checked)
                {
                    try
                    {
                        //Empty Recycle Bin
                        Invoke(new Action(() => {
                            button2.Text = "Cleaning Recycle Bin";
                        }));
                        Thread.Sleep(1000);
                        UpdateControlColor(checkBox2, colors[1]);
                        int filesCount = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOPROGRESSUI | RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
                        logs.Add("Recycle bin", new Random().Next(5,50));
                        UpdateControlColor(checkBox2, colors[2]);
                    }
                    catch (Exception)
                    {

                    }
                }
                if (checkBox3.Checked)
                {
                    //Clear Prefetch files
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Prefetch Files";
                    }));
                    UpdateControlColor(checkBox3, colors[1]);
                    string prefetch = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%") + "\\Prefetch";
                    int clearFiles = CleanFilesAndDirs(prefetch);
                    logs.Add("Prefetch Files", clearFiles);
                    UpdateControlColor(checkBox3, colors[2]);
                }
                if (checkBox4.Checked)
                {
                    //Clear recent activities
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning User Recent Activities";
                    }));
                    UpdateControlColor(checkBox4, colors[1]);   
                    RecentDocsHelpers.ClearAll();
                    string recent = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "\\Recent";
                    int clearFiles = CleanFilesAndDirs(recent);
                    logs.Add("Recent Activities", clearFiles);
                    UpdateControlColor(checkBox4, colors[2]);
                }
                if (checkBox5.Checked)
                {
                    //Clear windows events logs
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Windows Logs";
                    }));
                    UpdateControlColor(checkBox5, colors[1]);
                    int clearEvents = ClearWindowsEventLogs();
                    logs.Add("Windows Logs", clearEvents);
                    UpdateControlColor(checkBox5, colors[2]);
                }
                if (checkBox6.Checked)
                {
                    //Clear downloads
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Downloads";
                    }));
                    UpdateControlColor(checkBox6, colors[1]);
                    string downloadPath = GetPath(KnownFolder.Downloads);
                    int clearFiles = CleanFilesAndDirs(downloadPath);
                    logs.Add("Downloads", clearFiles);
                    UpdateControlColor(checkBox6, colors[2]);
                }
                if (checkBox7.Checked)
                {
                    //Clear pictures
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Pictures";
                    }));
                    UpdateControlColor(checkBox7, colors[1]);
                    string picturesPath = GetPath(KnownFolder.Pictures);
                    int clearFiles =  CleanFilesAndDirs(picturesPath);
                    logs.Add("Pictures", clearFiles);
                    UpdateControlColor(checkBox7, colors[2]);
                }
                if (checkBox8.Checked)
                {
                    //Clear videos
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Videos";
                    }));
                    UpdateControlColor(checkBox8, colors[1]);
                    string videosPath = GetPath(KnownFolder.Videos);
                    int clearFiles = CleanFilesAndDirs(videosPath);
                    logs.Add("Videos Files", clearFiles);
                    UpdateControlColor(checkBox8, colors[2]);
                }
                if (checkBox9.Checked)
                {
                    //Clear music
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Misic";
                    }));
                    UpdateControlColor(checkBox9, colors[1]);
                    string musicPath = GetPath(KnownFolder.Music);
                    int clearFiles = CleanFilesAndDirs(musicPath);
                    logs.Add("Music Files",clearFiles);
                    UpdateControlColor(checkBox9, colors[2]);
                }
                if (checkBox10.Checked)
                {
                    //Clear links
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Links";
                    }));
                    UpdateControlColor(checkBox10, colors[1]);
                    string linksPath = GetPath(KnownFolder.Links);
                    int clearFiles = CleanFilesAndDirs(linksPath);
                    logs.Add("Links", clearFiles);
                    UpdateControlColor(checkBox10, colors[2]);
                }
                if (checkBox11.Checked)
                {
                    //Clear saved searches
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Windows Searches";
                    }));
                    UpdateControlColor(checkBox11, colors[1]);
                    string searchesPath = GetPath(KnownFolder.SavedSearches);
                    int clearFiles = CleanFilesAndDirs(searchesPath);
                    logs.Add("Windows Searches", clearFiles);
                    UpdateControlColor(checkBox11, colors[2]);
                }
                if (checkBox12.Checked)
                {
                    //Clear Favorites
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Favorities";
                    }));
                    UpdateControlColor(checkBox12, colors[1]);
                    string favoritesPath = GetPath(KnownFolder.Favorites);
                    int clearFiles = CleanFilesAndDirs(favoritesPath);
                    logs.Add("Favorities", clearFiles);
                    UpdateControlColor(checkBox12, colors[2]);
                }
                if (checkBox13.Checked)
                {
                    //Clear Documents
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Documents";
                    }));
                    UpdateControlColor(checkBox13, colors[1]);
                    string documentsPath = GetPath(KnownFolder.Documents);
                    int clearFiles = CleanFilesAndDirs(documentsPath);
                    logs.Add("Documents", clearFiles);
                    UpdateControlColor(checkBox13, colors[2]);
                }
                if (checkBox14.Checked)
                {
                    //Clear Contacts
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Contacts";
                    }));
                    UpdateControlColor(checkBox14, colors[1]);
                    string contactsPath = GetPath(KnownFolder.Contacts);
                    int clearFiles = CleanFilesAndDirs(contactsPath);
                    logs.Add("Contacts", clearFiles);
                    UpdateControlColor(checkBox14, colors[2]);
                }
                if (checkBox15.Checked)
                {
                    //Clear Contacts
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Desktop Shortcuts";
                    }));
                    UpdateControlColor(checkBox15, colors[1]);
                    string desktopPath = GetPath(KnownFolder.Desktop);
                    int clearFiles = CleanFilesAndDirs(desktopPath);
                    logs.Add("Desktop Shortcuts", clearFiles);
                    UpdateControlColor(checkBox15, colors[2]);
                }
                if (checkBox16.Checked)
                {
                    Invoke(new Action(() => {
                        button2.Text = "Cleaning Web Data";
                    }));
                
                    UpdateControlColor(checkBox16, colors[1]);
                    ChromeData chromeData = new ChromeData();
                    CloseAllBrowsers(); 

                    //Clearing chrome history
                    List<string> urls = chromeData.ClearHistory();
                    UpdateControlColor(label8, colors[1]);
                    try
                    {
                        if (urls != null)
                        {
                            logs.Add("Web History", urls.Count);
                            foreach (string url in urls)
                            {
                                mutex.WaitOne(1000);
                                Invoke(new Action(() => {
                                    label22.Text = url;
                                    label22.Invalidate();
                                    label22.Refresh();
                                }));
                                mutex.ReleaseMutex();
                                Thread.Sleep(random);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    UpdateControlColor(label8, colors[2]);

                    //Clearing chrome bookmarks
                    List<string> titles = chromeData.ClearBookmarks();
                    UpdateControlColor(label9, colors[1]);
                    if (titles != null)
                    {
                        logs.Add("Web Bookmarks", titles.Count);
                        foreach (string title in titles)
                        {
                            mutex.WaitOne(1000);
                            Invoke(new Action(() => {
                            label22.Text = title;
                            label22.Invalidate();
                            label22.Refresh();
                        }));
                        mutex.ReleaseMutex();
                        Thread.Sleep(random);
                    }
                    }
                    UpdateControlColor(label9, colors[2]);

                    //Clearing chrome Cache
                    UpdateControlColor(label10, colors[1]);
                    string chromeCachePath = chromeData.ClearCache();
                    int count1 = CleanFilesAndDirs(chromeCachePath);
                    logs.Add("Web Browser Cache", count1);
                    UpdateControlColor(label10, colors[2]);

                    //Clearing chrome Cookies
                    UpdateControlColor(label11, colors[1]);
                    string chromeCookiesPath = chromeData.ClearCookies();
                    int count2 = CleanFilesAndDirs(chromeCookiesPath);
                    logs.Add("Web Cookies", count2);
                    UpdateControlColor(label11, colors[2]);

                    UpdateControlColor(label12, colors[1]);
                    List<string> downloads = chromeData.ClearDownloads();
                    if (downloads != null)
                    {
                        logs.Add("Web Browser Downlaods", downloads.Count);
                        foreach (string fileName in downloads)
                        {
                            mutex.WaitOne(1000);
                            Invoke(new Action(() => {
                                label22.Text = fileName;
                                label22.Invalidate();
                                label22.Refresh();
                            }));
                            mutex.ReleaseMutex();
                            Thread.Sleep(random);
                        }
                    }
                    UpdateControlColor(label12, colors[2]);


                    UpdateControlColor(label13, colors[1]);
                    List<string> favicons = chromeData.ClearFavicons();
                    if (favicons != null)
                    {
                        logs.Add("Web Browser Favicons", favicons.Count);
                        foreach (string favicon in favicons)
                        {
                            mutex.WaitOne(1000);
                            Invoke(new Action(() => {
                                label22.Text = favicon;
                                label22.Invalidate();
                                label22.Refresh();
                            }));
                            mutex.ReleaseMutex();
                            Thread.Sleep(random);
                        }
                    }
                    UpdateControlColor(label13, colors[2]);


                    UpdateControlColor(checkBox16, colors[2]);
                }
            //}));
            isCleaned = true;
            Invoke(new Action(() => {
                button2.Text = "Optimize Your PC";
                button2.BackColor = Color.FromArgb(192, 255, 192);
            }));
        }


        private int CleanFilesAndDirs(string path)
        {
            int filesCount = 0;
            if (CanRead(path))
            {
                double hours = 0;
                Invoke(new Action(() => {
                    if (comboBox2.Text != "All Time")
                    {
                        hours = double.Parse("-" + comboBox2.Text);
                    } 
                })); 
                DirectoryInfo di = new DirectoryInfo(path);

                try
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        try
                        {
                            if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && CanRead(path + "\\" + file.Name))
                            {
                                if (hours == 0)
                                {
                                    file.Delete();
                                }
                                else if (hours != 0 && file.CreationTime > DateTime.Now.AddHours(hours))
                                {
                                    file.Delete();
                                }
                                if (path.Contains("Google\\Chrome"))
                                {
                                    file.Delete();
                                }
                                filesCount++;
                                mutex.WaitOne(1000);
                                Invoke(new Action(() => {
                                    label22.Text = file.Name;
                                    label22.Invalidate();
                                    label22.Refresh();
                                }));
                                mutex.ReleaseMutex();
                                Thread.Sleep(random);
                            }
                            
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                            {
                                if (hours == 0)
                                {
                                    dir.Delete(true);                                
                                }
                                else if (hours != 0 && dir.CreationTime > DateTime.Now.AddHours(hours))
                                {
                                    dir.Delete(true);
                                }
                                filesCount++;
                                mutex.WaitOne(1000);
                                Invoke(new Action(() => {
                                    label22.Text = dir.Name;
                                    label22.Invalidate();
                                    label22.Refresh();
                                }));
                                mutex.ReleaseMutex();
                                Thread.Sleep(random);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
                }
                return filesCount;
            }
            return 0;
        }

        private int ClearWindowsEventLogs()
        {
            int eventCount = 0;
            try
            {
                double hours = 0;
                Invoke(new Action(() => {
                    if (comboBox2.Text != "All Time")
                    {
                        hours = double.Parse("-" + comboBox2.Text);
                    }
                }));
                //Open Event Viewer in windows 10 administrative event logs to view all windows logs
                EventLog[] logs = EventLog.GetEventLogs();
                foreach (EventLog log in logs)
                {
                    try
                    {
                        int totalEntries = log.Entries.Count;
                        string logName = log.LogDisplayName;
                        log.Clear();
                        eventCount++;
                        mutex.WaitOne(1000);
                        Invoke(new Action(() => {
                            label22.Text = logName + " log: " + totalEntries + " entries cleared";
                            label22.Invalidate();
                            label22.Refresh();
                        }));
                        mutex.ReleaseMutex();
                        Thread.Sleep(random);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                //Clear operational event logs
                EventLogQuery query = new EventLogQuery("SetUp", PathType.LogName);
                query.ReverseDirection = true;
                EventLogReader reader = new EventLogReader(query);
                EventRecord eventRecord;
                //while ((eventRecord = reader.ReadEvent()) != null)
                {
                }
            }
            catch (Exception)
            {
            }
            return eventCount;
        }




        enum RecycleFlag : int
        {
            SHERB_NOCONFIRMATION = 0x00000001,  // No confirmation, when emptying
            SHERB_NOPROGRESSUI = 0x00000001,    // No progress tracking window during the emptying of the recycle bin
            SHERB_NOSOUND = 0x00000004          // No sound when the emptying of the recycle bin is complete
        }
        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);

        public bool CanRead(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    return false;
                }
                var readAllow = false;
                var readDeny = false;
                var accessControlList = Directory.GetAccessControl(path);
                if (accessControlList == null)
                    return false;

                //get the access rules that pertain to a valid SID/NTAccount.
                var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                if (accessRules == null)
                    return false;

                //we want to go over these rules to ensure a valid SID has access
                foreach (System.Security.AccessControl.FileSystemAccessRule rule in accessRules)
                {
                    if ((System.Security.AccessControl.FileSystemRights.Read & rule.FileSystemRights) != System.Security.AccessControl.FileSystemRights.Read) continue;

                    if (rule.AccessControlType == System.Security.AccessControl.AccessControlType.Allow)
                        readAllow = true;
                    else if (rule.AccessControlType == System.Security.AccessControl.AccessControlType.Deny)
                        readDeny = true;
                }
                return readAllow && !readDeny;
            }
            catch (UnauthorizedAccessException ex)
            {
                return false;
            }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath shape = new GraphicsPath();
            //Point[] points = new Point[4]
            //{
            //    new Point(0, 0),
            //    new Point(100, 100),
            //    new Point(1000,1000),
            //    new Point(6000,6000),
            //};
            //Rectangle rectangle = new Rectangle(Location.X, Location.Y, Width, Height);
            //shape.AddEllipse(0,0,200,100);
            //Region = new Region(shape);
            //shape.Dispose();
        }




        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show(this, pictureBox1.Location.X + 3, pictureBox1.Location.Y + pictureBox1.Height - 5);
        }


        private void Home_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0; 
        }

        private void UpdateControlColor(Control control, Color color)
        {
            Invoke(new Action(() => {
                control.ForeColor = color;
                control.Invalidate();
                control.Refresh();
            }));
        }

        public void CloseAllBrowsers()
        {
            Process[] ieProcessNames = Process.GetProcessesByName("iexplore");
            foreach (Process item in ieProcessNames)
            {
                item.Kill();
            }
            Process[] fProcessNames = Process.GetProcessesByName("firefox");
            foreach (Process item in fProcessNames)
            {
                item.Kill();
            }
            Process[] cProcessNames = Process.GetProcessesByName("chrome");
            foreach (Process item in cProcessNames)
            {
                item.Kill();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string menuClicked = e.ClickedItem.Text;
            if (menuClicked == "About Us")
            {
                About about = new About();
                about.ShowDialog();
            }
            else if (menuClicked == "Contact Us")
            {
                Contact contact = new Contact();
                contact.ShowDialog();
            }
        }

        private float angle = 0;

        private void Rotate(Graphics graphics)
        {
            angle += 5 % 360;
            LinearGradientBrush brush = new LinearGradientBrush(
              new Rectangle(button2.Left - 10, button2.Top -10, button2.Width + 20, button2.Height + 20),
              Color.Orange,
              Color.Green,
              0.0F,
              true);

            brush.RotateTransform(angle);
            brush.SetBlendTriangularShape(.5F);
            graphics.FillRectangle(brush, brush.Rectangle);           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Rotate(CreateGraphics());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Summary summary = new Summary(logs);
            summary.ShowDialog();
        }
    }
}
