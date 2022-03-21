/*
 * Created by SharpDevelop.
 * User: user
 * Date: 3/21/2009
 * Time: 11:34 AM
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
	/// Description of wfeImage.
	/// </summary>
	public class CtrlImage: NovaNet.wfe.wItemControl
	{
		private int proj_Key;
  		private int batch_key;
  		private int	box_number;
  		private int policy_number;
		private string imageName;
		private string docType;
		
		public CtrlImage(int projKey, int batchKey,int boxNumber,int policyNumber,string prmImageName,string prmDocType)
		{
			proj_Key=projKey;
			batch_key=batchKey;
			box_number=boxNumber;
			policy_number=policyNumber;
			imageName =prmImageName;
			docType=prmDocType;
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
		public string ImageName
		{
			get
			{
				return imageName;
			}
		}
		public string DocType
		{
			get
			{
				return docType;
			}
		}
	}
	/// <summary>
	/// Description of wfePolicy.
	/// </summary>
	public class wfeImage: wItem, StateData
	{
		OdbcConnection sqlCon;
        MemoryStream stateLog;
        byte[] tmpWrite;
		public CtrlImage ctrlImage=null;
		wItemControl wic=null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
		public wfeImage(OdbcConnection prmCon): base(prmCon, NovaNet.Utils.Constants._ADDING)
		{
			sqlCon=prmCon;
            exMailLog.SetNextLogger(exTxtLog);
            
		}
		
		public wfeImage(OdbcConnection prmCon, CtrlImage prmCtrl): base(prmCon, NovaNet.Utils.Constants._EDITING)
		{
			sqlCon=prmCon;
			ctrlImage = prmCtrl;
            exMailLog.SetNextLogger(exTxtLog);
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
		public override udtCmd LoadValuesFromDB()
		{
			throw new NotImplementedException();
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
            int status;
			OdbcCommand sqlCmd=new OdbcCommand();
            //2. Collect the state of the image
            status = GetImageStatus();
            //3. Check whether state (parameter) is to export and current state of the image is rescanned_but_not_indexed
            if (status ==(int) eSTATES.PAGE_RESCANNED_NOT_INDEXED && state == eSTATES.PAGE_EXPORTED)
            {
                return false;
            }
            else
            {
                sqlStr = @"update image_master" +
                    " set status=" + (int)state + " , modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey +
                    " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                    " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "' and (status <> " + (int)eSTATES.PAGE_EXPORTED + ")";
            }
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
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
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
        /// <summary>
        /// Update image status with transaction eanbled
        /// </summary>
        /// <param name="state"></param>
        /// <param name="prmCrd"></param>
        /// <returns></returns>
        public bool UpdateAllImageStatus(eSTATES state, Credentials prmCrd,OdbcTransaction prmTrans)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update image_master" +
                " set status=" + (int)state + " , modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and (status <> " + (int)eSTATES.PAGE_DELETED + " and status <> " + (int)eSTATES.PAGE_ON_HOLD + " and status <> " + (int)eSTATES.PAGE_RESCANNED_NOT_INDEXED + " )";

            try
            {

                sqlTrans = prmTrans;
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
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
        /// <summary>
        /// Update image status with transaction eanbled
        /// </summary>
        /// <param name="state"></param>
        /// <param name="prmCrd"></param>
        /// <returns></returns>
        public bool UpdateAllImageStatus(eSTATES state, Credentials prmCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update image_master" +
                " set status=" + (int)state + " , modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and (status <> " + (int)eSTATES.PAGE_DELETED + " and status <> " + (int)eSTATES.PAGE_ON_HOLD + " and status <> " + (int)eSTATES.PAGE_RESCANNED_NOT_INDEXED + " and status<>" + (int) eSTATES.PAGE_EXPORTED + ")";

            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
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
		public bool TotalImageUpdateStatus(eSTATES state)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update image_master" +
				" set status=" + (int)state + " where proj_key=" + ctrlImage.ProjectKey +
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and status <>29";
				
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
        public bool RearrangePoposalDoctype(DataSet pTotImageName,int pMaxSerialNo)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();
            sqlTrans = sqlCon.BeginTransaction();
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sqlTrans;
            try
            {
                for (int i = 0; i < pTotImageName.Tables[0].Rows.Count; i++)
                {
                sqlStr = @"update image_master" +
                    " set serial_no=" + (pMaxSerialNo+i+2) + " where proj_key=" + ctrlImage.ProjectKey +
                    " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                    " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + pTotImageName.Tables[0].Rows[i]["page_name"].ToString() + "'";

                
                    sqlCmd.CommandText = sqlStr;
                    sqlCmd.ExecuteNonQuery();
                    
                    commitBol = true;
                    
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
            if (commitBol == true)
            {
                sqlTrans.Commit();
            }
            return commitBol;
        }
		public bool UpdateStatusAndDockType(eSTATES state,string prmDocType,string prmIndexImageName,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			
			OdbcCommand sqlCmd=new OdbcCommand();

            if (state != eSTATES.PAGE_DELETED)
            {
                sqlStr = @"update image_master" +
                    " set status=" + (int)state + " , page_index_name='" + prmIndexImageName + "', doc_type='" + prmDocType + "',modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey +
                    " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                    " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "'";
            }
            else
            {
                sqlStr = @"update image_master" +
                    " set status=" + (int)state + " ,modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey +
                    " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                    " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "'";
            }
			try
			{
				
				sqlTrans=sqlCon.BeginTransaction();
				sqlCmd.Connection = sqlCon;
				sqlCmd.Transaction=sqlTrans;
	            sqlCmd.CommandText = sqlStr;
	            int i = sqlCmd.ExecuteNonQuery();
	            sqlTrans.Commit();
	            if(i>0)
	            {
	            	commitBol=true;
	            }
	            else
	            	commitBol = false;
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
		
		public bool UpdateCustomException(int prmStatus,string prmProblemType,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update custom_exception" +
                " set status=" + (int)prmStatus + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and status=2 and image_name='" + ctrlImage.ImageName + "' and problem_type='" + prmProblemType.Trim() + "'";
				
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
		
		
		public bool AddCustomException(int prmStatus,string prmProblemType,string prmRemarks,Credentials prmCrd)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();

            sqlStr = @"insert into custom_exception(Proj_key,batch_key,box_number,policy_number,problem_type,Image_name,Remarks,status,created_by,created_dttm)" +
				" values(" + ctrlImage.ProjectKey + " ," + ctrlImage.BatchKey + ", " + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber + ",'" + prmProblemType + "','" + ctrlImage.ImageName + "','" + prmRemarks + "'," + prmStatus + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "' )";	
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
		
		public DataSet GetCustomException(int prmState)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select problem_type,remarks from custom_exception " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and status=" + (int)prmState + " and image_name='" + ctrlImage.ImageName + "'";
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
			return dsImage;
		}

        public int GetImageStatus()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            int status = 0;

            sqlStr = "select status from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
                if (dsImage.Tables[0].Rows.Count > 0)
                {
                    status = Convert.ToInt32(dsImage.Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            
            
            return status;
        }
		

		public bool GetImageCount(eSTATES state)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select count(*) from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and status=" + (int)state + " and status<>" + (int)eSTATES.PAGE_DELETED;
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
			int value;
            if (dsImage.Tables[0].Rows.Count > 0)
            {
                value = Convert.ToInt32(dsImage.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                value = 0;
            }
            if (value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
		}
		
		public int GetImageCount(eSTATES[] state)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select page_name from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber;
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
			sqlStr = sqlStr + ")";
			
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
			return dsImage.Tables[0].Rows.Count;
		}
		public int GetImageCount()
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select max(serial_no) from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber;			
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
            if (dsImage.Tables[0].Rows[0][0].ToString() != string.Empty )
            {
                string i = dsImage.Tables[0].Rows[0][0].ToString();
                return Convert.ToInt32(dsImage.Tables[0].Rows[0][0]);
            }
            else
            {
                return 0;
            }
		}

        public int GetMaxPageCount()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            int pagenumberSt = 0;

            pagenumberSt = ctrlImage.PolicyNumber.ToString().Length + 2;

            sqlStr = "select max(substring(page_name," + pagenumberSt + ",3)) from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber;
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
            if (dsImage.Tables[0].Rows[0][0].ToString() != string.Empty)
            {
                string i = dsImage.Tables[0].Rows[0][0].ToString();
                return Convert.ToInt32(dsImage.Tables[0].Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

		public DataSet GetReadyImageCount(eSTATES[] state,eSTATES[] prmPolicyState)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select count(page_name) as page_Count,sum(qc_size) as index_size from image_master A,policy_master B" +
                    " where A.proj_key = B.proj_key and A.batch_key = B.batch_key and A.box_number = B.box_number and A.policy_number = B.policy_number and B.proj_key=" + ctrlImage.ProjectKey + 
				" and B.batch_key=" + ctrlImage.BatchKey + " and B.box_number=" + ctrlImage.BoxNumber + " and A.status<>29";
            /*
			for(int j=0;j<state.Length;j++)
			{
				if((int)state[j]!= 0)
				{
					if(j==0)
					{
						sqlStr=sqlStr + " and (A.status=" + (int)state[j] ;
					}
					else
						sqlStr=sqlStr + " or A.status=" + (int)state[j] ;
				}
			}
			sqlStr = sqlStr + " and A.status<>" + (int)eSTATES.PAGE_DELETED + " )";
            */
            for (int j = 0; j < state.Length; j++)
            {
                if ((int)state[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (B.status=" + (int)prmPolicyState[j];
                    }
                    else
                        sqlStr = sqlStr + " or B.status=" + (int)prmPolicyState[j];
                }
            }
            sqlStr = sqlStr + " )";
            
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
			return dsImage;
		}
		
		public DataSet GetPolicyWiseImageInfo(eSTATES[] state)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select count(page_name) as page_Count,sum(qc_size) as qc_size from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber;
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
			sqlStr = sqlStr + " )";
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
			return dsImage;
		}
		
		public int GetDocTypeCount(string prmDocType)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select page_name from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and doc_type='" + prmDocType + "' and status<>" + (int)eSTATES.PAGE_DELETED;
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
			return dsImage.Tables[0].Rows.Count;
		}
		public int GetDocTypeCount(eSTATES[] state)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			
			sqlStr="select distinct doc_type from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber;
			
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
			sqlStr = sqlStr + " )";
			
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
			return dsImage.Tables[0].Rows.Count;
		}
		public string GetIndexedImageName()
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			string indexPageName=string.Empty;
			
			sqlStr="select page_index_name from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "'";
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
			if(dsImage.Tables[0].Rows.Count > 0)
			{
				indexPageName= dsImage.Tables[0].Rows[0]["page_index_name"].ToString();
			}
			return indexPageName;
		}
        public DataSet GetAllIndexedImage()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select page_index_name,status,page_name,doc_type from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and status<>29 order by serial_no";
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
            return dsImage;
        }
        public DataSet GetAllImages(out OdbcDataAdapter pAdp)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select page_index_name,status,page_name,doc_type,policy_number,proj_key,batch_key,box_number,serial_no from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and status<>29 order by policy_number,serial_no";
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
            finally
            {
                pAdp = sqlAdap;
            }
            return dsImage;
        }
        /// <summary>
        /// This method is used for deleting image name from database
        /// </summary>
        /// <returns>bool</returns>
        public bool DeleteImage()
        {
            string sqlStr = null;
            OdbcCommand sqlCmd = new OdbcCommand();
            bool commitBol = true;

            sqlStr = "delete from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + ctrlImage.ImageName + "'";
            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
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
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                commitBol = false;
            }
            return commitBol;
        }
        /// <summary>
        /// Get all indexed page name against one doctype
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetAllIndexedImageName()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select page_index_name from image_master " +
                    " where proj_key=" + ctrlImage.ProjectKey +
                " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
                " and policy_number=" + ctrlImage.PolicyNumber + " and doc_type='" + ctrlImage.DocType + "' and status<>" + (int)eSTATES.PAGE_DELETED + " order by serial_no";
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
            return dsImage;
        }

		public string GetPhotoImageName()
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			string indexPageName=string.Empty;
			
			sqlStr="select page_name from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and photo=1";
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
			if(dsImage.Tables[0].Rows.Count > 0)
			{
				indexPageName= dsImage.Tables[0].Rows[0]["page_name"].ToString();
			}
			return indexPageName;
		}
		public DataSet GetIndexedImageName(string prmDocType)
		{
			string sqlStr=null;
			DataSet dsImage=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			string indexPageName=string.Empty;
			
			sqlStr="select page_index_name,status,page_name from image_master " + 
					" where proj_key=" + ctrlImage.ProjectKey + 
				" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
				" and policy_number=" + ctrlImage.PolicyNumber + " and doc_type='" + prmDocType + "' and status<>29 order by serial_no";
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
			return dsImage;
		}
		public ArrayList GetDeletedPageList(eSTATES[] prmPolicyState,eSTATES[] prmImageState, wItem wi)
		{
			ArrayList arrItem=new ArrayList();
			OdbcDataAdapter wAdap=null;
			DataSet ds=new DataSet();
			string strQuery=null;
			
			try 
			{
				wfePolicy queryPolicy = (wfePolicy) wi;
				strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.doc_type from image_master A,policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.box_number=B.box_number and A.policy_number=B.policy_number and A.proj_key=" + queryPolicy.ctrlPolicy.ProjectKey + " and A.batch_key=" + queryPolicy.ctrlPolicy.BatchKey + " and A.box_number=" + queryPolicy.ctrlPolicy.BoxNumber + " and A.policy_number=" + queryPolicy.ctrlPolicy.PolicyNumber + " and A.status= " + (int)prmImageState[0] ;
				for(int j=0;j<prmPolicyState.Length;j++)
				{
					if((int)prmPolicyState[j]!= 0)
					{
						if(j==0)
						{
							strQuery=strQuery + " and (B.status=" + (int)prmPolicyState[j] ;
						}
						else
							strQuery=strQuery + " or B.status=" + (int)prmPolicyState[j] ;
					}
				}
				strQuery = strQuery + " )";
				wAdap=new OdbcDataAdapter(strQuery,sqlCon);
				wAdap.Fill(ds);	
			}
			catch(Exception EX)
			{
				wAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(strQuery + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(EX, this);
			}
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
				wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()),Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()),Convert.ToInt32(ds.Tables[0].Rows[i]["box_number"].ToString()),Convert.ToInt32(ds.Tables[0].Rows[i]["policy_number"].ToString()),ds.Tables[0].Rows[i]["page_name"].ToString(),ds.Tables[0].Rows[i]["doc_type"].ToString());
					arrItem.Add (wic);
				}
			return arrItem;
		}
		public bool Save(Credentials prmCrd,eSTATES prmState,double prmImageSize,int prmPhoto,int prmSrlNo,string prmIndexImageName)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			switch (prmState)
			{
				case eSTATES.PAGE_SCANNED:
                    sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,scanned_size,photo,serial_no,page_index_name) values(" +
						ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
						",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + "," + prmImageSize + "," + prmPhoto + "," + prmSrlNo + ",'" + prmIndexImageName + "')";
					break;
				case eSTATES.PAGE_QC:
                    sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,QC_size,photo,serial_no,page_index_name) values(" +
						ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
						",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + "," + prmImageSize + "," + prmPhoto + "," + prmSrlNo + ",'" + prmIndexImageName + "')";
					break;
				case eSTATES.PAGE_RESCANNED_NOT_INDEXED:
                    sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,qc_size,photo,serial_no,page_index_name) values(" +
						ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
						",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + "," + prmImageSize + "," + prmPhoto + "," + prmSrlNo + ",'" + prmIndexImageName + "')";
					break;
                case eSTATES.PAGE_FQC:
                    sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,QC_size) values(" +
                        ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
                        ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + "," + prmImageSize + ")";
                    break;
                case eSTATES.PAGE_NOT_INDEXED:
                    sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,QC_size,serial_no,page_index_name) values(" +
                        ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
                        ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + "," + prmImageSize + ", " + prmSrlNo + ",'" + prmIndexImageName + "')";
                    break;
			}
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
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n" + "Wfe State--" + Convert.ToString(Convert.ToInt32(prmState)) + "\n");
                
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}

        public bool Save(Credentials prmCrd, eSTATES prmState,string prmDocType,string prmIndexName,int prmSerialNo)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

                
            sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,page_index_name,doc_type,serial_no) values(" +
                ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
                ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + ",'" + prmIndexName + "','" + prmDocType + "'," + prmSerialNo + ")";
    
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

        public bool Save(Credentials prmCrd, eSTATES prmState, string prmDocType, string prmIndexName,double prmImageSize,int prmSrlNo)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
			
            int imageSize=Convert.ToInt32(prmImageSize);

            sqlStr = @"insert into image_master(proj_key,batch_key,box_number, policy_number,created_by,created_dttm,Page_name,status,page_index_name,doc_type,serial_no,qc_size) values(" +
                ctrlImage.ProjectKey + "," + ctrlImage.BatchKey + "," + ctrlImage.BoxNumber + "," + ctrlImage.PolicyNumber +
                ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','" + ctrlImage.ImageName + "'," + (int)prmState + ",'" + prmIndexName + "','" + prmDocType + "'," + prmSrlNo + "," + imageSize + ")";

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

		public bool DeletePage()
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"delete from image_master where proj_key=" + ctrlImage.ProjectKey + " and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber + " and policy_number=" + ctrlImage.PolicyNumber +
				" and page_name='" + ctrlImage.ImageName + "'";
				
			try
			{
					sqlTrans=sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;
	                sqlCmd.CommandText = sqlStr;
	                int i= sqlCmd.ExecuteNonQuery();
	                sqlTrans.Commit();
	                if(i>0)
	                {
	                	commitBol=true;
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
		public bool UpdateImageSize(Credentials prmCrd,eSTATES prmState,double prmImageSize)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			
			OdbcCommand sqlCmd=new OdbcCommand();
			int pos;
			string originalImage;
            ///changed on version 1.0.0.1 for update image size in qc_size field
            int imageSize =Convert.ToInt32(prmImageSize);///changed line
			pos = ctrlImage.ImageName.IndexOf("-");	
			if(pos > 0)
			{
				originalImage = ctrlImage.ImageName.Substring(0,pos);	
			}
			else
			{
				 originalImage= ctrlImage.ImageName;
			}
			switch (prmState)
			{
				
				case eSTATES.PAGE_QC:
					sqlStr=@"update image_master" +
						" set QC_size=" + prmImageSize + " where proj_key=" + ctrlImage.ProjectKey +
						" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
						" and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + originalImage + "'";
					break;
				case eSTATES.PAGE_INDEXED:
					sqlStr=@"update image_master" +
						" set qc_size=" + imageSize + " where proj_key=" + ctrlImage.ProjectKey +
						" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
						" and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + originalImage + "'";	
					break;
				case eSTATES.PAGE_FQC:
					sqlStr=@"update image_master" +
						" set qc_size=" + imageSize + " where proj_key=" + ctrlImage.ProjectKey +
						" and batch_key=" + ctrlImage.BatchKey + " and box_number=" + ctrlImage.BoxNumber +
						" and policy_number=" + ctrlImage.PolicyNumber + " and page_name='" + originalImage + "'";	
					break;
			}
			
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
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n" + "Wfe State--" + Convert.ToString(Convert.ToInt32(prmState)) + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return commitBol;
		}
	}
}
