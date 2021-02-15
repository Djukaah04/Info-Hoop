using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Projekat3
{
    public partial class Treneri : Form
    {
        public Treneri()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Trener>("treneri");

            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "")
            {
                Trener trener = new Trener { ime = textBox1.Text, prezime = textBox2.Text, godisnjaplata = Convert.ToInt32(textBox4.Text) };

                collection.Insert(trener);
                MessageBox.Show("Dodat trener " + textBox1.Text + " " + textBox2.Text + ".");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox4.Text = "";

                //dataGridView1.Rows.Add(trener.ime, trener.prezime, trener.godisnjaplata, nazivkluba);
            }
            else
            {
                MessageBox.Show("Popuni sva polja (ime/prezime/godisnja plata) !!");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Trener>("treneri");

            var query = Query.And(
                            Query.EQ("ime", textBox1.Text),
                            Query.EQ("prezime", textBox2.Text)
                            );

            collection.Remove(query);

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";

            dataGridView1.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trenerOpcija2Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            trenerOpcija1Pnl.BackColor = trenerOpcija3Pnl.BackColor = trenerOpcija4Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var Tcollection = db.GetCollection("treneri");

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "GOdisnja platae";

            Int32 plata;

            if (textBox6.Text == "") { plata = 0; }
            else { plata = Convert.ToInt32(textBox6.Text); }

            var query =
                 (from trener in Tcollection.AsQueryable<Trener>()
                  where trener.godisnjaplata > plata
                  orderby trener.godisnjaplata descending
                  select trener);

            foreach (Trener t in query)
            {
                dataGridView1.Rows.Add(t.ime, t.prezime, t.godisnjaplata);
            }

            textBox6.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var Kcollection = db.GetCollection("klubovi");
            var Tcollection = db.GetCollection("treneri");

            var queryK =
                 from klub in Kcollection.AsQueryable<Klub>()
                 where klub.naziv == textBox3.Text
                 select klub;

            Klub k = queryK.FirstOrDefault();

            var queryT =
                 from trener in Tcollection.AsQueryable<Trener>()
                 where trener.ime == textBox1.Text && trener.prezime == textBox2.Text
                 select trener;

            Trener t = queryT.FirstOrDefault();

            if (k != null && t != null)
            {
                t.Klub = new MongoDBRef("klubovi", k.Id);
                Tcollection.Save(t);
                MessageBox.Show("Uspelo!");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                MessageBox.Show("Nevalidni podaci!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Trener>("treneri");

            var query = Query.And(
                Query.EQ("ime", textBox1.Text),
                Query.EQ("prezime", textBox2.Text)
                );
            Trener treneri = collection.FindOne(query);

            if (treneri.stiligre == null)
            {
                var update = MongoDB.Driver.Builders.Update.Set("stiligre", BsonValue.Create(new List<string> { textBox5.Text }));
                collection.Update(query, update);
            }
            else
            {
                var update = MongoDB.Driver.Builders.Update.Push("stiligre", textBox5.Text);
                collection.Update(query, update);
            }

            textBox5.Text = "";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //var connectionString = "mongodb://localhost/?safe=true";
            //var server = MongoServer.Create(connectionString);
            //var db = server.GetDatabase("kosarka");

            //var collection = db.GetCollection<Trener>("treneri");

            //var result2 = (from trener in collection.AsQueryable<Trener>()
            //               where trener.ime == textBox1.Text && trener.prezime == textBox2.Text
            //               select trener).FirstOrDefault();

            //if (result2 != null)
            //{
            //    dataGridView1.Rows.Clear();
            //    dataGridView1.ColumnCount = 1;
            //    dataGridView1.Columns[0].Name = "Stil igre";
            //    textBox4.Text = Convert.ToString(result2.godisnjaplata);

            //    if (result2.stiligre != null)
            //    {
            //        foreach (string stiligre in result2.stiligre.ToList())
            //        {
            //            //dataGridView1.Rows.Add(stiligre);
            //        }
            //    }
            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            trenerOpcija1Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            trenerOpcija2Pnl.BackColor = trenerOpcija3Pnl.BackColor = trenerOpcija4Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var Tcollection = db.GetCollection<Trener>("treneri");
            var Kcollection = db.GetCollection<Klub>("klubovi");
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Godisnja plata";
            dataGridView1.Columns[3].Name = "Naziv kluba";
            dataGridView1.Columns[4].Name = "Stil igre";
            string nazivkluba = "";

            string stilovi;
            foreach (Trener t in Tcollection.FindAll().SetSortOrder(new string[] { "ime", "prezime" }))
            {
                if (t.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(t.Klub);
                    nazivkluba = klub.naziv;
                }
                else
                {
                    nazivkluba = "";
                }
                if (t.stiligre != null)
                {
                    stilovi = t.stiligre.Aggregate((x, y) => x + "," + y);
                }
                else
                {
                    stilovi = "";
                }

                dataGridView1.Rows.Add(t.ime, t.prezime, t.godisnjaplata, nazivkluba, stilovi);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            trenerOpcija3Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            trenerOpcija2Pnl.BackColor = trenerOpcija1Pnl.BackColor = trenerOpcija4Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var Tcollection = db.GetCollection<Trener>("treneri");
            var Kcollection = db.GetCollection<Klub>("klubovi");
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Godisnja plata";
            dataGridView1.Columns[3].Name = "Naziv kluba";


            var query = (from trener in Tcollection.AsQueryable<Trener>()
                           orderby trener.ime, trener.prezime
                           select trener);

            foreach (Trener t in query)
            {
                if (t.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(t.Klub);
                    if (klub.divizija == textBox7.Text)
                    {
                        dataGridView1.Rows.Add(t.ime, t.prezime, t.godisnjaplata, klub.naziv);
                    }
                }
            }

            textBox7.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            trenerOpcija4Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            trenerOpcija2Pnl.BackColor = trenerOpcija3Pnl.BackColor = trenerOpcija1Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Trener>("treneri");

            dataGridView1.Rows.Clear();

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            //dataGridView1.Columns[2].Name = "God";
            //dataGridView1.Columns[3].Name = "Vlasnik";
            //dataGridView1.Columns[4].Name = "Dvorana";
            //dataGridView1.Columns[5].Name = "Titule";

            foreach (Trener t in collection.FindAll())
            {
                if (t.stiligre != null)
                    foreach (string stil in t.stiligre)
                    {
                        if (stil.Contains(textBox8.Text))
                            dataGridView1.Rows.Add(t.ime, t.prezime);
                    }
            }

            textBox8.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Trener>("treneri");

            var query = Query.And(
                            Query.EQ("ime", textBox1.Text),
                            Query.EQ("prezime", textBox2.Text)
                            );

            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "")
            {

                var update = MongoDB.Driver.Builders.Update.Set("godisnjaplata", textBox4.Text);
                collection.Update(query, update);

                textBox1.Text = "";
                textBox2.Text = "";
                textBox4.Text = "";
            }

            else
            {
                MessageBox.Show("Popuni sva polja (ime/prezime/plata) !!");
            }

            
        }

        private void izmeniTablbl_Click(object sender, EventArgs e)
        {
            izmeniTablPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            dodajTabPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = textBox4.BackColor = textBox5.BackColor = textBox3.BackColor = System.Drawing.Color.White;
            textBox1.Enabled = textBox2.Enabled = textBox4.Enabled = textBox5.Enabled = textBox3.Enabled = true;

            label1.ForeColor = label2.ForeColor = label4.ForeColor = label5.ForeColor = label3.ForeColor = System.Drawing.Color.White;

            button1.Visible = button4.Visible = false;
            button5.Visible = button9.Visible = button3.Visible = true;
        }

        private void dodajTabLbl_Click(object sender, EventArgs e)
        {
            dodajTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            izmeniTablPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = textBox4.BackColor = System.Drawing.Color.White;
            textBox5.BackColor = textBox3.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = textBox4.Enabled = true;
            textBox5.Enabled = textBox3.Enabled = false;

            label1.ForeColor = label2.ForeColor = label4.ForeColor = System.Drawing.Color.White;
            label5.ForeColor = label3.ForeColor = System.Drawing.Color.DimGray;

            button1.Visible = true;
            button4.Visible = button5.Visible = button9.Visible = button3.Visible = false;
        }

        private void brisanjeTablLbl_Click(object sender, EventArgs e)
        {
            obrisiTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            izmeniTablPnl.BackColor = dodajTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = System.Drawing.Color.White;
            textBox4.BackColor = textBox5.BackColor = textBox3.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = true;
            textBox4.Enabled = textBox5.Enabled = textBox3.Enabled = false;

            label1.ForeColor = label2.ForeColor = System.Drawing.Color.White;
            label4.ForeColor = label5.ForeColor = label3.ForeColor = System.Drawing.Color.DimGray;

            button1.Visible = button5.Visible = button9.Visible = button3.Visible = false;
            button4.Visible = true;
        }
    }
}
