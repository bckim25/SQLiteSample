using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace SQLiteSample
{
    public partial class Form1 : Form
    {
        private string connecString;
        
        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void SQLiteConn(string DBFilePath)
        {
            connecString = "Data Source=" + DBFilePath + ";Pooling=true;FailIfMissing=false";
        }

        //트랜잭션 시작
        private void BeginTran(SQLiteConnection conn)
        {
            SQLiteCommand command = new SQLiteCommand("Begin", conn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        //트랜잭션 완료
        private void CommitTran(SQLiteConnection conn)
        {
            SQLiteCommand command = new SQLiteCommand("Commit", conn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        private void InsertDB(string tablename, DataRow data)
        {
            //SQLite 연결
            SQLiteConnection SQConn = new SQLiteConnection();
            SQConn.ConnectionString = connecString;
            SQConn.Open();

            //트랜잭션 시작
            BeginTran(SQConn);

            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + tablename + " VALUES (" 
                + "'" + data.ItemArray[0].ToString() + "', " 
                + data.ItemArray[1].ToString() + ", "
                 + data.ItemArray[2].ToString() + ", "
                  + data.ItemArray[3].ToString() + ", "
                   + data.ItemArray[4].ToString() + ", "
                    + data.ItemArray[5].ToString() + ", "
                     + data.ItemArray[6].ToString() + ", "
                      + data.ItemArray[7].ToString() + ", "
                       + data.ItemArray[8].ToString() + ")", SQConn);
            cmd.ExecuteNonQuery();

            //트랜잭션을 완료한다.
            CommitTran(SQConn);
            
            SQConn.Close();
            SQConn.Dispose();
            cmd.Dispose();
        }

        private void UpdateDB(string tablename, DataRow data)
        {
            //SQLite 연결
            SQLiteConnection SQConn = new SQLiteConnection();
            SQConn.ConnectionString = connecString;
            SQConn.Open();

            //트랜잭션 시작
            BeginTran(SQConn);

            SQLiteCommand cmd = new SQLiteCommand("UPDATE " + tablename + " SET " +
                "Env_Location=" + data.ItemArray[1].ToString() + ", " +
                 "Env_Season=" + data.ItemArray[2].ToString() + ", " +
                  "Env_Weather=" + data.ItemArray[3].ToString() + ", " +
                   "Env_VisDist=" + data.ItemArray[4].ToString() + ", " +
                    "Env_TimeOfDay=" + data.ItemArray[5].ToString() + ", " +
                     "Env_CloudLow=" + data.ItemArray[6].ToString() + ", " +
                      "Env_CloudHigh=" + data.ItemArray[7].ToString() + ", " +
                       "Env_CloudQuantity=" + data.ItemArray[8].ToString() + " WHERE " + "Scenario_name='" + data.ItemArray[0].ToString() + "'", SQConn);
            cmd.ExecuteNonQuery();

            //트랜잭션을 완료한다.
            CommitTran(SQConn);

            SQConn.Close();
            SQConn.Dispose();
            cmd.Dispose();
        }

        private void DeleteDB(string tablename, string scn_name)
        {
            //SQLite 연결
            SQLiteConnection SQConn = new SQLiteConnection();
            SQConn.ConnectionString = connecString;
            SQConn.Open();

            //트랜잭션 시작
            BeginTran(SQConn);

            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM " + tablename + " WHERE Scenario_name='" + scn_name + "'", SQConn);
            cmd.ExecuteNonQuery();
            
            //트랜잭션을 완료한다.
            CommitTran(SQConn);

            //사용했던 객체 삭제
            SQConn.Close();
            SQConn.Dispose();
            cmd.Dispose();
        }

        private void FindDB(string tablename, string scn_name, ref DataTable dt)
        {
            //SQLite 연결
            SQLiteConnection SQConn = new SQLiteConnection();
            SQConn.ConnectionString = connecString;
            SQConn.Open();

            //select 할 command객체 생성
            SQLiteCommand sqCommend = new SQLiteCommand(SQConn);
            sqCommend.CommandType = CommandType.Text;
            //select query
            sqCommend.CommandText = "select * from " + tablename + " WHERE Scenario_name='" + scn_name +"'";
            //데이터를 받아올 adapter 생성
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqCommend);

            //datatable 생성하고 그 테이블에 데이터를 받아온다.
            dt = new DataTable();
            adapter.Fill(dt);

            //사용헀던 객체 삭제
            adapter.Dispose();
            sqCommend.Dispose();
            SQConn.Close();
            SQConn.Dispose();
        }

        private void GetAllDataFromTable(string tablename, ref DataTable dt)
        {
            //SQLite 연결
            SQLiteConnection SQConn = new SQLiteConnection();
            SQConn.ConnectionString = connecString;
            SQConn.Open();

            //select 할 command객체 생성
            SQLiteCommand sqCommend = new SQLiteCommand(SQConn);
            sqCommend.CommandType = CommandType.Text;
            //select query
            sqCommend.CommandText = "select Scenario_name, Env_Location, Env_Season, Env_Weather, Env_VisDist, Env_TimeOfDay, Env_CloudLow, Env_CloudHigh, Env_CloudQuantity from " + tablename;
            //데이터를 받아올 adapter 생성
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqCommend);

            //datatable 생성하고 그 테이블에 데이터를 받아온다.
            dt = new DataTable();
            adapter.Fill(dt);

            //사용헀던 객체 삭제
            adapter.Dispose();
            sqCommend.Dispose();
            SQConn.Close();
            SQConn.Dispose();
        }

        private void RefreshDataGridView()
        {
            DataTable dt = null;
            GetAllDataFromTable("Table_Env", ref dt);

            dataGridView1.DataSource = dt;

            dt.Dispose();
        }

        private void btn_OpenDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SQLiteConn(dlg.FileName);
                RefreshDataGridView();
            }
        }        

        private void btn_Update_Click(object sender, EventArgs e)
        {

            InsertForm dlg = new InsertForm();

            dlg.setData(dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[3].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[4].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[5].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[6].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[7].Value.ToString(),
                dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[8].Value.ToString());

            dlg.ChangeBtnTxt("update");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = null;
                GetAllDataFromTable("Table_Env", ref dt);

                DataRow data = dt.NewRow();
                data["Scenario_name"] = dlg.Scn_name;
                data["Env_Location"] = dlg.location;
                data["Env_Season"] = dlg.season;
                data["Env_Weather"] = dlg.weather;
                data["Env_VisDist"] = dlg.visdist;
                data["Env_TimeOfDay"] = dlg.timeofday;
                data["Env_CloudLow"] = dlg.cloudlow;
                data["Env_CloudHigh"] = dlg.cloudhigh;
                data["Env_CloudQuantity"] = dlg.cloudquantity;

                UpdateDB("Table_Env", data);

                RefreshDataGridView();

                dt.Dispose();
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            InsertForm dlg = new InsertForm();
            dlg.ChangeBtnTxt("insert");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = null;
                GetAllDataFromTable("Table_Env", ref dt);

                DataRow data = dt.NewRow();
                data["Scenario_name"] = dlg.Scn_name;
                data["Env_Location"] = dlg.location;
                data["Env_Season"] = dlg.season;
                data["Env_Weather"] = dlg.weather;
                data["Env_VisDist"] = dlg.visdist;
                data["Env_TimeOfDay"] = dlg.timeofday;
                data["Env_CloudLow"] = dlg.cloudlow;
                data["Env_CloudHigh"] = dlg.cloudhigh;
                data["Env_CloudQuantity"] = dlg.cloudquantity;

                InsertDB("Table_Env", data);

                RefreshDataGridView();

                dt.Dispose();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DeleteDB("Table_Env", dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value.ToString());
            RefreshDataGridView();
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            FindDB("Table_Env", dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value.ToString(), ref dt);

            if (dt.Rows.Count > 1)
                MessageBox.Show("Find " + dt.Rows.Count + " matched result");

            string str = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                

                str = dt.Rows[i][0].ToString() + " / " + dt.Rows[i][1].ToString() + " / " + dt.Rows[i][2].ToString() + " / " + dt.Rows[i][3].ToString() +
                    " / " + dt.Rows[i][4].ToString() + " / " + dt.Rows[i][5].ToString() + " / " + dt.Rows[i][6].ToString() + " / " + dt.Rows[i][7].ToString() +
                    " / " + dt.Rows[i][8].ToString();
                MessageBox.Show(str);
            }
        }

        
    }
}
