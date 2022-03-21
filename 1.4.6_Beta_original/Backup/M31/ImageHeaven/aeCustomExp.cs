/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 24/4/2008
 * Time: 6:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data.Odbc;
using System.Data;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeCustomExp.
	/// </summary>
	public partial class aeCustomExp : Form
	{
		private wfeImage wImage=null;
		private wfeProject pProject=null;
		private wfeBatch pBatch=null;
		private OdbcConnection sqlCon;
        private Credentials crd = new Credentials();

		public aeCustomExp(wfeImage prmImage,OdbcConnection prmCon,Credentials prmCrd)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			wImage = prmImage;
			sqlCon = prmCon;
            crd = prmCrd;
			InitializeComponent();
			this.Text = "Custom exception details";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void AeCustomExpLoad(object sender, EventArgs e)
		{
			pProject=new wfeProject(sqlCon);
			pBatch=new wfeBatch(sqlCon);
			lblProject.Text =pProject.GetProjectName(wImage.ctrlImage.ProjectKey);
			lblBatch.Text =pBatch.GetBatchName(wImage.ctrlImage.ProjectKey,wImage.ctrlImage.BatchKey);
			lblBox.Text = wImage.ctrlImage.BoxNumber.ToString();
			lblPolicy.Text = wImage.ctrlImage.PolicyNumber.ToString();
			lblImageName.Text = wImage.ctrlImage.ImageName.ToString();
		}
		
		void CmdCancelClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void CmdSaveClick(object sender, EventArgs e)
		{
			string prblem=string.Empty;
			if(rdoDarkImage.Checked == true)
			{
				prblem=rdoDarkImage.Text;
			}
			if(rdoDifferentPolicyNo.Checked == true)
			{
				prblem=rdoDifferentPolicyNo.Text;
			}
			if(rdoDocumentMixed.Checked == true)
			{
				prblem=rdoDocumentMixed.Text;
			}
			if(rdoImagesMissing.Checked == true)
			{
				prblem=rdoImagesMissing.Text;
			}
			if(rdoOther.Checked == true)
			{
				prblem=rdoOther.Text;
			}
			if(rdoPhotoScan.Checked == true)
			{
				prblem=rdoPhotoScan.Text;
			}
			if(rdoPolicyNumberMismatch.Checked == true)
			{
				prblem=rdoPolicyNumberMismatch.Text;
			}
			if(rdoSkewedImage.Checked == true)
			{
				prblem=rdoSkewedImage.Text;
			}
			if(wImage.AddCustomException(ihConstants._NOT_RESOLVED,prblem,txtRemarks.Text,crd) == true)
			{
				this.Close();
			}
			else
			{
				MessageBox.Show("Error while adding exception","Error");
			}
		}
	}
}
