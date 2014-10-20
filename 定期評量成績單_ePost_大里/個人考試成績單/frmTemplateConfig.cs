using System;
using System.IO;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data.Configuration;

namespace 班級考試成績單
{
    /// <summary>
    /// 設定樣板
    /// </summary>
    public partial class frmTemplateConfig : FISCA.Presentation.Controls.BaseForm
    {
        private int _useTemplateNumber = 0;
        private byte[] _buffer = null;
        private string _base64string = null;
        private bool _isUpload = false;

        /// <summary>
        /// 建構式
        /// </summary>
        public frmTemplateConfig(byte[] buffer, int useTemplateNumber,int ReceiverIndex,int ReceiverAddressIndex)
        {
            InitializeComponent();

            _buffer = buffer;
            _base64string = Convert.ToBase64String(_buffer);
            _useTemplateNumber = useTemplateNumber;

            switch (_useTemplateNumber)
            {
                case 0:
                    chkDefault.Checked = true;
                    break;
                case 1:
                    chkCustom.Checked = true;
                    break;
                default:
                    break;
            }

            cmbCustodian.SelectedIndex = ReceiverIndex;
            cmbAddress.SelectedIndex = ReceiverAddressIndex;
        }

        #region private function
        /// <summary>
        /// 下載
        /// </summary>
        /// <param name="sfd"></param>
        /// <param name="fileData"></param>
        private void DownloadTemplate(SaveFileDialog sfd, byte[] fileData)
        {
            if ((fileData == null) || (fileData.Length == 0))
            {
                throw new Exception("檔案不存在，無法檢視。");
            }

            try
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);

                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
                System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch
            {
                throw new Exception("指定路徑無法存取。");
            }
        }

        /// <summary>
        /// 上傳
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="uploadIndex"></param>
        /// <param name="uploadData"></param>
        private void UploadTemplate(string fileName, ref bool uploadIndex, ref string uploadData)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);

            byte[] tempBuffer = new byte[fs.Length];
            fs.Read(tempBuffer, 0, tempBuffer.Length);

            MemoryStream ms = new MemoryStream(tempBuffer);

            try
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                wb.Open(ms, Aspose.Cells.FileFormatType.Excel2003);
                wb = null;
            }
            catch
            {
                throw new Exception("此版高中個人評量成績單範本限用相容於 Excel 2003 檔案。");
            }

            try
            {
                uploadData = Convert.ToBase64String(tempBuffer);
                uploadIndex = true;

                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 使用預設範本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            _useTemplateNumber = 0;
        }

        /// <summary>
        /// 使用自訂範本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCustom_CheckedChanged(object sender, EventArgs e)
        {
            _useTemplateNumber = 1;
        }

        /// <summary>
        /// 檢視預設範本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "另存新檔";
            sfd.FileName = "高中個人評量成績單.xls";
            sfd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DownloadTemplate(sfd, Resource.高中評量成績單);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 檢視自訂範本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkCustom_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "另存新檔";
            sfd.FileName = "自訂高中個人評量成績單.xls";
            sfd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DownloadTemplate(sfd, _buffer);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 上傳自訂範本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkUpload(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "上傳自訂高中個人評量成績單";
            ofd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UploadTemplate(ofd.FileName, ref _isUpload, ref _base64string);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            #region 儲存 Preference
            ConfigData config = K12.Data.School.Configuration["高中個人評量成績單"];

            config["TemplateNumber"] = ""+ _useTemplateNumber;

            if (chkCustom.Checked)
                config["CustomizeTemplate"] = _base64string;

            config["Custodian"] = "" + cmbCustodian.SelectedIndex;
            config["Address"] = "" + cmbAddress.SelectedIndex;

            config.Save();
            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }
    }
}