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


        private void LoadGareVinteData(string filtroNomeGara = "", DateTime? filtroDataGara = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT gare_vinte.nome_gara, gare_vinte.data_gara, piloti.cognome AS vincitore_cognome " +
                             "FROM gare_vinte " +
                             "LEFT JOIN piloti ON gare_vinte.vincitore_id = piloti.pilota_id";

                // Aggiungi filtri, se specificati
                if (!string.IsNullOrEmpty(filtroNomeGara))
                {
                    sql += " WHERE gare_vinte.nome_gara LIKE @filtroNomeGara";
                }

                if (filtroDataGara.HasValue && filtroDataGara != DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(filtroNomeGara))
                    {
                        sql += " AND";
                    }
                    else
                    {
                        sql += " WHERE";
                    }

                    sql += " gare_vinte.data_gara = @filtroDataGara";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // Aggiungi i parametri per i filtri
                if (!string.IsNullOrEmpty(filtroNomeGara))
                {
                    cmd.Parameters.AddWithValue("@filtroNomeGara", $"%{filtroNomeGara}%");
                }

                if (filtroDataGara.HasValue && filtroDataGara != DateTime.MinValue)
                {
                    cmd.Parameters.AddWithValue("@filtroDataGara", filtroDataGara.Value.Date.ToString("yyyy-MM-dd"));
                }

                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = cmd;
                DataTable dati = new DataTable();
                MyAdapter.Fill(dati);
                dataGridViewGareVinte.DataSource = dati;

                conn.Close();
            }
        }





        private void LoadPilotiTeam(string filtroCognomePilota = "", string filtroNomeTeam = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT piloti.cognome, team.nome_team, piloti_team.anno_ingresso " +
                                 "FROM piloti_team " +
                                 "JOIN piloti ON piloti_team.pilota_id = piloti.pilota_id " +
                                 "JOIN team ON piloti_team.team_id = team.team_id";

                    // Aggiungi filtri, se specificati
                    if (!string.IsNullOrEmpty(filtroCognomePilota))
                    {
                        sql += $" WHERE piloti.cognome LIKE '%{filtroCognomePilota}%'";
                    }

                    if (!string.IsNullOrEmpty(filtroNomeTeam))
                    {
                        if (sql.Contains("WHERE"))
                        {
                            sql += $" AND team.nome_team LIKE '%{filtroNomeTeam}%'";
                        }
                        else
                        {
                            sql += $" WHERE team.nome_team LIKE '%{filtroNomeTeam}%'";
                        }
                    }

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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filtroNomeGara = textBox4.Text;
            DateTime filtroDataGara = dateTimePicker1.Checked ? dateTimePicker1.Value : DateTime.MinValue;

            // Se la data del filtro è la data di default, imposta il valore su DateTime.MinValue
            if (filtroDataGara.Date == dateTimePicker1.MinDate)
            {
                filtroDataGara = DateTime.MinValue;
            }

            // Usa i filtri solo se sono stati forniti valori diversi dai valori di default
            if (!string.IsNullOrEmpty(filtroNomeGara) || filtroDataGara != DateTime.MinValue)
            {
                LoadGareVinteData(filtroNomeGara, filtroDataGara);
            }
            else
            {
                // Se entrambi i filtri sono vuoti o impostati ai valori di default, carica tutti i dati
                LoadGareVinteData();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Aggiungi controlli grafici per il filtro dei piloti team (TextBox per il cognome del pilota e TextBox per il nome del team)
            string filtroCognomePilota = textBox5.Text;
            string filtroNomeTeam = textBox6.Text;

            LoadPilotiTeam(filtroCognomePilota, filtroNomeTeam);
        }
    }
}
    



