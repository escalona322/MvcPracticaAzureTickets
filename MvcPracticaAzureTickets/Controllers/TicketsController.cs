using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcPracticaAzureTickets.Models;
using MvcPracticaAzureTickets.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcPracticaAzureTickets.Controllers
{
    public class TicketsController : Controller
    {
        private ServiceApiTickets service;
        private ServiceStorageBlobs serviceBlobs;

        public TicketsController(ServiceApiTickets service, ServiceStorageBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
        }
        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(Usuario usu)
        {
            await this.service.InsertUsuario(usu);
            return View();
        }

        public IActionResult CrearTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearTicket(int idticket, int idusuario, DateTime fecha
            , string importe, string producto, IFormFile file)
        {
            string containerName = "tickets";
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync(containerName, blobName, stream);
            }

            Ticket ticket = new Ticket
            {
                IdTicket = idticket,
                IdUsuario = idusuario,
                Fecha = fecha,
                Importe = importe,
                Producto = producto,
                FileName = blobName,
                StoragePath = "https://storagetajamarjalt.blob.core.windows.net/tickets/" + blobName
            };
            //List<BlobClass> blobs = await this.serviceBlobs.GetBlobsAsync(containerName);

            //foreach(BlobClass blob in blobs)
            //{
            //    if(blob.Nombre.Contains(blobName))
            //    {
            //        ticket.StoragePath = blob.Url;
            //    }
            //}

            await this.service.InsertTicket(ticket);

            return View();
        }

        public async Task<IActionResult> TicketsUsuario(int idusuario)
        {
            List<Ticket> tickets = await this.service.GetTicketsUsuarioAsync(idusuario);

            return View(tickets);
        }

    }
}
