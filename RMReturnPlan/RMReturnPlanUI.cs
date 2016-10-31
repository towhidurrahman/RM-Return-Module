using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CrystalDecisions.Shared;
using RMReturnPlan.DAL;
using RMReturnPlan;
using RMReturnPlan.Reports;

namespace RMReturnPlan
{
    public partial class RMReturnPlanUI : Form
    {
        SQL sq= new SQL();
        public DataTable dt= new DataTable();
        public int slNo;
        public string planningDate;
        public string productCategory;
        public string productSegment;
        public decimal WIPCost;
        public Boolean oldRecord;
        public Boolean enableEdit;

        public int goButtonClicked = 0;
        public int editButtonClicked = 0;
        public int tranStatus = 0;
        public int postButtonClicked = 0;
        public int saveButtonClicked = 0;

        public AccpacCOMAPI.AccpacSession session;
        public AccpacCOMAPI.AccpacDBLink mDBLinkCmpRW;
        public AccpacCOMAPI.AccpacDBLink mDBLinkSysRW;

        DataProvider dataProvider= new DataProvider();
        public RMReturnPlanUI()
        {
            InitializeComponent();
        }
        private void RMTransferPlanUI_Load(object sender, EventArgs e)
        {
            string[] column = new string[] { "Sl No", "Item No", "Description", "Type", "UOM", "Location", "Issue Qty", "Return Qty",  "Consumption"};
            int col = 9;
            dataGridViewShow.ColumnCount = 9;
            for (int i = 0; i < col; i++)
            {
                dataGridViewShow.Columns[i].Name = column[i];
            }
            //*******************Data generate to cmbProductionGroup*********************
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
            Application.Exit();
        }
        private void btnGo_Click(object sender, EventArgs e)
        {      
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Columns.Clear();
            dataGridViewShow.Refresh();
            dataGridViewShow.ReadOnly = false;

            editButtonClicked = 0;
            postButtonClicked = 0;
            tranStatus = 0;
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
            dt = sq.get_rs("select * from T_PCTRN where TRANDATE ='" + planningDate + "' and ITEMTYPE = 'RM' and ITEMNO not like  '12-%' AND CATEGORY = '" + productCategory + "' order by itemno");
            if (dt.Rows.Count > 0)
            {
                slNo=1;
                oldRecord = true;                             
                tranStatus = Convert.ToInt32(dt.Rows[0]["TRANSTATUS"].ToString());
                foreach (DataRow r in dt.Rows)
                {                                        
                    this.dataGridViewShow.Rows.Add(slNo, r["ITEMNO"].ToString(), r["ITEMDESC"].ToString(), r["ITEMTYPE"].ToString(), r["UNIT"].ToString(), r["SOURCELOCATION"].ToString(), r["TRANSFERQTY"].ToString(), r["RETURNQTY"].ToString(),"");
                    slNo++;
                }
                dataGridViewShow.AllowUserToAddRows = false;

                //*******************DataGridview not sortable***********************
                foreach (DataGridViewColumn column in dataGridViewShow.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }            
            }

            //else
            //{
            //    oldRecord = false;
            //    string selectQurey;
            //    selectQurey = "SELECT COMPONENT AS ITEMNO , [Component Name] AS ITEMDESC, ITEMBRKID AS ITEMTYPE, UNIT, SOURCELOCATION, MAXVARIANCEPERCENT,QTYONHAND, SUM(REQUIREDQTY) as REQUIREDQTY, '0' AS TRANSFERQTY, '' as COMMENT FROM IC_BOM_TRANSFER";
            //    selectQurey = selectQurey + " where PRODUCTIONDATE = '" + planningDate + "' AND ITEMBRKID = 'RM' and COMPONENT not like '12-%' and REQUIREDQTY > 0 and RIGHT(LEFT(WIPITEMNO,5),2) = '" + productSegment + "'";
            //    selectQurey = selectQurey + " group by COMPONENT, [Component Name], ITEMBRKID, UNIT, sourcelocation, MAXVARIANCEPERCENT, qtyonhand order by COMPONENT";

            //    dt = sq.get_rs(selectQurey);
            //    slNo = 1;
            //    foreach (DataRow r in dt.Rows)
            //    {
            //        this.dataGridViewShow.Rows.Add(slNo, r["ITEMNO"].ToString(), r["ITEMDESC"].ToString(), r["ITEMTYPE"].ToString(), r["UNIT"].ToString(), r["SOURCELOCATION"].ToString(), r["qtyonhand"].ToString(), r["requiredqty"].ToString(), r["transferqty"].ToString(), r["Comment"].ToString(), r["MAXVARIANCEPERCENT"], "");
            //        slNo++;
            //    }
            //    dataGridViewShow.AllowUserToAddRows = false;              
            //}         
            
            //******************DataGridview not sortable************************
            foreach (DataGridViewColumn column in dataGridViewShow.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            buttonStatus();
        }
        public void DataGrid()
        {
            dataGridViewShow.Rows.Clear();
            dataGridViewShow.Columns.Clear();
            dataGridViewShow.Refresh();
            dataGridViewShow.ColumnCount = 9;
            string[] formatGrid = new string[] { "Sl No", "Item No", "Description", "Type", "UOM", "Location", "Issue Qty", "Return Qty","Consumption" };
            
            int len = formatGrid.Length;
            for (int i = 0; i < len; i++)
            {
                dataGridViewShow.Columns[i].Name = formatGrid[i];
                if (i < 7)
                {
                    dataGridViewShow.Columns[i].ReadOnly = true;
                }
                //dataGridViewShow.EditMode = DataGridViewEditMode.EditOnKeystroke;
            }
            dataGridViewShow.Columns["Sl No"].Width = 40;
            dataGridViewShow.Columns["Item No"].Width = 78;
            dataGridViewShow.Columns["Description"].Width = 150;
            dataGridViewShow.Columns["Type"].Width = 50;
            dataGridViewShow.Columns["UOM"].Width = 35;
            dataGridViewShow.Columns["Location"].Width = 80;
            dataGridViewShow.Columns["Issue Qty"].Width = 80;
            dataGridViewShow.Columns["Return Qty"].Width = 80;
            dataGridViewShow.Columns["Consumption"].Width = 120;
        }
        
        // **********************Validation for only numeric number******************
        private void dataGridViewShow_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);

