using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApp1
{
    public partial class ParticipantRegisterationForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        public ParticipantRegisterationForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void ParticipantRegisteration_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {
               
                con.Open();
                // Connection is now open and ready to use.

            }
            catch (Exception ex)
            {
                // Handle connection opening errors.
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }


        //private void SendEmail(string to, string subject, string body)
        //{
        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential("riyanazhar10@gmail.com", "rljdhlfubjicrgwr")
        //    };
        //    using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
        //    {
        //        client.Port = 587;
        //        client.Credentials = new NetworkCredential("riyanazhar10@gmail.com", "rljdhlfubjicrgwr"); 
        //        client.EnableSsl = true;
        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress("riyanazhar10@gmail.com");
        //        message.To.Add(to);
        //        message.Subject = subject;
        //        message.Body = body;
        //        client.Send(message);
        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string phone = textBox2.Text;
            string email = textBox3.Text;
            string selectedEvent = textBox4.Text;

            if (name == "" || phone == "" || name == "" || email == "")
            {
                MessageBox.Show("please fill the required terms");
            }
            else
            {
                if (checkBox1.Checked == false)
                {
                    MessageBox.Show("Please accept terms & conditions");
                }
                else
                {

                    user_regis();
                        MessageBox.Show("Registered Successfully");

                    
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void user_regis()
        {
            // Assuming 'con' is your OracleConnection object declared somewhere in your class.

            // Get the data from textboxes

            string name = textBox1.Text;
            string phone = textBox2.Text;
            string email = textBox3.Text;
            string eventName = textBox4.Text;
            int userId = loggedInUserId;

            // Construct the SQL query to insert data into the database with the sequence for USER_ID
            string insertQuery = "INSERT INTO USER_REGISTERATION (USER_ID,IDS_ID, NAME, PHONE, EMAIL, EVENT) VALUES (USER_ID_SEQ.NEXTVAL, :userId, :name, :phone, :email, :eventName)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                cmd.Parameters.Add(":name", OracleDbType.Char, 50).Value = name;
                cmd.Parameters.Add(":phone", OracleDbType.Varchar2).Value = phone;
                cmd.Parameters.Add(":email", OracleDbType.Char, 50).Value = email;
                cmd.Parameters.Add(":eventName", OracleDbType.Char, 50).Value = eventName;

                try
                {
                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    // Optionally, you can display a message indicating successful insertion
                    MessageBox.Show("User registration data inserted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle database insertion errors
                    MessageBox.Show("Error inserting user registration data: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    con.Close();
                }
            };
           

        }

    }
}