using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace AutoCleaner
{
    class ChromeData
    {
        
        public string ClearCache()
        {
            string chromeCacheDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cache\Cache_Data";
            return chromeCacheDirPath;   
        } 
        public List<string> ClearHistory()
        {
            //GOOGLE CHROME
            //C:\Users\admin\AppData\Local\Google\Chrome\User Data\Default\History
            //History is sqlite database file
            //It contains two tables, urls and visits

            try
            {
                string chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                if (File.Exists(chromeHistoryFile))
                {
                    SqliteConnection connection = new SqliteConnection("Data Source=" + chromeHistoryFile + ";");
                    connection.Open();
                    string query = "select * from urls order by last_visit_time desc";
                    SqliteCommand sqliteCommand = new SqliteCommand(query, connection);

                    List<string> urls = new List<string>();
                    using (var reader = sqliteCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                //col-0: primary key    
                                //col-3: Number of time URL was accessed
                                //col-5: Last visit timestamp
                                var url = reader.GetString(1);
                                var title = reader.GetString(2);
                                long tick = long.Parse(reader.GetString(5));
                                var dateTime = new DateTime(tick);
                                urls.Add(url);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                    string deleteQuery = "delete from urls";
                    SqliteCommand deleteCommand = new SqliteCommand(deleteQuery, connection);
                    deleteCommand.ExecuteNonQuery();
                    string deleteQuery1 = "delete from visits";
                    SqliteCommand deleteCommand1 = new SqliteCommand(deleteQuery1, connection);
                    deleteCommand1.ExecuteNonQuery();
                    return urls;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<string> ClearDownloads()
        {
            try
            {
                string chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                List<string> downloads = new List<string>();
                if (File.Exists(chromeHistoryFile))
                {
                    try
                    {
                        SqliteConnection connection = new SqliteConnection("Data Source=" + chromeHistoryFile + ";");
                        connection.Open();

                        SqliteCommand command = new SqliteCommand("select * from downloads", connection);
                        SqliteDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            try
                            {
                                string fileName = dataReader.GetString(2);
                                fileName = fileName.Substring(fileName.LastIndexOf("\\")).Replace("\\", "");
                                downloads.Add(fileName);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }


                        string deleteQuery = "delete from downloads";
                        SqliteCommand deleteCommand = new SqliteCommand(deleteQuery, connection);
                        deleteCommand.ExecuteNonQuery();
                        string deleteQuery1 = "delete from downloads_url_chains";
                        SqliteCommand deleteCommand1 = new SqliteCommand(deleteQuery1, connection);
                        deleteCommand1.ExecuteNonQuery();

                        return downloads;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }



        public string ClearCookies()
        {
            try
            {
                string chromeCookiesDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Network";
                return chromeCookiesDirPath;
            }
            catch (Exception)
            {
            }
            return null;
            //List<string> cookies = new List<string>();
            //if (File.Exists(chromeCookiesDirPath))
            //{
            //    SqliteConnection connection = new SqliteConnection("Data Source=" + chromeCookiesDirPath + ";");
            //    connection.Open();
            //    string query = "select host_key, name, value, encrypted_value from cookies";
            //    SqliteCommand sqliteCommand = new SqliteCommand(query, connection);
            //    SqliteDataReader reader = sqliteCommand.ExecuteReader();
         
            //    while (reader.Read())
            //    {
            //        //col-0: primary key    
            //        //col-3: Number of time URL was accessed
            //        //col-5: Last visit timestamp
            //        var host = reader.GetString(0);
            //        var name = reader.GetString(1);
                
            //    }
  
            //    string deleteQuery = "delete from cookies";
            //    SqliteCommand deleteCommand = new SqliteCommand(deleteQuery, connection);
            //    deleteCommand.ExecuteNonQuery();
            //}
            //return cookies;
        }

        public List<string> ClearBookmarks()
        {
            string chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Bookmarks";
            List<string> titles = new List<string>();
            if (File.Exists(chromeHistoryFile))
             {
                try
                {
                    string bookmarks = File.ReadAllText(chromeHistoryFile);
                    JsonReader reader = new JsonTextReader(new StringReader(bookmarks));
                    while (reader.Read())
                    {
                        try
                        {
                            if (reader.Depth == 5)
                            {
                                string a = reader.ReadAsString();
                                reader.Read();
                                string b = reader.ReadAsString();
                                reader.Read();
                                string c = reader.ReadAsString();
                                reader.Read();
                                string title = reader.ReadAsString();
                                reader.Read();
                                string e = reader.ReadAsString();
                                reader.Read();
                                string url = reader.ReadAsString();
                                titles.Add(title);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    var json = File.ReadAllText(chromeHistoryFile);
                    File.Delete(chromeHistoryFile);
                }
                catch (Exception)
                {
                }
            }
            
            //var jObject = JObject.Parse(json);
            //JArray arr = (JArray)jObject["roots"];
            //bool k = jObject.Remove("children");
            //string output = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            //File.WriteAllText(chromeHistoryFile, output);

            return titles;
        }

        

        public List<string> ClearFavicons()
        {
            string chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Favicons";
            List<string> favicons = new List<string>();
            if (File.Exists(chromeHistoryFile))
            {
                try
                {
                    SqliteConnection connection = new SqliteConnection("Data Source=" + chromeHistoryFile + ";");
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("select * from favicons", connection);
                    SqliteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        try
                        {
                            string fileName = dataReader.GetString(1);
                            favicons.Add(fileName);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }


                    string deleteQuery = "delete from favicons";
                    SqliteCommand deleteCommand = new SqliteCommand(deleteQuery, connection);
                    deleteCommand.ExecuteNonQuery();
                    string deleteQuery1 = "delete from favicon_bitmaps";
                    SqliteCommand deleteCommand1 = new SqliteCommand(deleteQuery1, connection);
                    deleteCommand1.ExecuteNonQuery();
                    string deleteQuery2 = "delete from icon_mapping";
                    SqliteCommand deleteCommand2 = new SqliteCommand(deleteQuery2, connection);
                    deleteCommand2.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
            }
            return favicons;
        }

    }
}
