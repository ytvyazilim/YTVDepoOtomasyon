using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DepoOtomasyon
{
    public partial class frmAnaMenu : Form
    {
        public frmAnaMenu()
        {
            InitializeComponent();
        }

        #region Kullanıcı Adı Çekme
        public string KULLANICIADI = "";
        private void frmAnaMenu_Load(object sender, EventArgs e)
        {
            
            label1.Text = $"Merhaba {KULLANICIADI}";
        }
        #endregion

       

        private void btnUrun_Click(object sender, EventArgs e)
        {
            frmUrun frUrun = new frmUrun();
            frUrun.Show();
            frUrun.TopMost=true;
        }

        private void btnStok_Click(object sender, EventArgs e)
        {
            frmStok frStok = new frmStok();
            frStok.Show();
            frStok.TopMost=true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            frmKategori fr = new frmKategori();
            fr.Show();
            fr.TopMost=true;
        }

        private void btnİstatistik_Click(object sender, EventArgs e)
        {
            frmİstatistik fr = new frmİstatistik();
            fr.Show();
            fr.TopMost=true;
        }
    }
}
