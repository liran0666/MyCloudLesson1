using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MyCloudLesson1.Models;

namespace MyCloudLesson1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        CosmosClient myClient;
        private void activation_Btn_Click(object sender, EventArgs e)
        {
            try { 
            myClient = new CosmosClient(uri, primarykey);
            MessageBox.Show("Created DB Successfully","DB Created ",MessageBoxButtons.OK,MessageBoxIcon.Information);
            activation_Btn.Enabled = false;
            tabControl1.SelectedTab = tabPage2;
            btnDbContCreate.Enabled = true;
            
            }
            catch(Exception ex)
            {
                MessageBox.Show("error " + ex, "error somthing went wrong", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        //developer data catch
        String devName = ConfigurationManager.AppSettings["Devname"];
        String devId= ConfigurationManager.AppSettings["DevId"];
        String developerEmail = ConfigurationManager.AppSettings["DevEmail"];

        //Enviroment data catch
        String envtype = ConfigurationManager.AppSettings["EnvType"];
        String uri = ConfigurationManager.AppSettings["URI"];
        String primarykey = ConfigurationManager.AppSettings["PrimaryKey"];

        

        private void Form1_Load(object sender, EventArgs e)
        {
            devname.Text = devName;
            devid.Text = devId;
            devEmail.Text = developerEmail;
            textBoxEnvType.Text = envtype;
            if(primarykey.Length>9)
                textBoxPrimaryKey.Text = primarykey.Substring(0,9)+"....";
            textBoxUri.Text = uri;
            btnDbContCreate.Enabled = false;
        }

        private async void btnDbContCreate_Click(object sender, EventArgs e)
        {
            String dbName = dataBaseTextBox.Text;
            String contName = containerTextBox.Text;
            try { 
            await createDataBaseAndContainer(dbName, contName);
                MessageBox.Show("Db and Container Created");
            }
            catch (Exception ex)
            {
                MessageBox.Show( " "+ex, "error somthing went wrong", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private async Task createDataBaseAndContainer(string dbName, string contName)
        {
            if (string.IsNullOrEmpty(dbName)) return;
            DatabaseResponse dbResponse= await myClient.CreateDatabaseIfNotExistsAsync(dbName);

            HttpStatusCode dbStatus = dbResponse.StatusCode;
            if (dbStatus == HttpStatusCode.Created)
                MessageBox.Show("DB was Created", "Db creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                if (dbStatus == HttpStatusCode.OK)
                    MessageBox.Show("DB was Not Created", "Db Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 else 
                     MessageBox.Show("DB Creation error", "Status code: "+dbResponse, MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (string.IsNullOrEmpty(contName)) return;

            Database dbObj = dbResponse.Database;

            ContainerResponse contResponse = await dbObj.CreateContainerIfNotExistsAsync(contName,"/id");

            HttpStatusCode contCreationStat = contResponse.StatusCode;

            if (contCreationStat == HttpStatusCode.Created)
                MessageBox.Show("Table was Created", "Table creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                if (contCreationStat == HttpStatusCode.OK)
                MessageBox.Show("Table was Not Created", "Table Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Table Creation error", "Status code: " + contResponse, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void countDbsBtn_Click(object sender, EventArgs e)
        {
            int numOfDb = await countNumOfDbs();
            numberOfDbBtn.Text = numOfDb.ToString();
        }

        private async Task <int> countNumOfDbs()
        {
            int numofdb = 0;
            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties currentdb in await dIter.ReadNextAsync())
                    numofdb++;
            }

            return numofdb;
        }

        private async void dbNamesInCloudButton_Click(object sender, EventArgs e)
        {
            comboBoxDbNames.DataSource = await getDbNamesFromSource();
        }

        private async Task<List<string>> getDbNamesFromSource()
        {
            List<string> dbNamesFromSource = new List<string>();

            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach(DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    dbNamesFromSource.Add(dbNamesToAdd.Id);
                }
            }

            return dbNamesFromSource;
        }

        private async void tableNamesInCloud_Click(object sender, EventArgs e)
        {
            comboBoxTableNames.DataSource = await getTableNamesFromSource();
        }

        private async Task<List<string>> getTableNamesFromSource()
        {
            List<string> tableNamesFromSource = new List<string>();

            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer=dbcurr.GetContainerQueryIterator<ContainerProperties>();

                    while (titer.HasMoreResults)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                            tableNamesFromSource.Add(dbNamesToAdd.Id+" - "+ tbToAdd.Id);

                        
                    }
                }
            }

            return tableNamesFromSource;
        }

        

        private async Task<int> countTablesBtn()
        {
            int tablesnum=0;

            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer = dbcurr.GetContainerQueryIterator<ContainerProperties>();

                    while (titer.HasMoreResults)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                            tablesnum++;


                    }
                }
            }

            return tablesnum;
        }

        private async void CoutntTbBtn_Click(object sender, EventArgs e)
        {
            int numofTables = await countTablesBtn();
            numberOfTablesTbox.Text = Convert.ToString(numofTables);
        }

        private async void btnTargi12_Click(object sender, EventArgs e)
        {
            string nameoftable = textBoxTargil12.Text;
            comboBoxTargil12NamesOfDbs.DataSource = await DbsNamesTargil12(nameoftable);
        }

        private async Task<List<string>> DbsNamesTargil12(string nameoftable)
        {
            List<string> tableNamesFromSource = new List<string>();

            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer = dbcurr.GetContainerQueryIterator<ContainerProperties>();

                    while (titer.HasMoreResults)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                            if (nameoftable.Equals(tbToAdd.Id))
                                tableNamesFromSource.Add(dbNamesToAdd.Id);



                    }
                }
            }
            if (tableNamesFromSource.Count == 0)
                tableNamesFromSource.Add("no Such Db");
            return tableNamesFromSource;
        }

        private async void checkIfDbExistBtn_Click(object sender, EventArgs e)
        {
            string dbnametocheckexistance = checkIfDbExistTextBox.Text;
            await checkIfDbExisttargil8(dbnametocheckexistance);
        }

        private async Task checkIfDbExisttargil8(string dbnametocheckexistance)
        {
            bool flag = false;
            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    if (dbnametocheckexistance.Equals(dbNamesToAdd.Id))
                        flag = true;
                }
            }
            if(flag)
                MessageBox.Show("Db Exists!", "Exist Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Db does not Exists!", "Exist Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DbsofTargil13ComboBox.DataSource = await DbsofTargil13getNames();
        }

        private async Task<List<string>> DbsofTargil13getNames()
        {
            List<string> tableNamesFromSource = new List<string>();
            int tablecount;
            bool flag;
            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    if (dbNamesToAdd.Id.Length % 2 == 0)
                        continue;
                    tablecount = 0;
                    flag = true;
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer = dbcurr.GetContainerQueryIterator<ContainerProperties>();
                    

                    while (titer.HasMoreResults&&flag)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                        { 
                            tablecount++;
                            if (tablecount == 3)
                            {
                                flag = false;
                                break;
                            }
                        }
                        


                    }
                    if (tablecount == 0 || tablecount >= 3)
                        tableNamesFromSource.Add(dbcurr.Id);
                }
            }
            if (tableNamesFromSource.Count == 0)
                tableNamesFromSource.Add("no Such Db");
            return tableNamesFromSource;
        }

        private async void tableAmountTargil14Btn_Click(object sender, EventArgs e)
        {
            int tableamount = Convert.ToInt32(tableAmountTargil14TextBox.Text);
            tableAmountResultTargil14TextBox.Text = await getStringOfDbsWIthExactTableAmount(tableamount);
            

        }

        private async Task <string> getStringOfDbsWIthExactTableAmount(int tableamount)
        {
            string namesofDbswiththatamount = "";
            int tablecount;
            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer = dbcurr.GetContainerQueryIterator<ContainerProperties>();
                    tablecount = 0;
                    while (titer.HasMoreResults)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                        {
                            tablecount++;
                        }


                    }
                    if (tablecount == tableamount)
                        namesofDbswiththatamount += dbNamesToAdd.Id + ",";
                }
            }
            if (namesofDbswiththatamount.Length == 0)
                namesofDbswiththatamount="no Such Dbs";
            return namesofDbswiththatamount;
        }

        private async void sortMinCharBtn_Click_1(object sender, EventArgs e)
        {
            int numOfChars;
            if (String.IsNullOrEmpty(charAmountTextBox.Text))
                numOfChars = 0;
            else
                numOfChars = Convert.ToInt32(charAmountTextBox.Text);

            resultTextBoxTar16.Text = await minChars(numOfChars);
        }
        private async Task<string> minChars(int numOfChars)
        {
            string tableNamesFromSource = "";
            bool flag;
           
            FeedIterator<DatabaseProperties> dIter = myClient.GetDatabaseQueryIterator<DatabaseProperties>();
            while (dIter.HasMoreResults)
            {
                foreach (DatabaseProperties dbNamesToAdd in await dIter.ReadNextAsync())
                {
                    flag = true;
                    Database dbcurr = myClient.GetDatabase(dbNamesToAdd.Id);
                    FeedIterator<ContainerProperties> titer = dbcurr.GetContainerQueryIterator<ContainerProperties>();

                    while (titer.HasMoreResults&&flag)
                    {
                        foreach (ContainerProperties tbToAdd in await titer.ReadNextAsync())
                            if (tbToAdd.Id.Length > (numOfChars))
                            {
                                tableNamesFromSource += (dbNamesToAdd.Id+", ");
                                flag = false;
                                break;
                            }
                               


                    }
                }
            }
            if (tableNamesFromSource.Length == 0)
                tableNamesFromSource=("no Such Db");
            return tableNamesFromSource;
        }

        private async void getDbsToJsonInsertBtn_Click(object sender, EventArgs e)
        {
            dbsNamesForJsonInsertComboBox.DataSource= await getDbNamesFromSource();
        }

        private async void saveDataForIsertionBtn_Click(object sender, EventArgs e)
        {
            String dbname = textBoxDbNameToInsertValues.Text;
            String tbname = textBoxTableNameToInsertValues.Text;
            Driver liran = new Driver();
            liran.id = Guid.NewGuid().ToString();
            liran.driverName = driverNameTextBox.Text;
            liran.age = 23.11;
            liran.passengers = new Passengers[2];
            liran.passengers[0] = new Passengers { name = "anat", age = 48.5, specialRequest = "none" };
            liran.passengers[1] = new Passengers { name = "ori", age = 17.11, specialRequest = "put music on" };


            liran.stations = new CabStations[1];
            liran.stations[0] = new CabStations { adress = "almog" };

            await SaveDriverDataInCloudAsync(dbname,tbname,liran);
        }

        private async Task SaveDriverDataInCloudAsync(string dbname, string tbname, Driver liran)
        {
            DatabaseResponse dbResponse = await myClient.CreateDatabaseIfNotExistsAsync(dbname);
            if(dbResponse.StatusCode==HttpStatusCode.OK||dbResponse.StatusCode==HttpStatusCode.Created)
            {
                Database db = dbResponse.Database;
                ContainerResponse contres = await db.CreateContainerIfNotExistsAsync(tbname,"/id");
                if (contres.StatusCode == HttpStatusCode.OK || contres.StatusCode == HttpStatusCode.Created)
                {
                    Microsoft.Azure.Cosmos.Container cont = contres.Container;
                    await cont.CreateItemAsync<Driver>(liran);
                }
            }
        }

        private async void carDataSaveBtn_Click(object sender, EventArgs e)
        {
            String dbname = dbnameToInsertValueTargil25TextBox.Text;
            String tbname = tbnameToInsertValueTargil25TextBox.Text;
            Car obj = new Car();
            obj.id = Guid.NewGuid().ToString();
            obj.brand = brandNameTextBox.Text;
            obj.model = modelNameTextBox.Text;
            obj.displacment = Convert.ToInt32(displacmentTextBox.Text);
            obj.color = carColorTextBox.Text;
            obj.laptimes = new LapTime[3];
            obj.laptimes[0] = new LapTime { time = 12.7 };
            obj.laptimes[1] = new LapTime { time = 13.9 };
            obj.laptimes[2] = new LapTime { time = 11.2 };

            await SaveCarDataInCloudAsync(dbname, tbname, obj);
        }
        private async Task SaveCarDataInCloudAsync(string dbname, string tbname, Car obj)
        {
            DatabaseResponse dbResponse = await myClient.CreateDatabaseIfNotExistsAsync(dbname);
            if (dbResponse.StatusCode == HttpStatusCode.OK || dbResponse.StatusCode == HttpStatusCode.Created)
            {
                Database db = dbResponse.Database;
                ContainerResponse contres = await db.CreateContainerIfNotExistsAsync(tbname, "/id");
                if (contres.StatusCode == HttpStatusCode.OK || contres.StatusCode == HttpStatusCode.Created)
                {
                    Microsoft.Azure.Cosmos.Container cont = contres.Container;
                    await cont.CreateItemAsync<Car>(obj);
                }
            }
        }
    }
}
