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
    
    public partial class frmİstatistik : Form
    {
        sqlbaglanti baglanti = new sqlbaglanti();
        public frmİstatistik()
        {
            InitializeComponent();
        }

        private void frmİstatistik_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            #region Toplam Ürün - En Düşük Stok - Toplam Kategori
            try
            {
                using (SqlConnection bgl = baglanti.baglanti())
                {
                    SqlCommand toplamurun = new SqlCommand
                        ("select COUNT(*) as ToplamUrun from TBLUrunler",bgl);
                    object toplamurunist = toplamurun.ExecuteScalar();
                    SqlCommand endusukstok = new SqlCommand
                        ("select top 1 UrunAd from TBLUrunler order by StokMiktarı asc", bgl);
                    object endusukstokist = endusukstok.ExecuteScalar();
                    SqlCommand toplamkategori = new SqlCommand
                        ("select count(*) from TBLKategori",bgl);
                    object toplamkategoriIst = toplamkategori.ExecuteScalar();
                    lblEnDüşükStok.Text=endusukstokist.ToString();
                    lblToplamÜrün.Text=toplamurunist.ToString();
                    lblToplamKategori.Text=toplamkategoriIst.ToString();
                    SqlCommand chart = new SqlCommand
                        ("select UrunAd+' - '+UrunMarka as Urun,(SatisFiyat-AlisFiyat) as KarMarkaji from TBLUrunler",bgl);
                    SqlDataAdapter da = new SqlDataAdapter(chart);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        chart1.Series[0].Points.AddXY(dr["Urun"].ToString(), Convert.ToDouble(dr["KarMarkaji"]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("İstatistikler Yüklenirken Bir Sıkıntı Oluştu.");
            }
            #endregion
        }


    }
}
