/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 20/3/2009
 * Time: 11:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using NovaNet.Utils;
using System.IO;

namespace LItems
{
	/// <summary>
	/// Structure for policy table
	/// </summary>
	public class udtPolicy: NovaNet.wfe.udtCmd
	{
		public int proj_key;
  		public int batch_key;
  		public int box_number;
  		public int policy_number;
  		public string policy_path;
  		public string created_by;
  		public string created_dttm;
  		public string modified_by;
  		public string modified_dttm;
  		public int count_of_pages;
  		public int status;
	}
	
	public struct policyException
	{
  		public int missing_img_exp;
  		public int crop_clean_exp;
  		public int poor_scan_exp;
  		public int wrong_indexing_exp;
  		public int linked_policy_exp;
  		public int decision_misd_exp;
  		public int extra_page_exp;
  		public int rearrange_exp;
  		public int other_exp;
  		public int move_to_respective_policy_exp;
  		public int solved;
  		public string comments;
  		public int status;
	}
	
	public class CtrlPolicy: NovaNet.wfe.wItemControl
	{
		private int proj_Key;
  		private int batch_key;
  		private int	box_number;
  		private int policy_number;
		
		
		public CtrlPolicy(int projKey, int batchKey,int boxNumber,int policyNumber)
		{
			proj_Key=projKey;
			batch_key=batchKey;
			box_number=boxNumber;
			policy_number=policyNumber;
		}
		public int BatchKey
		{
			get
			{
				return batch_key;
			}
		}
		
		public int ProjectKey
		{
			get
			{
				return proj_Key;
			}
		}
		public int BoxNumber
		{
			get
			{
				return box_number;
			}
		}
		public int PolicyNumber
		{
			get
			{
				return policy_number;
			}
		}
	}
	
	/// <summary>
	/// Description of wfePolicy.
	/// </summary>
	public class wfePolicy: wItem, StateData
	{
        MemoryStream stateLog;
        byte[] tmpWrite;
		OdbcConnection sqlCon;
		public CtrlPolicy ctrlPolicy=null;
		udtPolicy Data=null;
		OdbcDataAdapter sqlAdap;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
		
		public wfePolicy(OdbcConnection prmCon): base(prmCon, NovaNet.Utils.Constants._ADDING)
		{
			sqlCon=prmCon;
            exMailLog.SetNextLogger(exTxtLog);
		}
		
