using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GlobeTable;

namespace sqlserverconnection
{

    public partial class Form1 : Form
    {
        GlobeTable globeTable= new GlobeTable();
        Row row=new Row();
        SqlConnection conn;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = GenerateCreateTableQuery(typeof(Row));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
   
                conn = new SqlConnection("Server=MYMON-PC;Database=Cglob;User Id=sa;Password = 1234;");
                conn.Open();
                richTextBox1.Text = "connection succesful";
            }
            catch(Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }
            //
            //rdr = cmd.ExecuteReader();
            //conn.Close();
            //conn.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (conn != null)
            {
                SqlDataReader rdr = null;
                SqlCommand cmd = new SqlCommand(richTextBox1.Text, conn);
                rdr = cmd.ExecuteReader();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        
            OpenFileDialog openFile = new OpenFileDialog();
            label1.Text = "loading from CSV";
            openFile.ShowDialog();
            globeTable.setFileName(openFile.FileName);

            //label1.Show();
            //Thread oThread = new Thread(new ThreadStart(globeTable.Load));
            //oThread.Start();
            globeTable.Load();
           // while (oThread.ThreadState == ThreadState.Running)
            //  ;

            label1.Text = "inserting to database";
            List<string> insertsList= globeTable.GetInsertValues();
            richTextBox1.Text = "loaded " + insertsList.Count + " rows";
            int i=0;
            foreach (string values in insertsList)
            {
                try
                {
                    SqlDataReader rdr = null;
                    String insertQuery = globeTable.getInsertInto() + values;
                    //richTextBox1.Text = insertQuery;
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);

                    rdr = cmd.ExecuteReader();
                    rdr.Close();
                    i++;
                    if (i % 5000 == 0)
                    {
                        Console.WriteLine(i + " rows inserted");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
           

        }
    }
}
