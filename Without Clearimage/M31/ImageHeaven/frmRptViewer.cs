using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LItems;
using NovaNet.Utils;
using Logistics_reports;
using NovaNet.wfe;

namespace ImageHeaven
{
    public partial class frmRptViewer : Form
    {
        private string reportName;
        private OdbcConnection sqlCon;
        eSTATES[] policyState;
        wfeBox wBox=null;
        wfePolicy wPolicy = null;
        DataSet ds;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev,Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
        public frmRptViewer(string prmReportName, wfeBox prmBox, OdbcConnection prmCon)
        {
            InitializeComponent();
            reportName = prmReportName;
            sqlCon = prmCon;
            wBox = prmBox;
            exMailLog.SetNextLogger(exTxtLog);
            
        }
        private void frmRptViewer_Load(object sender, EventArgs e)
        {
            CtrlBox pBox = null;
            CtrlPolicy pPolicy = null;
            wfeProject tmpProj = null;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            int polPending = 0;
            //LogisticControl lgs = new LogisticControl(sqlCon);
            //if (reportName == "ExceptionRpt")
            //{
            //    ExceptionRpt expRpt = new ExceptionRpt();
            //    expRpt.SetDataSource(ds.Tables[0]);
            //    crv.DisplayGroupTree = false;
            //    crv.ReportSource = expRpt;
            //}

            //if (reportName == "RMFChallan")
            //{
            //    RMFChallan challanRpt = new RMFChallan();
            //    challanRpt.SetDataSource(ds.Tables[0]);
            //    challanRpt.SetParameterValue("txtBatch", lgs.GetBatchName(batchKey));
            //    crv.DisplayGroupTree = false;
            //    crv.ReportSource = challanRpt;
            //}
            try
            {
                if (reportName == "UATAnxA")
                {
                    UATAnxA uat = new UATAnxA();
                    tmpProj = new wfeProject(sqlCon);
                    ds = new DataSet();
                    ds = tmpProj.GetConfiguration();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        uat.SetParameterValue("txtScanCenter", ds.Tables[0].Rows[0]["SCAN_CENTER"].ToString());
                        uat.SetParameterValue("txtVendorCode", ds.Tables[0].Rows[0]["VENDOR_CODE"].ToString());
                        uat.SetParameterValue("txtVersionNumber", ds.Tables[0].Rows[0]["VERSION_NUMBER"].ToString());
                        uat.SetParameterValue("txtVendorName", ds.Tables[0].Rows[0]["VENDOR_NAME"].ToString());
                    }
                    else
                    {
                        uat.SetParameterValue("txtScanCenter", 0);
                        uat.SetParameterValue("txtVendorCode", 0);
                        uat.SetParameterValue("txtVersionNumber", 0);
                        uat.SetParameterValue("txtVendorName", 0);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    ds = tmpProj.GetMainConfiguration(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        uat.SetParameterValue("txtDoCode", ds.Tables[0].Rows[0]["DO_CODE"].ToString());
                        uat.SetParameterValue("txtBoCode", ds.Tables[0].Rows[0]["BO_CODE"].ToString());

                        uat.SetParameterValue("txtBatchNo", wBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey));
                    }
                    else
                    {
                        uat.SetParameterValue("txtDoCode", 0);
                        uat.SetParameterValue("txtBoCode", 0);

                        uat.SetParameterValue("txtBatchNo", 0);
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        uat.SetParameterValue("txtNo" + i, i);
                    }
                    //wfeBox wBox;
                    eSTATES state = new eSTATES();

                    eSTATES[] tempState = new eSTATES[5];
                    policyState = new eSTATES[5];
                    pBox = new CtrlBox(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), 0);
                    wBox = new wfeBox(sqlCon, pBox);
                    uat.SetParameterValue("txtDesc1", wBox.GetTotalPolicies(state).ToString());
                    uat.SetParameterValue("txtDesc2", Convert.ToString((Convert.ToInt32(wBox.GetTotalPolicies(state).ToString()) - Convert.ToInt32(wBox.GetTotalPolicies(eSTATES.POLICY_MISSING)))));

                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                    policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                    policyState[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                    policyState[4] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
                    policyState[5] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;

                    uat.SetParameterValue("txtDesc3", wBox.GetTotalPolicies(policyState).ToString());

                    tempState[0] = eSTATES.PAGE_INDEXED;
                    tempState[1] = eSTATES.PAGE_FQC;
                    tempState[2] = eSTATES.PAGE_CHECKED;
                    tempState[3] = eSTATES.PAGE_EXCEPTION;
                    tempState[4] = eSTATES.PAGE_EXPORTED;

                    uat.SetParameterValue("txtDesc4", wBox.GetTotalImageCount(tempState, false, policyState).ToString());
                    uat.SetParameterValue("txtDesc5", Convert.ToString(wBox.GetTotalImageCount(tempState, false, policyState) - wBox.GetTotalImageCount(tempState, true, policyState)));
                    uat.SetParameterValue("txtDesc6", Convert.ToString(wBox.GetTotalImageCount(tempState, false, policyState) /Convert.ToInt32(wBox.GetTotalPolicies(policyState).ToString())));
                    uat.SetParameterValue("txtDesc7", Math.Round(Convert.ToDouble(wBox.GetTotalBatchSize().ToString()) / Convert.ToInt32(wBox.GetTotalPolicies(eSTATES.POLICY_SCANNED).ToString()), 2));
                    uat.SetParameterValue("txtDesc8", Math.Round(Convert.ToDouble(wBox.GetTotalBatchSize().ToString()) / Convert.ToInt32(wBox.GetTotalImageCount(tempState, false, policyState).ToString()), 2));
                    uat.SetParameterValue("txtDesc9", wBox.GetLICCheckedCount());

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), wBox.ctrlBox.BoxNumber, 0);
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    ds = wPolicy.GetPolicyList(policyState);
                    uat.SetParameterValue("txtDesc10", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), wBox.ctrlBox.BoxNumber, 0);
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    ds = wPolicy.GetPolicyList(policyState);
                    string policyList = string.Empty;
                    int k, j;
                    for (k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        polPending = polPending + 1;
                        policyList = policyList + "," + ds.Tables[0].Rows[k][0].ToString();
                    }
                    //policyList = policyList.Substring(1);
                    uat.SetParameterValue("txtDummyDockets", policyList);

