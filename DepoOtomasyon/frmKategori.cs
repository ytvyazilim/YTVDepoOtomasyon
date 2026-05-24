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
    public partial class frmKategori : Form
    {
        sqlbaglanti baglanti = new sqlbaglanti();
        public frmKategori()
        {
            InitializeComponent();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void KategoriListele()
        {
            try
            {
                using (SqlConnection bgl = baglanti.baglanti())
                {
                    SqlCommand cmd = new SqlCommand
                        ("select * from TBLKategori",bgl);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch
            {
                MessageBox.Show("Datalar Listelenirken Sorun Oluştu.");
            }
        }
        private void frmKategori_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtKategori.Text))
                {
                    return;
                }
                else
                {
                    using (SqlConnection bgl = baglanti.baglanti())
                    {
                        SqlCommand cmd = new SqlCommand
                            ("insert into TBLKategori values(@p1)", bgl);
                        cmd.Parameters.Add("@p1", SqlDbType.VarChar).Value = txtKategori.Text;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"{txtKategori.Text} Adlı Kategori Başarıyla Eklendi");
                        
                    }
                }
            }
            catch
            {
                MessageBox.Show("Kategori Eklenirken Bir Hata Oluştu.");   
            }
            finally { KategoriListele(); txtKategori.Clear(); }
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if(secilenindex<=0 || string.IsNullOrEmpty(txtKategori.Text))
                {
                    return;
                }
                else
                {
                    using (SqlConnection bgl = baglanti.baglanti())
                    {
                        SqlCommand cmd2 = new SqlCommand(
                            "select KategoriAd from TBLKategori where KategoriID=@p1",bgl);
                        cmd2.Parameters.Add("@p1",SqlDbType.SmallInt).Value=secilenindex;
                        string eskiad = cmd2.ExecuteScalar().ToString();
                        SqlCommand cmd = new SqlCommand
                            ("update TBLKategori set KategoriAd=@p1 where KategoriID=@p2",bgl);
                        cmd.Parameters.Add("@p1",SqlDbType.VarChar).Value=txtKategori.Text;
                        cmd.Parameters.Add("@p2", SqlDbType.SmallInt).Value = secilenindex;
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show($"-{eskiad}- Adlı Kategori -{txtKategori.Text} Olarak Değiştirilmiştir.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Güncelleme Yapılırken Bir Hata Oluştu.");
            }
            finally
            {
                KategoriListele();
                txtKategori.Clear();
            }
        }
        int secilenindex=0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            secilenindex = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if(secilenindex<=0)
                {
                    return;
                }
                else
                {
                    using (SqlConnection bgl = baglanti.baglanti())
                    {
                        SqlCommand cmd2 = new SqlCommand("select KategoriAd from TBLKategori where KategoriID=@p1",bgl);
                        cmd2.Parameters.Add("@p1",SqlDbType.SmallInt).Value=secilenindex;
                        object eskiad = cmd2.ExecuteScalar();
                        SqlCommand cmd = new SqlCommand
                            ("delete from TBLKategori where KategoriID=@p1",bgl);
                        cmd.Parameters.Add("@p1",SqlDbType.SmallInt).Value=secilenindex;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"{secilenindex} Numaralı {eskiad} Adlı Kategori Silinmiştir.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Silerken Bir Sorun Oluştu.");
            }
            finally
            {
                KategoriListele();
                txtKategori.Clear();
            }
        }
    }
}
