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
    public partial class frmFilmKayit : Form
    {
        public frmFilmKayit()
        {
            InitializeComponent();
        }

        OleDbConnection connect = new OleDbConnection("Provider=Microsoft.Ace.Oledb.12.0;Data Source=sinema.accdb"); //Not: Access dosyası hazır.
        void baglantiKur()
        {
            if (connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
        }

        private void frmFilmKayit_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmGenel frmG = new frmGenel();
            frmG.Show();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglantiKur();
            string sql = "INSERT INTO filmler(film_Adi,yonetmen,film_tur) VALUES('" + txtFilmAdi.Text + "','" + txtYonetmen.Text + "','" + txtTur.Text + "')";
            OleDbCommand cmd = new OleDbCommand(sql, connect);
            cmd.ExecuteNonQuery();
            connect.Close();

            MessageBox.Show(txtFilmAdi.Text + " / " + txtYonetmen.Text + " / " +txtTur.Text + " filmi eklendi");
            txtFilmAdi.Text = "";
            txtTur.Text = "";
            txtYonetmen.Text = "";
        }        
    }
}
