using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Formula1
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "server=127.0.0.1;uid=programma;pwd=12345;database=formula_one_db";
        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Quando la scheda selezionata cambia, carica i dati corrispondenti
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    LoadPilotiData();
                    break;
                case 1:
                    LoadTeamData();
                    break;
                case 2:
                    LoadGareVinteData();
                    break;
                case 3:
                    LoadPilotiTeam();
                    break;
                    // Aggiungi altri casi per eventuali ulteriori schede
            }
        }

        private void LoadPilotiData(string filtroNome = "")
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM piloti";

                if (!string.IsNullOrEmpty(filtroNome))
                {
                    // Aggiungi il filtro per il nome
                    sql += $" WHERE cognome LIKE '%{filtroNome}%'";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = cmd;
                DataTable dati = new DataTable();
                MyAdapter.Fill(dati);
                dataGridViewPiloti.DataSource = dati;

                conn.Close();
            }
        }


        private void LoadTeamData(string filtroNomeTeam = "", string filtroSede = "")
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM team";

                // Aggiungi filtri, se specificati
                if (!string.IsNullOrEmpty(filtroNomeTeam))
                {
                    sql += $" WHERE nome_team LIKE '%{filtroNomeTeam}%'";
                }

                if (!string.IsNullOrEmpty(filtroSede))
                {
                    if (sql.Contains("WHERE"))
                    {
                        sql += $" AND sede LIKE '%{filtroSede}%'";
                    }
                    else
                    {
                        sql += $" WHERE sede LIKE '%{filtroSede}%'";
                    }
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = cmd;
                DataTable dati = new DataTable();
                MyAdapter.Fill(dati);
                dataGridViewTeam.DataSource = dati;

                conn.Close();
            }
        }


        private void LoadGareVinteData()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM gare_vinte;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = cmd;
                DataTable dati = new DataTable();
                MyAdapter.Fill(dati);
                dataGridViewGareVinte.DataSource = dati;

                conn.Close();
            }
        }

        private void LoadPilotiTeam()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT piloti.cognome, team.nome_team, piloti_team.anno_ingresso " +
              "FROM piloti_team " +
              "JOIN piloti ON piloti_team.pilota_id = piloti.pilota_id " +
              "JOIN team ON piloti_team.team_id = team.team_id;";


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd;
                    DataTable dati = new DataTable();
                    MyAdapter.Fill(dati);
                    dataGridViewPilotiTeam.DataSource = dati;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il caricamento dei dati da piloti_team: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewPiloti.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeam.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewGareVinte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPilotiTeam.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabControl1.SelectedIndex = 0;


            

        }

        private void dataGridViewPiloti_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void btnFiltraPiloti_Click(object sender, EventArgs e)
        {
            LoadPilotiData(txtFiltroNome.Text);

        }

        private void txtFiltroNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void yy_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPilotiData(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadTeamData(textBox2.Text, textBox3.Text);

        }
    }
}
    



