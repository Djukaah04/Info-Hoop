using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

using System.Drawing;
using System.IO;

namespace Projekat3
{
    public partial class Form1 : Form
    {
        private bool showTable = false;
        public Form1()
        {
            InitializeComponent();
            placeholderLbl.Visible = true;
            dataGridView1.Visible = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            if (textBox1.Text != "" && textBox8.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                var collection = db.GetCollection<Klub>("klubovi");
                Klub klub = new Klub { naziv = textBox1.Text, konferencija = textBox8.Text, divizija = textBox2.Text, vlasnik = textBox3.Text, dvorana = textBox4.Text };

                collection.Insert(klub);

                dataGridView1.Rows.Add(textBox1.Text, textBox8.Text, textBox2.Text, textBox3.Text, textBox4.Text, "");

                MessageBox.Show("Dodat "+textBox1.Text+".");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox8.Text = "";
            }

            else
            {
                MessageBox.Show("Popuni sva polja (naziv/konferencija/divizija/vlasnik/dvorana) !!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Klub>("klubovi");
            string naziv = textBox1.Text;
            var query = Query.EQ("naziv", naziv);

            //bool found = false;
            //foreach (DataGridViewRow r in dataGridView1.Rows)
            //{
            //    if (r.Cells[0].FormattedValue.ToString().Equals(textBox1.Text))
            //        found = true;
            //}
            //if (!found)
            //{
            //    MessageBox.Show("Nema takvog kluba!");
            //    return;
            //}

            if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox8.Text != "")
            {
                var update = MongoDB.Driver.Builders.Update.Set("konferencija", textBox8.Text)
                                                           .Set("divizija", textBox2.Text)
                                                           .Set("vlasnik", textBox3.Text)
                                                           .Set("dvorana", textBox4.Text);
                collection.Update(query, update);
                MessageBox.Show("Izmenjen klub "+ naziv +".");
            }
            else
            {
                MessageBox.Show("Popuni sva polja (konferencija/divizija/vlasnik/dvorana) !!");
                return;
            }

            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells["naziv"].FormattedValue.ToString().Equals(textBox1.Text))
                     .First();

            int rowIndex = row.Index;
            

            dataGridView1.Rows[rowIndex].Cells[1].Value = textBox8.Text;
            dataGridView1.Rows[rowIndex].Cells[2].Value = textBox2.Text;
            dataGridView1.Rows[rowIndex].Cells[3].Value = textBox3.Text; 
            dataGridView1.Rows[rowIndex].Cells[4].Value = textBox4.Text;

            textBox8.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Uneti naziv kluba!");
                return;
            }
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Klub>("klubovi");

            var query = Query.EQ("naziv", textBox1.Text);

