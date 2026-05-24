using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DepoOtomasyon
{
    public partial class frmGiris : Form
    {
        sqlbaglanti sql = new sqlbaglanti();
        public frmGiris()
        {
            InitializeComponent();
        }

        #region TextBox Modern Arayüz İşlemleri
        bool Booltxtkullanici = false;
        bool Booltxtsifre = false;
        private void txtKullanici_Click(object sender, EventArgs e)
        {
            if(Booltxtkullanici==false)
            {
                Booltxtkullanici=true;
                txtKullanici.Text = string.Empty;
            }
            
            panel3.BackColor = Color.FromArgb(0,192,192);
            txtKullanici.ForeColor = Color.FromArgb(0, 192, 192);

            txtSifre.ForeColor = Color.White;
            panel4.BackColor = Color.White;
        }

        private void txtSifre_Click(object sender, EventArgs e)
        {
            if(Booltxtsifre==false)
            {
                Booltxtsifre=true;
                txtSifre.Text = string.Empty;
            }
            panel4.BackColor = Color.FromArgb(0,192,192);
            txtSifre.ForeColor = Color.FromArgb(0, 192, 192);

            txtKullanici.ForeColor= Color.White;
            panel3.BackColor= Color.White;
        }
        #endregion

        frmAnaMenu frmAna = new frmAnaMenu();
        
        #region Giriş Animasyon Başlatma Ve btnGiris VT Giriş İşlemleri
        
        private void btnGiris_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = sql.baglanti())
                {
                    //Bağlantı Sınıfta Önceden Açık Durumdadır.
                    SqlCommand cmd = new SqlCommand
                        ("select kullaniciAdi,sifre from TBLKullanıcı where kullaniciAdi=@Pkullaniciadi and sifre=@Psifre",baglanti); //Kullanıcı Adı Ve Şifrenin Doğruluğunu Kontrol Eder.
                    cmd.Parameters.Add("@Pkullaniciadi", SqlDbType.VarChar).Value = txtKullanici.Text.Trim(); //Parametreye Değer Tanımlama Ve Sağ Sol Boşlukları Kapatma
                    cmd.Parameters.Add("@Psifre", SqlDbType.VarChar).Value = txtSifre.Text.Trim(); //Parametreye Değer Tanımlama Ve Sağ Sol Boşlukları Kapatma
                    SqlDataReader dr = cmd.ExecuteReader();
                    if(dr.Read())
                    {
                        frmAna.KULLANICIADI = txtKullanici.Text; //Ana Menü Formuna Ad Çektirme
                         //Canlı Sohbet Formuna Ad Çektirme
                        frmAna.Show();
                        this.TopMost = true;
                        timer1.Start(); //Giriş Animasyon
                        
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı Adı Veya Şifreniz Yanlış");
                    }
                    
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        #endregion

        #region Giriş Animasyon İşlemleri Ve Load İşlemleri
        private void timer1_Tick(object sender, EventArgs e)
        {
            frmAna.Left += 10;
            if(frmAna.Left>=900)
            {
                timer1.Stop();
                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            frmAna.Left -= 10;
            frmAna.TopMost = true;
            if (frmAna.Left<=468)
            {
                
                timer2.Stop();
                
            }
        }
        #endregion

        #region PictureBox Modern Arayüz İşlemleri Ve PictureBox İşlemleri
        private void pctrBoxExit_MouseEnter(object sender, EventArgs e)
        {
            pctrBoxExit.BackColor = Color.YellowGreen;
        }

        private void pctrBoxExit_MouseLeave(object sender, EventArgs e)
        {
            pctrBoxExit.BackColor = Color.FromArgb(34, 36, 69);
        }

        private void pctrBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        

        private void frmGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
