using DisasterNGOpart1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Security.Cryptography;
namespace DisasterNGOpart1.Controllers
{
    //begining of part 1
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        string conn = "Server=tcp:st10152771.database.windows.net,1433;Initial Catalog=djpromo;Persist Security Info=False;User ID=djadmin;Password=Mazibuko@9300;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        DAFEntities dbs = new DAFEntities();

        //GET: Donator
        public ActionResult RegisterUserList()
        {
            var sum = dbs.Mon_Donation.Select(mon => mon.Mon_Amt).Sum();
            ViewBag.Sum = sum;
            return View(dbs.Registers.ToList());
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registers(Register reg)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                String query = "INSERT INTO Register VALUES (@Username, @FirstName, @LastName, @Email, @Reg_Password)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", reg.Username);
                    cmd.Parameters.AddWithValue("@FirstName", reg.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", reg.LastName);
                    cmd.Parameters.AddWithValue("@Email", reg.Email);
                    cmd.Parameters.AddWithValue("@Reg_Password", reg.Reg_Password);

                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            return RedirectToAction("Login", "Admin");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(Register registers)
        {
            var confirmLogin = dbs.Registers.Where(x => x.Username.Equals(registers.Username) && x.Reg_Password.Equals(registers.Reg_Password)).FirstOrDefault();
            if (confirmLogin != null)
            {
                Session["Username"] = registers.Username.ToString();
                Session["Password"] = registers.Reg_Password.ToString();
                MessageBox.Show("Donator has been successfully logged in ");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                MessageBox.Show("Wrong Username or Password");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Disaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Disaster(Disaster dis)
        {
            if (Session["Username"] == null || !(Session["Username"] is string username))
            {

                return RedirectToAction("Disaster_List");
            }
            else
            {
                using (SqlConnection Sql = new SqlConnection(conn))
                {
                    Sql.Open();

                    // Remove @Username from the SQL query
                    string djpromo = "Insert into Disaster values (@StartDate, @EndDate, @Location, @Dis_Description, @AidTypes, @Username)";
                    using (SqlCommand com = new SqlCommand(djpromo, Sql))
                    {
                        com.CommandType = CommandType.Text;

                        com.Parameters.AddWithValue("@StartDate", dis.StartDate);
                        com.Parameters.AddWithValue("@EndDate", dis.EndDate);
                        com.Parameters.AddWithValue("@Location", dis.Location);
                        com.Parameters.AddWithValue("@Dis_Description", dis.Dis_Description);
                        com.Parameters.AddWithValue("@AidTypes", dis.AidTypes);
                        com.Parameters.AddWithValue("@Username", Session["Username"]);

                        com.ExecuteNonQuery();
                        Sql.Close();

                        MessageBox.Show("Disaster has been captured successfully!");

                        return RedirectToAction("Disaster_List");
                    }
                }

            }
        }
        public ActionResult Disaster_Details(int id, Disaster dis)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM Disaster WHERE Disaster_ID = @Disaster_ID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@Disaster_ID", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    dis.Disaster_ID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    dis.StartDate = Convert.ToDateTime(dt.Rows[0][2].ToString());
                    dis.EndDate = Convert.ToDateTime(dt.Rows[0][2].ToString());
                    dis.Location = dt.Rows[0][1].ToString();
                    dis.Dis_Description = dt.Rows[0][1].ToString();
                    dis.AidTypes = dt.Rows[0][1].ToString();

                    return View(dis);
                }
                else
                {
                    return RedirectToAction("Disaster_List");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        public ActionResult Goods_Donations()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Goods_Donations(Goods_Donations goods)
        {

            if (Session["Username"] == null || !(Session["Username"] is string username))
            {


                return RedirectToAction("Goods_Donations_list");
            }
            else
            {
                using (SqlConnection Sql = new SqlConnection(conn))
                {
                    Sql.Open();

                    string djpromo = "INSERT INTO Goods_Donations VALUES (@Goods_Date, @No_of_Items, @Category, @Item_description, @Annonymous)";


                    using (SqlCommand com = new SqlCommand(djpromo, Sql))
                    {
                        com.CommandType = CommandType.Text;


                        com.Parameters.AddWithValue("@Goods_Date", goods.Goods_Date);
                        com.Parameters.AddWithValue("@No_of_items", goods.No_of_items);
                        com.Parameters.AddWithValue("@Category", goods.Category);
                        com.Parameters.AddWithValue("@Item_description", goods.Item_description);
                        com.Parameters.AddWithValue("@Annonymous", goods.Annonymous);

                        com.ExecuteNonQuery();
                        Sql.Close();

                        MessageBox.Show("Goods Donation has been captured successfully!");

                        return RedirectToAction("Goods_Donations_list");

                    }

                }
            }
        }
        public ActionResult Goods_Donations_Details(int id, Goods_Donations goods)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM Goods_Donations WHERE Goods_ID = @Goods_ID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@Goods_ID", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    goods.Goods_ID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    goods.Good_Name = dt.Rows[0][1].ToString();
                    goods.Goods_Date = Convert.ToDateTime(dt.Rows[0][2].ToString());
                    goods.No_of_items = Convert.ToInt32(dt.Rows[0][3].ToString());
                    goods.Category = dt.Rows[0][1].ToString();
                    goods.Item_description = dt.Rows[0][1].ToString();
                    goods.Username = dt.Rows[0][1].ToString();
                    goods.Annonymous = dt.Rows[0][0].ToString();
                    return View(goods);
                }
                else
                {
                    return RedirectToAction("Goods_List");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }




        [HttpGet]
        public ActionResult Mon_Donation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Mon_Donation(Mon_Donation mon)
        {
            if (mon.Mon_Date == null || mon.Mon_Date == DateTime.MinValue)
            {
                ModelState.AddModelError("Mon_Date", "Mon_Date is required.");
                return View();
            }
            if (Session["Username"] == null || !(Session["Username"] is string username))
            {


                return RedirectToAction("Mon_Donation_list");
            }
            else
            {


                using (SqlConnection Sql = new SqlConnection(conn))
                {
                    Sql.Open();

                    string djpromo = "INSERT INTO Mon_Donation VALUES (@Mon_Date, @Mon_Amt,@Annonymous)";

                    using (SqlCommand com = new SqlCommand(djpromo, Sql))
                    {

                        com.CommandType = CommandType.Text;



                        com.Parameters.AddWithValue("@Mon_Date", mon.Mon_Date);
                        com.Parameters.AddWithValue("@Mon_Amt", mon.Mon_Amt);
                        com.Parameters.AddWithValue("@Annonymous", mon.Annonymous);
                        com.ExecuteNonQuery();
                        Sql.Close();

                        MessageBox.Show("Monetary Donations has been captured successfully!");

                        return RedirectToAction("Mon_Donation_list");
                    }
                }
            }
        }
        public ActionResult Mon_Donation_Details(int id, Mon_Donation mon)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM Mon_Donation WHERE Mon_ID = @Mon_ID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@Mon_ID", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    mon.Mon_ID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    mon.Mon_Name = dt.Rows[0][1].ToString();
                    mon.Username = dt.Rows[0][1].ToString();
                    mon.Mon_Date = Convert.ToDateTime(dt.Rows[0][2].ToString());
                    mon.Mon_Amt = Convert.ToInt32(dt.Rows[0][0].ToString());
                    mon.Annonymous = dt.Rows[0][0].ToString();
                    return View(mon);
                }
                else
                {
                    return RedirectToAction("Mon_Donation_List");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult Mon_Donation_List()
        {
            List<Mon_Donation> monetaryDonations = new List<Mon_Donation>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from Mon_Donation", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Mon_Donation don = new Mon_Donation
                    {
                        Mon_ID = Convert.ToInt32(row["Mon_ID"]),
                        Mon_Date = Convert.ToDateTime(row["Mon_Date"]),
                        Mon_Amt = Convert.ToDecimal(row["Mon_Amt"]),
                        Annonymous = row["Annonymous"].ToString()
                    };

                    monetaryDonations.Add(don);
                }
            }

            return View(monetaryDonations);
        }

