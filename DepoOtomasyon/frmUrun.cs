using AForge.Video;
using AForge.Video.DirectShow;
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
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Speech.Recognition;
using System.Runtime.Remoting.Messaging;

namespace DepoOtomasyon
{
    public partial class frmUrun : Form
    {
        sqlbaglanti sql = new sqlbaglanti();
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        public frmUrun()
        {
            InitializeComponent();
        }
        void UrunleriListele()
        {
            try
            {
                using (SqlConnection baglanti = sql.baglanti())
                {
                    SqlCommand cmd = new SqlCommand
                        ("select * from TBLUrunler",baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                } 
            }
            catch
            {
                MessageBox.Show("VeriTabanı Yüklenirken Hata Oluştu.","YTV Yazılım");
            }
        }
        void KameraDoldur(params System.Windows.Forms.ComboBox[] kutular)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (var cb in kutular)
            {
                cb.Items.Clear();

                foreach (FilterInfo device in filterInfoCollection)
                    cb.Items.Add(device.Name);

                if (cb.Items.Count > 0)
                    cb.SelectedIndex = 0;
            }
        }
        private void frmUrun_Load(object sender, EventArgs e)
        {
            #region KategoriListeleCombobox
            using (SqlConnection bgl = sql.baglanti())
            {
                SqlCommand cmd = new SqlCommand
                    ("select KategoriID,KategoriAd from TBLKategori",bgl);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbKategori.DataSource = dt;
                cmbKategori.DisplayMember = "KategoriAd";
                cmbKategori.ValueMember = "KategoriID";
                cmbKategori.SelectedIndex = -1;
                cmbKategori.Text = "KATEGORİ";
            }
            #endregion
            KameraDoldur(cmbAraCihaz, cmbCrudCihaz); //KAMERA Barkod İçin
            UrunleriListele();
            dataGridView1.RowHeadersVisible = false;
        }
        void BarkodTara(System.Windows.Forms.TextBox targetTextbox, PictureBox targetPictureBox, int deviceIndex)
        {
            lastResult = "";

            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[deviceIndex].MonikerString);

            videoCaptureDevice.NewFrame += (sender, eventArgs) =>
            {
                try
                {
                    Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
                    BarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(bmp);

                    
                    if (result != null && result.Text != lastResult && result.Text.Length == 13)
                    {
                        lastResult = result.Text;

                        
                        targetTextbox.BeginInvoke(new Action(() =>
                        {
                            Console.Beep();
                            targetTextbox.Text = result.Text;
                        }));
                    }

                    
                    targetPictureBox.Image = bmp;
                }
                catch { }
            };

            videoCaptureDevice.Start();
        }