            bool found = false;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[0].FormattedValue.ToString().Equals(textBox1.Text))
                    found = true;
            }
            if (!found)
            {
                MessageBox.Show("Nema takvog kluba!");
                return;
            }

            collection.Remove(query);

            DataGridViewRow row = dataGridView1.Rows
                   .Cast<DataGridViewRow>()
                   .Where(r => r.Cells["naziv"].Value.ToString().Equals(textBox1.Text))   
                   .FirstOrDefault();

        

            int rowIndex = row.Index;

            dataGridView1.Rows[rowIndex].Selected = true;

            dataGridView1.Rows.RemoveAt(rowIndex);
            textBox1.Text = "";
            textBox8.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            dataGridView1.Rows[rowIndex].Selected = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Uneti potrebne podatke!");
                return;
            }
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Klub>("klubovi");

            var query = Query.EQ("naziv", textBox1.Text);
            Klub klubovi = collection.FindOne(query);

            if(klubovi == null)
            {
                MessageBox.Show("Nema takvog kluba!");
                return;
            }

            if (klubovi.titule == null)
            {
               var  update = MongoDB.Driver.Builders.Update.Set("titule", BsonValue.Create(new List<string> { textBox5.Text }));
               collection.Update(query, update);
            }
            else
            {
                var update = MongoDB.Driver.Builders.Update.Push("titule", textBox5.Text);
                collection.Update(query, update);
            }

            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells["naziv"].Value.ToString().Equals(textBox1.Text))
                                 .First();

            int rowIndex = row.Index;

            dataGridView1.Rows[rowIndex].Cells[5].Value = dataGridView1.Rows[rowIndex].Cells[5].Value + "," + textBox5.Text;

            textBox5.Text = "";


        }

         private void Form1_Load(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            //db.CreateCollection("preduzece");

            var collection = db.GetCollection<Klub>("klubovi");

            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";
            dataGridView1.Columns[5].Name = "Titule";
            dataGridView1.Columns[6].Name = "Broj titula";

            string titule;
            int brojtitula;

            foreach (Klub k1 in collection.FindAll())
            {
                if (k1.titule != null)
                { titule = k1.titule.Aggregate((x, y) => x + "," + y); brojtitula = k1.titule.Count(); }
                else
                { titule = ""; brojtitula = 0; }

                dataGridView1.Rows.Add(k1.naziv, k1.konferencija, k1.divizija, k1.vlasnik, k1.dvorana, titule, brojtitula);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            //db.CreateCollection("preduzece");

            var collection = db.GetCollection<Klub>("klubovi");

            var result2 = (from klub in collection.AsQueryable<Klub>()
                           where klub.naziv == textBox1.Text
                           select klub).FirstOrDefault();

            if (result2 != null)
            { 
                textBox8.Text = result2.konferencija;
                textBox2.Text = result2.divizija;
                textBox3.Text = result2.vlasnik;
                textBox4.Text = result2.dvorana;
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub1Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;
           
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query = from klub in collection.AsQueryable<Klub>()
                        orderby klub.naziv
                        select klub;

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";
            dataGridView1.Columns[5].Name = "Titule";
            dataGridView1.Columns[6].Name = "Broj titula";

            string titule;
            int broj;

            foreach (Klub k in query)
            {
                if (k.titule != null)
                { 
                    titule = k.titule.Aggregate((x, y) => x + "," + y); broj = k.titule.Count();
                }
                else
                { 
                    titule = "";  broj = 0; 
                }

                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana, titule, broj);

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub2Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub1Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where klub.titule.Any()
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";
            dataGridView1.Columns[5].Name = "Broj titula";

            foreach (Klub k in query)
            {
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana, k.titule.Count());

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub3Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub1Pnl.BackColor = klub2Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where !(klub.titule.Any())
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencja";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";

            foreach (Klub k in query)
            {
                //   MessageBox.Show(k.naziv + " " + k.divizija  + " " + k.vlasnik + " " + k.dvorana);
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana);

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub6Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub1Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where klub.divizija == textBox6.Text
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";

            foreach (Klub k in query)
            {
                //   MessageBox.Show(k.naziv + " " + k.divizija  + " " + k.vlasnik + " " + k.dvorana);
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana);

            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub4Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub1Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where klub.titule.Count() == Convert.ToInt32(textBox7.Text)
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";

            foreach (Klub k in query)
            {
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub5Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub1Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where klub.konferencija == textBox9.Text
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";

            foreach (Klub k in query)
            {
                
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana);

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub7Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub1Pnl.BackColor = klub8Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 from klub in collection.AsQueryable<Klub>()
                 where klub.konferencija == textBox9.Text && klub.divizija == textBox6.Text
                 select klub;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";

            foreach (Klub k in query)
            {
                dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            placeholderLbl.Visible = false;

            klub8Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub1Pnl.BackColor = klub9Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Klub>("klubovi");

            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Naziv";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";
            dataGridView1.Columns[5].Name = "Titule";

            foreach (Klub k in collection.FindAll())
            {
                if (k.titule != null)
                    foreach (string titula in k.titule)
                    {
                        if (titula.Contains(textBox10.Text))
                            dataGridView1.Rows.Add(k.naziv, k.konferencija, k.divizija, k.vlasnik, k.dvorana, titula);
                    }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            klub9Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            klub2Pnl.BackColor = klub3Pnl.BackColor = klub4Pnl.BackColor = klub5Pnl.BackColor = klub6Pnl.BackColor = klub7Pnl.BackColor = klub8Pnl.BackColor = klub1Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("klubovi");

            var query =
                 (from klub in collection.AsQueryable<Klub>()
                  where klub.konferencija == "Zapadna" 
                  select klub).Count(); ;


            MessageBox.Show("Broj klubova u zapadnoj konferenciji:  " + Convert.ToInt32(query));
        }

        private void probaBtn_Click(object sender, EventArgs e)
        {

        }

        private void dodajTabLbl_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button9.Visible = button3.Visible = button4.Visible = false;

            dodajTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            titulaTabPnl.BackColor = izmeniTablPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;
           
            button2.Enabled = true;
            dodajTabLbl.BorderStyle = BorderStyle.FixedSingle;

            textBox1.BackColor = textBox8.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = System.Drawing.Color.White;
            textBox5.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox8.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = true;
            textBox5.Enabled = false;

            label1.ForeColor = label8.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = System.Drawing.Color.White;
            label5.ForeColor = System.Drawing.Color.DimGray;

            button3.Visible = button4.Visible = false;
        }

        private void izmeniTablbl_Click(object sender, EventArgs e)
        {
            button3.Visible = true;
            button9.Visible = button2.Visible = button4.Visible = false;

            izmeniTablPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            titulaTabPnl.BackColor = dodajTabPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox8.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = System.Drawing.Color.White;
            textBox5.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox8.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = true;
            textBox5.Enabled = false;

            label1.ForeColor = label8.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = System.Drawing.Color.White;
            label5.ForeColor = System.Drawing.Color.DimGray;
        }

        private void brisanjeTablLbl_Click(object sender, EventArgs e)
        {
            button4.Visible = true;
            button9.Visible = button2.Visible = button3.Visible = false;

            obrisiTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            titulaTabPnl.BackColor = dodajTabPnl.BackColor = izmeniTablPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.Enabled = true;
            textBox8.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = false;
            //textBox5.Enabled = textBox8.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = false;

            textBox1.BackColor = System.Drawing.Color.White;
            textBox8.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = textBox5.BackColor = System.Drawing.Color.DimGray;

            label1.ForeColor = System.Drawing.Color.White;
            label5.ForeColor = label8.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = System.Drawing.Color.DimGray;
        }

        private void titulaTabLbl_Click(object sender, EventArgs e)
        {
            button2.Visible = button3.Visible = button4.Visible = false;
            button9.Visible = true;

            izmeniTablPnl.BackColor = dodajTabPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;
            titulaTabPnl.BackColor = System.Drawing.Color.LightBlue;

            textBox1.Enabled = textBox5.Enabled = true;
            textBox8.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = false;

            textBox1.BackColor = textBox5.BackColor = System.Drawing.Color.White;
            textBox8.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = System.Drawing.Color.DimGray;

            label5.ForeColor = label1.ForeColor = System.Drawing.Color.White;
            label8.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = System.Drawing.Color.DimGray;
        }
    }

}