        public ActionResult Goods_Donations_List()
        {
            List<Goods_Donations> goodsDonations = new List<Goods_Donations>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from Goods_Donations", con);
                DataTable data = new DataTable();
                ad.Fill(data);

                foreach (DataRow row in data.Rows)
                {
                    Goods_Donations good = new Goods_Donations
                    {
                        Goods_ID = Convert.ToInt32(row["Goods_ID"]),
                        Good_Name = row["Good_Name"].ToString(),
                        Goods_Date = Convert.ToDateTime(row["Goods_Date"]),
                        No_of_items = Convert.ToInt32(row["No_of_items"]),
                        Category = row["Category"].ToString(),
                        Item_description = row["Item_description"].ToString(),
                        Annonymous = row["Annonymous"].ToString()
                    };

                    goodsDonations.Add(good);
                }
            }


            return View(goodsDonations);
        }
        public ActionResult Disaster_List()
        {
            List<Disaster> disasters = new List<Disaster>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from Disaster", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Disaster disa = new Disaster
                    {
                        Disaster_ID = Convert.ToInt32(row["Disaster_ID"]),
                        StartDate = Convert.ToDateTime(row["StartDate"]),
                        EndDate = Convert.ToDateTime(row["EndDate"]),
                        Location = row["Location"].ToString(),
                        Dis_Description = row["Dis_Description"].ToString(),
                        AidTypes = row["Dis_Description"].ToString(),

                    };
                    disasters.Add(disa);
                }
            }
            return View(disasters);

        }
        public ActionResult DisasterCreate()
        {
            return View();
        }
        public ActionResult DisasterAdd(Disaster dis)
        {
            DAFEntities db = new DAFEntities();
            db.Disasters.Add(dis);
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");
        }
        public ActionResult DisasterEdit(string Username)
        {
            DAFEntities db = new DAFEntities(); ;
            Disaster dis = db.Disasters.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(dis);

        }
        public ActionResult DisasterSubmit(Disaster e)
        {
            DAFEntities db = new DAFEntities();
            Disaster dis = db.Disasters.Where(d => d.Username == e.Username).FirstOrDefault();

            dis.Username = e.Username;
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");

        }
        public ActionResult Disaster_Details(string Username)
        {
            DAFEntities db = new DAFEntities();
            Disaster dis = db.Disasters.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(dis);
        }
        public ActionResult GoodsCreate()
        {
            return View();
        }
        public ActionResult GoodsAdd(Goods_Donations good)
        {
            DAFEntities db = new DAFEntities();
            db.Goods_Donations.Add(good);
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");
        }
        public ActionResult GoodsEdit(string Username)
        {
            DAFEntities db = new DAFEntities();
            Goods_Donations good = db.Goods_Donations.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(good);

        }
        public ActionResult GoodsSubmit(Disaster e)
        {
            DAFEntities db = new DAFEntities();
            Goods_Donations good = db.Goods_Donations.Where(d => d.Username == e.Username).FirstOrDefault();

            good.Username = e.Username;
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");

        }
        public ActionResult Goods_Donation_Details(string Username)
        {
            DAFEntities db = new DAFEntities();
            Goods_Donations good = db.Goods_Donations.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(good);
        }
        public ActionResult MonCreate()
        {
            return View();
        }
        public ActionResult MonAdd(Mon_Donation mon)
        {
            DAFEntities db = new DAFEntities();
            db.Mon_Donation.Add(mon);
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");
        }
        public ActionResult MonEdit(string Username)
        {
            DAFEntities db = new DAFEntities();
            Mon_Donation good = db.Mon_Donation.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(good);

        }
        public ActionResult MonSubmit(Mon_Donation e)
        {
            DAFEntities db = new DAFEntities();
            Mon_Donation good = db.Mon_Donation.Where(d => d.Username == e.Username).FirstOrDefault();

            good.Username = e.Username;
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");

        }
        public ActionResult Mon_Donation_Details(string Username)
        {
            DAFEntities db = new DAFEntities();
            Mon_Donation good = db.Mon_Donation.Where(d => d.Username == Username).FirstOrDefault();

            db.Dispose();
            return View(good);
        }

        //begining of part 2
        //public ActionResult Mon_Allocation()
        //{
        //    var balance = dbs.Mon_Donation.Select(mon => mon.Mon_Amt).Sum() - dbs.Mon_Allocation.Select(all => all.MonAll_Amt).Sum() - dbs.Goods_Purchase.Select(pu => pu.GoodsPurch_Amt).Sum();
        //    ViewBag.Available = balance;
        //    var disa = dbs.Disasters.ToList();
        //    //ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");
        //    ViewBag.DisasterList = new SelectList(disa, "Disaster_Id", "Dis_Description");

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Mon_Allocation(Mon_Allocation money)
        //{
        //    var maxEndDate = dbs.Disasters.Max(d => d.EndDate);
        //    if (money.MonAll_Date < maxEndDate)
        //    {
        //        dbs.Mon_Allocation.Add(money);
        //        dbs.SaveChanges();
        //        TempData["Message"] = "Allocation successful"; // Provide feedback
        //        return View(); // Redirect to a different view
        //    }
        //    else
        //    {
        //        ViewBag.error = "Disaster is not active";
        //        return View();
        //    }
        //}
        //public ActionResult Goods_Allocation()
        //{
        //    var availableGoods = dbs.Goods_Donations.Select(number => number.No_of_items).Sum() - dbs.Goods_Allocation.Select(good => good.No_of_item).Sum() + dbs.Goods_Purchase.Select(items => items.No_of_items).Sum();

        //    ViewBag.goodAvailable = availableGoods;

        //    var disasters = dbs.Disasters.ToList();
        //    var goods = dbs.Goods_Donations.ToList();
        //    ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");
        //    ViewBag.GoodsList = new SelectList(goods, "GoodId", "Category");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Goods_Allocation(Goods_Allocation goods)
        //{
        //    var maxDate = dbs.Disasters.Max(d => d.EndDate);
        //    if (goods.GoodsAll_Date < maxDate)
        //    {
        //        dbs.Goods_Allocation.Add(goods);
        //        dbs.SaveChanges();
        //        return View();
        //    }
        //    else
        //    {
        //        ViewBag.error = "Disaster is not active";
        //        return View();
        //        // Handle the case where AllocationDate is not less than maxEndDate
        //    }

        //}
        //public ActionResult Goods_Purchase()
        //{
        //    var balance = dbs.Mon_Donation.Select(money => money.Mon_Date).Sum() - dbs.Mon_Allocation.Select(allo => allo.MonAll_Amt).Sum() - dbs.Goods_Purchase.Select(purch => purch.GoodsPurch_Amt).Sum();
        //    ViewBag.Available = balance;
        //    var disasters = balance.Disasters.ToList();
        //    var goods = balance.GoodDonations.ToList();
        //    ViewBag.Disaster_list = new SelectList(disasters, "DisasterId", "Description");
        //    ViewBag.Goods_Donation_list = new SelectList(goods, "GoodId", "Category");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Goods_Purchase(Goods_Purchase pur)
        //{

        //    dbs.Goods_Purchase.Add(pur);
        //    dbs.SaveChanges();
        //    return View();

        //}


        //public ActionResult Goods_Purchase(string Username)
        //{
        //    return View();

        //}




        //public ActionResult DisplayMon_Allocation()
        //{
        //    List<Mon_Allocation> money = new List<Mon_Allocation>();
        //    using (SqlConnection con = new SqlConnection(conn))
        //    {
        //        {
        //            con.Open();
        //            SqlDataAdapter adapter = new SqlDataAdapter("Select Mon_Allocation.MonAll_Date, Mon_Allocation.MonAll_Amt, Disaster.Dis_Description FROM Mon_Allocation.MonAll_Amt,Disaster.Dis_Description WHERE Mon_Allocation.Disaster_ID = Disaster.Disaster_ID");

        //            DataTable data = new DataTable();
        //            adapter.Fill(data);

        //            foreach (DataRow rows in data.Rows)
        //            {
        //                Mon_Allocation allo = new Mon_Allocation
        //                {
        //                    MonAll_Date = (DateTime)rows["MonAll_Date"],
        //                    MonAll_Amt = (decimal)rows["MonAll_Amt"],
        //                    Disaster = new Disaster
        //                    {
        //                        Dis_Description = rows["Dis_Description"].ToString(),
        //                    }
        //                };
        //                money.Add(allo);
        //            }
        //        }

        //        return View(money);
        //    }
        //}
        //public ActionResult DisplayGoods_Allocation()
        //{
        //    List<Goods_Allocation> goods1 = new List<Goods_Allocation>();
        //    using (SqlConnection con = new SqlConnection(conn))
        //    {
        //        {
        //            con.Open();
        //            SqlDataAdapter adapter = new SqlDataAdapter("Select Goods_Allocation.GoodsPurch_Date, Goods_Allocation.GoodsPurch_Amt, Goods_Allocation.No_of_items, Disaster.Dis_Description" + "Goods_Donations.Category" + "FROM Goods_Purchase, Disaster, Goods_Donations" + ".MonAll_Amt,Disaster.Dis_Description WHERE Goods_Allocation = Disaster.Disaster_ID AND Goods_Purchase.Goods_ID = Goods_Donations.Goods_ID", con);

        //            DataTable data = new DataTable();
        //            adapter.Fill(data);

        //            foreach (DataRow rows in data.Rows)
        //            {
        //                Goods_Allocation good2 = new Goods_Allocation
        //                {
        //                    GoodsAll_Date = (DateTime)rows["GoodsAll_Date"],
        //                    No_of_item = (int)rows["No_of_item"],
        //                    Disaster = new Disaster
        //                    {
        //                        Dis_Description = rows["Dis_Description"].ToString(),
        //                    },
        //                    GoodsDonations = new Goods_Donations
        //                    {
        //                        Category = rows["Item_description"].ToString(),
        //                    }
        //                };
        //                goods1.Add(good2);
        //            }
        //        }

        //        return View(goods1);
        //    }
        //}

        //Allocate Money
        [HttpPost]
        public ActionResult AllocateMoney(Mon_Allocation mon_Allocation)
        {
            SqlCommand cmd;
            string connectionstring = "Server=tcp:st10152771.database.windows.net,1433;Initial Catalog=djpromo;Persist Security Info=False;User ID=djadmin;Password=Mazibuko@9300;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();//open connection
            string query = "INSERT INTO MoneyAllocate (DisasterID,AllocatedMoney) VALUES (@DisasterID,@AllocatedMoney)";
            cmd = new SqlCommand(query, conn);//using the command 

            cmd.Parameters.AddWithValue("@DisasterID", mon_Allocation.DisasterId);
            cmd.Parameters.AddWithValue("@AllocatedMoney", mon_Allocation.AllocatedAmount);

            cmd.ExecuteNonQuery();
            conn.Close();//Close connecting
            return View("AllocateMoneySuccessView");
        }

        [HttpGet]
        public ActionResult AllocateMoney()
        {
            return View();
        }

        //Alocate Goods
        [HttpPost]
        public ActionResult AllocateGoods(Goods_Allocation goodsAllocation)
        {
            SqlCommand cmd;
            string connectionstring = "Server=tcp:st10152771.database.windows.net,1433;Initial Catalog=djpromo;Persist Security Info=False;User ID=djadmin;Password=Mazibuko@9300;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();//open connection
            string query = "INSERT INTO GoodsAllocate (DisasterID,DisasterName,Goods,NumOfItems) VALUES (@DisasterID,@DisasterName,@Goods,@NumOfItems)";
            cmd = new SqlCommand(query, conn);//using the command 

            cmd.Parameters.AddWithValue("@DisasterID", goodsAllocation.DisasterId);
            cmd.Parameters.AddWithValue("@DisasterName", goodsAllocation.DisasterName);
            cmd.Parameters.AddWithValue("@Goods", goodsAllocation.Goods);
            cmd.Parameters.AddWithValue("@NumOfItems", goodsAllocation.NumOfItems);


            cmd.ExecuteNonQuery();
            conn.Close();//Close connecting

            return View("AllocateGoodsSuccessView");
        }
        [HttpGet]
        public ActionResult AllocateGoods()
        {
            return View();
        }

        //Capture purchase
        [HttpPost]
        public ActionResult PurchaseCapture(Goods_Purchase purchase)
        {
            decimal avaAmount = CalAvaAmount();
            if (purchase.Amount > avaAmount)
            {
                return RedirectToAction("Index");
            }

            SqlCommand cmd;
            string connectionstring = "Server=tcp:st10152771.database.windows.net,1433;Initial Catalog=djpromo;Persist Security Info=False;User ID=djadmin;Password=Mazibuko@9300;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();//open connection
            string query = "INSERT INTO GoodsPurchase (GoodsName,Quantity,Price,DisasterID) VALUES (@GoodsName,@Quantity,@Price,@DisasterID)";
            cmd = new SqlCommand(query, conn);//using the command 

            cmd.Parameters.AddWithValue("@GoodsName", purchase.GoodsName);
            cmd.Parameters.AddWithValue("@Quantity", purchase.Quantity);
            cmd.Parameters.AddWithValue("@Price", purchase.Amount);
            cmd.Parameters.AddWithValue("@DisasterID", purchase.DisasterId);


            cmd.ExecuteNonQuery();
            conn.Close();//Close connecting

            UpdatedAvaAmount(avaAmount - purchase.Amount);
            decimal upt = avaAmount - purchase.Amount;
            ViewBag.upt = upt;
            return View("PurchaseDone");

            decimal CalAvaAmount()
            {
                decimal AvaAmount = 10000 * 20;
                return AvaAmount;
            }


            AvailMoney(avaAmount - purchase.Amount);
            decimal go = avaAmount - purchase.Amount;
            ViewBag.go = go;
            return View("PurchaseDone");



            decimal CalAvailMoney()
            {
                decimal AvaAmount = 10000 * 20;
                return AvaAmount;
            }

            AvaActiveDis(avaAmount - purchase.Amount);
            decimal dis = avaAmount - purchase.Amount;
            ViewBag.dis = dis;
            return View("PurchaseDone");



            decimal CalAvaActiveDis()
            {
                decimal AvaAmount = 10000 * 20;
                return AvaAmount;
            }
        }
        private void UpdatedAvaAmount(decimal NewAmount)
        {

        }


        private void CalAvailMoney(decimal NewAmount)
        {

        }

        private void AvailMoney(decimal NewAmount)
        {

        }

        private void AvaActiveDis(decimal NewAmount)
        {

        }


        private void CalAvaActiveDis(decimal NewAmount)
        {

        }



        [HttpGet]
        public ActionResult PurchaseCapture()
        {
            return View();
        }

        [HttpPost]

        [HttpGet]
        public ActionResult MonPurchaseCapture()
        {
            return View();
        }
    }
}


    