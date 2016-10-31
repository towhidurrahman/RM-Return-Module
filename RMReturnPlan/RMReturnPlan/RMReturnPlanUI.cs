using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using RMTransferPlan.DAL;
using RMTransferPlan;
using RMTransferPlan.Reports;

namespace RMTransferPlan
{
    public partial class RMTransferPlanUI : Form
    {
        SQL sq= new SQL();
        public DataTable dt= new DataTable();
        public int slNo;
        public string planningDate;
        public string productCategory;
        public string productSegment;
        public int goButtonClicked = 0;
        public int editButtonClicked = 0;
        public int postedEntry = 0;
        public int postButtonClicked = 0;
        public int saveButtonClicked = 0;


        DataProvider dataProvider= new DataProvider();
        public RMTransferPlanUI()
        {
            InitializeComponent();
        }

        private void RMTransferPlanUI_Load(object sender, EventArgs e)
        {
            string[] column = new string[] { "Sl No", "Item No", "Description", "Type", "UOM", "Location", "Avl.Qty", "Std.Qty", "Issue Qty", "Comments", "Max Scrap", "Excess(%)" };
            int col = 12;
            dataGridViewShow.ColumnCount = 12;
            for (int i = 0; i < col; i++)
            {
                dataGridViewShow.Columns[i].Name = column[i];
            }
            //*******************Data generate to cmbProducitonGroup*********************
            dt = sq.get_rs("Select RTRIM(SEGVAL)+'-'+ LTRIM([desc]) AS des from ICSEGv where segment = 2");

            foreach (DataRow r in dt.Rows)
            {
                cmbProductGroup.Items.Add(r["des"].ToString());
            }
            cmbProductLine.Items.Add("Line1");
            dateTimePickerHead.Value = DateTime.Today;
            cmbProductGroup.SelectedIndex = 2;
            cmbProductLine.SelectedIndex = 0;

            buttonStatus();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {      
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Columns.Clear();
            dataGridViewShow.Refresh();
            dataGridViewShow.ReadOnly = false;

            editButtonClicked = 0;
            postButtonClicked = 0;
            postedEntry = 0;
            goButtonClicked = 1;

            btnGoFillData();
            buttonStatus();
        }
        public void btnGoFillData()
        {
            DataGrid();

            planningDate = dateTimePickerHead.Value.ToString("yyyyMMdd");
            productCategory = cmbProductGroup.Text.Substring(3, 10);
            productSegment = cmbProductGroup.Text.Substring(0,2); 

            dt = sq.get_rs("select * from T_PCTRN where TRANDATE ='" + planningDate + "' and ITEMTYPE = 'RM' and ITEMNO <> '15502-05' AND CATEGORY = '" + productCategory + "'");
            if (dt.Rows.Count > 0)
            {
                slNo=1;
                foreach (DataRow r in dt.Rows)
                {
                    this.dataGridViewShow.Rows.Add(slNo, r["ITEMNO"].ToString(), r["ITEMDESC"].ToString(), r["ITEMTYPE"].ToString(), r["UNIT"].ToString(), r["SOURCELOCATION"].ToString(), r["MAXVARIANCEPERCENT"].ToString(), r["QTYONHAND"].ToString(), r["REQUIREDQTY"].ToString(), r["TRANSFERQTY"].ToString(), r["RETURNQTY"].ToString(), r["COMMENT"].ToString(),0,0);
                    slNo++;
                }
                dataGridViewShow.AllowUserToAddRows = false;

                //********** ********DataGridview not sortable************************
                foreach (DataGridViewColumn column in dataGridViewShow.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                string selectQurey;
                selectQurey = "SELECT COMPONENT AS ITEMNO , [Component Name] AS ITEMDESC, ITEMBRKID AS ITEMTYPE, UNIT, SOURCELOCATION, MAXVARIANCEPERCENT,QTYONHAND, SUM(REQUIREDQTY) as REQUIREDQTY, '0' AS TRANSFERQTY, '' as COMMENT FROM IC_BOM_TRANSFER";
                selectQurey = selectQurey + " where PRODUCTIONDATE = '" + planningDate + "' AND ITEMBRKID = 'RM' and COMPONENT <> '12-55005' and REQUIREDQTY > 0 and RIGHT(LEFT(WIPITEMNO,5),2) = '" + productSegment + "'";
                selectQurey = selectQurey + " group by COMPONENT, [Component Name], ITEMBRKID, UNIT, sourcelocation, MAXVARIANCEPERCENT, qtyonhand";

                dt = sq.get_rs(selectQurey);
                slNo = 1;
                foreach (DataRow r in dt.Rows)
                {
                    //DataTable dt1 = sq.get_rs("SELECT QTYONHAND FROM ICILOC WHERE ITEMNO = '" + r["ITEMNO"].ToString().Replace("-", "") + "' AND LOCATION = '" + r["SOURCELOCATION"].ToString() + "'");
                    this.dataGridViewShow.Rows.Add(slNo, r["ITEMNO"].ToString(), r["ITEMDESC"].ToString(), r["ITEMTYPE"].ToString(), r["UNIT"].ToString(), r["SOURCELOCATION"].ToString(), r["qtyonhand"].ToString(), r["requiredqty"].ToString(), r["transferqty"].ToString(), r["Comment"].ToString(), "", "");
                    slNo++;
                }

                dataGridViewShow.AllowUserToAddRows = false;              
            }         
            //********** ********DataGridview not sortable************************8
            foreach (DataGridViewColumn column in dataGridViewShow.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public void DataGrid()
        {
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Columns.Clear();
            dataGridViewShow.Refresh();

            dataGridViewShow.ColumnCount = 12;
            string[] formatGrid = new string[] { "Sl No", "Item No", "Description","Type","UOM","Location","Avl.Qty","Std.Qty","Issue Qty", "Comments","Max Scrap","Excess(%)"};

            int len = formatGrid.Length;
            for (int i = 0; i < len; i++)
            {
                dataGridViewShow.Columns[i].Name = formatGrid[i];
                if (i < 8)
                {
                    dataGridViewShow.Columns[i].ReadOnly = true;
                }
                dataGridViewShow.EditMode = DataGridViewEditMode.EditOnKeystroke;
            }
            dataGridViewShow.Columns["Sl No"].Width = 40;
            dataGridViewShow.Columns["Item No"].Width = 78;
            dataGridViewShow.Columns["Description"].Width = 150;
            dataGridViewShow.Columns["Type"].Width = 50;
            dataGridViewShow.Columns["UOM"].Width = 35;
            dataGridViewShow.Columns["Location"].Width = 80;
            dataGridViewShow.Columns["Avl.Qty"].Width = 80;
            dataGridViewShow.Columns["Std.Qty"].Width = 80;
            dataGridViewShow.Columns["Issue Qty"].Width = 80;
            dataGridViewShow.Columns["Comments"].Width = 120;
            dataGridViewShow.Columns["Max Scrap"].Width = 80;
            dataGridViewShow.Columns["Excess(%)"].Width = 80;
        }
        
        // **********************validation for only numeric number******************
        private void dataGridViewShow_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);

            if ((dataGridViewShow.CurrentCell.ColumnIndex == 8))
            {
                TextBox tb = e.Control as TextBox;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {

            int col = dataGridViewShow.CurrentCell.ColumnIndex;
            int row = dataGridViewShow.CurrentCell.RowIndex;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char) (Keys.Enter))
            {
                //************** general shift********************************
                if (col == 9)
                {
                    SendKeys.Send("{left}");
                }
                else
                {
                    SendKeys.Send("{up}");
                    SendKeys.Send("{right}");
                }
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowCnt = dataGridViewShow.Rows.Count;
            if (dataGridViewShow.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("No Data to Save");
                goto done;
            }
            saveButtonClicked = 1;
            try
            {
                btnSave.Focus();
                planningDate = dateTimePickerHead.Value.ToString("yyyyMMdd");
                //dateTimePickerHead.Value = DateTime.Now;
                int totalRow = dataGridViewShow.RowCount;

                //dt = sq.get_rs("Select isnull(TRANSTATUS,'0') as POSTATUS from T_PCTRN where TRANDATE = '" + planningDate + "' and ITEMTYPE = 'RM' AND CATEGORY = '" + productCategory + "'");
                //for (int i = 0; i < totalRow; i++)
                //{
                //    string avlQty = dataGridViewShow.Rows[i].Cells[6].Value.ToString();
                //    string issueQty = dataGridViewShow.Rows[i].Cells[8].Value.ToString();
                //    if (Convert.ToInt32(avlQty) < Convert.ToInt32(issueQty))
                //    {
                //        MessageBox.Show("Not Enough Stock Avialble");
                //    }
                //}

                if (editButtonClicked == 1)
                {
                    dt = sq.get_rs("delete from T_PCTRN where tranDate = '" + planningDate + "' and category = '" + cmbProductGroup.Text.Substring(3, 10) + "'");
                }
                for (int i = 0; i < totalRow; i++)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        string itemNo = dataGridViewShow.Rows[i].Cells[j].Value.ToString();
                        string itemDescription = dataGridViewShow.Rows[i].Cells[j + 1].Value.ToString();
                        string type = dataGridViewShow.Rows[i].Cells[j + 2].Value.ToString();
                        string unit = dataGridViewShow.Rows[i].Cells[j + 3].Value.ToString();
                        string category = cmbProductGroup.Text.Substring(3, 10);
                        string location = dataGridViewShow.Rows[i].Cells[j + 4].Value.ToString();
                        string maxScrap = dataGridViewShow.Rows[i].Cells[j + 5].Value.ToString();
                        string avlQty = dataGridViewShow.Rows[i].Cells[j + 6].Value.ToString();
                        string stdQty = dataGridViewShow.Rows[i].Cells[j + 7].Value.ToString();
                        string issueQty = dataGridViewShow.Rows[i].Cells[j + 8].Value.ToString();
                        string comments = dataGridViewShow.Rows[i].Cells[j + 9].Value.ToString();
                        string excess = dataGridViewShow.Rows[i].Cells[j + 10].Value.ToString();

                        string insertQuery = "insert into T_PCTRN([TRANDATE],[ITEMNO],[ITEMDESC],[ITEMTYPE],[UNIT],[CATEGORY],[SOURCELOCATION],[MAXVARIANCEPERCENT],[QTYONHAND],[REQUIREDQTY],[TRANSFERQTY],[RETURNQTY],[COMMENT],[RMTRANSFER],[PMTRANSFER],[TRANSTATUS]) VALUES('" + planningDate + "','" + itemNo + "','" + itemDescription + "','" + type + "','" + unit + "','" + category + "','" + location + "','" + maxScrap + "','" + avlQty + "','" + stdQty + "','" + issueQty + "',0,'" + comments + "',1,1,0)";
                        dt = sq.get_rs(insertQuery);
                        //dt = sq.get_rs("insert into T_PCTRN([TRANDATE],[ITEMNO],[ITEMDESC],[ITEMTYPE],[UNIT],[CATEGORY],[SOURCELOCATION],[MAXVARIANCEPERCENT],[QTYONHAND],[REQUIREDQTY],[TRANSFERQTY],[RETURNQTY],[COMMENT],[RMTRANSFER],[PMTRANSFER],[TRANSTATUS]) VALUES('" +
                        //     planningDate + "','" + itemNo + "','" + itemDescription + "','" + type + "','" + unit + "','" + category + "','" + location + "','" + maxScrap + "','" + avlQty + "','" + stdQty + "','" + issueQty + "',0,'" + comments + "',1,1,0)");                 
                    }
                }
                MessageBox.Show("Data Saved Successfully");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        done:
            ;
            buttonStatus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            editButtonClicked = 1;
            saveButtonClicked = 0;
            postButtonClicked = 0;
            dataGridViewShow.ReadOnly = false;
            MessageBox.Show("You can edit This Data");
            buttonStatus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {        
            rmReport();             
        }
        public void rmReport()
        {
            string planningDate = dateTimePickerHead.Value.ToString("yyyyMMdd");
            productCategory = cmbProductGroup.Text.Substring(3, 10);
            rptViewer rptViewer = new rptViewer();
            DataSet dsGeneral = new DataSet();
            RMTransfer objRpt = new RMTransfer();
            //rptRM objRpt = new rptRM();

            string strSql = "select * from RM_View where trandate='" + planningDate + "' and category='" + productCategory + "'";
            dsGeneral = dataProvider.getDataSet(strSql, "RM_View", "TCPL"); 
            objRpt.SetDataSource(dsGeneral);
            objRpt.VerifyDatabase();

            rptViewer.crystalReportViewer.ReportSource = objRpt;
            rptViewer.Refresh();
            rptViewer.Show();
        }
        private void btnPost_Click(object sender, EventArgs e)
        {
            productCategory = cmbProductGroup.Text.Substring(3, 10);
            planningDate = dateTimePickerHead.Value.ToString("yyyyMMdd");
            DataTable dtpost = new DataTable();
            dtpost = sq.get_rs("update T_PCTRN set TRANSTATUS=1 where tranDate = '" + planningDate + "' and category = '" + cmbProductGroup.Text.Substring(3, 10) + "'");

            postButtonClicked = 1;
            buttonStatus();
            MessageBox.Show("Posted successfully");
            dataGridViewShow.ReadOnly = true;


            //********************for Macro***********************************
            AccpacCOMAPI.AccpacSession session = new AccpacCOMAPI.AccpacSession();
            this.Cursor = Cursors.WaitCursor;
            session.Init("", "IC60A", "IC2000", "60A");

            session.Open("ADMIN", "ADMIN", "TCPDAT", DateTime.Now, 0, "");

            AccpacCOMAPI.AccpacDBLink mDBLinkCmpRW = session.OpenDBLink(AccpacCOMAPI.tagDBLinkTypeEnum.DBLINK_COMPANY, AccpacCOMAPI.tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);
            AccpacCOMAPI.AccpacDBLink mDBLinkSysRW = session.OpenDBLink(AccpacCOMAPI.tagDBLinkTypeEnum.DBLINK_SYSTEM, AccpacCOMAPI.tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);

            //MessageBox.Show("Done");

            Boolean temp;
            AccpacCOMAPI.AccpacView ICADE1header;
            AccpacCOMAPI.AccpacViewFields ICADE1headerFields;
            mDBLinkCmpRW.OpenView("IC0120", out ICADE1header);
            ICADE1headerFields = ICADE1header.Fields;


            AccpacCOMAPI.AccpacView ICADE1detail1;
            AccpacCOMAPI.AccpacViewFields ICADE1detail1Fields;
            mDBLinkCmpRW.OpenView("IC0110", out ICADE1detail1);
            ICADE1detail1Fields = ICADE1detail1.Fields;

            AccpacCOMAPI.AccpacView ICADE1detail2;
            AccpacCOMAPI.AccpacViewFields ICADE1detail2Fields;
            mDBLinkCmpRW.OpenView("IC0125", out ICADE1detail2);
            ICADE1detail2Fields = ICADE1detail2.Fields;

            AccpacCOMAPI.AccpacView ICADE1detail3;
            AccpacCOMAPI.AccpacViewFields ICADE1detail3Fields;
            mDBLinkCmpRW.OpenView("IC0115", out ICADE1detail3);
            ICADE1detail3Fields = ICADE1detail3.Fields;

            AccpacCOMAPI.AccpacView ICADE1detail4;
            AccpacCOMAPI.AccpacViewFields ICADE1detail4Fields;
            mDBLinkCmpRW.OpenView("IC0113", out ICADE1detail4);
            ICADE1detail4Fields = ICADE1detail4.Fields;


            AccpacCOMAPI.AccpacView ICADE1detail5;
            AccpacCOMAPI.AccpacViewFields ICADE1detail5Fields;
            mDBLinkCmpRW.OpenView("IC0117", out ICADE1detail5);
            ICADE1detail5Fields = ICADE1detail5.Fields;


            AccpacCOMAPI.AccpacView[] array1 = new AccpacCOMAPI.AccpacView[2];
            array1[0] = ICADE1detail1;
            array1[1] = ICADE1detail2;
            Object obj1 = (Object)array1;
            ICADE1header.Compose(ref obj1);

            AccpacCOMAPI.AccpacView[] array2 = new AccpacCOMAPI.AccpacView[10];
            array2[0] = ICADE1header;
            array2[1] = null;
            array2[2] = null;
            array2[3] = null;
            array2[4] = null;
            array2[5] = null;
            array2[6] = null;
            array2[7] = ICADE1detail3;
            array2[8] = ICADE1detail5;
            array2[9] = ICADE1detail4;
            Object obj2 = (Object)array2;
            ICADE1detail1.Compose(ref obj2);

            AccpacCOMAPI.AccpacView[] array3 = new AccpacCOMAPI.AccpacView[1];
            array3[0] = ICADE1header;
            Object obj3 = (Object)array3;
            ICADE1detail2.Compose(ref obj3);


            AccpacCOMAPI.AccpacView[] array4 = new AccpacCOMAPI.AccpacView[1];
            array4[0] = ICADE1detail1;
            Object obj4 = (Object)array4;
            ICADE1detail3.Compose(ref obj4);


            AccpacCOMAPI.AccpacView[] array5 = new AccpacCOMAPI.AccpacView[1];
            array5[0] = ICADE1detail1;
            Object obj5 = (Object)array5;
            ICADE1detail4.Compose(ref obj5);


            AccpacCOMAPI.AccpacView[] array6 = new AccpacCOMAPI.AccpacView[1];
            array6[0] = ICADE1detail1;
            Object obj6 = (Object)array6;
            ICADE1detail5.Compose(ref obj6);

            ICADE1header.Order = 3;
            ICADE1header.FilterSelect("(DELETED = 0)", true, 3, 0);
            ICADE1header.Order = 3;
            ICADE1header.Order = 0;  /////////////////////////////////////////

            ICADE1headerFields.get_FieldByName("ADJENSEQ").PutWithoutVerification("0");

            ICADE1header.Init();
            temp = ICADE1detail1.Exists;
            ICADE1detail1.RecordClear();
            ICADE1header.Order = 3;


            //Object TRANDATE = (Object)"1/25/2016";
            
            String formatDate = planningDate.Substring(4, 2) + "/" + planningDate.Substring(6, 2) + "/" + planningDate.Substring(0, 4);


            Object TRANSDATE = (Object)formatDate;
            ICADE1headerFields.get_FieldByName("TRANSDATE").set_Value(ref TRANSDATE);
            ICADE1headerFields.get_FieldByName("PROCESSCMD").PutWithoutVerification("1");     //' Process Command
            ICADE1header.Process();    


            for (int i = 0; i < dataGridViewShow.RowCount; i++)
            {
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordClear();
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordCreate(0);
                temp = ICADE1detail1.Exists;


                string dgvItemNo = dataGridViewShow.Rows[i].Cells[1].Value.ToString();


                Object ITEMNO = (Object) dgvItemNo;
                ICADE1detail1Fields.get_FieldByName("ITEMNO").set_Value(ref ITEMNO);
                ICADE1detail1Fields.get_FieldByName("PROCESSCMD").PutWithoutVerification("1"); //' Process Command

                ICADE1detail1.Process();

                Object TRANSTYPE = (Object) "5";
                ICADE1detail1Fields.get_FieldByName("TRANSTYPE").set_Value(ref TRANSTYPE);


                string dgvLocation = dataGridViewShow.Rows[i].Cells[5].Value.ToString();

                Object LOCATION = (Object) dgvLocation;
                ICADE1detail1Fields.get_FieldByName("LOCATION").set_Value(ref LOCATION);


                string dgvIssueQty = dataGridViewShow.Rows[i].Cells[8].Value.ToString();

                Object QUANTITY = (Object) dgvIssueQty;
                ICADE1detail1Fields.get_FieldByName("QUANTITY").set_Value(ref QUANTITY);

                ICADE1detail4.RecordClear();
                ICADE1detail4.RecordCreate(0);

                Object LOTNUMF = (Object) "lot";
                ICADE1detail4Fields.get_FieldByName("LOTNUMF").set_Value(ref LOTNUMF);

                Object QTY = (Object) dgvIssueQty;
                ICADE1detail4Fields.get_FieldByName("QTY").set_Value(ref QTY);

                decimal transCost = ICADE1detail1Fields.get_FieldByName("EXTCOST").get_Value();
                decimal unitcost = transCost/Convert.ToDecimal(dgvIssueQty);
                decimal adjcost = unitcost*Convert.ToDecimal(dgvIssueQty);

                Object COST = (Object) adjcost;
                ICADE1detail4Fields.get_FieldByName("COST").set_Value(ref COST);

                ICADE1detail4.Insert();

                ICADE1detail4Fields.get_FieldByName("LOTNUMF").PutWithoutVerification("lot");



                ICADE1detail4.Read();
                ICADE1detail1.Process();
                ICADE1detail1.Insert();


                ICADE1detail1Fields.get_FieldByName("LINENO").PutWithoutVerification(i*-1); //' Line Number


                ICADE1detail1.Read();
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordCreate(0);

                ICADE1detail1Fields.get_FieldByName("LINENO").PutWithoutVerification(i*-1); //' Line Number

                ICADE1detail1.Read();
            }
            /* ICADE1detail1.Read
 temp = ICADE1detail1.Exists
 ICADE1detail1.RecordCreate 0
 temp = ICADE1detail1.Exists

 ICADE1detail1Fields("ITEMNO").Value = "10-16010"                      ' Item Number
 ICADE1detail1Fields("PROCESSCMD").PutWithoutVerification("1")        ' Process Command

 ICADE1detail1.Process

 ICADE1detail1Fields("TRANSTYPE").Value = "5"                          ' Transaction Type
 ICADE1detail1Fields("LOCATION").Value = "100115"                      ' Location
 ICADE1detail1Fields("QUANTITY").Value = "100.0000"                    ' Quantity
 ICADE1detail4.RecordClear
 ICADE1detail4.RecordCreate 0

 ICADE1detail4Fields("LOTNUMF").Value = "lot"                          ' Lot Number
 ICADE1detail4Fields("QTY").Value = "100.0000"                         ' Transaction Quantity
 ICADE1detail4Fields("COST").PutWithoutVerification("4082.240")       ' Lot Cost

 ICADE1detail4.Insert

 ICADE1detail4Fields("LOTNUMF").PutWithoutVerification("lot")         ' Lot Number

 ICADE1detail4.Read
 ICADE1detail1.Process
 ICADE1detail1.Insert

 ICADE1detail1Fields("LINENO").PutWithoutVerification("-2")           ' Line Number

 ICADE1detail1.Read */


            ICADE1headerFields.get_FieldByName("HDRDESC").PutWithoutVerification("DESC MACRO");  // ' Description

            ICADE1headerFields.get_FieldByName("REFERENCE").PutWithoutVerification("REF MACRO"); //  ' Reference

            ICADE1header.Insert();
            ICADE1header.Order = 0;

            ICADE1headerFields.get_FieldByName("ADJENSEQ").PutWithoutVerification("0"); //  ' Sequence Number

            ICADE1header.Init();
            temp = ICADE1detail1.Exists;
            ICADE1detail1.RecordClear();
            ICADE1header.Order = 3;

            MessageBox.Show("Done");

        }
        public void buttonStatus()
        {
            if (postedEntry == 0)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
            if (saveButtonClicked == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnPost.Enabled = true;
            }
            //if (postedEntry == 1)
            //{
            //    btnSave.Enabled = false;
            //    btnEdit.Enabled = true;
            //    btnPost.Enabled = true;
            //}
            if (editButtonClicked == 1)
            {
                btnSave.Enabled = true;
                btnPost.Enabled = true;

            }
            if (postedEntry == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
            if (postButtonClicked == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
        }
    }
}
