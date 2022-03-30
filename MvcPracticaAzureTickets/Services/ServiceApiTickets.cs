using MvcPracticaAzureTickets.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcPracticaAzureTickets.Services
{
    public class ServiceApiTickets
    {
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiTickets(string url)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Ticket>> GetTicketsUsuarioAsync(int idusuario)
        {
            string request = "/api/Ticket/TicketUsuario/" + idusuario;
            List<Ticket> tickets =
                await this.CallApiAsync<List<Ticket>>(request);

            return tickets;
        }

        public async Task<Ticket> FindTicket(int idticket)
        {
            string request = "/api/Ticket/FindTicket/" + idticket;
            Ticket tickets =
                await this.CallApiAsync<Ticket>(request);

            return tickets;
        }

        public async Task InsertUsuario(Usuario usu)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Ticket/InsertUsuario";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
  
                string json = JsonConvert.SerializeObject(usu);

                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task InsertTicket(Ticket ticket)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Ticket/InsertTicket";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
 
                string json = JsonConvert.SerializeObject(ticket);

                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task ProcesarTicket(Ticket ticket)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Ticket/ProcesarTicket";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string json = JsonConvert.SerializeObject(ticket);

                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
    }
}
