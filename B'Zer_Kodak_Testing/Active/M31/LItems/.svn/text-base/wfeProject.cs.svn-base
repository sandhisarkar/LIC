/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/2/2009
 * Time: 11:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NovaNet.wfe;
using System.Collections;
using System.Data.Odbc;
using NovaNet.Utils;
using System.Data;
using System.IO;

namespace LItems
{
	/// <summary>
	/// Structure for project details
	/// </summary>
	public class udtProject: NovaNet.wfe.udtCmd
	{
		public int proj_key; 
		public string Code; 
		public string Project_Path;
		public string Created_By;
		public string Created_DTTM;
		public string Modified_By;
		public string Modified_DTTM;
	}
	public class CtrlProject: NovaNet.wfe.wItemControl
	{
		private int proj_key;
		private string Code; 
		public CtrlProject(int key, string code)
		{
			proj_key = key;
			Code = code;
		}
		public int ProjectKey
		{
			get
			{
				return proj_key;
			}
		}
		public string ProjectCode
		{
			get
			{
				return Code;
			}
		}
	}
	/// <summary>
	/// Description of wfeProject.
	/// </summary>
	public class wfeProject: wItem, ErrReporter,StateData
	{
        MemoryStream stateLog;
        byte[] tmpWrite;
		private udtProject objProject;
		private Hashtable errList;
		private OdbcConnection sqlCon=null;
		private OdbcDataAdapter sqlAdap=null;
		private INIReader rd=null;
		private KeyValueStruct udtKeyValue;
		public string err=null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	

		public wfeProject(OdbcConnection prmCon): base(prmCon, NovaNet.Utils.Constants._ADDING)
		{
			sqlCon=prmCon;
            exMailLog.SetNextLogger(exTxtLog);
            
		}
		public wfeProject(String prmCtrl, OdbcConnection prmCon): base(prmCon, NovaNet.Utils.Constants._EDITING)
		{
			sqlCon=prmCon;
            exMailLog.SetNextLogger(exTxtLog);
            
		}
//		public override bool Display()
//		{
//			frmAddEdit frmDisp;
//			frmMain frmMan=new frmMain();
//			if (mode==1)
//				frmDisp = new aeProject(this);
//			else
//				frmDisp = new aeProject();
//			frmDisp.ShowDialog(frmMan);
//			return true;
//		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
		public override bool TransferValues(udtCmd cmd)
		{
			
			objProject = (udtProject) (cmd);
			objProject.Project_Path=objProject.Project_Path + "\\" + objProject.Code;
			if (KeyCheck(objProject.Code)==false)
			{
				if (Validate(objProject)==true)
				{
					if( Commit()==true)
					{
						return true;
					}
					else
					{
						rd=new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
						udtKeyValue.Key=Constants.SAVE_ERROR.ToString();
						udtKeyValue.Section=Constants.COMMON_EXCEPTION_SECTION;
						string ErrMsg=rd.Read(udtKeyValue);
						throw new DbCommitException(ErrMsg);
					}
				}
				else
				{
					//throw new ValidationException(Constants.ValidationException) ;
					return false;
				}
			}
			else
			{
				rd=new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
				udtKeyValue.Key=Constants.DUPLICATE_KEY_CHECK.ToString();
				udtKeyValue.Section=Constants.COMMON_EXCEPTION_SECTION;
				string ErrMsg=rd.Read(udtKeyValue);
				throw new KeyCheckException(ErrMsg);
			}
		}
		
