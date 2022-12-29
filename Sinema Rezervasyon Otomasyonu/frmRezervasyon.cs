using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Collections;

namespace Sinema_Rezervasyon_Otomasyonu
{
    public partial class frmRezervasyon : Form
    {
        public frmRezervasyon()
        {
            InitializeComponent();
        }

        public string film_Adi = "";
        public string salon_Adi = "";
        public string seans = "";
        ArrayList koltuklar = new ArrayList();
        ArrayList iptalKoltuk = new ArrayList();
        int filmID = 0;
        int salonID = 0; 
                
        OleDbConnection connect = new OleDbConnection("Provider=Microsoft.Ace.Oledb.12.0;Data Source=sinema.accdb");

        void baglantiKur()
        {
            if (connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmGenel frmG = new frmGenel();
            frmG.Show();
            this.Hide();
        }

        private void btnKoltuk_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Transparent)
            {
                ((Button)sender).BackColor = Color.SpringGreen;
                if (!koltuklar.Contains(((Button)sender).Text))
                {
                    koltuklar.Add(((Button)sender).Text);
                }
                koltukYazdir();
            }
            else if (((Button)sender).BackColor == Color.SpringGreen)
            {
                ((Button)sender).BackColor = Color.Transparent;
                if (koltuklar.Contains(((Button)sender).Text))
                {
                     koltuklar.Remove(((Button)sender).Text);
                }
                koltukYazdir();
            }
            else
            {
                if (!iptalKoltuk.Contains(((Button)sender).Text))
                {
                    iptalKoltuk.Add(((Button)sender).Text);
                }
                else
                {
                    iptalKoltuk.Remove(((Button)sender).Text);
                }

                string koltuk = "";
                for (int i = 0; i < iptalKoltuk.Count; i++)
                {
                    koltuk += iptalKoltuk[i].ToString() + ",";
                }
                if (iptalKoltuk.Count >= 1)
                {
                    koltuk = koltuk.Remove(koltuk.Length - 1, 1);
                }
                txtKoltukIptal.Text = koltuk;
            }           
        }

        void koltukYazdir()
        {
            string koltuk = "";
            for (int i = 0; i < koltuklar.Count; i++)
            {
                koltuk += koltuklar[i].ToString() + ",";
            }
            if (koltuklar.Count >= 1)
            {
                koltuk = koltuk.Remove(koltuk.Length - 1, 1);
            }
            txtKoltukNo.Text = koltuk;
        }
        

        private void frmRezervasyon_Load(object sender, EventArgs e)
        {
            lblFilmAdi.Text = film_Adi;
            lblSalonSeans.Text = salon_Adi + " / " + seans;
            timer1.Enabled = true;
            filmID = Convert.ToInt32(araGetir("SELECT * FROM filmler WHERE film_Adi='" + film_Adi + "'"));
            salonID = Convert.ToInt32(araGetir("SELECT * FROM salon WHERE salon_Adi='" + salon_Adi + "'"));
            LogAl();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblSaat.Text = DateTime.Now.ToLongTimeString();
            lblTarih.Text = DateTime.Now.Date.ToShortDateString();
        }

        string araGetir(string sql)
        {
            baglantiKur();
            OleDbCommand cmd = new OleDbCommand(sql, connect);
            OleDbDataReader oku = cmd.ExecuteReader();
            oku.Read();
            string deger = oku[0].ToString();
            connect.Close();
            return deger;
        }

        void biletAyir()
        {
            baglantiKur();
            string ucret = "";
            if (rbOgrenci.Checked)  ucret = "6";
            else ucret = "10";

            for (int i = 0; i < koltuklar.Count; i++)
            {
                string sql = "INSERT INTO satis(film_ID,salon_ID,tarih,seans,satis_Adi,satis_Soyadi,koltuk_No,ucret) VALUES (" + filmID + "," + salonID + ",'" + lblTarih.Text + "','" + seans + "','" + txtAdi.Text + "','" + txtSoyadi.Text + "'," + Convert.ToInt32(koltuklar[i]) + ",'" + ucret + "')";
                OleDbCommand cmd = new OleDbCommand(sql, connect);
                cmd.ExecuteNonQuery();
                this.Controls.Find("btn" + koltuklar[i].ToString(), true)[0].BackColor = Color.DimGray;
            }

            connect.Close();
        }

        private void btnBiletAyir_Click(object sender, EventArgs e)
        {
            if (txtKoltukNo.Text != "")
            {
                if (txtKoltukNo.Text != "" && txtAdi.Text != "" && txtSoyadi.Text != "")
                {
                    biletAyir();
                    MessageBox.Show(txtAdi.Text + " " + txtSoyadi.Text + " bilgili kişinin " + txtKoltukNo.Text + " no'lu koltukları ayrılmıştır");
                    txtKoltukNo.Text = "";
                    txtAdi.Text = "";
                    txtSoyadi.Text = "";
                    koltuklar.Clear();
                }
                else
                {
                    MessageBox.Show("Tüm bilgileri eksiksiz doldurmalısınız.");
                }
            }
            else
            {
                MessageBox.Show("Koltuk numarasını seçmediniz.","Dikkat");
            }
        }

        void LogAl()
        {
            baglantiKur();
            string sql = "SELECT * FROM satis WHERE film_ID=" + filmID + " AND salon_ID=" + salonID + " AND seans='" + seans + "'";
            OleDbCommand cmd = new OleDbCommand(sql, connect);
            OleDbDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                string koltuk_No = oku[7].ToString();
                this.Controls.Find("btn" + koltuk_No, true)[0].BackColor = Color.DimGray;
            }
            connect.Close();
        }

        private void btnBilet_iptal_Click(object sender, EventArgs e)
        {
            if (txtKoltukIptal.Text != "")
            {
                baglantiKur();
                for (int i = 0; i < iptalKoltuk.Count; i++)
                {
                    string sql = "DELETE FROM satis WHERE koltuk_No=" + Convert.ToInt32(iptalKoltuk[i]);
                    OleDbCommand cmd = new OleDbCommand(sql, connect);
                    cmd.ExecuteNonQuery();
                    this.Controls.Find("btn" + iptalKoltuk[i].ToString(), true)[0].BackColor = Color.Transparent;
                }

                connect.Close();
                iptalKoltuk.Clear();
                MessageBox.Show(txtKoltukNo.Text + " koltuk numaraları bileti iptal edilmiştir.");
                txtKoltukIptal.Text = "";
                txtAdi.Text = "";
                txtSoyadi.Text = "";                
            } 
            else
            {
                MessageBox.Show("Koltuk numarasını seçmediniz.");                
            }
        }
        

        private void frmRezervasyon_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmGenel frmG = new frmGenel();
            frmG.Show();
        }
    }
}
