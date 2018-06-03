using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WEBAPI_VENDINGMACHINE;

namespace WEBAPI_VENDINGMACHINE.Controllers
{
    public class SoldedsController : ApiController
    {
        private MachineVendingEntities machindeVendingContext = new MachineVendingEntities();

        /// <summary>
        /// Inserta en la base de datos una nueva venta realizada
        /// </summary>
        /// <param name="idMachine"></param>
        /// <param name="tokenKey"></param>
        /// <param name="idProduct"></param>
        /// <param name="onecent"></param>
        /// <param name="twocents"></param>
        /// <param name="fivecents"></param>
        /// <param name="tencents"></param>
        /// <param name="twentyents"></param>
        /// <param name="fiftycents"></param>
        /// <param name="twoeur"></param>
        /// <returns></returns>
        // POST: api/Soldeds/PostNewSale/idMachine/tokenKey/idProduct-onecent-twocents-fivecents-tencents-twentyents-fiftycents-oneeur-twoeur
        //example : http://localhost:49906/api/Soldeds/PostNewSale/12/00840076005b00070049003d00db00dc006f001a002e0081003d000f00f6001a/3/1/1-1-1-1-1-1-1-1
        public IHttpActionResult PostNewSale(int idMachine, string tokenKey, int idProduct, int slotNumber,
            int oneCent, int twoCents, int fiveCents, int tenCents, int twentyCents, int fiftyCents, int oneEur, int twoEur)
        {
            if (idMachine < 0  || tokenKey == string.Empty || idProduct < 0 || slotNumber < 0 || oneCent < 0 || twoCents < 0
                                                 || fiveCents < 0 || tenCents < 0 || twentyCents < 0
                                                 || fiftyCents < 0 || oneEur < 0 || twoEur < 0)

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.PreconditionFailed));

            else
            {
                var temp = machindeVendingContext.Vending_Machine.Find(idMachine);
              if (tokenKey !=  machindeVendingContext.Vending_Machine.Find(idMachine).TokenId)
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.PreconditionFailed));

                var date = DateTime.Now.Date.Date;
                var time = DateTime.Now.TimeOfDay;

                var newSolded = new Solded();
                newSolded.Time = time;
                newSolded.Date = date;
                newSolded.IdMachine = idMachine;
                newSolded.IdProduct = idProduct;

                //se comprueba si existe el producto y la máquina que se pasan por parámetros
                var vending = machindeVendingContext.Vending_Machine.FirstOrDefault(x => x.Id_Machine == idMachine);
                var item = machindeVendingContext.Items.FirstOrDefault(x => x.Id == idProduct);

                //si existen se intenta meter en la base de datos, sino se devuelve un error.
                if (vending != null && item != null)
                {
                    machindeVendingContext.Solded.Add(newSolded);

                    if (machindeVendingContext.SaveChanges() < 0)
                        ResponseMessage(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    else
                    {
                        if (DiscountItemElement(idMachine, idProduct, slotNumber) && DiscountCoins(idMachine, oneCent, twoCents, fiveCents, tenCents, twentyCents, fiftyCents, oneEur, twoEur))
                            return Ok();
                        else
                            ResponseMessage(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    }
                }

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

        }

        /// <summary>
        /// Descuenta de la base de datos el producto vendido
        /// </summary>
        /// <param name="idMachine"></param>
        /// <param name="idProduct"></param>
        /// <param name="slotNumber"></param>
        /// <returns></returns>
        public bool DiscountItemElement(int idMachine, int idProduct, int slotNumber)
        {
            ///se comprueba si existe un slot de una máquina que disponga el producto y cuyo identificador sea el mismo que se ha pasado
            ///por argumento
            var idSlot = machindeVendingContext.Slots.Where(x => x.Id_Machine == idMachine &&
                                                                 x.Id_Product == idProduct &&
                                                                 x.Slot_Number == slotNumber).Select(x => x.Id_Slot).FirstOrDefault();

            //se busca el slot.
            var slot = machindeVendingContext.Slots.Find(idSlot, idMachine, idProduct);

            //se descuenta el producto y se carga en la bbdd.
            slot.Stock--;

            if (machindeVendingContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Se descuentan las monedas gastadas en devolver cambio en la máquina.
        /// </summary>
        /// <param name="idMachine"></param>
        /// <param name="oneCent"></param>
        /// <param name="twoCents"></param>
        /// <param name="fiveCents"></param>
        /// <param name="tenCents"></param>
        /// <param name="twentyCents"></param>
        /// <param name="fiftyCents"></param>
        /// <param name="oneEur"></param>
        /// <param name="twoEur"></param>
        /// <returns></returns>
        public bool DiscountCoins(int idMachine, int oneCent, int twoCents, int fiveCents, int tenCents,
                                    int twentyCents, int fiftyCents, int oneEur, int twoEur)
        {
            var idCoins = machindeVendingContext.Change.Where(x => x.Id_Machine == idMachine).Select(x => x.Id_Change);
            var coinObject = machindeVendingContext.Change.Find(idCoins.First());

            coinObject.Coins_1_Cents -= oneCent;
            coinObject.Coins_2_Cents -= twoCents;
            coinObject.Coins_5_Cents -= fiveCents;
            coinObject.Coins_10_Cents -= tenCents;
            coinObject.Coins_20_Cents -= twentyCents;
            coinObject.Coins_50_Cents -= fiftyCents;
            coinObject.Coins_1_Eur -= oneEur;
            coinObject.Coins_2_Eur -= twoEur;

            if (machindeVendingContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }
    }
}