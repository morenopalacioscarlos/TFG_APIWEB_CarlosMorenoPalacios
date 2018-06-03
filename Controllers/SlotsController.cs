using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WEBAPI_VENDINGMACHINE;

namespace WEBAPI_VENDINGMACHINE.Controllers
{
    public class SlotsController : ApiController
    {
        private MachineVendingEntities machindeVendingContext = new MachineVendingEntities();

        /// <summary>
        /// Devuelve el precio de un articulo de una máquina en formato json
        /// </summary>
        /// <param name="idMachine"></param>
        /// <param name="idSlot"></param>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        //Example : http://localhost:49906/api/Slots/GetSlots/12/asdasdsa54sad5as4das54dsad4/2/2
        // GET: api/Slots/GetSlots/MachineId/tokenId/Idslot/ProductId
        [ResponseType(typeof(int))]
        public IHttpActionResult GetSlots(int idMachine,string tokenId, int idSlot, int idProduct)
        {
            
           if(tokenId !=  machindeVendingContext.Vending_Machine.Find(idMachine).TokenId)
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.PreconditionFailed));

            try
            {
                var slots = machindeVendingContext.Slots.Where(x => x.Id_Machine == idMachine && x.Id_Product == idProduct && x.Slot_Number == idSlot).Select(x => x.Price).First();

                if (slots == null)
                {
                    return NotFound();
                }

                return Ok(slots);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Devuelve si hay actualizaciónes de precio de algún producto de la máquina
        /// </summary>
        /// <param name="idMachine"></param>
        /// <param name="idSlot"></param>
        /// <returns></returns>
        //Example : http://localhost:49906/api/Slots/GetSlots/12/asdasdsa54sad5as4das54dsad4/
        // GET: api/Slots/GetSlots/MachineId/tokenId/
        [ResponseType(typeof(int))]
        public IHttpActionResult GetUpdateSlots(int idMachine, string tokenId)
        {

            if (tokenId != machindeVendingContext.Vending_Machine.Find(idMachine).TokenId)
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.PreconditionFailed));


            try
            {
                var dbContext = machindeVendingContext.PriceUpdated.ToList();
                var UpdateSlotPrice = dbContext.Where(x => x.IdMachine == idMachine).Select(x =>new { x.IdSlot, x.IdProduct, x.IdMachine, x.NewPrice, x.IdUpdate });
                if (UpdateSlotPrice == null)
                {
                    return NotFound();
                }
                
                //Se eliminan de la base de datos una vez obtenidos los nuevos precios
                foreach (var priceForRemove in UpdateSlotPrice)
                {
                    var priceFound = machindeVendingContext.PriceUpdated.Find(priceForRemove.IdUpdate);
                    machindeVendingContext.PriceUpdated.Remove(priceFound);
                }
                machindeVendingContext.SaveChanges();
                return Ok(UpdateSlotPrice);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}