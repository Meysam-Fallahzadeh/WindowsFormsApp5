using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private string ShahkarUrl => "https://api.asax.ir:8243/Shahkar/1.0.0/MatchNationalIdMobile";

        private string TokenAddressApi => "https://apimanager.asax.ir/oauth2/token";

        private string ApiMangerUserName = "xcD6CSme6yqOkMebm6HnUAjDa1Ua";
        private string ApiMangerPassword = "AJ6qkkV4C3ByMYNe7AZhcVyB6OQa";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var requst = new ShahkarServiceRequest
            {
                Mobile = "09120037905",
                NationalId = "4849962203"
            };
            var httpClient = new HttpClient();

            var text = textBox3.Text.ToString();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", text);

            var json = JsonConvert.SerializeObject(requst);
            //var json = textBox4.Text;

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(ShahkarUrl, data).Result;

            textBox1.Text = response.Content.ReadAsStringAsync().Result;
        }







        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var key = ApiMangerUserName + ":" + ApiMangerPassword;

            var keyBase64Encode = Base64Encode(key);

            var request = new HttpRequestMessage(new HttpMethod("POST"), TokenAddressApi);

            request.Headers.TryAddWithoutValidation("Authorization", "Basic " + keyBase64Encode);

            request.Content = new StringContent("grant_type=client_credentials");

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var response = client.SendAsync(request).Result;

            var jsonValue = response.Content.ReadAsStringAsync().Result;

            textBox2.Text = jsonValue;

            var result = JsonConvert.DeserializeObject<AccessTokenDto>(jsonValue);

            textBox3.Text = result.Access_token;

        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var requst = new ShahkarServiceRequest
            {
                Mobile = "09120037905",
                NationalId = "4849962203"
            };
            var client = new RestClient("https://api.asax.ir:8243/Shahkar/1.0.0/MatchNationalIdMobile");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + textBox3.Text);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(request);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            textBox1.Text=response.Content;

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }
    }


    public class AccessTokenDto
    {
        public string Access_token { get; set; }

        public string Scope { get; set; }

        public string Token_type { get; set; }

        public int Expires_in { get; set; }
    }

    public class ShahkarServiceRequest
    {
        public string NationalId { get; set; }
        public string Mobile { get; set; }
    }
}
