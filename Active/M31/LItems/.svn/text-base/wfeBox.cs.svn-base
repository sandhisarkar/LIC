/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/3/2008
 * Time: 4:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NovaNet.wfe;
using NovaNet.Utils;
using System.Data;
using System.Data.Odbc;
using System.IO;

namespace LItems
{
	public class udtBox: NovaNet.wfe.udtCmd
	{
		public int projKey; 
		public int batchKey; 
		public int boxNumber;
		public int policyNumber;
	}
	public class CtrlBox: NovaNet.wfe.wItemControl
	{
		private int batch_key; 
		private int proj_Key; 
		private int box_number;
		
		
		public CtrlBox(int projKey, int batchKey,int boxNumber)
		{
			proj_Key=projKey;
			batch_key=batchKey;
			box_number=boxNumber;
		}
		public int BatchKey
		{
			get
			{
				return batch_key;
			}
		}
		
		public int ProjectCode
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
	}
	/// <summary>
	/// Description of wfeBox.
	/// </summary>
	public class wfeBox: wItem, StateData
	{
        MemoryStream stateLog;
		OdbcConnection sqlCon;		
		public CtrlBox ctrlBox=null;
		OdbcDataAdapter sqlAdap=null;
		udtBox Data=null;
        byte[] tmpWrite;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev,Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
		public wfeBox(OdbcConnection prmCon): base(prmCon, NovaNet.Utils.Constants._ADDING)
		{
			sqlCon=prmCon;
            exMailLog.SetNextLogger(exTxtLog);
		}
		public wfeBox(OdbcConnection prmCon, CtrlBox prmCtrl): base(prmCon, NovaNet.Utils.Constants._EDITING)
		{
			ctrlBox = prmCtrl;
			sqlCon = prmCon;
			LoadValuesFromDB();
            exMailLog.SetNextLogger(exTxtLog);
            
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
			string sqlStr=null;
			
			DataSet boxDs=new DataSet();
			
			try 
			{
				sqlStr="select policy_number from policy_master where proj_key=" + ctrlBox.ProjectCode + " and batch_key=" + ctrlBox.BatchKey + " and box_number=" + ctrlBox.BoxNumber ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(boxDs);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			
			return Data;
		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
		public override bool TransferValues(udtCmd cmd)
		{
			throw new NotImplementedException();
		}
		public DataSet GetBox(eSTATES[] state,int prmProjKey,int prmBatchKey)
		{
			string sqlStr=null;
			DataSet dsBox=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select distinct box_number,count(policy_number) as policy_number from policy_master where proj_key=" + prmProjKey + " and trim(batch_key)=" + prmBatchKey ;
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
			sqlStr = sqlStr + ") group by box_number";
			try 
			{
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(dsBox);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex,this);
                
            }
			return dsBox;
		}

        public DataSet GetScannedBox(int projKey,int prmBatchKey)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select distinct box_number from box_master  where proj_key="+ projKey + " and trim(batch_key)=" + prmBatchKey + " and status=10";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);

            }
            return dsBox;
        }

