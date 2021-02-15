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

using System.Resources;

namespace Projekat3
{
    public partial class Kosarkasi : Form
    {
        public Kosarkasi()
        {
            InitializeComponent();
            placeholderLbl.Visible = true;
            dataGridView1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Kosarkas>("kosarkasi");

     //       string sl = pictureBox1.ImageLocation;

            Image img = pictureBox1.Image;

            byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && img != null)
            {
                Kosarkas kosarkas = new Kosarkas
                {
                    ime = textBox1.Text,
                    prezime = textBox2.Text,
                    pozicija = textBox3.Text,
                    brojnadresu = Convert.ToInt32(textBox4.Text),
                    visina = Convert.ToInt32(textBox5.Text),
                    tezina = Convert.ToInt32(textBox6.Text),
                    koledz = textBox7.Text,
                    slika = bytes
                };

                collection.Insert(kosarkas);

                MessageBox.Show("Dodat "+ textBox1.Text + " "+ textBox2.Text +".");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                pictureBox1.Image = null;
            }
            else
            {
                MessageBox.Show("Popuni sva polja (ime/prezime/pozicija/broj na dresu/visina/tezina/koledz/slika) !!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Kosarkas>("kosarkasi");

            var query = Query.And(
                            Query.EQ("ime", textBox1.Text),
                            Query.EQ("prezime", textBox2.Text)
                            );

      //      string sl = pictureBox1.ImageLocation;

            Image img = pictureBox1.Image;

            byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && img != null)
            {

                var update = MongoDB.Driver.Builders.Update.Set("pozicija", textBox3.Text)
                                                           .Set("brojnadresu", textBox4.Text)
                                                           .Set("visina", textBox5.Text)
                                                           .Set("tezina", textBox6.Text)
                                                           .Set("koledz", textBox7.Text)
                                                           .Set("slika", bytes);
                collection.Update(query, update);

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                pictureBox1.Image = null;
            }

            else
            {
                MessageBox.Show("Popuni sva polja ime/prezime/pozicija/broj na dresu/visina/tezina/koledz/slika) !!");
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Kosarkas>("kosarkasi");

            var query = (from kosarkas in collection.AsQueryable<Kosarkas>()
                           where kosarkas.ime == textBox1.Text && kosarkas.prezime == textBox2.Text
                           select kosarkas).FirstOrDefault();

            if (query != null)
            {
                textBox3.Text = query.pozicija;
                textBox4.Text = Convert.ToString(query.brojnadresu);
                textBox5.Text = Convert.ToString(query.visina);
                textBox6.Text = Convert.ToString(query.tezina);
                textBox7.Text = query.koledz;
                pictureBox1.Image = Image.FromStream(new MemoryStream(query.slika));
            }

            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                pictureBox1.Image = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Kosarkas>("kosarkasi");

            var query = Query.And(
                            Query.EQ("ime", textBox1.Text),
                            Query.EQ("prezime", textBox2.Text)
                            );

            collection.Remove(query);

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            pictureBox1.Image = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            placeholderLbl.Visible = false;
            dataGridView1.Visible = true;

            igracOpcija2Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            igracOpcija1Pnl.BackColor = igracOpcija3Pnl.BackColor = igracOpcija4Pnl.BackColor = igracOpcija5Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("kosarkasi");

            var query =
                 from kosarkasi in collection.AsQueryable<Kosarkas>()
                 where kosarkasi.pozicija == textBox8.Text
                 orderby kosarkasi.ime, kosarkasi.prezime
                 select kosarkasi;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Klub";

            string nazivkluba;

            foreach (Kosarkas kos in query)
            {

                if (kos.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(kos.Klub);
                    nazivkluba = klub.naziv;
                }
                else
                {
                    nazivkluba = "";
                }

                dataGridView1.Rows.Add(kos.ime, kos.prezime, nazivkluba);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            placeholderLbl.Visible = false;
            dataGridView1.Visible = true;

            igracOpcija3Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            igracOpcija2Pnl.BackColor = igracOpcija1Pnl.BackColor = igracOpcija4Pnl.BackColor = igracOpcija5Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("kosarkasi");

            var query =
                 from kosarkasi in collection.AsQueryable<Kosarkas>()
                 orderby kosarkasi.ime, kosarkasi.prezime
                 select kosarkasi;


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";

            string nazivkluba;

            foreach (Kosarkas kos in query)
            {

                if (kos.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(kos.Klub);
                    nazivkluba = klub.naziv;
                    if (nazivkluba == textBox9.Text)
                        dataGridView1.Rows.Add(kos.ime, kos.prezime);
                }
            }

        }


        private void button6_Click(object sender, EventArgs e)
        {
            placeholderLbl.Visible = false;
            dataGridView1.Visible = true;

            igracOpcija1Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            igracOpcija2Pnl.BackColor = igracOpcija3Pnl.BackColor = igracOpcija4Pnl.BackColor = igracOpcija5Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection("kosarkasi");

            var query = from kosarkas in collection.AsQueryable<Kosarkas>()
                        orderby kosarkas.ime, kosarkas.prezime
                        select kosarkas;

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Pozicija";
            dataGridView1.Columns[3].Name = "Visina";
            dataGridView1.Columns[4].Name = "Tezina";
            dataGridView1.Columns[5].Name = "Koledz";
            dataGridView1.Columns[6].Name = "Klub";
            string nazivkluba = "";

            foreach (Kosarkas kos in query)
            {
                if (kos.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(kos.Klub);
                    nazivkluba = klub.naziv;
                }
                dataGridView1.Rows.Add(kos.ime, kos.prezime, kos.pozicija, kos.visina, kos.tezina, kos.koledz, nazivkluba);

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var klubcollection = db.GetCollection("klubovi");
            var kosarkascollection = db.GetCollection("kosarkasi");

            var queryKlub =
                 from klub in klubcollection.AsQueryable<Klub>()
                 where klub.naziv == textBox10.Text
                 select klub;

            Klub kl = queryKlub.FirstOrDefault();

            var queryKosarkas =
                 from kosarkas in kosarkascollection.AsQueryable<Kosarkas>()
                 where kosarkas.ime == textBox1.Text && kosarkas.prezime == textBox2.Text
                 select kosarkas;

            Kosarkas kos = queryKosarkas.FirstOrDefault();

            if (kl != null && kos != null)
            {
                kos.Klub = new MongoDBRef("klubovi", kl.Id);
                kosarkascollection.Save(kos);

                //
                MongoDBRef aa = new MongoDBRef("kosarkasi", kos.Id);
                kl.Kosarkasi.Add(aa);
                klubcollection.Save(kl);
                //
                MessageBox.Show(textBox1.Text+ " " + textBox2.Text +" igra za "+ textBox10.Text+ ".");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox10.Text = "";
            }
           else
            {
                MessageBox.Show("Nevalidni podaci!");
                return;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var klubCollection = db.GetCollection<Klub>("klubovi");


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Pozicija";
            dataGridView1.Columns[3].Name = "Visina";
            dataGridView1.Columns[4].Name = "Tezina";
            dataGridView1.Columns[5].Name = "Koledz";

            var queryKlub =
                 (from klub in klubCollection.AsQueryable<Klub>()
                  where klub.naziv == textBox9.Text
                  select klub).FirstOrDefault();

            if (queryKlub != null)
            {
                foreach (MongoDBRef kosarkasiRef in queryKlub.Kosarkasi.ToList())
                {
                    Kosarkas kosarkas = db.FetchDBRefAs<Kosarkas>(kosarkasiRef);
                    dataGridView1.Rows.Add(kosarkas.ime, kosarkas.prezime, kosarkas.pozicija, kosarkas.visina, kosarkas.tezina, kosarkas.koledz);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            placeholderLbl.Visible = false;
            dataGridView1.Visible = true;

            igracOpcija4Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            igracOpcija2Pnl.BackColor = igracOpcija3Pnl.BackColor = igracOpcija1Pnl.BackColor = igracOpcija5Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var collection = db.GetCollection<Kosarkas>("kosarkasi");

            var query =
                 (from kosarkas in collection.AsQueryable<Kosarkas>()
                  orderby kosarkas.visina descending
                  select kosarkas).Take(10);

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Ime";
            dataGridView1.Columns[1].Name = "Prezime";
            dataGridView1.Columns[2].Name = "Pozicija";
            dataGridView1.Columns[3].Name = "Visina";
            dataGridView1.Columns[4].Name = "Tezina";
            dataGridView1.Columns[5].Name = "Koledz";
            string nazivkluba = "";


            foreach (Kosarkas kos in query)
            {
                if (kos.Klub != null)
                {
                    Klub klub = db.FetchDBRefAs<Klub>(kos.Klub);
                    nazivkluba = klub.naziv;
                }
                dataGridView1.Rows.Add(kos.ime, kos.prezime, kos.pozicija, kos.visina, kos.tezina, kos.koledz, nazivkluba);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                pictureBox1.ImageLocation = open.FileName;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            placeholderLbl.Visible = false;
            dataGridView1.Visible = true;

            igracOpcija5Pnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            igracOpcija2Pnl.BackColor = igracOpcija3Pnl.BackColor = igracOpcija4Pnl.BackColor = igracOpcija1Pnl.BackColor = System.Drawing.Color.Transparent;

            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase("kosarka");

            var klubCollection = db.GetCollection<Klub>("klubovi");


            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Naziv tima";
            dataGridView1.Columns[1].Name = "Konferencija";
            dataGridView1.Columns[2].Name = "Divizija";
            dataGridView1.Columns[3].Name = "Vlasnik";
            dataGridView1.Columns[4].Name = "Dvorana";
            dataGridView1.Columns[5].Name = "Broj igraca";

            var query =
                 (from klub in klubCollection.AsQueryable<Klub>()
                  orderby klub.naziv
                  select klub);

            foreach (Klub kl in query)
                dataGridView1.Rows.Add(kl.naziv, kl.konferencija, kl.divizija, kl.vlasnik, kl.dvorana, kl.Kosarkasi.Count);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                pictureBox1.ImageLocation = open.FileName;
            }
        }

        private void dodajTabLbl_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button7.Visible = button2.Visible = button3.Visible = false;
            button10.Visible = button1.Visible = true;

            dodajTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            obrisiTabPnl.BackColor = izmeniTablPnl.BackColor = klubTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = textBox5.BackColor = textBox6.BackColor = textBox7.BackColor = System.Drawing.Color.White;
            textBox10.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = textBox6.Enabled = textBox7.Enabled = true;
            textBox10.Enabled = false;

            label1.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = label5.ForeColor = label6.ForeColor = label7.ForeColor = System.Drawing.Color.White;
            label10.ForeColor = System.Drawing.Color.DimGray;
        }

        private void izmeniTablbl_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button10.Visible = button7.Visible = button1.Visible = button3.Visible = false;
            button2.Visible = true;

            izmeniTablPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            obrisiTabPnl.BackColor = dodajTabPnl.BackColor = klubTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = textBox3.BackColor = textBox4.BackColor = textBox5.BackColor = textBox6.BackColor = textBox7.BackColor = System.Drawing.Color.White;
            textBox10.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = textBox6.Enabled = textBox7.Enabled = true;
            textBox10.Enabled = false;

            label1.ForeColor = label2.ForeColor = label3.ForeColor = label4.ForeColor = label5.ForeColor = label6.ForeColor = label7.ForeColor = System.Drawing.Color.White;
            label10.ForeColor = System.Drawing.Color.DimGray;
        }

        private void brisanjeTablLbl_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button10.Visible = button7.Visible = button1.Visible = button2.Visible = false;
            button3.Visible = true;

            obrisiTabPnl.BackColor = System.Drawing.Color.FromArgb(253, 185, 39);
            izmeniTablPnl.BackColor = dodajTabPnl.BackColor = klubTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = System.Drawing.Color.White;
            textBox3.BackColor = textBox4.BackColor = textBox5.BackColor = textBox6.BackColor = textBox7.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = true;
            textBox10.Enabled = textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = textBox6.Enabled = textBox7.Enabled = false;

            label1.ForeColor = label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = label4.ForeColor = label5.ForeColor = label6.ForeColor = label7.ForeColor = System.Drawing.Color.DimGray;
            label10.ForeColor = System.Drawing.Color.DimGray;
        }

        private void klubTabLbl_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button7.Visible = true;
            button10.Visible = button1.Visible = button2.Visible = button3.Visible = false;

            klubTabPnl.BackColor = System.Drawing.Color.LightBlue;
            izmeniTablPnl.BackColor = dodajTabPnl.BackColor = obrisiTabPnl.BackColor = System.Drawing.Color.Transparent;

            textBox1.BackColor = textBox2.BackColor = textBox10.BackColor = System.Drawing.Color.White;
            textBox3.BackColor = textBox4.BackColor = textBox5.BackColor = textBox6.BackColor = textBox7.BackColor = System.Drawing.Color.DimGray;

            textBox1.Enabled = textBox2.Enabled = textBox10.Enabled = true;
            textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = textBox6.Enabled = textBox7.Enabled = false;

            label10.ForeColor = label1.ForeColor = label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = label4.ForeColor = label5.ForeColor = label6.ForeColor = label7.ForeColor = System.Drawing.Color.DimGray;

        }
    }
}