            if ((dataGridViewShow.CurrentCell.ColumnIndex == 7))
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
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar!='.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char) (Keys.Enter))
            {               
                if (col == 8)
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
            //if (oldRecord == true && enableEdit == false)
            //{
            //    MessageBox.Show("Cannot modify the record....");
            //    return;                
            //}                      
            //dataGridViewShow.Rows[rowCnt].ReadOnly = true;            
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
                int totalRow = dataGridViewShow.RowCount;        
                if (oldRecord == true && enableEdit == true)
                {
                    dt = sq.get_rs("delete from T_PCTRN where tranDate = '" + planningDate + "' and category = '" + cmbProductGroup.Text.Substring(3, 10) + "'");
                }
                if (Convert.ToDecimal(dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[7].Value) == 0)
                {
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
                           
                            string issueQty = dataGridViewShow.Rows[i].Cells[j + 5].Value.ToString();
                            string returnQty = dataGridViewShow.Rows[i].Cells[j + 6].Value.ToString();
                            //string consupmtion = dataGridViewShow.Rows[i].Cells[j + 7].Value.ToString();

                            string insertQuery = "insert into T_PCTRN([TRANDATE],[ITEMNO],[ITEMDESC],[ITEMTYPE],[UNIT],[CATEGORY],[SOURCELOCATION],[TRANSFERQTY],[RETURNQTY],[TRANSTATUS]) VALUES('" +
                                                planningDate + "','" + itemNo + "','" + itemDescription + "','" + type + "','" + unit + "','" + category + "','" + location + "','" + issueQty + "','" + returnQty + "',0)";
                            dt = sq.get_rs(insertQuery);                          
                        }
                    }
                    MessageBox.Show("Data Inserted Successfully");
                }
                else if (Convert.ToDecimal(dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[7].Value) >=0)
                {
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

                            string issueQty = dataGridViewShow.Rows[i].Cells[j + 5].Value.ToString();
                            string returnQty = dataGridViewShow.Rows[i].Cells[j + 6].Value.ToString();
                            //string consupmtion = dataGridViewShow.Rows[i].Cells[j + 7].Value.ToString();

                            string insertQuery = "insert into T_PCTRN([TRANDATE],[ITEMNO],[ITEMDESC],[ITEMTYPE],[UNIT],[CATEGORY],[SOURCELOCATION],[TRANSFERQTY],[RETURNQTY],[TRANSTATUS]) VALUES('" +
                                                planningDate + "','" + itemNo + "','" + itemDescription + "','" + type + "','" + unit + "','" + category + "','" + location + "','" + issueQty + "','" + returnQty + "',0)";
                            dt = sq.get_rs(insertQuery);
                        }
                    }
                    MessageBox.Show("Data updated Successfully");
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error From Save Event!!"+ex.Message);
                Environment.Exit(0);
            }
        done:
            ;
            
            btnGoFillData();           
            buttonStatus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (oldRecord == true)
            {
                if (tranStatus != 0)
                {
                    MessageBox.Show("Cannot modify posted record....");
                    return;                    
                }
            }            
                enableEdit = true;
                dataGridViewShow.ReadOnly = false;
                MessageBox.Show("You can edit this record....");

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

            string strSql = "select *, ISNULL((select sum(extcost) from ICADED where ITEMNO = T_PCTRN.ITEMNO " +
                            "and rtrim(MANITEMNO) = T_PCTRN.TRANDATE and COMMENTS = 'RMT-" + productCategory + "'),'0') " +
                            "as TRANCOST, ISNULL((select sum(extcost) from ICADED where ITEMNO = T_PCTRN.ITEMNO and rtrim(MANITEMNO) = T_PCTRN.TRANDATE and COMMENTS = 'RMR-" + productCategory + "'),'0') " +
                            "as RTNCOST from T_PCTRN where TRANDATE = '" + planningDate + "' and ITEMTYPE = 'RM' and ITEMNO <> '15502-05' AND CATEGORY = '" + productCategory + "' ORDER BY ITEMNO";
            
            dsGeneral = dataProvider.getDataSet(strSql, "T_PCTRN", "TCPL"); 
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
            DataTable dtPost = new DataTable();
            dtPost = sq.get_rs("update T_PCTRN set TRANSTATUS=1 where tranDate = '" + planningDate + "' and category = '" + cmbProductGroup.Text.Substring(3, 10) + "'");

            postButtonClicked = 1;         
           
            dataGridViewShow.ReadOnly = true;

            //**********************call Macro For Material and WIP Consumption***********************************
            MaterialConsumption();
            WIPConsumption();
            MessageBox.Show("Posted successfully");
            btnGoFillData();
        }   
   
        //*************Calculating for Consumption column and Validation for Return Quantity***********
        private void dataGridViewShow_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            decimal issueQty = Convert.ToDecimal(dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[6].Value.ToString());
            decimal returnQty = Convert.ToDecimal(dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[7].Value.ToString());                      
            
            if (Convert.ToDecimal(returnQty) > Convert.ToDecimal(issueQty))
            {
                MessageBox.Show("Issue Quantity must be greater than Return Quantity");
                dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[7].Value = 0;
            }                      
            dataGridViewShow.Rows[dataGridViewShow.CurrentCell.RowIndex].Cells[8].Value = issueQty - returnQty;
        }
        public void buttonStatus()
        {
            if (oldRecord == false)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
            if (oldRecord == true && enableEdit == false && tranStatus == 0)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnPost.Enabled = true;
            }
            if (oldRecord == true && enableEdit == true && tranStatus == 0)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnPost.Enabled = true;
                dataGridViewShow.ReadOnly = false;
            }         
            if (tranStatus == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
            if (tranStatus == 2)
            {                
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
            if (tranStatus == 3)
            {                
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnPost.Enabled = false;
            }
        }
        public void MaterialConsumption()
        {
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
                string dgvIssueQty = dataGridViewShow.Rows[i].Cells[8].Value.ToString();
                if (Convert.ToDecimal(dgvIssueQty) <= 0)
                {
                    goto down;
                }
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordClear();
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordCreate(0);
                temp = ICADE1detail1.Exists;

                string dgvItemNo = dataGridViewShow.Rows[i].Cells[1].Value.ToString();

                Object ITEMNO = (Object)dgvItemNo;
                ICADE1detail1Fields.get_FieldByName("ITEMNO").set_Value(ref ITEMNO);
                ICADE1detail1Fields.get_FieldByName("PROCESSCMD").PutWithoutVerification("1"); //' Process Command

                ICADE1detail1.Process();

                Object TRANSTYPE = (Object)"6";   //Transtype 5 for increase data and transtype 6 for decrease data
                ICADE1detail1Fields.get_FieldByName("TRANSTYPE").set_Value(ref TRANSTYPE);

                Object MANITEMNO = (Object) planningDate;
                ICADE1detail1Fields.get_FieldByName("MANITEMNO").set_Value(ref MANITEMNO);

                Object COMMENTS = (Object)"RMT-" + productCategory;
                ICADE1detail1Fields.get_FieldByName("COMMENTS").set_Value(ref COMMENTS);


                string dgvLocation = dataGridViewShow.Rows[i].Cells[5].Value.ToString();

                Object LOCATION = (Object)dgvLocation;
                ICADE1detail1Fields.get_FieldByName("LOCATION").set_Value(ref LOCATION);

                Object QUANTITY = (Object)dgvIssueQty;
                ICADE1detail1Fields.get_FieldByName("QUANTITY").set_Value(ref QUANTITY);

                decimal TransferCost = ICADE1detail1Fields.get_FieldByName("EXTCOST").get_Value();
                Decimal TransferQuantity = Convert.ToDecimal(dgvIssueQty);
                
                decimal UnitCost = TransferCost / Convert.ToDecimal(dgvIssueQty);
                WIPCost = WIPCost + TransferCost;
                decimal LotQuantity;
                decimal LotCost;

                DataTable dtLot = new DataTable();

                dtLot = sq.get_rs("Select LOTNUM, QTYAVAIL FROM ICXLOT WHERE ITEMNUM = '" + dgvItemNo.Replace("-", "") + "' and location = '" + dgvLocation + "' and QTYAVAIL <> 0 order by EXPIRYDATE");

                while (TransferQuantity != 0)
                {
                    foreach (DataRow rlot in dtLot.Rows)
                    {
                        if (TransferQuantity <= Convert.ToDecimal(rlot["QTYAVAIL"].ToString()))
                        {
                            LotQuantity = TransferQuantity;
                            LotCost = TransferCost;
                            TransferQuantity = TransferQuantity - LotQuantity;
                        }
                        else
                        {
                            LotQuantity = Convert.ToDecimal(rlot["QTYAVAIL"].ToString());
                            LotCost = (UnitCost * LotQuantity);

                            TransferQuantity = TransferQuantity - LotQuantity;
                            TransferCost = TransferCost - LotCost;
                        }
                        ICADE1detail4.RecordClear();
                        ICADE1detail4.RecordCreate(0);

                        Object LOTNUMF = (Object)rlot["LOTNUM"].ToString();
                        ICADE1detail4Fields.get_FieldByName("LOTNUMF").set_Value(ref LOTNUMF);

                        Object QTY = (Object)LotQuantity;
                        ICADE1detail4Fields.get_FieldByName("QTY").set_Value(ref QTY);


                        Object COST = (Object)LotCost;
                        ICADE1detail4Fields.get_FieldByName("COST").set_Value(ref COST);

                        ICADE1detail4.Insert();

                        ICADE1detail4Fields.get_FieldByName("LOTNUMF").PutWithoutVerification(rlot["LOTNUM"].ToString());

                        ICADE1detail4.Read();

                        if (TransferQuantity == 0)
                        {
                            goto loopOut;
                        }
                    }
                loopOut:
                    ;
                }
                ICADE1detail1.Process();
                ICADE1detail1.Insert();

                ICADE1detail1Fields.get_FieldByName("LINENO").PutWithoutVerification(i * -1); //' Line Number

                ICADE1detail1.Read();
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordCreate(0);

                ICADE1detail1Fields.get_FieldByName("LINENO").PutWithoutVerification(i * -1); //' Line Number

                ICADE1detail1.Read();
            down:
                ;
            }

            ICADE1headerFields.get_FieldByName("HDRDESC").PutWithoutVerification("Automated Adjustment for Production - RMT-" + productCategory.Trim());  // ' Description

            ICADE1headerFields.get_FieldByName("REFERENCE").PutWithoutVerification(planningDate); //  ' Reference

            ICADE1header.Insert();
            ICADE1header.Order = 0;

            ICADE1headerFields.get_FieldByName("ADJENSEQ").PutWithoutVerification("0"); //  ' Sequence Number

            ICADE1header.Init();
            temp = ICADE1detail1.Exists;
            ICADE1detail1.RecordClear();
            ICADE1header.Order = 3;
            //MessageBox.Show("Posted successfully");
        }
        public void WIPConsumption()
        {
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
            WIPCost = 0;

            String formatDate = planningDate.Substring(4, 2) + "/" + planningDate.Substring(6, 2) + "/" + planningDate.Substring(0, 4);
            Object TRANSDATE = (Object)formatDate;
            ICADE1headerFields.get_FieldByName("TRANSDATE").set_Value(ref TRANSDATE);
            ICADE1headerFields.get_FieldByName("PROCESSCMD").PutWithoutVerification("1");     //' Process Command
          
            ICADE1header.Process();
            DataTable rsMaterial = new DataTable();
            rsMaterial = sq.get_rs("select WIPITEMNO as  ITEMNO, QUANTITYPRD AS  ACTUALQUANTITY from T_PCPLN where PRODUCTIONDATE = '" + planningDate + "' AND QUANTITYPRD > 0 AND CATEGORY = '" + productCategory + "'");

            DataTable rsTemp = new DataTable();
            rsTemp = sq.get_rs("select sum(QUANTITYPRD) AS  QUANTITYPRD from T_PCPLN where PRODUCTIONDATE = '" + planningDate + "' AND CATEGORY = '" + productCategory + "'");            
            decimal totalWIPQuantity = Convert.ToDecimal(rsTemp.Rows[0]["QUANTITYPRD"].ToString());
            
            for (int i = 1; i <= Convert.ToInt32(rsMaterial.Rows.Count); i++)
            {
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordClear();
                temp = ICADE1detail1.Exists;
                ICADE1detail1.RecordCreate(0);
                temp = ICADE1detail1.Exists;

                Object ITEMNO = (Object)rsMaterial.Rows[i-1]["ITEMNO"].ToString();
                ICADE1detail1Fields.get_FieldByName("ITEMNO").set_Value(ref ITEMNO);
                ICADE1detail1Fields.get_FieldByName("PROCESSCMD").PutWithoutVerification("1"); //' Process

                ICADE1detail1.Process();

                Object TRANSTYPE = (Object) "5";
                ICADE1detail1Fields.get_FieldByName("TRANSTYPE").set_Value(ref TRANSTYPE);

                Object LOCATION = (Object) "100130";
                ICADE1detail1Fields.get_FieldByName("LOCATION").set_Value(ref LOCATION);

                Object QUANTITY = (Object)rsMaterial.Rows[i-1]["ACTUALQUANTITY"].ToString();
                ICADE1detail1Fields.get_FieldByName("QUANTITY").set_Value(ref QUANTITY);

                rsTemp =sq.get_rs("Select sum(extcost) AS RMCOST from icaded where MANITEMNO = '" + planningDate + "' and COMMENTS = 'RMT-" + productCategory.Trim() + "'");

                decimal MaterialCost = Convert.ToDecimal(rsTemp.Rows[0]["RMCOST"].ToString());

                WIPCost = (MaterialCost/totalWIPQuantity)* Convert.ToDecimal(rsMaterial.Rows[i-1]["ACTUALQUANTITY"].ToString());

                Object EXTCOST = (Object) Math.Round(WIPCost,2);
                ICADE1detail1Fields.get_FieldByName("EXTCOST").set_Value(ref EXTCOST);

                ICADE1detail1Fields.get_FieldByName("COMMENTS").set_Value("WIP-" + productCategory);

                Object MANITEMNO = (Object) planningDate;
                ICADE1detail1Fields.get_FieldByName("MANITEMNO").set_Value(ref MANITEMNO);

                ICADE1detail4.RecordClear();
                ICADE1detail4.RecordCreate(0);

                Object LOTNUMF = (Object)planningDate + rsMaterial.Rows[i-1]["ITEMNO"].ToString().Replace("-", "");
                ICADE1detail4Fields.get_FieldByName("LOTNUMF").set_Value(ref LOTNUMF);

                Object QTY = (Object)rsMaterial.Rows[i-1]["ACTUALQUANTITY"].ToString();
                ICADE1detail4Fields.get_FieldByName("QTY").set_Value(ref QTY);

                Object COST = (Object)Math.Round(WIPCost,2);
                ICADE1detail4Fields.get_FieldByName("COST").PutWithoutVerification(ref COST);

                ICADE1detail4.Insert();
                ICADE1detail4Fields.get_FieldByName("LOTNUMF").PutWithoutVerification(planningDate + rsMaterial.Rows[i-1]["ITEMNO"].ToString().Replace("-", ""));

                ICADE1detail4.Read();
                ICADE1detail4.Process();

                ICADE1detail1.Insert();

                ICADE1detail1Fields.get_FieldByName("LINENO").PutWithoutVerification(-1*i);

                ICADE1detail1.Read();

            }

            ICADE1headerFields.get_FieldByName("HDRDESC").PutWithoutVerification("Automated Adjustment for Production WIP-"+productCategory);   // ' Description

            ICADE1headerFields.get_FieldByName("REFERENCE").PutWithoutVerification(planningDate);  //' Reference

            ICADE1header.Insert();
            ICADE1header.Order = 0;

            ICADE1headerFields.get_FieldByName("ADJENSEQ").PutWithoutVerification("0"); // ' Sequence Number

            ICADE1header.Init();
            temp = ICADE1detail1.Exists;
            ICADE1detail1.RecordClear();
            ICADE1header.Order = 3;
        }
    }
}