        /// <summary>
        /// Get total policy against one batch
        /// </summary>
        /// <param name="state"></param>
        /// <param name="prmBatchKey"></param>
        /// <returns>integer</returns>
        public int GetTotalPolicies(eSTATES prmState)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select policy_number as policy_number from policy_master where proj_key=" + ctrlBox.ProjectCode + " and batch_key=" + ctrlBox.BatchKey;
            if ((int)prmState == 0)
            {
                sqlStr = sqlStr + " and 1=1 order by policy_number";
            }
            else
            {
                sqlStr = sqlStr + " and status=" + (int)prmState + " order by policy_number";
            }
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex,this);
            }
            
            return dsBox.Tables[0].Rows.Count;
        }

        public int GetTotalPolicies(eSTATES[] prmState)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select policy_number as policy_number from policy_master where proj_key=" + ctrlBox.ProjectCode + " and trim(batch_key)=" + ctrlBox.BatchKey;

            for (int j = 0; j < prmState.Length; j++)
            {
                if ((int)prmState[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (status=" + (int)prmState[j];
                    }
                    else
                        sqlStr = sqlStr + " or status=" + (int)prmState[j] ;
                }
            }
            sqlStr = sqlStr + ") order by policy_number";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return dsBox.Tables[0].Rows.Count;
        }

        public int GetLICCheckedCount()
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select distinct policy_number as policy_number from lic_qa_log where proj_key=" + ctrlBox.ProjectCode + " and trim(batch_key)=" + ctrlBox.BatchKey + " and (qa_status=0 or qa_status=2)";
            
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return dsBox.Tables[0].Rows.Count;
        }


        public double GetTotalBatchSize()
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            double size=0;

            ///changed in version 1.0.0.1
            sqlStr = "select sum(A.qc_size) as size from image_master A,policy_master B where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.policy_number=B.policy_number and A.proj_key=" + ctrlBox.ProjectCode + " and A.batch_key=" + ctrlBox.BatchKey + " and B.status<>" + (int)eSTATES.POLICY_ON_HOLD + " and A.status<>" + (int)eSTATES.PAGE_DELETED;
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
                size = Convert.ToInt32(dsBox.Tables[0].Rows[0]["size"]) / 1024;
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            
            
            return size;
        }

        public int GetTotalImageCount(eSTATES[] state, bool prmIsSignaturePage, eSTATES[] prmPolicyState)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select count(page_name) as page_Count,sum(qc_size) as index_size from image_master A,policy_master B" +
                    " where A.proj_key = B.proj_key and A.batch_key = B.batch_key and A.box_number = B.box_number and A.policy_number = B.policy_number and B.proj_key=" + ctrlBox.ProjectCode +
                " and B.batch_key=" + ctrlBox.BatchKey + " and A.status<>29";
            /*
            for (int j = 0; j < state.Length; j++)
            {
                if ((int)state[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (A.status=" + (int)state[j];
                    }
                    else
                        sqlStr = sqlStr + " or A.status=" + (int)state[j];
                }
            }
             
            sqlStr = sqlStr + " and A.status<>" + (int)eSTATES.PAGE_DELETED + " )";
             */
            for (int j = 0; j < prmPolicyState.Length; j++)
            {
                if ((int)prmPolicyState[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (B.status=" + (int)prmPolicyState[j];
                    }
                    else
                        sqlStr = sqlStr + " or B.status = " + (int)prmPolicyState[j];
                }
            }
            if (prmIsSignaturePage == false)
            {
                sqlStr = sqlStr + " )";
            }
            else
            {
                sqlStr = sqlStr + " ) and A.doc_type='" + "Signature_page" + "'";
            }
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }

            return Convert.ToInt32(dsBox.Tables[0].Rows[0]["page_Count"].ToString());
        }
        public int GetTotalImageCount()
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            int count;

            try
            {
                sqlStr = @"select count(*) from image_master where proj_key=" + ctrlBox.ProjectCode + " and batch_key=" + ctrlBox.BatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                count = Convert.ToInt32(projDs.Tables[0].Rows[0][0].ToString());
            }
            else
                count = 0;

            return count;
        }
		public DataSet GetFQCBox(eSTATES[] state,int prmBatchKey)
		{
			string sqlStr=null;
			DataSet dsBox=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select box_number from box_master where trim(batch_key)=" + prmBatchKey ;
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
			sqlStr = sqlStr + ") order by box_number";
			try 
			{
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(dsBox);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return dsBox;
		}
		
		public DataSet GetExportableBox(eSTATES[] state)
		{
			string sqlStr=null;
			DataSet dsBox=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select box_number from box_master where trim(batch_key)=" + ctrlBox.BatchKey + " and proj_key=" + ctrlBox.ProjectCode ;
            //for(int j=0;j<state.Length;j++)
            //{
            //    if((int)state[j]!= 0)
            //    {
            //        if(j==0)
            //        {
            //            sqlStr=sqlStr + " and (status=" + (int)state[j] ;
            //        }
            //        else
            //            sqlStr=sqlStr + " or status=" + (int)state[j] ;
            //    }
            //}
			sqlStr = sqlStr + " order by box_number";
			try 
			{
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(dsBox);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return dsBox;
		}
		public DataSet GetAllBox(int prmBatchKey)
		{
			string sqlStr=null;
			DataSet dsBox=new DataSet();
			OdbcDataAdapter sqlAdap=null;
			
			sqlStr="select distinct box_number,count(policy_number) as policy_number from policy_master where proj_key=" + ctrlBox.ProjectCode + " and batch_key=" + prmBatchKey + " group by box_number order by box_number";
			try 
			{
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(dsBox);
			}
			catch (Exception ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return dsBox;
		}
        public int GetBoxCount(eSTATES[] state)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select box_number from box_master " +
                    " where proj_key=" + ctrlBox.ProjectCode +
                " and batch_key=" + ctrlBox.BatchKey;

            for (int j = 0; j < state.Length; j++)
            {
                if ((int)state[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (status=" + (int)state[j];
                    }
                    else
                        sqlStr = sqlStr + " or status=" + (int)state[j];
                }
            }
            sqlStr = sqlStr + ") ";
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
            return dsImage.Tables[0].Rows.Count;
        }
		public bool UpdateStatus(eSTATES state)
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			
			OdbcCommand sqlCmd=new OdbcCommand();
			
			sqlStr=@"update box_master" +
				" set status=" + (int)state + " where proj_key=" + ctrlBox.ProjectCode +
				" and batch_key=" + ctrlBox.BatchKey + " and box_number=" + ctrlBox.BoxNumber ;
				
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

        public bool UpdateStatus(eSTATES state,OdbcTransaction prmTrans)
        {
            string sqlStr = null;
            bool commitBol = true;
            
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update box_master" +
                " set status=" + (int)state + " where proj_key=" + ctrlBox.ProjectCode +
                " and batch_key=" + ctrlBox.BatchKey + " and box_number=" + ctrlBox.BoxNumber;

            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = prmTrans;
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
	}
}
