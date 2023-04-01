using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Services.Interfaces
{
    public interface ITicketService
    {
        public List<Ticket> GetTickets();

        public int BuyTicket(int id, TicketViewModel ticketForm);
    }
}
