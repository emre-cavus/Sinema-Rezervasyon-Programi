using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;

namespace Sinema_Rezervasyon_Otomasyonu
{
    public partial class frmGenel : Form
    {
        public frmGenel()
        {
            InitializeComponent();
        }

        OleDbConnection connect = new OleDbConnection("Provider=Microsoft.Ace.Oledb.12.0;Data Source=sinema.accdb");
        void baglantiKur()
        {
            if (connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
        }
        // : )
        private void btnRezerve_Click(object sender, EventArgs e)
        {
            if (cbSalon.SelectedIndex != -1 && cbFilm.SelectedIndex != -1 && cbSeans.SelectedIndex != -1)
            {
                frmRezervasyon rezerve = new frmRezervasyon();
                rezerve.film_Adi = cbFilm.Text;
                rezerve.salon_Adi = cbSalon.Text;
                rezerve.seans = cbSeans.Text;
                rezerve.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Lütfen film bilgilerini eksiksiz doldurunuz.");
            }           
        }


        void bilgiAl(string sql , ComboBox cmb)
        {
            cmb.Items.Clear();
            baglantiKur();            
            OleDbCommand cmd = new OleDbCommand(sql, connect);
            OleDbDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                cmb.Items.Add(oku[1].ToString());
            }
            connect.Close();
        }
        private void cbSalon_Click(object sender, EventArgs e)
        {
            bilgiAl("SELECT * FROM salon", cbSalon);
        }

        private void cbFilm_Click(object sender, EventArgs e)
        {
            bilgiAl("SELECT * FROM filmler", cbFilm);
        }

        private void btnFilmEkle_Click(object sender, EventArgs e)
        {
            frmFilmKayit frmK = new frmFilmKayit();
            frmK.Show();
            this.Hide();
        }

    }
}