		public DataSet GetConfiguration()
		{
			string sqlStr=null;
			
			//int pageCount = 0;
			DataSet policyDs=new DataSet();
			
			try 
			{
				sqlStr="select DO_CODE,BO_CODE,VENDOR_CODE,VERSION_NUMBER,scan_center,vendor_name from configuration_master  where active = 1" ;
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

        public DataSet GetMainConfiguration(int prmProjectCode,int prmBatchCode)
        {
            string sqlStr = null;
            
            //int pageCount = 0;
            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select distinct division_code as DO_CODE,branch_code as BO_CODE from rawdata  where proj_key=" + prmProjectCode + " and batch_key=" + prmBatchCode;
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
		public override udtCmd LoadValuesFromDB()
		{
			throw new NotImplementedException();
		}
		public override bool Commit()
		{
			string sqlStr=null;
			OdbcTransaction sqlTrans=null;
			bool commitBol=true;
			string scanPath=null;
			
			OdbcCommand sqlCmd=new OdbcCommand();
			errList=new Hashtable();
			
			scanPath=objProject.Project_Path.Replace("\\","\\\\");
			
			sqlStr=@"insert into project_master(proj_code,Project_path,created_by" +
				",Created_DTTM) values(" +
				"'" + objProject.Code.ToUpper() + "','" + scanPath + "'," +
				 "'" + objProject.Created_By + "','" + objProject.Created_DTTM + "')";
			try
			{
				if (KeyCheck(objProject.Code)==false)
				{
					sqlTrans=sqlCon.BeginTransaction();
					sqlCmd.Connection = sqlCon;
					sqlCmd.Transaction=sqlTrans;
	                sqlCmd.CommandText = sqlStr;
	                sqlCmd.ExecuteNonQuery();
	                
	                scanPath=objProject.Project_Path;
					
	                if (FileorFolder.CreateFolder(scanPath)==true)
	                {
	                	commitBol=true;    
	            		sqlTrans.Commit();
	                }
	                else
	                {
	                	commitBol=false;    
	                	sqlTrans.Rollback();
	                	rd=new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
						udtKeyValue.Key=Constants.PROJECT_FOLDER_CREATE_ERROR.ToString();
						udtKeyValue.Section=Constants.PROJECT_EXCEPTION_SECTION;
						string ErrMsg=rd.Read(udtKeyValue);
						throw new CreateFolderException(ErrMsg);
	                }
	            	
				}
				else
				{
					commitBol=false;
				}
			}
			catch(Exception ex)
			{
				errList.Add(Constants.DBERRORTYPE,ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
				commitBol=false;
				sqlTrans.Rollback();
				sqlCmd.Dispose();
			}
			return commitBol;
		}
		
		public override bool KeyCheck(string prmValue)
		{
			string sqlStr=null;
			OdbcCommand cmd=null;
			bool existsBol=true;
			
			sqlStr="select proj_code from project_master where proj_code='" + prmValue.ToUpper() + "'";
			cmd=new OdbcCommand(sqlStr,sqlCon);
			existsBol=cmd.ExecuteReader().HasRows;
				
			return existsBol;
		}
		
		public Hashtable GetErrors()
		{
			return errList;
		}
		private bool Validate(udtProject cmd)
			
		{
			bool validateBol=true;
			errList = new Hashtable();
			if (cmd.Code==string.Empty)
			{
				validateBol=false;
				errList.Add("Code",Constants.NOT_VALID);
			}
						
			if (cmd.Project_Path==string.Empty)
			{
				validateBol=false;
				errList.Add("Scanned_Path",Constants.NOT_VALID);
			}
			
			if (cmd.Created_By==string.Empty && mode==Constants._ADDING)
			{
				validateBol=false;
				errList.Add("Created_By",Constants.NOT_VALID);
			}
			
			if (cmd.Created_DTTM==string.Empty && mode==Constants._ADDING)
			{
				validateBol=false;
				errList.Add("Created_DTTM",Constants.NOT_VALID);
			}
						
			///Required at the time of editing
			if (cmd.Modified_By==string.Empty && mode==Constants._EDITING)
			{
				validateBol=false;
				errList.Add("Modified_By",Constants.NOT_VALID);
			}
						
			if (cmd.Modified_DTTM==string.Empty && mode==Constants._EDITING)
			{
				validateBol=false;
				errList.Add("Modified_DTTM",Constants.NOT_VALID);
			}
						
			return validateBol;
		}
		
		public System.Data.DataSet GetAllValues()
		{
			string sqlStr=null;
			
			DataSet projDs=new DataSet();
			
			try 
			{
				sqlStr="select proj_key,proj_code from project_master order by proj_code";
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
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
			return projDs;
		}
		
		public System.Data.DataSet GetPath(int prmProjKey)
		{
			string sqlStr=null;
			DataSet projDs=new DataSet();
			
			try 
			{
				sqlStr=@"select project_path from project_master where proj_key=" + prmProjKey ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
				sqlAdap.Fill(projDs);
				
				//Test whether the path exists or not
				//throw new ValidationException(1234);
			}
			catch (OdbcException ex) 
			{
				sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n" );
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return projDs;
		}
		public string GetProjectName(int prmProjectKey)
		{
			string sqlStr=null;
			string projName=null;
			
			DataSet projDs=new DataSet();
			
			try 
			{
				sqlStr="select proj_key,proj_code from project_master where proj_key=" + prmProjectKey ;
				sqlAdap=new OdbcDataAdapter(sqlStr,sqlCon);
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
			if(projDs.Tables[0].Rows.Count>0)
			{
				projName=projDs.Tables[0].Rows[0]["proj_code"].ToString();
			}
			else
				projName=string.Empty;
			return projName;
		}
	}
}
