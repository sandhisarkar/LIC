/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 23/2/2009
 * Time: 6:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.Odbc;

namespace NovaNet
{
namespace Utils
{
	/// <summary>
	/// Description of Writer.
	/// </summary>
	public abstract class Writer
	{

		public Writer()
		{
		}
		public abstract bool SaveData (DataSet prmCSVData,Credentials prmCrd,ControlInfo prmInfo, NotifyProgress nt,int prmBoxStatus,int prmPolicyStatus);
	}
	public class MySqlWriter: Writer
	{
		OdbcConnection sqlCon=null;
		OdbcCommand sqlCmd=null;
		OdbcTransaction sqlTrans=null;
        DataSet ds = null;
		private string err=null;
		
		public MySqlWriter(OdbcConnection prmCon)
		{
			sqlCon=prmCon;
		}
		public override bool SaveData(DataSet prmCSVData,Credentials prmCrd,ControlInfo prmInfo, NotifyProgress nt,int prmBoxStatus,int prmPolicyStatus)
		{
			try
			{
				sqlTrans=sqlCon.BeginTransaction();
                ds = prmCSVData;
				SaveRawData(sqlTrans,prmCSVData,prmCrd,prmInfo);
				if (nt != null)
					nt(33);
				SaveBoxData(sqlTrans,prmCrd,prmInfo,prmBoxStatus);
				if (nt != null)
					nt(66);
				SavePolicyMasterData(sqlTrans,prmCSVData,prmCrd,prmInfo,prmPolicyStatus);
				if (nt != null)
					nt(100);
				sqlTrans.Commit();
				return true;
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				err=ex.Message;
				return false;
			}
			
		}
		private void SaveRawData(OdbcTransaction prmTransaction,DataSet prmCSVData,Credentials prmCrd,ControlInfo prmInfo)
		{
			string sqlStr=null;
			sqlCmd=new OdbcCommand();
            
            for (int i = 0; i < prmCSVData.Tables[0].Rows.Count; i++)
            {
                sqlStr = "INSERT INTO rawdata (proj_key, batch_key, serial_number, division_code, branch_code, batch_serial, policy_type, policy_no, link_policy_number, name_of_policyholder, date_of_commencement, date_of_birth, status_code, customer_id, no_of_pages_in_the_dkt, no_of_pages_to_be_scanned, date_handover, date_scanned, date_upload, no_of_images_uploaded, carton_number, available_docket, scan_upload_flag, unused, created_by, created_dttm)" +
                    " VALUES (" + prmInfo.proj_Key + "," + prmInfo.batch_Key + "," +
                    "'" + prmCSVData.Tables[0].Rows[i]["serialno"] + "', '" + prmCSVData.Tables[0].Rows[i]["divisioncode"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["branchcode"] + "', '" + prmCSVData.Tables[0].Rows[i]["batchserial"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["policytype"] + "', '" + prmCSVData.Tables[0].Rows[i]["policyno"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["linkpolicyno"] + "', '" + prmCSVData.Tables[0].Rows[i]["PolicyHolderName"].ToString().Replace("'", "''") + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["DateCommencement"] + "', '" + prmCSVData.Tables[0].Rows[i]["DOB"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["statuscode"] + "', '" + prmCSVData.Tables[0].Rows[i]["customerid"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["DocketPageNo"] + "', '" + prmCSVData.Tables[0].Rows[i]["PageToBeScanned"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["HandoverDate"] + "', '" + prmCSVData.Tables[0].Rows[i]["ScannedDate"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["UploadDate"] + "', '" + prmCSVData.Tables[0].Rows[i]["NoOfImageUploaded"] + "', '" + prmCSVData.Tables[0].Rows[i]["CartonNo"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["AvaillableDocket"] + "', '" + prmCSVData.Tables[0].Rows[i]["ScanUploadFlagText"] + "'," +
                    "'" + prmCSVData.Tables[0].Rows[i]["unused"] + "', '" + prmCrd.created_by + "', '" + prmCrd.created_dttm + "')";

                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = prmTransaction;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
            }
		}
		
		private void SaveBoxData(OdbcTransaction prmTransaction,Credentials prmCrd,ControlInfo prmInfo,int prmBoxStatus)
		{
			string sqlStr=null;
			sqlCmd=new OdbcCommand();
			string batchPath=null;
			string boxPath=null;
            
            DataTable distinctDT = ds.Tables[0].DefaultView.ToTable(true, new string[] { "CartonNo" });
			batchPath=GetBatchPath(prmInfo.proj_Key,prmInfo.batch_Key,prmTransaction);
            for (int i = 0; i < distinctDT.Rows.Count; i++)
			{
                
                boxPath = batchPath + "\\" + Convert.ToInt32(distinctDT.Rows[i]["CartonNo"].ToString());
                boxPath = boxPath.Replace("\\", "\\\\");
                sqlStr = @"INSERT INTO box_master (proj_key, batch_key, box_number, box_path, created_by, created_dttm,status)" +
                    "VALUES (" + prmInfo.proj_Key + "," + prmInfo.batch_Key + "," + Convert.ToInt32(distinctDT.Rows[i]["CartonNo"].ToString()) +", '" + boxPath + "','" + prmCrd.created_by + "', '" + prmCrd.created_dttm + "'," + prmBoxStatus + ")";
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = prmTransaction;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
                
			}
		}
		
		private string GetBatchPath(int prmProjKey,int prmBatchKey,OdbcTransaction prmTrans)
		{
			string sqlStr=null;
			DataSet dsPath=new DataSet();
			string batchPath=null;
			OdbcDataAdapter sqlAdap;
			sqlCmd=new OdbcCommand();
			
			sqlCmd.Connection=sqlCon;
			sqlStr=@"select batch_path from batch_master where proj_code=" + prmProjKey + " and batch_key=" + prmBatchKey ;
			sqlCmd.Transaction=prmTrans;
			sqlCmd.CommandText=sqlStr;
			sqlAdap=new OdbcDataAdapter(sqlCmd);
			sqlAdap.Fill(dsPath);
			if (dsPath.Tables[0].Rows.Count>0)
	        {
				batchPath=dsPath.Tables[0].Rows[0]["batch_Path"].ToString();
			}
			return batchPath;
		}
		
		private void SavePolicyMasterData(OdbcTransaction prmTransaction,DataSet prmCSVData,Credentials prmCrd,ControlInfo prmInfo,int prmPolicyStatus)
		{
			string sqlStr=null;
			sqlCmd=new OdbcCommand();
			string boxPath=null;
			string policyPath=null;
			
            //prmCSVData.Tables[0].Columns.Add("boxnumber");
            //for (int i=0;i<prmCSVData.Tables[0].Rows.Count;i++)
            //{
            //    if(k==100)
            //    {
            //        j=j+1;
            //        k=0;
            //    }
            //    prmCSVData.Tables[0].Rows[i]["boxnumber"]=j.ToString();
            //    k=k+1;
            //}
			
			for (int i=0;i<prmCSVData.Tables[0].Rows.Count;i++)
			{
                boxPath = GetBoxPath(prmInfo.proj_Key, prmInfo.batch_Key, Convert.ToInt32(prmCSVData.Tables[0].Rows[i]["CartonNo"].ToString()), sqlTrans);
				policyPath=boxPath + "\\" + prmCSVData.Tables[0].Rows[i]["PolicyNo"];
				policyPath=policyPath.Replace("\\","\\\\");
				sqlStr=@"INSERT INTO policy_master (proj_key, batch_key, box_number, policy_number, policy_path, created_by, created_dttm,status,scan_upload_flag)" +
                    "VALUES (" + prmInfo.proj_Key + "," + prmInfo.batch_Key + ",'" + prmCSVData.Tables[0].Rows[i]["CartonNo"].ToString() + "'," + Convert.ToInt32(prmCSVData.Tables[0].Rows[i]["policyno"].ToString()) + "," +
					"'" + policyPath + "','" + prmCrd.created_by + "', '" + prmCrd.created_dttm + "'," + prmPolicyStatus + ",'" + Constants._SCAN_PENDING + "')";
	
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=prmTransaction;
		            sqlCmd.CommandText = sqlStr;
		            sqlCmd.ExecuteNonQuery();
			}
		}
		private string GetBoxPath(int prmProjKey,int prmBatchKey,int prmBoxNo,OdbcTransaction prmTrans)
		{
			string sqlStr=null;
			DataSet dsPath=new DataSet();
			string boxPath=null;
			OdbcDataAdapter sqlAdap;
			sqlCmd=new OdbcCommand();
			
			sqlStr=@"select box_path from box_master where proj_key=" + prmProjKey + " and batch_key=" + prmBatchKey + " and box_number=" + prmBoxNo;
			sqlCmd.Connection=sqlCon;
			sqlCmd.Transaction=prmTrans;
			sqlCmd.CommandText=sqlStr;
			
			sqlAdap=new OdbcDataAdapter(sqlCmd);
			sqlAdap.Fill(dsPath);
			if (dsPath.Tables[0].Rows.Count>0)
	        {
				boxPath=dsPath.Tables[0].Rows[0]["box_path"].ToString();
			}
			return boxPath;
		}
	}
}
}