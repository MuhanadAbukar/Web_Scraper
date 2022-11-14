using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data.SqlClient;

internal class Program
{
    static string connstr = "Data Source=DESKTOP-QJBSGQ4\\MSSQLSERVER01;Initial Catalog=mslearnEXP;Persist Security Info=True;User ID=sa;Password=muhanad123";
    static SqlConnection conn = new SqlConnection(connstr);

    static void Main(string[] args)
    {
        var us = File.ReadAllText(@"C:\Users\muhan\Desktop\usernames.txt");
        var users = us.Split(",");
        var userLevelTemp = new Dictionary<string, int>();
        var userExpTemp = new Dictionary<string, string>();
        foreach (string user in users)
        {
            void getReal()
            {
                var driver = new ChromeDriver();
                driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
                driver.Navigate().GoToUrl($"https://learn.microsoft.com/en-us/users/{user}");
                Thread.Sleep(2500);
                var EXP = driver.FindElement(By.Id($@"level-status-points"));
                var LEVEL = driver.FindElement(By.Id($@"level-status-text"));
                var PROFILENAME = driver.FindElement(By.XPath($@"//*[@id=""profile-hero-section""]/div[1]/div/div/div[2]/div[1]/h1"));
                var temp = EXP.Text.Replace("XP", "").Split("/");
                var xp = temp[0].Replace(",", "");
                var outofxp = temp[1].Replace(",", "");
                var cmd = new SqlCommand($"IF EXISTS (SELECT * FROM users WHERE Username = '{user}') BEGIN update users set exp = {xp},expoutof = {outofxp}, levels = {LEVEL.Text.Replace("LEVEL ", "")} where username='{user}' END ELSE BEGIN insert into users values('{user}',{xp},{outofxp},{LEVEL.Text.Replace("LEVEL ", "")},'{PROFILENAME.Text}') END", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.Clear();
                Console.WriteLine(PROFILENAME.Text);
                userLevelTemp.Add(PROFILENAME.Text + " " + user, int.Parse(LEVEL.Text.Replace("LEVEL ", "")));
                userExpTemp.Add(PROFILENAME.Text + " " + user, EXP.Text.Replace("XP", "").Replace(" ", "") + " EXP");
                driver.Quit();
            }
            var thread = new Thread(getReal);
            thread.Start();
            Thread.Sleep(800);
        }
        Thread.Sleep(4000);
        Console.Clear();
        var userLevel = userLevelTemp.OrderByDescending(k => k.Value);
        var userExp = userExpTemp.OrderByDescending(k => k.Value);
        Console.WriteLine("LEVELS");
        Console.WriteLine(string.Join("\n", userLevel));
        Console.WriteLine();
        Console.WriteLine("EXP");
        Console.WriteLine(string.Join("\n", userExp));
        Console.WriteLine("\n\n\n");
        Console.ReadLine();
    }
}