        #region Barkod İşlemleri
        private void btnBaslat_Click(object sender, EventArgs e)
        {
            txtCrudBarkod.Focus();
            BarkodTara(txtCrudBarkod,pcbCrudKamera,cmbAraCihaz.SelectedIndex);
        }
        string lastResult = "";
        private void frmUrun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                }
            }
        }
        #endregion
        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region CRUD İşlemleri
        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbKategori.SelectedValue == null) { MessageBox.Show("Kategori Seçin"); return; }
                using (SqlConnection baglanti = sql.baglanti())
                {
                    SqlCommand cmd = new SqlCommand
                        ("insert into TBLUrunler values(@urunad,@urunmarka,getdate(),@kategori,@alis,@satis,1,0); SELECT SCOPE_IDENTITY();", baglanti);
                    cmd.Parameters.AddWithValue("@urunad",txtCrudUrunAd.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@urunmarka",txtCrudMarka.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@alis",decimal.Parse(txtCrudAlisFiyat.Text));
                    cmd.Parameters.AddWithValue("@satis",decimal.Parse(txtCrudSatisFiyat.Text));
                    cmd.Parameters.AddWithValue("@kategori", int.Parse(cmbKategori.SelectedValue.ToString()));
                    // 1 yazılan personel sonradan eklicem
                    int urunID = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    SqlCommand cmd2 = new SqlCommand
                        ("insert into TBLBarkod values(@barkod,@urunid)",baglanti);
                    cmd2.Parameters.AddWithValue("@barkod",txtCrudBarkod.Text);
                    cmd2.Parameters.AddWithValue("@urunid",urunID);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                UrunleriListele();
                txtTemizle();
            }
        }
        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtCrudUrunAd.Text)&&!string.IsNullOrEmpty(txtCrudMarka.Text)&&!string.IsNullOrEmpty(txtCrudAlisFiyat.Text)&&!string.IsNullOrEmpty(txtCrudSatisFiyat.Text)&&cmbKategori.SelectedIndex>0) 
                {
                    using (SqlConnection bgl = sql.baglanti())
                    {
                        SqlCommand cmd = new SqlCommand
                            ("update TBLUrunler set UrunAd=@1,UrunMarka=@2,KATEGORI=@3,AlisFiyat=@4,SatisFiyat=@5 where UrunID=@6",bgl);
                        cmd.Parameters.Add("@6", SqlDbType.Int).Value = secilenindex;
                        cmd.Parameters.Add("@1",SqlDbType.VarChar).Value= txtCrudUrunAd.Text;
                        cmd.Parameters.Add("@2",SqlDbType.VarChar).Value = txtCrudMarka.Text;
                        cmd.Parameters.Add("@3",SqlDbType.SmallInt).Value = cmbKategori.SelectedValue;
                        cmd.Parameters.Add("@4", SqlDbType.Decimal).Value = decimal.Parse(txtCrudAlisFiyat.Text);
                        cmd.Parameters.Add("@5", SqlDbType.Decimal).Value = decimal.Parse(txtCrudSatisFiyat.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ürünler Başarıyla Güncellendi.","YTV YAZILIM");
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("Güncellenirken Bir Sorun Oluştu.");
            }
            finally
            {
                UrunleriListele();
            }
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection bgl = sql.baglanti())
                {
                    if(secilenindex<=0)
                    {
                        return;
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("select UrunAd from TBLUrunler where UrunID=@1",bgl);
                        cmd2.Parameters.Add("@1",SqlDbType.Int).Value = secilenindex;
                        object silinenurun= cmd2.ExecuteScalar();
                        SqlCommand cmdBarkod = new SqlCommand
                        ("delete from TBLBarkod where UrunID=@1", bgl);
                        cmdBarkod.Parameters.Add("@1", SqlDbType.Int).Value = secilenindex;
                        cmdBarkod.ExecuteNonQuery();
                        SqlCommand cmd = new SqlCommand
                        ("delete from TBLUrunler where UrunID=@1", bgl);
                        cmd.Parameters.Add("@1",SqlDbType.Int).Value=secilenindex;
                        cmd.ExecuteNonQuery ();
                        MessageBox.Show($"{secilenindex} ID'li {silinenurun} Adlı Ürün Başarıyla Silinmiştir.","YTV YAZILIM");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                UrunleriListele();
            }
        }
        #endregion
        void txtTemizle()
        {
            txtCrudUrunAd.Text=string.Empty;
            txtCrudMarka.Text=string.Empty;
            txtCrudAlisFiyat.Text=string.Empty;
            txtCrudSatisFiyat.Text = string.Empty;
            
        }
        private void btnBaslatUrunAra_Click(object sender, EventArgs e)
        {
            txtAraBarkod.Focus();
            BarkodTara(txtAraBarkod,pictureBox2,cmbCrudCihaz.SelectedIndex);
        }

        private void txtBarkodUrunArat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection baglanti = sql.baglanti())
                {
                    SqlCommand cmd = new SqlCommand
                        ("exec BarkodListele @barkod", baglanti);
                    cmd.Parameters.Add("@barkod", SqlDbType.VarChar).Value = txtAraBarkod.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewUrunArat.DataSource = dt;
                    txtTemizle();
                    btnUrunAra.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCrudBarkod_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }
        
        private void txtCrudUrunAd_Click(object sender, EventArgs e)
        {
            
        }
        int secilenindex=0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex<0) {
                 return; }
            secilenindex = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtCrudUrunAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCrudMarka.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        
    }
}