                    policyList = string.Empty;

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                    ds = wPolicy.GetPolicyList(policyState);
                    policyList = string.Empty;
                    for (j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        polPending = polPending + 1;
                        policyList = policyList + "," + ds.Tables[0].Rows[j][0].ToString();
                    }
                    //policyList = policyList.Substring(1);

                    uat.SetParameterValue("txtNotReceived", policyList);
                    uat.SetParameterValue("txtDesc11", polPending);
                    uat.SetParameterValue("txtDesc12", "200");
                    crv.ReportSource = uat;
                }

                if (reportName == "BatchSummary")
                {
                    BatchSummery bsy = new BatchSummery();

                    tmpProj = new wfeProject(sqlCon);
                    ds = new DataSet();
                    ds = tmpProj.GetConfiguration();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bsy.SetParameterValue("txtScanCenter", ds.Tables[0].Rows[0]["SCAN_CENTER"].ToString());
                        bsy.SetParameterValue("txtVendorCode", ds.Tables[0].Rows[0]["VENDOR_CODE"].ToString());
                        bsy.SetParameterValue("txtVersionNumber", ds.Tables[0].Rows[0]["VERSION_NUMBER"].ToString());
                        bsy.SetParameterValue("txtVendorName", ds.Tables[0].Rows[0]["VENDOR_NAME"].ToString());
                    }
                    else
                    {
                        bsy.SetParameterValue("txtScanCenter", 0);
                        bsy.SetParameterValue("txtVendorCode", 0);
                        bsy.SetParameterValue("txtVersionNumber", 0);
                        bsy.SetParameterValue("txtVendorName", 0);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    ds = tmpProj.GetMainConfiguration(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bsy.SetParameterValue("txtDoCode", ds.Tables[0].Rows[0]["DO_CODE"].ToString());
                        bsy.SetParameterValue("txtBoCode", ds.Tables[0].Rows[0]["BO_CODE"].ToString());

                        bsy.SetParameterValue("txtBatchNo", wBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey));
                    }
                    else
                    {
                        bsy.SetParameterValue("txtDoCode", 0);
                        bsy.SetParameterValue("txtBoCode", 0);
                        bsy.SetParameterValue("txtBatchNo", 0);
                    }
                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), 0, 0);
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    ///For Showing not scanned count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtScan", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                    ///For Showing not QC count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtQC", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_QC;

                    ///For Showing not index count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtIndex", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;

                    ///For Showing Policy Exception(LIC gievn) count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtException", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;

                    ///For Showing Policy Exception(LIC gievn) count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtOnHold", ds.Tables[0].Rows.Count);
                    polPending = ds.Tables[0].Rows.Count;

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;

                    ///For Showing Policy Exception(LIC gievn) count
                    ds = wPolicy.GetPolicyList(policyState);
                    bsy.SetParameterValue("txtMissing", ds.Tables[0].Rows.Count);
                    polPending = polPending + ds.Tables[0].Rows.Count;

                    policyState = new eSTATES[3];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                    policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                    ///For Showing Policy Exception(LIC gievn) count
                    ds = wPolicy.GetPolicyList(policyState);
                    int indexed = ds.Tables[0].Rows.Count;
                    bsy.SetParameterValue("txtIndexed", indexed);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;

                    ///For Showing Policy Exception(LIC gievn) count
                    ds = wPolicy.GetPolicyList(policyState);
                    indexed = ds.Tables[0].Rows.Count;
                    bsy.SetParameterValue("txtExported", indexed);

                    ///Details for UAT details
                    eSTATES state = new eSTATES();

                    eSTATES[] tempState = new eSTATES[5];
                    policyState = new eSTATES[5];
                    pBox = new CtrlBox(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), 0);
                    wBox = new wfeBox(sqlCon, pBox);
                    bsy.SetParameterValue("txtDesc1", wBox.GetTotalPolicies(state).ToString());
                    bsy.SetParameterValue("txtDesc2", Convert.ToString((Convert.ToInt32(wBox.GetTotalPolicies(state).ToString()) - Convert.ToInt32(wBox.GetTotalPolicies(eSTATES.POLICY_MISSING)))));

                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                    policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                    policyState[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                    policyState[4] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;

                    bsy.SetParameterValue("txtDesc3", wBox.GetTotalPolicies(policyState).ToString());

                    tempState[0] = eSTATES.PAGE_INDEXED;
                    tempState[1] = eSTATES.PAGE_FQC;
                    tempState[2] = eSTATES.PAGE_CHECKED;
                    tempState[3] = eSTATES.PAGE_EXCEPTION;
                    tempState[4] = eSTATES.PAGE_EXPORTED;

                    bsy.SetParameterValue("txtDesc4", wBox.GetTotalImageCount(tempState, false, policyState).ToString());
                    eSTATES[] scannedStatus = new eSTATES[1];
                    bsy.SetParameterValue("txtTotImageScanned", wBox.GetTotalImageCount());
                    bsy.SetParameterValue("txtDesc5", Convert.ToString(wBox.GetTotalImageCount(tempState, false, policyState) - wBox.GetTotalImageCount(tempState, true, policyState)));
                    bsy.SetParameterValue("txtDesc6", Convert.ToString(wBox.GetTotalImageCount(tempState, false, policyState) / Convert.ToInt32(wBox.GetTotalPolicies(policyState).ToString())));
                    decimal totBatchSizeInMB = Convert.ToDecimal(wBox.GetTotalBatchSize().ToString());
                    decimal totBatchSizeInKB = Convert.ToDecimal(wBox.GetTotalBatchSize().ToString());

                    bsy.SetParameterValue("txtDesc7", Math.Round(Convert.ToDouble(totBatchSizeInMB) / Convert.ToInt32(wBox.GetTotalPolicies(policyState).ToString()), 2) + " MB");
                    string y = Math.Round(Convert.ToDouble(totBatchSizeInKB) * 1024 / Convert.ToInt32(wBox.GetTotalImageCount(tempState, false, policyState).ToString()), 2) + " KB";
                    bsy.SetParameterValue("txtDesc8", y);
                    bsy.SetParameterValue("txtDesc9", wBox.GetLICCheckedCount());


                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), wBox.ctrlBox.BoxNumber, 0);
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    ds = wPolicy.GetRectifiedPolicyCount();
                    bsy.SetParameterValue("txtDesc10", ds.Tables[0].Rows.Count);

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), wBox.ctrlBox.BoxNumber, 0);
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    ds = wPolicy.GetPolicyList(policyState);
                    
                    string policyList = string.Empty;
                    int k, j;
                    for (k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        policyList = policyList + "," + ds.Tables[0].Rows[k][0].ToString();
                    }
                    //policyList = policyList.Substring(1);
                    bsy.SetParameterValue("txtDummyDockets", policyList);

                    policyList = string.Empty;

                    policyState = new eSTATES[1];
                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                    ds = wPolicy.GetPolicyList(policyState);
                    policyList = string.Empty;
                    for (j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        policyList = policyList + "," + ds.Tables[0].Rows[j][0].ToString();
                    }
                    //policyList = policyList.Substring(1);

                    bsy.SetParameterValue("txtNotReceived", policyList);

                    bsy.SetParameterValue("txtDesc11", polPending);
                    crv.ReportSource = bsy;
                }
                //if (reportName == "RMFSummary")
                //{
                //    RMFSummary rmf = new RMFSummary();
                //    DataSet ds = new DataSet();
                //    DataSet ds1 = new DataSet();
                //    DataSet ds2 = new DataSet();
                //    DataSet ds3 = new DataSet();

                //    int RcvdPolicy;
                //    int RtndPolicy;

                //    ds = lgs.GetRMFData(projKey,batchKey);
                //    int k = 1;
                //    string minPolicy;
                //    string maxPolicy;

                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        {
                //            ds1 = lgs.GetExcpCount(projKey, batchKey, ds.Tables[0].Rows[i]["boxno"].ToString());
                //            rmf.SetParameterValue("txtBoxNo" + k, ds.Tables[0].Rows[i]["boxno"].ToString());
                //            RcvdPolicy = Convert.ToInt32(ds.Tables[0].Rows[i]["imagename"].ToString());
                //            rmf.SetParameterValue("txtTotRcvd" + k, RcvdPolicy);
                //            RtndPolicy = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());
                //            rmf.SetParameterValue("txtTotRtnd" + k, RcvdPolicy - RtndPolicy);

                //            ds3 = lgs.GetMaxMinPolicy(projKey, batchKey, ds.Tables[0].Rows[i]["boxno"].ToString());
                //            minPolicy = ds3.Tables[0].Rows[0]["minpolicy"].ToString();
                //            maxPolicy = ds3.Tables[0].Rows[0]["maxpolicy"].ToString();
                //            rmf.SetParameterValue("txtPolicyStEnd" + k, minPolicy + "-" + maxPolicy);
                //            ds2 = lgs.GetPolicyWithExcp(projKey, batchKey, ds.Tables[0].Rows[i]["boxno"].ToString());

                //            string excpPolicy = string.Empty;
                //            for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                //            {
                //                excpPolicy = excpPolicy + "," + ds2.Tables[0].Rows[j]["imagename"].ToString();
                //            }
                //            if (excpPolicy != string.Empty)
                //            {
                //                rmf.SetParameterValue("txtExcpDtls" + k, excpPolicy.Substring(1, excpPolicy.Length - 1));
                //                rmf.SetParameterValue("txtRemarks" + k, "Returned to LIC");
                //            }
                //            else
                //            {
                //                rmf.SetParameterValue("txtExcpDtls" + k, string.Empty);
                //                rmf.SetParameterValue("txtRemarks" + k, string.Empty);
                //            }

                //            k = k + 1;
                //        }
                //        rmf.SetParameterValue("txtBatchNo",lgs.GetBatchName(batchKey));
                //        crv.DisplayGroupTree = false;
                //        crv.ReportSource = rmf;
                //    }
                //    else
                //    {
                //        MessageBox.Show("Data not found for this search criteria","Not Found",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //    }

                //   }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating the report");
                exMailLog.Log(ex);
            }
        }
    }
}