		public wfePolicy(OdbcConnection prmCon, CtrlPolicy prmCtrl): base(prmCon, NovaNet.Utils.Constants._EDITING)
		{
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
			sqlCon=prmCon;
			ctrlPolicy = prmCtrl;
            //exMailLog.SetNextLogger(exTxtLog);
            //LoadValuesFromDB();
		}
		public override bool Commit()
		{
			throw new NotImplementedException();
		}
		public override bool KeyCheck(string prmValue)
		{
			throw new NotImplementedException();
		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
		public override udtCmd LoadValuesFromDB()
		{
			string sqlStr=null;
			
			DataSet policyDs=new DataSet();
			Data=new udtPolicy();
			try 
			{
				sqlStr="select proj_key,batch_key,box_number,policy_number,policy_path,created_by,created_dttm,modified_by,modified_dttm,count_of_pages,status from policy_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			if(policyDs.Tables[0].Rows.Count>0)
			{
				Data.policy_path=policyDs.Tables[0].Rows[0]["policy_path"].ToString();
			}
			return Data;
		}
		
		public int  GetPolicyCount(eSTATES[] state)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select count(*) from policy_master " + 
					" where proj_key=" + ctrlPolicy.ProjectKey + 
				" and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber;
			
			for(int j=0;j<state.Length;j++)
			{
				if((int)state[j]!= 0)
				{
					if(j==0)
					{
						sqlStr=sqlStr + " and (status=" + (int)state[j] ;
					}
					else
						sqlStr=sqlStr + " or status=" + (int)state[j] ;
				}
			}
			sqlStr = sqlStr + ") ";
			try 
			{
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(dsImage);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
            if (dsImage.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsImage.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
		}

        public int GetTransactionLogCount(string prmBatchId,string prmDate,string prmUser,eSTATES state)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            switch (state)
            {
                case eSTATES.POLICY_QC:
                    {
                        sqlStr = "SELECT count(*) FROM transaction_log where qc_user='" + prmUser + "' and date_format(qc_dttm,'%d/%m/%Y')='" + prmDate + "'";
                        break;
                    }
                case eSTATES.POLICY_INDEXED:
                    {
                        sqlStr = "SELECT count(*) FROM transaction_log where index_user='" + prmUser + "' and date_format(index_dttm,'%d/%m/%Y')='" + prmDate + "'";
                        break;
                    }
                case eSTATES.POLICY_FQC:
                    {
                        sqlStr = "SELECT count(*) FROM transaction_log where fqc_user='" + prmUser + "' and date_format(fqc_dttm,'%d/%m/%Y')='" + prmDate + "'";
                        break;
                    }
            }
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            if (dsImage.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsImage.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public int GetPolicyStatus()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from policy_master " +
                    " where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0]["status"]);
        }

		public DataSet GetPolicyDetails()
		{
			string sqlStr=null;
			
			DataSet policyDs=new DataSet();
			
			try 
			{
                sqlStr = "select A.name_of_policyholder,A.date_of_birth,A.date_of_commencement,A.customer_id,A.policy_no,B.Scan_upload_flag,date_format(B.scanned_date,'%Y%m%d') as scanned_date,B.Incremented_Scan,B.status,A.serial_number from rawdata A,policy_master B where (A.proj_key=B.proj_key and A.batch_key=b.batch_key and A.policy_no = B.policy_number) and (B.proj_key=" + ctrlPolicy.ProjectKey + " and B.batch_key=" + ctrlPolicy.BatchKey + " and B.box_number=" + ctrlPolicy.BoxNumber + " and B.policy_number=" + ctrlPolicy.PolicyNumber + ")";
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return policyDs;
		}
        public DataSet GetPolicyLog()
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select Scanned_user,qc_user,index_user,fqc_user from transaction_log where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return policyDs;
        }
        public DataSet GetAllPolicyDetailsRaw(wfeBox wBox)
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select A.name_of_policyholder,A.date_of_birth,A.date_of_commencement,A.customer_id,trim(A.policy_no) as policy_no,A.serial_number,A.batch_serial from rawdata A where A.proj_key=" + wBox.ctrlBox.ProjectCode + " and A.batch_key=" + wBox.ctrlBox.BatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            
            return policyDs;
        }
        /*
        public int GetMaxSerial(wfeBox wBox)
        {
            string sqlStr = null;
            string maxSerial = string.Empty;
            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select max(A.serial_number) from rawdata A,policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.proj_key=" + ctrlPolicy.ProjectKey + " and A.batch_key=" + ctrlPolicy.BatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            if (policyDs.Tables[0].Rows.Count > 0)
            {
                maxSerial = policyDs.Tables[0].Rows[0][0].ToString();
                return Convert.ToInt32(maxSerial);
            }
            else
            {
                maxSerial = 0;
            }
        }
         */ 
        public DataSet GetAllPolicyDetails(wfeBox wBox,out OdbcDataAdapter pAdp)
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select Scan_upload_flag,date_format(scanned_date,'%Y%m%d') as scanned_date,Incremented_Scan,status,policy_number,box_number,proj_key,batch_key from policy_master where proj_key=" + wBox.ctrlBox.ProjectCode + " and batch_key=" + wBox.ctrlBox.BatchKey + " order by box_number,policy_number";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
                
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            finally
            {
                pAdp = sqlAdap;
            }
            return policyDs;
        }
        public DataSet GetCustExcpList()
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select box_number,policy_number,problem_type,image_name,remarks,created_by as User_Name,if(status=2,'Unresolved','Resolved') as ImageStatus from custom_exception where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return policyDs;
        }

