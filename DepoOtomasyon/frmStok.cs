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
using System.IO;

namespace DepoOtomasyon
{
    public partial class frmStok : Form
    {
        sqlbaglanti baglanti = new sqlbaglanti();
        public frmStok()
        {
            InitializeComponent();
        }
        void ÜrünleriListele()
        {
            try
            {
                using (SqlConnection bgl = baglanti.baglanti())
                {
                    SqlCommand cmd = new SqlCommand
                        ("select u.UrunID,u.UrunAd,u.UrunMarka,u.UrunGirisTarihi,k.KategoriAd,u.PERSONEL,u.AlisFiyat,u.SatisFiyat,(u.SatisFiyat-u.AlisFiyat) as KarMarkajı from TBLUrunler u INNER JOIN TBLKategori k on u.KATEGORI=k.KategoriID",bgl);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUrun.DataSource = dt;
                }
            }
            catch
            {
                MessageBox.Show("Ürünleri Listelerken Bir Hata Oluştu.");
            }
        }
        void StokListele(int UrunID)
        {
            try
            {
                using (SqlConnection bgl = baglanti.baglanti())
                {
                    SqlCommand cmd = new SqlCommand("select StokID,u.UrunAd,HareketTipi,Miktar,Tarih,Aciklama from StokHareketleri s inner join TBLUrunler u on s.URUN=u.UrunID where u.UrunID=@p1",bgl);
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = UrunID;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvStok.DataSource = dt;

                }
            }
            catch
            {
                MessageBox.Show("Stokları Listelerken Bir Sorun Oluştu.");
            }
            
        }
        private void frmStok_Load(object sender, EventArgs e)
        {
            #region Ürünleri Listeleme
            ÜrünleriListele();
            #endregion
        }
        
        private void dgvUrun_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            i = Convert.ToInt32(dgvUrun.Rows[e.RowIndex].Cells[0].Value);
            StokListele(i);
        }
        int i;
        private void button1_Click(object sender, EventArgs e)
        {
            
            
                if(i>0 && numericUpDown1.Value>0 && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(richTextBox1.Text))
                {
                        try
                        {
                            using (SqlConnection bgl = baglanti.baglanti())
                            {
                                SqlCommand cmd = new SqlCommand
                                ("insert into StokHareketleri values(@urunid,@harekettipi,@miktar,@aciklama)", bgl);
                                cmd.Parameters.Add("@urunid", SqlDbType.Int).Value = i;
                                cmd.Parameters.Add("@harekettipi", SqlDbType.NVarChar).Value = comboBox1.Text;
                                cmd.Parameters.Add("@miktar", SqlDbType.Int).Value = numericUpDown1.Value;
                                cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar).Value = richTextBox1.Text;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Lütfen Tüm Alanları Doldurunuz.");
                        }
                finally { ÜrünleriListele(); StokListele(i); }
                }
                else { return; }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (i == 0)
            {
                MessageBox.Show("Lütfen önce bir ürün seçin.");
                return;
            }
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12);
            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.Filter = "PDF Dosyası|*.pdf";
            kaydet.FileName = "StokRaporu";

            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc,
                        new System.IO.FileStream(kaydet.FileName, System.IO.FileMode.Create));
                    doc.Open();

                    doc.Add(new iTextSharp.text.Paragraph("Stok Hareketi Raporu", font));
                    doc.Add(new iTextSharp.text.Paragraph("Tarih: " + DateTime.Now.ToString(), font));
                    doc.Add(new iTextSharp.text.Paragraph(" "));

                    foreach (DataGridViewRow row in dgvStok.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string satir = "";
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            satir += cell.Value + "  |  ";
                        }
                        doc.Add(new iTextSharp.text.Paragraph(satir, font));
                    }

                    doc.Close();
                    MessageBox.Show("Rapor kaydedildi!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
    }
}


