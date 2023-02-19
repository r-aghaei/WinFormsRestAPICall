using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace WinFormsRestAPICall
{
    public partial class Form1 : Form
    {
        private static HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var url = "https://northwind.vercel.app/api/categories";
            var response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(content);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.DataSource = categories;
            }
        }

        private async void addToolStripButton_Click(object sender, EventArgs e)
        {
            var url = "https://northwind.vercel.app/api/categories";
            var data = new Category() { Name = "Lorem", Description = "Ipsum" };
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(jsonData, Encoding.Unicode, "application/json");
            var response = await client.PostAsync(url, requestContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                var createdCategory = JsonConvert.DeserializeObject<Category>(content);
                MessageBox.Show(createdCategory.Id.ToString());
            }
        }

        private async void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            var id = ((Category)dataGridView1.CurrentRow.DataBoundItem).Id;
            var url = $"https://northwind.vercel.app/api/categories/{id}";
            var response = await client.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                MessageBox.Show(content.ToString());
            }
        }
    }
}