        public DataSet GetMaxScannedDate()
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select min(date_format(B.scanned_date,'%Y%m%d')) as scanned_date from policy_master B where b.proj_key=" + ctrlPolicy.ProjectKey + " and b.batch_key=" + ctrlPolicy.BatchKey + " and b.box_number=" + ctrlPolicy.BoxNumber;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return policyDs;
        }
		public DataSet GetAllException()
		{
			string sqlStr=null;
			
			DataSet expDs=new DataSet();
			
			try 
			{
				sqlStr="select missing_img_exp,crop_clean_exp,poor_scan_exp,wrong_indexing_exp,linked_policy_exp,decision_misd_exp,extra_page_exp,rearrange_exp,other_exp,move_to_respective_policy_exp,comments from lic_qa_log where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and policy_number=" + ctrlPolicy.PolicyNumber + " and qa_status<>0";
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(expDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return expDs;
		}
        public int GetLICLogStatus()
        {
            string sqlStr = null;

            DataSet expDs = new DataSet();

            try
            {
                sqlStr = "select qa_status from lic_qa_log where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and policy_number=" + ctrlPolicy.PolicyNumber;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(expDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            if (expDs.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(expDs.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }
        public DataSet GetMissingDocumentPolicyLst()
        {
            string sqlStr = null;

            DataSet expDs = new DataSet();

            try
            {
                sqlStr = "select doc_type,count(*),policy_number,box_number from image_master where batch_key = " + ctrlPolicy.BatchKey + " and policy_number=" + ctrlPolicy.PolicyNumber + " and doc_type in ('Proposal_form','Policy_bond','Signature_page','Proposal_review_slip') group by policy_number, doc_type";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(expDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return expDs;
        }
		public DataSet GetPolicyList(eSTATES[] prmState)
		{
			string sqlStr=null;
			
			DataSet policyDs=new DataSet();
			
			try 
			{
				if (prmState.Length == 0)
				{
                    sqlStr = "select A.policy_no,A.name_of_policyholder,A.date_of_birth,A.date_of_commencement,B.status,B.count_of_pages,B.photo from rawdata A, policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.policy_no = B.policy_number and B.proj_key=" + ctrlPolicy.ProjectKey + " and B.batch_key=" + ctrlPolicy.BatchKey + " and B.box_number=" + ctrlPolicy.BoxNumber;
				}
				else
				{
                    if (ctrlPolicy.BoxNumber != 0)
                    {
                        sqlStr = "select A.policy_no,A.name_of_policyholder,A.date_of_birth,A.date_of_commencement,B.status,B.count_of_pages,B.photo,B.status from rawdata A, policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.policy_no = B.policy_number and B.proj_key=" + ctrlPolicy.ProjectKey + " and B.batch_key=" + ctrlPolicy.BatchKey + " and B.box_number=" + ctrlPolicy.BoxNumber;
                    }
                    else
                    {
                        sqlStr = "select A.policy_no,A.name_of_policyholder,A.date_of_birth,A.date_of_commencement,B.status,B.count_of_pages,B.photo,B.status,B.box_number from rawdata A, policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.policy_no = B.policy_number and B.proj_key=" + ctrlPolicy.ProjectKey + " and B.batch_key=" + ctrlPolicy.BatchKey;
                    }

                    for (int j = 0; j < prmState.Length; j++)
                    {
                        if ((int)prmState[j] != 0)
                        {
                            if (j == 0)
                            {
                                sqlStr = sqlStr + " and (B.status=" + (int)prmState[j];
                            }
                            else
                                sqlStr = sqlStr + " or B.status=" + (int)prmState[j];
                        }
                    }
                    sqlStr = sqlStr + ")";
                }

				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return policyDs;
		}

        public DataSet GetImportablePolicyList(eSTATES[] prmState)
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                
                    sqlStr = "select A.policy_number,A.STATUS from policy_master A,image_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.box_number=B.box_number and A.policy_number=B.policy_number and B.proj_key=" + ctrlPolicy.ProjectKey + "  and B.batch_key=" + ctrlPolicy.BatchKey + " and B.box_number=" + ctrlPolicy.BoxNumber;
                
                    for (int j = 0; j < prmState.Length; j++)
                    {
                        if ((int)prmState[j] != 0)
                        {
                            if (j == 0)
                            {
                                sqlStr = sqlStr + " and (a.status=" + (int)prmState[j];
                            }
                            else
                                sqlStr = sqlStr + " or a.status=" + (int)prmState[j];
                        }
                    }
                    sqlStr = sqlStr + ") and (B.policy_number in (select policy_number from image_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and doc_type='Policy_bond' group by policy_number having count(policy_number) = 1) or B.policy_number not in " +
                             "(select policy_number from image_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and doc_type='Policy_bond' group by policy_number)) group by policy_number";

                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return policyDs;
        }

        public string GetPolicyPath()
        {
            string sqlStr = null;
            string path = string.Empty;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select B.policy_path from policy_master B  where B.proj_key=" + ctrlPolicy.ProjectKey + " and B.batch_key=" + ctrlPolicy.BatchKey + " and B.box_number=" + ctrlPolicy.BoxNumber + " and B.policy_number=" + ctrlPolicy.PolicyNumber;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            if (policyDs.Tables[0].Rows.Count > 0)
            {
                path = policyDs.Tables[0].Rows[0]["policy_path"].ToString();
            }
            return path;
        }


		public int GetPolicyPhotoStatus()
		{
			string sqlStr=null;
			
			DataSet policyDs=new DataSet();
			
			try 
			{
				sqlStr="select photo from policy_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return Convert.ToInt32(policyDs.Tables[0].Rows[0]["photo"].ToString());
		}
		
		public int GetPolicyPageCount()
		{
			string sqlStr=null;
			int pageCount = 0;
			DataSet policyDs=new DataSet();
			
			try 
			{
				sqlStr="select policy_number,count_of_pages from policy_master  where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			if(policyDs.Tables[0].Rows.Count>0)
			{
				pageCount =Convert.ToInt32(policyDs.Tables[0].Rows[0]["count_of_pages"].ToString());
			}
			return pageCount;
		}
		
//		public bool GetPolicyPhotoStatus()
//		{
//			string sqlStr=null;
//			string err=null;
//			int pageCount = 0;
//			DataSet policyDs=new DataSet();
//			
//			try 
//			{
//				sqlStr="select policy_number,photo from ih_db.policy_master  where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber ;
//				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
//				sqlAdap.Fill(policyDs);
//			}
//			catch (Exception ex) 
//			{
//				sqlAdap.Dispose();
//				err=ex.Message;
//			}
//			
//			if(policyDs.Tables[0].Rows.Count>0)
//			{
//				return true;
//			}
//			else
//				return false;
//		}
//		
		public int GetInventoryInExcp()
		{
			string sqlStr=null;
			int excpType = 2;
			DataSet policyDs=new DataSet();
			
			try 
			{
				sqlStr="select exception_type from inventory_in_exception  where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			if(policyDs.Tables[0].Rows.Count>0)
			{
				excpType =Convert.ToInt32(policyDs.Tables[0].Rows[0]["exception_type"].ToString());
			}
			return excpType;
		}
		
		public int GetLicExpCount()
		{
			string sqlStr=null;
			DataSet policyDs=new DataSet();
			
			try 
			{
				sqlStr="select policy_number from lic_qa_log where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(policyDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return policyDs.Tables[0].Rows.Count;
		}
		
		public override bool TransferValues(udtCmd cmd)
		{
			throw new NotImplementedException();
		}
		public bool UpdateStatus(eSTATES state,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update policy_master" +
                " set status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
				" and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
				" and policy_number=" + ctrlPolicy.PolicyNumber + " and status<>" + (int)eSTATES.POLICY_EXPORTED;
				
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            int i = sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
        public bool UpdateStatus(eSTATES state, Credentials prmCrd,bool pLock)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update policy_master" +
                " set Locked_uid = null,expires_dttm=null,invalid=0,status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                " and policy_number=" + ctrlPolicy.PolicyNumber + " and status<>" + (int)eSTATES.POLICY_EXPORTED;

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
        public bool UpdateAllPolicyStatus(eSTATES state,Credentials pCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update policy_master" +
                " set status=" + (int)state + ",modified_by='" + pCrd.created_by + "',modified_dttm='" + pCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                " and (status<>" + (int)eSTATES.POLICY_ON_HOLD + " and status<>" + (int)eSTATES.POLICY_MISSING + ")";

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
public bool UpdateStatus(int pStatus,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update policy_master" +
                " set status=" + pStatus + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
				" and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
				" and policy_number=" + ctrlPolicy.PolicyNumber + " and status<>" + (int)eSTATES.POLICY_EXPORTED;
				
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            int i = sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
        public bool UpdateStatus(eSTATES state, Credentials prmCrd,OdbcTransaction prmTrans)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update policy_master" +
                " set status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                " and policy_number=" + ctrlPolicy.PolicyNumber;

            try
            {

                sqlTrans = prmTrans;
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }

        public bool DeleteTransLogEntry()
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"delete from transaction_log" +
                 " where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber;

            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }

        public bool UpdateTransactionLog(eSTATES state,Credentials prmCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            switch (state)
            {
                case eSTATES.POLICY_SCANNED:
                    {
                        sqlStr = @"insert into transaction_log (proj_key,Batch_Key,Box_number,Policy_number,Scanned_user,scanned_dttm,fqc_user)" +
                        " values(" + ctrlPolicy.ProjectKey + "," + ctrlPolicy.BatchKey + "," + ctrlPolicy.BoxNumber + "," + ctrlPolicy.PolicyNumber + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','')";
                        break;
                    }
                case eSTATES.POLICY_QC:
                    {
                        sqlStr = @"update transaction_log" +
                        " set QC_User='" + prmCrd.created_by + "',Qc_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                        " and policy_number=" + ctrlPolicy.PolicyNumber;
                        break;
                    }
                case eSTATES.POLICY_INDEXED:
                    {
                        sqlStr = @"update transaction_log" +
                        " set Index_User='" + prmCrd.created_by + "',Index_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                        " and policy_number=" + ctrlPolicy.PolicyNumber;
                        break;
                    }
                case eSTATES.POLICY_FQC:
                    {
                        sqlStr = @"update transaction_log" +
                        " set Fqc_User=concat(fqc_user,'," + prmCrd.created_by + "'),fqc_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                        " and policy_number=" + ctrlPolicy.PolicyNumber;
                        break;
                    }
            }
            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n" + "Wfe State--" + Convert.ToString(Convert.ToInt32(state)) + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }

		public bool SavePolicyPageCount(int prmPageCount,int prmPhoto,eSTATES prmstatus,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update policy_master" +
                " set count_of_pages=" + prmPageCount + ",photo=" + prmPhoto + ",status=" + (int)prmstatus + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
				" and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
				" and policy_number=" + ctrlPolicy.PolicyNumber ;
				
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
	            commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}

        public bool DeleteAllPage()
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"delete from image_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;

            try
            {
                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i= sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
		
		public bool UpdateScanDetails(string prmScanDate,string prmScanUploadFlag)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update policy_master" +
				" set Scan_upload_flag='" + prmScanUploadFlag + "',scanned_date='" + prmScanDate + "' where proj_key=" + ctrlPolicy.ProjectKey +
				" and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
				" and policy_number=" + ctrlPolicy.PolicyNumber;
				
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
	            commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
		public bool UpdateQaPolicyException(NovaNet.Utils.Credentials prmCrd,policyException udtException)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			try
			{
				sqlTrans = sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;

                    sqlStr = @"update lic_qa_log set missing_img_exp=" + udtException.missing_img_exp + ",crop_clean_exp=" + udtException.crop_clean_exp + ",poor_scan_exp=" + udtException.poor_scan_exp + ",wrong_indexing_exp=" + udtException.wrong_indexing_exp + ",linked_policy_exp=" + udtException.linked_policy_exp + ",decision_misd_exp=" + udtException.decision_misd_exp + ",extra_page_exp=" + udtException.extra_page_exp + ",rearrange_exp=" + udtException.rearrange_exp + ",other_exp=" + udtException.other_exp + ",move_to_respective_policy_exp=" + udtException.move_to_respective_policy_exp + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "',SOLVED=" + udtException.solved + ",comments='" + udtException.comments + "' where proj_key= " + ctrlPolicy.ProjectKey + " and box_number= " + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber + " and batch_key=" + ctrlPolicy.BatchKey ;
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                sqlTrans.Commit();
	                commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
        public bool UnLockPolicy()
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            try
            {
                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;

                sqlStr = @"update policy_master set Locked_uid = null,expires_dttm=null,invalid=0 where proj_key= " + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number= " + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();

                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
        public bool LockPolicy(Credentials pCrd,OdbcTransaction pTrns)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            DataSet ds = new DataSet();
            long time=0;
            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = pTrns;
                sqlStr = "select SYSVALUES from SYSCONFIG  where SYSKEYS='LOCK_EXPIRES_TIME'";
                sqlCmd.CommandText = sqlStr;
                sqlAdap = new OdbcDataAdapter(sqlCmd);
                sqlAdap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    time = Convert.ToInt32(ds.Tables[0].Rows[0]["SYSVALUES"].ToString());
                }

                sqlStr = @"update policy_master set expires_dttm = Now()+interval " + time + " SECOND,Locked_uid ='" + pCrd.created_by + "',invalid=1 where proj_key= " + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number= " + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
		public bool InitiateQaPolicyException(NovaNet.Utils.Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			try
			{
					sqlTrans = sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;
	                
					sqlStr=@"insert into lic_qa_log (proj_key,box_number,policy_number,batch_key,created_by,created_dttm) values(" + ctrlPolicy.ProjectKey + "," + ctrlPolicy.BoxNumber + "," + ctrlPolicy.PolicyNumber + "," + ctrlPolicy.BatchKey + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm +"')";
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                sqlTrans.Commit();
	                commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
		
		public bool SaveInventoryInException(NovaNet.Utils.Credentials prmCrd,int prmInvtInExcpType)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			try
			{
				sqlStr=@"delete from inventory_in_exception where proj_key=" +
					ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and policy_number=" + ctrlPolicy.PolicyNumber;
					sqlTrans=sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                sqlStr=@"insert into inventory_in_exception(proj_key,batch_key,box_number, policy_number,exception_type,created_by,created_dttm) values(" +
					ctrlPolicy.ProjectKey + "," + ctrlPolicy.BatchKey + "," + ctrlPolicy.BoxNumber + "," + ctrlPolicy.PolicyNumber + "," + prmInvtInExcpType + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "')";
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                sqlTrans.Commit();
	                commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
		
		public bool SavePolicyPageCountException(NovaNet.Utils.Credentials prmCrd,int prmPageToBeScanned,int prmPageScanned)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			try
			{
	                sqlStr=@"insert into policy_page_count_exception_log(Proj_key,Btach_key,box_number,policy_number,tot_page_to_be_scanned,page_scanned,created_by,created_dttm) values(" +
					ctrlPolicy.ProjectKey + "," + ctrlPolicy.BatchKey + "," + ctrlPolicy.BatchKey + "," + ctrlPolicy.PolicyNumber + "," + prmPageToBeScanned +
					"," + prmPageScanned + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "')";
	                sqlTrans=sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                sqlTrans.Commit();
	                commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
		
		public bool QaExceptionStatus(int prmStatus,int prmExpStatus)
		{
			string sqlStr=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			OdbcTransaction prmTrans;
				
			try
			{
                prmTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = prmTrans;
                
                sqlStr = @"update lic_qa_log" +
                " set solved=" + prmStatus + " where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                " and policy_number=" + ctrlPolicy.PolicyNumber + " and solved <>" + 7;

				
	            sqlCmd.CommandText = sqlStr;
	            sqlCmd.ExecuteNonQuery();

                sqlStr = @"update lic_qa_log" +
                " set qa_status=" + prmExpStatus + " where proj_key=" + ctrlPolicy.ProjectKey +
                " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber +
                " and policy_number=" + ctrlPolicy.PolicyNumber;


                sqlCmd.CommandText = sqlStr;
                int i=sqlCmd.ExecuteNonQuery();

	            prmTrans.Commit();
	            commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
		public bool UpdatePolicyDetails(string prmName,string prmDob,string prmCommDt)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update rawdata" +
				" set name_of_policyholder='" + prmName + "',date_of_birth='" + prmDob + "',date_of_commencement='" + prmCommDt + "' where proj_key=" + ctrlPolicy.ProjectKey +
				" and batch_key=" + ctrlPolicy.BatchKey +
				" and policy_no=" + ctrlPolicy.PolicyNumber ;
				
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
	            commitBol=true;
			}
			catch(Exception ex)
			{
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
        public DataSet GetFilteredPolicy(string pDocType, string pFilterSign, int pCount,eSTATES[] pState)
        {
            string sqlStr = null;
            string path = string.Empty;
            string stateSql = string.Empty;
            DataSet policyDs = new DataSet();

            try
            {
                for (int j = 0; j < pState.Length; j++)
                {
                    if ((int)pState[j] != 0)
                    {
                        if (j == 0)
                        {
                            stateSql = stateSql + "(A.status=" + (int)pState[j];
                        }
                        else
                            stateSql = stateSql + " or A.status=" + (int)pState[j];
                    }
                }
                stateSql = stateSql + ")";
                if ((pCount == 0) && (pFilterSign == "="))
                {
                    sqlStr = "select distinct A.policy_number from policy_master A where " + stateSql + " and A.proj_key=" + ctrlPolicy.ProjectKey +
                                " and A.batch_key=" + ctrlPolicy.BatchKey + " and A.box_number=" + ctrlPolicy.BoxNumber + " and A.policy_number not in " +
                               "(" +
                                "select policy_number from image_master where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber + " and doc_type = '" + pDocType + "' and status<>29 group by policy_number, doc_type )";
                }
                else
                {
                    //sqlStr = "select A.policy_number from image_master A,policy_master B " +
                    //        "where A.proj_key = B.proj_key and A.batch_key = B.batch_key and A.box_number = B.box_number and A.policy_number = B.policy_number and " + stateSql +
                    //        " and A.proj_key=" + ctrlPolicy.ProjectKey + " and A.batch_key=" + ctrlPolicy.BatchKey + " and A.box_number=" + ctrlPolicy.BoxNumber + " and A.status <> 29 and A.doc_type='" + pDocType + "' group by A.policy_number, A.doc_type having count(A.policy_number) " + pFilterSign + pCount;    
                    //if (pFilterSign == "<")
                    //{
                    //    pFilterSign = ">";
                    //}
                    //else
                    //{
                    //    pFilterSign = "<";
                    //}
                        sqlStr = "select policy_number from policy_master where policy_number in( " +
                            "select A.policy_number from image_master A " +
                            "where  A.proj_key=" + ctrlPolicy.ProjectKey + " and A.batch_key=" + ctrlPolicy.BatchKey + " and A.box_number=" + ctrlPolicy.BoxNumber + " and A.status <> 29 " +
                            " and A.doc_type='" + pDocType + "' group by A.policy_number, A.doc_type having count(A.policy_number) " + pFilterSign + pCount + ") and " +
                            stateLog + " proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number=" + ctrlPolicy.BoxNumber;
                    
                }
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return policyDs;
        }
        public DataSet GetRectifiedPolicyCount()
        {
            string sqlStr = null;

            DataSet expDs = new DataSet();

            try
            {
                sqlStr = "select policy_number from lic_qa_log where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and solved=7";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(expDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return expDs;
        }
        public bool DeleteFromInventory()
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = "delete FROM inventory_in_exception where policy_number not in(select policy_number from policy_master where (status=36 or status=37) and batch_key='" + ctrlPolicy.BatchKey + "' and box_number='" + ctrlPolicy.BoxNumber + "')";

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return commitBol;
        }
		public string GetBatchSerial()
		{
			string sqlStr=null;
			string batchName=null;
			
			DataSet batchDs=new DataSet();
			
			try 
			{
				sqlStr="select distinct batch_serial from rawdata where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(batchDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                exMailLog.Log(ex);
			}
			if(batchDs.Tables[0].Rows.Count>0)
			{
				batchName=batchDs.Tables[0].Rows[0]["batch_serial"].ToString();
			}
			else
				batchName=string.Empty;
			return batchName;
		}
        public string GetBatchSerial(wfeBox pBox)
        {
            string sqlStr = null;
            string batchName = null;

            DataSet batchDs = new DataSet();

            try
            {
                sqlStr = "select distinct batch_serial from rawdata where proj_key=" + pBox.ctrlBox.ProjectCode + " and batch_key=" + pBox.ctrlBox.BatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                exMailLog.Log(ex);
            }
            if (batchDs.Tables[0].Rows.Count > 0)
            {
                batchName = batchDs.Tables[0].Rows[0]["batch_serial"].ToString();
            }
            else
                batchName = string.Empty;
            return batchName;
        }
//		public bool UpdatePolicy(string prmPolicyNo)
//		{
//			
//		}
//		private bool QueryPolicyStatus(string prmPolicyNo)
//		{
//			
//		}
		public bool UpdateSrl(ArrayList arr)
        {
            String sqlStr=string.Empty;
            int tmp;
            OdbcTransaction sqlTrans=null;
            OdbcCommand sqlCmd=new OdbcCommand();
            int lengthOfPageName = 0;
            try
            {
            sqlTrans=sqlCon.BeginTransaction();
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction=sqlTrans;
            lengthOfPageName = Convert.ToInt32(ctrlPolicy.PolicyNumber.ToString().Length) + 10;    
                for (int i=0; i < arr.Count; i++)
                {
                    tmp = i+1;
                    if (arr[i].ToString().Trim().Length > lengthOfPageName)
                    {
                        sqlStr = "Update image_master set serial_no = " + tmp + " where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number = " + ctrlPolicy.BoxNumber + " and policy_number = " + ctrlPolicy.PolicyNumber + " and page_name = '" + arr[i].ToString().Substring(0,lengthOfPageName) + "'";
                    }
                    else
                    {
                        sqlStr = "Update image_master set serial_no = " + tmp + " where proj_key=" + ctrlPolicy.ProjectKey + " and batch_key=" + ctrlPolicy.BatchKey + " and box_number = " + ctrlPolicy.BoxNumber + " and policy_number = " + ctrlPolicy.PolicyNumber + " and page_name = '" + arr[i].ToString() + "'";
                    }
                    sqlCmd.CommandText = sqlStr;
                    sqlCmd.ExecuteNonQuery();
                }
            sqlTrans.Commit();
            }
            catch(Exception ex)
            {
                sqlTrans.Rollback();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                return false;
            }
            return true;
        }
		
	}
}
